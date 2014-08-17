using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameJam
{
    class RedBloodCell : SpriteBase
    {
        //Vector2 Velocity;
        float speed = 1.0f;
        public Vector2 groupCentre;
        public List<RedBloodCell> group;
        public bool dead = false;

        int hitBy = 1;

        Random random;

        public RedBloodCell(Texture2D texture, Vector2 position)
            : base(texture)
        {
            Position = position;
            Rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            random = new Random((int)(position.X * 1000));

            float x = (float)random.NextDouble();
            float y = (float)Math.Sqrt(1 - Math.Pow(x, 2));

            Velocity = new Vector2(x, y);
        }


        public override void Update(GameTime gameTime, SpriteBatch bactch)
        {
            // infection!
            float distance = (Position - VirusHelper.VirusPosition).Length();

            if (distance < (Rectangle.Width / 2.0f + VirusHelper.Virus.Rectangle.Width / 2.0f) * Scale)
            {
                    hitBy = 1;
                    Die();
                
            }

            ///////////////
            if (InputHelper.Players == 2)
            {
                float distance2 = (Position - VirusHelper.VirusPositionP2).Length();

                if (distance2 < (Rectangle.Width / 2.0f + VirusHelper.VirusP2.Rectangle.Width / 2.0f) * Scale)
                {
                    hitBy = 2;
                    Die();

                }
            }
            ///////////////

            // movement


            if ((groupCentre - Position).Length() > 150)
            {
                Vector2 extra = (groupCentre - Position);
                extra.Normalize();
                Velocity += extra * 1.0f;
                Vector2 norm = Velocity;
                norm.Normalize();
                Velocity = norm;
            }

            if ((new Vector2(ViewPortHelper.X / 2, ViewPortHelper.Y / 2) - Position).Length() > 350)
            {
                Vector2 extra = (new Vector2(ViewPortHelper.X / 2, ViewPortHelper.Y / 2) - Position);
                extra.Normalize();
                Velocity += extra * 1.0f;
                Vector2 norm = Velocity;
                norm.Normalize();
                Velocity = norm;
            }

            Vector2 away = Vector2.Zero;

            foreach (RedBloodCell gbc in group)
            {
                float dist = (Position - gbc.Position).Length();

                if (dist > 0 && dist < 30)
                {
                    Vector2 add = (Position - gbc.Position);
                    add.Normalize();
                    away += add*1.5f;
                }
            }

            if (away.Length() > 0)
            {
                away.Normalize();
            }


            double theta = random.NextDouble() - 0.5;

            Velocity = new Vector2((float)(Velocity.X * Math.Cos(theta)) - (float)(Velocity.Y * Math.Sin(theta)),
                                        (float)(Velocity.X * Math.Sin(theta)) + (float)(Velocity.Y * Math.Cos(theta)));

            Velocity += away;

            Position += Velocity * speed;

            base.Update(gameTime, bactch);
        }

        public override void  Die()
        {
            if (hitBy == 1)
            {
                VirusHelper.Virus.AddViruslingsHere(3, Position);
            }
            else if (hitBy == 2)
            {
                VirusHelper.VirusP2.AddViruslingsHere(3, Position);
            }
            dead = true;
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch, float layer)
        {
            
            base.Draw(gameTime, batch, layer);
            
        }
       
    }
}
