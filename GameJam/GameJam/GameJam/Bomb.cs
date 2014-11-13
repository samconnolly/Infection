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
        private bool red = false;
        private int redTime = 200;
        private int redTimer = 0;
        private Vector2 start;
        private bool up = true;
        private Vector2 offset;
        private Vector2 crossOffset;

        public bool hit = false;

        private int frate;
        private int timer = 0;

        private bool splash = false;

        public Bomb(Texture2D texture,Texture2D CrossTexture,Vector2 startPos, Vector2 target, float speed)
            : base(texture)
        {
            tex = texture;
            crossTex = CrossTexture;
            start = startPos;
            fallSpeed = speed;

            Texture = tex;
            crossPosition = target;
            drawRect = new Rectangle(0, 0, crossTex.Width, crossTex.Height);

            Position = startPos;
            SheetSize = new Vector2(4, 1);
            Scale = 0.4f;

            frate = ViewPortHelper.FrameRate;
            offset = DrawOffset;
            crossOffset = DrawOffset;

        }

        public override void Update(GameTime gameTime, SpriteBatch bactch)
        {
            if (up == true)
            {
                Position -= new Vector2(0, 1) * fallSpeed;

                if (Position.Y < -30)
                {
                    Position = new Vector2(crossPosition.X, 0);
                    up = false;
                }
            }

            if (up == false)
            {
                // falling
                if (Position.Y < crossPosition.Y + fallOffset)
                {
                    Position += new Vector2(0, 1) * fallSpeed;
                }
            }
           

            

            // killing
            if (Position.Y > crossPosition.Y + fallOffset && up == false)
            {
                if ((this.Position - VirusHelper.VirusPosition).Length() < VirusHelper.Virus.width * VirusHelper.Virus.Scale && Position.Y >= crossPosition.Y + fallOffset)
                {
                    ScoreHelper.PlayerHit(VirusHelper.Virus);
                    DeathHelper.KillCell.Add(this);
                    hit = true;
                }
                else if (InputHelper.Players == 2)
                {
                    if ((this.Position - VirusHelper.VirusPositionP2).Length() < VirusHelper.Virus.width * VirusHelper.Virus.Scale && Position.Y >= crossPosition.Y + fallOffset)
                    {
                        ScoreHelper.PlayerHit(VirusHelper.VirusP2);
                        DeathHelper.KillCell.Add(this);
                        hit = true;
                    }
                }
            }

            // anim
            if (Position.Y > crossPosition.Y + fallOffset && up == false)
            {
                if (splash == false)
                {
                    SoundEffectPlayer.PlaySplash();
                    splash = true;
                }

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

            // flashing
            redTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (redTimer > redTime)
            {
                redTimer = 0;
                if (red == true)
                {
                    red = false;
                }
                else
                {
                    red = true;
                }
            }            

        }

        public override void Draw(GameTime gameTime, SpriteBatch batch, float layer)
        {
            // draw warning
            if (Position.Y < crossPosition.Y + fallOffset && up == false)
            {
                if (red == true)
                {
                    batch.Draw(crossTex, crossPosition - crossOffset * Scale, drawRect, Color.Red, 0, Vector2.Zero, Scale, SpriteEffects.None, layer);
                }
                else
                {
                    batch.Draw(crossTex, crossPosition - crossOffset * Scale, drawRect, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, layer);
                }
            }

            if (up == true)
            {
                Rotation = (float)Math.PI;
                DrawOffset = -offset;
            }
            else
            {
                Rotation = 0;
                DrawOffset = offset;
            }

            // draw bomb
            base.Draw(gameTime, batch, layer);
        }
    }
}

