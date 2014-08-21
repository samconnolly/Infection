using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameJam
{
    public abstract class SpriteBase
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Vector2 _velocity;
        private Rectangle _rectangle;
        private Vector2 _drawoffset;
        private Rectangle _collisionRectangle;
        private bool _hasCollided;
        private int _speed;
        private bool _rotate;
        private float _rotateAngle;
        private float _scale;
        private int _columns = 1;
        private int _rows = 1;
        private int _xframe = 0;
        private int _yframe = 0;

        private float _rotation = 0;

        private bool bounced = false;
        private int bounceCounter = 0;
        private int bounceTime = 100;

        #region Cnstructors

        public SpriteBase(Texture2D tex)
        {
            _texture = tex;
            _position = new Vector2(0, 0);
            _velocity = new Vector2(0, 0);
            _rectangle = new Rectangle(0, 0, tex.Width, tex.Height);
            _drawoffset = new Vector2(_rectangle.Width / 2.0f, _rectangle.Height / 2.0f);
            _speed = 2;
            _rotate = false;
            _rotateAngle = 0.0f;
            _scale = 1.0f;
            _columns = 1;
            _rows = 1;
        }

        #endregion

        #region Properties (public)

        public Rectangle CollisionRectangle
        {
            get { return _collisionRectangle; }
            set { _collisionRectangle = value; }
        }

        public Rectangle Rectangle
        {
            get { return _rectangle; }
            set { _rectangle = value; }
        }
        
        public bool HasCollided
        {
            get { return _hasCollided; }
            set { _hasCollided = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public Vector2 DrawOffset
        {
            get { return _drawoffset; }
            set { _drawoffset = value; }
        }

        public bool Rotate
        {
            get { return _rotate; }
            set { _rotate = value; }
        }

        public float RotateAngle
        {
            get { return _rotateAngle; }
            set { _rotateAngle = value; }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public Texture2D Texture 
        {
            get { return _texture; }
            set { _texture = value; } 
        }

        public float Scale
        {
            get { return this._scale; }
            set { _scale = value; }
        }

        public Vector2 SheetSize
        {
            set {
                _columns = (int)value.X; 
                _rows = (int)value.Y;
                _rectangle.Width = Texture.Width / _columns;
                _rectangle.Height = Texture.Height / _rows;
                _drawoffset = new Vector2(_rectangle.Width / 2.0f, _rectangle.Height / 2.0f);
            }
        }

        public int XFrame
        {
            get { return _xframe; }
            set
            {
                _rectangle.X = value * _rectangle.Width;
                _xframe = value;
            }
        }

        public int YFrame
        {
            get { return _yframe; }
            set
            {
                _rectangle.Y = value * _rectangle.Height;
                _yframe = value;
            }
        }

        public int Rows
        {
            get { return this._rows; }
            set { _rows = value; }
        }

        #endregion

        #region Game Loop

        public virtual void Die()
        {
            DeathHelper.KillCell.Add(this);
        }

        public virtual void Bounce(Vector2 bouncePoint, Vector2 bouncerVelocity)
        {
            if (bounced == false)
            {
                Vector2 diff = bouncePoint - Position;          // reflection line
                double theta = Math.Atan(diff.X / diff.Y);      // angle from vertical

                // rotate to vertical
                Vector2 refl = new Vector2((float)(Velocity.X * Math.Cos(theta)) - (float)(Velocity.Y * Math.Sin(theta)),
                                                    (float)(Velocity.X * Math.Sin(theta)) + (float)(Velocity.Y * Math.Cos(theta)));
                // relect in y-axis
                refl = new Vector2(refl.X, -refl.Y);

                // rotate back to original position
                theta = -theta;
                refl = new Vector2((float)(refl.X * Math.Cos(theta)) - (float)(refl.Y * Math.Sin(theta)),
                                                    (float)(refl.X * Math.Sin(theta)) + (float)(refl.Y * Math.Cos(theta)));
                // set Velocity
                Velocity = refl + bouncerVelocity;

                bounced = true;
            }
        }

        public virtual void Update(GameTime gameTime, SpriteBatch bactch)
        {
            // avoid double bounces
            bounceCounter += gameTime.ElapsedGameTime.Milliseconds;

            if (bounceCounter > bounceTime)
            {
                bounced = false;
                bounceCounter = 0;
            }

            //Check not going off the left hand screen.
            if (Position.X <= Rectangle.Width/2.0f * Scale)
            {
                Position = new Vector2(Rectangle.Width / 2.0f * Scale, Position.Y);
                Bounce(new Vector2(10, Position.Y),Vector2.Zero);
            }

            //Check not going off the Top of screen.
            if (Position.Y <= Rectangle.Height / 2.0f * Scale)
            {
                Position = new Vector2(Position.X, (Rectangle.Height / 2.0f) * Scale);
                Bounce(new Vector2(Position.X, 10), Vector2.Zero);
            }

            //Check not going off the right hand screen.
            if (Position.X * ViewPortHelper.XScale >= (ViewPortHelper.X - Rectangle.Width / 2.0 * Scale))
            {
                Position = new Vector2((ViewPortHelper.X / ViewPortHelper.XScale - Rectangle.Width / 2.0f * Scale), Position.Y);
                Bounce(new Vector2(-10, Position.Y), Vector2.Zero);
            }

            //Check not going off the Bottom of screen.
            if (Position.Y * ViewPortHelper.YScale >= (ViewPortHelper.Y - Rectangle.Height / 2.0 * Scale))
            {
                Position = new Vector2(Position.X, (ViewPortHelper.Y / ViewPortHelper.YScale - Rectangle.Height / 2.0f * Scale));
                Bounce(new Vector2(Position.X, -10), Vector2.Zero);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch batch, float layer)
        {
            batch.Draw(_texture, _position - _drawoffset*Scale, _rectangle, Color.White, _rotation, Vector2.Zero, Scale, SpriteEffects.None, layer);
        }

        #endregion
    }
}