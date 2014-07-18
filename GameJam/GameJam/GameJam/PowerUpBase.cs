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
    class PowerUpBase : SpriteBase
    {

        public PowerUpBase(Texture2D texture, Vector2 position)
            : base(texture)
        {
            Position = position;
            Rectangle = new Rectangle(0, 0, texture.Width, texture.Height);           
        }


        public override void Update(GameTime gameTime, SpriteBatch bactch)
        {
            // collected
            float distance = (Position - VirusHelper.VirusPosition).Length();

            if (distance < (Rectangle.Width / 2.0f + VirusHelper.Virus.Rectangle.Width / 2.0f) * Scale)
            {
                PowerUp();                
            }            

            base.Update(gameTime, bactch);
        }

        public virtual void PowerUp()
        {
            Die();
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch, float layer)
        {            
           base.Draw(gameTime, batch, layer);           
        }

    }
}

