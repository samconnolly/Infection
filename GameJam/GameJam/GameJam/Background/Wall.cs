using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam
{
    public class Wall
    {
        private Texture2D _wallTexture;
        private Rectangle _wallRectangle;
        private Rectangle _drawRectangle;
        private Vector2 _position;

        public Wall(int x, int y, int width, int height, int angle)
        {
            _wallRectangle = new Rectangle(x, y, width, height);
            _position = new Vector2(x, y);
        }

        public Rectangle WallRectangle
        {
            get 
            {
                return new Rectangle((int)_position.X, (int)_position.Y, _drawRectangle.Width, _drawRectangle.Height); 
            }
        }

        internal Vector2 CheckCollision(SpriteBase sprite)
        {
            Vector2 newPos = Vector2.Zero;
            if (sprite.CollisionRectangle.Intersects(this.WallRectangle))
            {
                Rectangle rec = this.WallRectangle;

                string side = this.GetIntersectSide(rec, sprite);

                switch(side)
                {
                    case "Right":
                        sprite.Bounce(sprite.Position - new Vector2(-10, 0), Vector2.Zero);
                        break;
                    case "Left":
                        sprite.Bounce(sprite.Position - new Vector2(10, 0), Vector2.Zero);
                        break;
                    case "Top":
                        sprite.Bounce(sprite.Position - new Vector2(0, -10), Vector2.Zero);
                        break;
                    case "Bottom":
                        sprite.Bounce(sprite.Position - new Vector2(0, 10), Vector2.Zero);
                        break;
                }
            }
  
            return newPos;
        }

        private string GetIntersectSide(Rectangle wall, SpriteBase sprite)
        {
            string side = string.Empty;

            //Right collision.
            if (sprite.Position.X > wall.Center.X && sprite.Position.Y > wall.Top + sprite.Texture.Height / 2.0f && sprite.Position.Y < (wall.Bottom - sprite.Texture.Height / 2.0f))
            {
                side = "Right";
            }

            //Left collision.
            if (sprite.Position.X < wall.Center.X && sprite.Position.Y > wall.Top + sprite.Texture.Height/2.0f && sprite.Position.Y < (wall.Bottom - sprite.Texture.Height/2.0f))
            {
                side = "Left";
            }

            //Top collision.
            if (sprite.Position.Y < wall.Center.Y && sprite.Position.X > wall.Left && sprite.Position.X + sprite.Texture.Width / 2.0f < (wall.Right - sprite.Texture.Width / 2.0f))
            {
                side = "Top";
            }

            //Bottom collision.
            if (sprite.Position.Y > wall.Center.Y && sprite.Position.X > wall.Left && sprite.Position.X + sprite.Texture.Width / 2.0f < (wall.Right - sprite.Texture.Width / 2.0f))
            {
                side = "Bottom";
            }

            return side;
        }

        internal void LoadContent(SpriteBatch batch, Game game)
        {
            this._wallTexture = game.Content.Load<Texture2D>("Large Background");
            _drawRectangle = new Rectangle(0, 0, _wallRectangle.Width, _wallRectangle.Height);
        }

        internal void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.Draw(_wallTexture, _position, _drawRectangle, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.7f);
        }
    }
}
