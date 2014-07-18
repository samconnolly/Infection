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
    class HealthBar
    {
        private Texture2D dummyTexture;
        public Vector2 position;

        public int init;

        private Color barColour;
        private Color boxColour;

        private Rectangle box;
        private Rectangle bar;
        private int fullWidth;

        public float value;
        public int width = 50;
        public int height = 10;
               
        public HealthBar(GraphicsDevice graphicsDevice, Vector2 position, int health)
        {
            this.init = health;
            this.value = health;

            dummyTexture = new Texture2D(graphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.Gray });

            box = new Rectangle();
            bar = new Rectangle();

            this.position = position;

            this.barColour = Color.LimeGreen;
            this.boxColour = Color.Gray;

            this.box.Width = width;
            this.box.Height = height;

            this.fullWidth = (int)(width - 4);

            this.bar.Height = (int)(height - 4);
            this.bar.Width = (int)(fullWidth * (value / (float)init));

           
        }

        public void Add(int n)
        {
            value += n;
        }

        public void Subtract(int n)
        {
            value -= n;
        }

        public void Update()
        {
           

            if (value > init)
            {
                value = init;               
            }


            bar.Width = (int)(fullWidth * (value / (float)init));           
        }

        public void Draw(SpriteBatch sbatch)
        {           

            // outline
            sbatch.Draw(dummyTexture, position, box, boxColour, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.115f);

            // bar
            sbatch.Draw(dummyTexture, position + new Vector2(2, 2), bar, barColour, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.11f);

        }

    }
}