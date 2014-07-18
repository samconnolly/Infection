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
    class RedBloodCellGroup : SpriteBase
    {
        public int count;
        List<RedBloodCell> group;
        Random random = new Random();

        public RedBloodCellGroup(Texture2D texture, Vector2 position, int n)
            : base(texture)
        {
            count = n;
            group = new List<RedBloodCell> { };

            for (int i = 0; i < n; i++)
            {
                Vector2 posvar = new Vector2(50, 50) - new Vector2((float)random.NextDouble() * 100, (float)random.NextDouble() * 100);
                group.Add(new RedBloodCell(texture, position + posvar));

                foreach (RedBloodCell rbc in group)
                {
                    rbc.groupCentre = Position;
                }

            }

        }

        public override void Update(GameTime gameTime, SpriteBatch bactch)
        {
            Vector2 av = Vector2.Zero;
            List<RedBloodCell> deadList = new List<RedBloodCell>{};

            foreach (RedBloodCell rbc in group)
            {
                av += rbc.Position;
            }

            Position = av / 3.0f;

            foreach (RedBloodCell rbc in group)
            {
                rbc.groupCentre = Position;
                rbc.group = group;

                rbc.Update(gameTime, bactch);
                if (rbc.dead == true)
                {
                    deadList.Add(rbc);
                }
            }

            foreach (RedBloodCell rbc in deadList)
            {
                group.Remove(rbc);
                count -= 1;
            }

            if (count <= 0)
            {
                SoundEffectPlayer.PlaySquelch();
                Die();
            }

            base.Update(gameTime, bactch);
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch,float layer)
        {
            int i = 0;

            foreach (RedBloodCell rbc in group)
            {
                rbc.Draw(gameTime, batch, +i * 0.00001f);
                i += 1;
            }

        }
    }
}

