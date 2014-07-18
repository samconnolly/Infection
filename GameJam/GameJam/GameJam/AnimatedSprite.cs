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
    public class AnimatedSprite : SpriteBase
    {
        float timer = 0f;
        float interval = 200f;
        int currentFrame = 1;
        int spriteWidth;
        int spriteHeight;
        int totalFrames;

        public AnimatedSprite(Texture2D texture, int frames, int currentFrame, float interval, int spriteWidth, int spriteHeight)
            : base(texture)
        {
            this.interval = interval;
            this.currentFrame = currentFrame;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.totalFrames = frames;
        }

        public virtual void Animate(GameTime gameTime)
        {
            //Default sprite sheet rotation.
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                currentFrame++;

                if (currentFrame == totalFrames)
                {
                   currentFrame = 1;
                }
                timer = 0f;
            }
        }

        public override void Update(GameTime gameTime, SpriteBatch bactch)
        {
            this.Rectangle = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            this.Animate(gameTime);
        }
    }
}
