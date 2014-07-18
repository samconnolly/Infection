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
    class PurpleBloodCellGroup:SpriteBase
    {
        public int count;
        public List<PurpleBloodCell> group;
        Random random = new Random();

        public PurpleBloodCellGroup(Texture2D texture, Texture2D hitTexture, Texture2D spawnTexture, Vector2 position, int n)
            : base(texture)
        {
            count = n;
            group = new List<PurpleBloodCell>{};

            for (int i=0; i<n;i++)
            {
                Vector2 posvar = new Vector2(50,50) - new Vector2((float)random.NextDouble()*100,(float)random.NextDouble()*100);
                group.Add(new PurpleBloodCell(texture,hitTexture,spawnTexture, position + posvar));

                foreach (PurpleBloodCell wbc in group)
                {
                    wbc.groupCentre = Position;
                }

            }
            
        }

        public override void Update(GameTime gameTime, SpriteBatch bactch)
        {
            Vector2 av = Vector2.Zero;
            List<PurpleBloodCell> deadList = new List<PurpleBloodCell> { };

            foreach (PurpleBloodCell wbc in group)
            {
                av += wbc.Position;
            }

            Position = av/3.0f;

            foreach (PurpleBloodCell wbc in group)
            {
                wbc.groupCentre = Position;
                wbc.Update(gameTime, bactch);
                if (wbc.dead == true)
                {
                    deadList.Add(wbc);
                }
            }

            foreach (PurpleBloodCell wbc in deadList)
            {
                group.Remove(wbc);
                count -= 1;
            }

            if (count <= 0)
            {
                Die();
            }

            base.Update(gameTime, bactch);
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch, float layer)
        {
            float i = 0;

            foreach (PurpleBloodCell wbc in group)
            {
                wbc.Draw(gameTime, batch, layer + i * 0.00001f);
                i += 1;
            }

        }
    }
}
