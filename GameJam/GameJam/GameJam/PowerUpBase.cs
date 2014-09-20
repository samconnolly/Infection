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
        int type;
        private List<int> cells = new List<int> { 8, 8, 0, 2, 8, 8, 3, 1, 1, 4, 1, 1, 1, 1, 0, 5, 6, 7, 5, 6, 7 };
        private List<string> names = new List<string>{"+3","+5","Proliferate", "Double", "-2", "-4", "Reverse", "Orbit Up", 
                                                        "Orbit Down", "Homing", "Radius Up", "Radius Down", "Speed Up","Speed Down", 
                                                            "Health Up", "Invincibility", "Pulse", "Stream", "Laser", "Freeze", "Antidote"};

        public PowerUpBase(Texture2D texture, Texture2D heartTexture, Vector2 position, int Type)
            : base(texture)
        {
            Position = position;
            Rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            type = Type;
            Scale = 0.3f;

            if (type == 15)
            {
                Texture = heartTexture;
                Rectangle = new Rectangle(0, 0, heartTexture.Width, heartTexture.Height);
                DrawOffset = new Vector2(Rectangle.Width / 2.0f, Rectangle.Height / 2.0f);
            }
            else
            {
                SheetSize = new Vector2(9, 1);
                XFrame = cells[type-1];
            }
        }


        public override void Update(GameTime gameTime, SpriteBatch bactch)
        {
            // collected
            float distance = (Position - VirusHelper.VirusPosition).Length();

            if (distance < (Rectangle.Width / 2.0f * Scale + VirusHelper.Virus.Rectangle.Width * VirusHelper.Virus.Scale / 2.0f))
            {
                PowerUp();                
            }            

            base.Update(gameTime, bactch);
        }

        public virtual void PowerUp()
        {
            // do one of all the thingies! (by ype)
            if (type == 1)
            {
                PlusThree();
            }
            else if (type == 2)
            {
                PlusFive();
            }
            else if (type == 3)
            {
                Proliferate();
            }
            else if (type == 4)
            {
                Double();
            }
            else if (type == 5)
            {
                MinusTwo();
            }
            else if (type == 6)
            {
                MinusFour();
            }
            else if (type == 7)
            {
                Reverse();
            }
            else if (type == 8)
            {
                OrbitUp();
            }
            else if (type == 9)
            {
                OrbitDown();
            }
            else if (type == 10)
            {
                Homing();
            }
            else if (type == 11)
            {
                RadiusUp();
            }
            else if (type == 12)
            {
                RadiusDown();
            }
            else if (type == 13)
            {
                SpeedUp();
            }
            else if (type == 14)
            {
                SpeedDown();
            }
            else if (type == 15)
            {
                HealthUp();
            }
            else if (type == 16)
            {
                Invincibility();
            }
            else if (type == 17)
            {
                Pulse();
            }
            else if (type == 18)
            {
                Stream();
            }
            else if (type == 19)
            {
                Laser();
            }
            else if (type == 20)
            {
                Freeze();
            }            
            else
            {
                Antidote();
            }

            Die();
        }

        // ------------------ passive ---------------------

        public void PlusThree()
        {
            VirusHelper.Virus.AddViruslings(3);            
        }

        public void PlusFive()
        {
            VirusHelper.Virus.AddViruslings(5);
        }

        public void MinusTwo()
        {
            if (VirusHelper.Viruslings.Count > 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    VirusHelper.Viruslings.RemoveAt(0);
                }
            }
        }

        public void MinusFour()
        {
            if (VirusHelper.Viruslings.Count > 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    VirusHelper.Viruslings.RemoveAt(0);
                }
            }
        }

        public void Proliferate()
        {
            VirusHelper.Virus.reproduce = true;           
        }

        public void Double()
        {
            if (VirusHelper.Virus.viruslingNo == 0)
            {
                VirusHelper.Virus.AddViruslings(2);
            }

            else
            {
                VirusHelper.Virus.AddViruslings(VirusHelper.Virus.viruslingNo);
            }
        }

        public void Reverse()
        {
            VirusHelper.Rotation *= -1;
        }

        public void OrbitUp()
        {
            VirusHelper.RotationSpeed += 0.02f; // problematic!!
            //VirusHelper.InnerAccn -= 0.05f;
        }

        public void OrbitDown()
        {
            VirusHelper.RotationSpeed -= 0.02f; // problematic!!
        }

        public void Homing()
        {
            VirusHelper.Virus.Homing();
        }

        public void RadiusUp()
        {
            VirusHelper.Radius1 += 10f;
        }

        public void RadiusDown()
        {
            VirusHelper.Radius1 -= 10f;
        }

        public void SpeedUp()
        {
            VirusHelper.Virus.maxSpeed += 1;
        }

        public void SpeedDown()
        {
            VirusHelper.Virus.maxSpeed -= 1;
        }

        public void HealthUp()
        {
            ScoreHelper.Lives += 1;
        }

        // ------------------ active ------------------

        public void Invincibility()
        {
            VirusHelper.Virus.activePowerup = 1;
        }

        public void Pulse()
        {
            VirusHelper.Virus.activePowerup = 2;
        }

        public void Stream()
        {
            VirusHelper.Virus.activePowerup = 3;
        }

        public void Laser()
        {
            VirusHelper.Virus.activePowerup = 4;
        }

        public void Freeze()
        {
            VirusHelper.Virus.activePowerup = 5;
        }

        public void Antidote()
        {
            VirusHelper.Virus.activePowerup = 6;
        }

        // --- get used -----

        public override void Die()
        {
            DeathHelper.UsedItems.Add(this);        
        }

        // ------------------ Draw ------------------

        public override void Draw(GameTime gameTime, SpriteBatch batch, float layer)
        {            
            // testing!

            batch.DrawString(FontHelper.Fonts[2], names[type-1],Position + new Vector2(0, 0), Color.White);

           base.Draw(gameTime, batch, layer);           
        }

    }
}

