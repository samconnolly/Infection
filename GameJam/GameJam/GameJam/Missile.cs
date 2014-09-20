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
    public class Missile:SpriteBase
    {
        private float speed;
        private Texture2D tex;
       
        public Missile(Texture2D texture,Vector2 position, Vector2 target, float moveSpeed)
            : base(texture)
        {
            tex = texture;

            speed = moveSpeed;
            Vector2 Velocity = target - position;
            Velocity.Normalize();
            Velocity *= speed;
            this.Velocity = Velocity;
            this.Position = position;
            SheetSize = new Vector2(4, 1);
            Scale = 0.2f;
            XFrame = 1;
            if (Velocity.Y < 0)
            {
                Rotation = (float)Math.Atan((double)(-Velocity.X / Velocity.Y));
            }
            else
            {
                Rotation = (float)Math.Atan((double)(-Velocity.X / Velocity.Y)) + (float)Math.PI;
            }
        }

        public override void Update(GameTime gameTime, SpriteBatch bactch)
        {


            this.Position += this.Velocity;

            //Check if going off the screen.
            if (Position.X <= Rectangle.Width / 2.0f * Scale |
                    Position.Y <= Rectangle.Height / 2.0f * Scale |
                        Position.X >= (ViewPortHelper.X - tex.Width / 2.0 * Scale) |
                            Position.Y >= (ViewPortHelper.Y - tex.Height / 2.0 * Scale))
            {
                DeathHelper.KillCell.Add(this);
            }


            else if ((this.Position - VirusHelper.VirusPosition).Length() < VirusHelper.Virus.width * VirusHelper.Virus.Scale)
            {
                ScoreHelper.PlayerHit(VirusHelper.Virus);
                DeathHelper.KillCell.Add(this);
            }
            else if (InputHelper.Players == 2)
            {
                if ((this.Position - VirusHelper.VirusPositionP2).Length() < VirusHelper.Virus.width * VirusHelper.Virus.Scale)
                {
                    ScoreHelper.PlayerHit(VirusHelper.VirusP2);
                    DeathHelper.KillCell.Add(this);
                }
            }

        }
    }
}
