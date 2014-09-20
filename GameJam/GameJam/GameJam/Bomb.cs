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
    public class Bomb : SpriteBase
    {
        private float fallSpeed;
        private Texture2D tex;
        private Texture2D crossTex;
        private Vector2 crossPosition;
        private Rectangle drawRect;
        private float fallOffset = -15;

        private int frate;
        private int timer = 0;

        public Bomb(Texture2D texture,Texture2D CrossTexture, Vector2 target, float speed)
            : base(texture)
        {
            tex = texture;
            crossTex = CrossTexture;
            fallSpeed = speed;

            Texture = tex;
            crossPosition = target;
            drawRect = new Rectangle(0, 0, crossTex.Width, crossTex.Height);

            Position = new Vector2(target.X, 0);
            SheetSize = new Vector2(4, 1);
            Scale = 0.2f;

            frate = ViewPortHelper.FrameRate;
        }

        public override void Update(GameTime gameTime, SpriteBatch bactch)
        {
            // falling
            if (Position.Y < crossPosition.Y + fallOffset)
            {
                Position += new Vector2(0, 1) * fallSpeed;
            }

            // killing
            if ((this.Position - VirusHelper.VirusPosition).Length() < VirusHelper.Virus.width * VirusHelper.Virus.Scale && Position.Y >= crossPosition.Y + fallOffset)
            {
                ScoreHelper.PlayerHit(VirusHelper.Virus);
                DeathHelper.KillCell.Add(this);
            }
            else if (InputHelper.Players == 2)
            {
                if ((this.Position - VirusHelper.VirusPositionP2).Length() < VirusHelper.Virus.width * VirusHelper.Virus.Scale && Position.Y >= crossPosition.Y + fallOffset)
                {
                    ScoreHelper.PlayerHit(VirusHelper.VirusP2);
                    DeathHelper.KillCell.Add(this);
                }
            }

            // anim
            if (Position.Y > crossPosition.Y + fallOffset)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;

                if (timer > frate)
                {                    
                    XFrame += 1;
                    timer = 0;
                    if (XFrame > 3)
                    {
                        DeathHelper.KillCell.Add(this);
                    }
                }
            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch batch, float layer)
        {
            // draw warning
            if (Position.Y < crossPosition.Y + fallOffset)
            {
                batch.Draw(crossTex, crossPosition - DrawOffset * Scale, drawRect, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, layer);
            }

            // draw bomb
            base.Draw(gameTime, batch, layer);
        }
    }
}

