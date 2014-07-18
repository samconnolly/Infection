﻿using System;
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
    class BlueBloodCellGroup:SpriteBase
    {
        public int count;
        List<BlueBloodCell> group;
        Random random = new Random();

        public BlueBloodCellGroup(Texture2D texture, Texture2D hitTexture, Texture2D spawnTexture,Texture2D missileTexture, Vector2 position, int n)
            : base(texture)
        {
            count = n;
            group = new List<BlueBloodCell>{};

            for (int i=0; i<n;i++)
            {
                Vector2 posvar = new Vector2(50,50) - new Vector2((float)random.NextDouble()*100,(float)random.NextDouble()*100);
                group.Add(new BlueBloodCell(texture,hitTexture,spawnTexture,missileTexture, position + posvar));

                foreach (BlueBloodCell wbc in group)
                {
                    wbc.groupCentre = Position;
                }

            }

        }

        public override void Update(GameTime gameTime, SpriteBatch bactch)
        {
            Vector2 av = Vector2.Zero;
            List<BlueBloodCell> deadList = new List<BlueBloodCell> { };

            foreach (BlueBloodCell wbc in group)
            {
                av += wbc.Position;
            }

            Position = av/3.0f;

            foreach (BlueBloodCell wbc in group)
            {
                wbc.groupCentre = Position;
                wbc.Update(gameTime, bactch);
                if (wbc.dead == true)
                {
                    deadList.Add(wbc);
                }
            }

            foreach (BlueBloodCell wbc in deadList)
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
            int i = 0;

            foreach (BlueBloodCell wbc in group)
            {
                wbc.Draw(gameTime, batch, +i * 0.00001f);
                i += 1;
            }

        }
    }
}
