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
    class AntiViralNodule : SpriteBase
    {
        int hitPoints = 20;
        float dieChance = 1.0f;
               
        Random random;
        
        List<Virusling> deathList = new List<Virusling> { };
        Virusling deadSpore = null;

        HealthBar health;

        public AntiViralNodule(Texture2D texture, Vector2 position)
            : base(texture)
        {
            Position = position;
            Rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            random = new Random();

            health = new HealthBar( Position + new Vector2(20, -20), hitPoints);
        }


        public override void Update(GameTime gameTime, SpriteBatch bactch)
        {
            // getting hit by spores

            deathList = new List<Virusling> { };
            deadSpore = null;

            foreach (Virusling v in VirusHelper.Viruslings)
            {


                if ((v.Position - this.Position).Length() < (Rectangle.Height / 2.0f + v.Rectangle.Width / 2.0f) * Scale)
                {
                    deadSpore = Hit(v);
                }

                if (deadSpore != null)
                {
                    deathList.Add(deadSpore);
                }
            }

            foreach (Virusling v in deathList)
            {
                VirusHelper.Viruslings.Remove(v);
                VirusHelper.Virus.viruslingNo -= 1;
            }

            if (hitPoints <= 0)
            {
                Die();
            }

            health.Update();

            base.Update(gameTime, bactch);
        }



        public override void Draw(GameTime gameTime, SpriteBatch batch, float layer)
        {
            health.Draw(batch);
            base.Draw(gameTime, batch, layer);

        }

        public Virusling Hit(Virusling v)
        {
            v.Bounce(Position, Velocity);

            if (v.damaging == true)
            {
                v.damaging = false;

                hitPoints -= 1;
                health.Subtract(1);
            }

            if (random.NextDouble() < dieChance)
            {
                return v;
            }

            else
            {
                return null;
            }
        }
    }
}
