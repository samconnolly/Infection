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
        int nspecials = 11;
        private List<int> cells = new List<int> { 8, 8, 14, 10, 8, 8, 13, 1, 1, 18, 1, 1, 1, 1, 11, 12, 19, 17, 16, 9, 15 };
        private List<string> names = new List<string>{"+3","+5","Proliferate", "Double", "-2", "-4", "Reverse", "Orbit Up", 
                                                        "Orbit Down", "Homing", "Radius Up", "Radius Down", "Speed Up","Speed Down", 
                                                            "Health Up", "Invincibility", "Pulse", "Stream", "Laser", "Freeze", "Antidote"};
        private int pickup = 0;
        private Texture2D textTex;

        private bool used = false;
        private int textTimer = 0;
        private int textTime = 1000;
        private Rectangle textRect;
        private List<int> textX = new List<int> { 0, 1, 0, 1, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 0, 0 };
        private List<int> textY = new List<int> { 5, 5, 6, 0, 3, 3, 8, 4, 4, 1, 7, 7, 9, 8, 7, 2, 6, 9, 2, 1, 0 };
        private Vector2 textOffset = Vector2.Zero;

        public PowerUpBase(Texture2D texture, Texture2D specialTexture, Texture2D textTexture, Vector2 position, int Type)
            : base(texture)
        {
            Position = position;
            Rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            type = Type;
            Scale = 0.3f;
            textTex = textTexture;

            if (cells[type - 1] > 8)
            {
                Texture = specialTexture;
                Rectangle = new Rectangle(0, 0, specialTexture.Width/nspecials, specialTexture.Height);
                DrawOffset = new Vector2(Rectangle.Width / 2.0f, Rectangle.Height / 2.0f);
                XFrame = cells[type - 1] - 9;
            }
            else
            {
                SheetSize = new Vector2(9, 1);
                int colour = cells[type-1] + CellsHelper.Colours;
                if (colour >= 9) {colour -= 9;}
                XFrame = colour;
            }

            textRect = new Rectangle(textX[type - 1] * textTex.Width / 2, textY[type - 1] * textTex.Height / 10, textTex.Width / 2, textTex.Height / 10);
        }


        public override void Update(GameTime gameTime, SpriteBatch bactch)
        {
            // collect
            if (used != true)
            {
                float distance = (Position - VirusHelper.VirusPosition).Length();


                if (distance < (Rectangle.Width / 2.0f * Scale + VirusHelper.Virus.Rectangle.Width * VirusHelper.Virus.Scale / 2.0f) && VirusHelper.Virus.dead == false)
                {
                    PowerUp(1);
                    pickup = 1;
                }
                if (InputHelper.Players == 2)
                {
                    float distance2 = (Position - VirusHelper.VirusPositionP2).Length();

                    if (distance2 < (Rectangle.Width / 2.0f * Scale + VirusHelper.VirusP2.Rectangle.Width * VirusHelper.VirusP2.Scale / 2.0f) && VirusHelper.VirusP2.dead == false)
                    {
                        PowerUp(2);
                        pickup = 2;
                    }
                }
            }
            else
            {
                textTimer += gameTime.ElapsedGameTime.Milliseconds;
                textOffset += new Vector2(0, -1);

                if (textTimer >= textTime)
                {
                    Die();
                }
            }

            base.Update(gameTime, bactch);
        }

        public virtual void PowerUp(int player)
        {
            // do one of all the thingies! (by ype)
            if (type == 1)
            {
                PlusThree(player);
            }
            else if (type == 2)
            {
                PlusFive(player);
            }
            else if (type == 3)
            {
                Proliferate(player);
            }
            else if (type == 4)
            {
                Double(player);
            }
            else if (type == 5)
            {
                MinusTwo(player);
            }
            else if (type == 6)
            {
                MinusFour(player);
            }
            else if (type == 7)
            {
                Reverse(player);
            }
            else if (type == 8)
            {
                OrbitUp(player);
            }
            else if (type == 9)
            {
                OrbitDown(player);
            }
            else if (type == 10)
            {
                Homing(player);
            }
            else if (type == 11)
            {
                RadiusUp(player);
            }
            else if (type == 12)
            {
                RadiusDown(player);
            }
            else if (type == 13)
            {
                SpeedUp(player);
            }
            else if (type == 14)
            {
                SpeedDown(player);
            }
            else if (type == 15)
            {
                HealthUp(player);
            }
            else if (type == 16)
            {
                Invincibility(player);
            }
            else if (type == 17)
            {
                Pulse(player);
            }
            else if (type == 18)
            {
                Stream(player);
            }
            else if (type == 19)
            {
                Laser(player);
            }
            else if (type == 20)
            {
                Freeze(player);
            }            
            else
            {
                Antidote(player);
            }

            used = true;
        }

        // ------------------ passive ---------------------

        public void PlusThree(int player)
        {
            if (player == 1)
            {
                VirusHelper.Virus.AddViruslings(3);
            }
            else
            {
                VirusHelper.VirusP2.AddViruslings(3);
            }
        }

        public void PlusFive(int player)
        {
            if (player == 1)
            {
                VirusHelper.Virus.AddViruslings(5);
            }
            else
            {
                VirusHelper.VirusP2.AddViruslings(5);
            }
        }

        public void MinusTwo(int player)
        {
            if (player == 1)
            {
                if (VirusHelper.Viruslings.Count > 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        VirusHelper.Viruslings.RemoveAt(0);
                    }
                }
            }
            else
            {
                if (VirusHelper.ViruslingsP2.Count > 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        VirusHelper.ViruslingsP2.RemoveAt(0);
                    }
                }
            }            
        }

        public void MinusFour(int player)
        {
            if (player == 1)
            {
                if (VirusHelper.Viruslings.Count > 2)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        VirusHelper.Viruslings.RemoveAt(0);
                    }
                }
            }
            else
            {
                if (VirusHelper.ViruslingsP2.Count > 2)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        VirusHelper.ViruslingsP2.RemoveAt(0);
                    }
                }
            } 
        }

        public void Proliferate(int player)
        {
            if (player == 1)
            {
                VirusHelper.Virus.reproduce = true; 
            }
            else
            {
                VirusHelper.VirusP2.reproduce = true; 
            }          
        }

        public void Double(int player)
        {
            if (player == 1)
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
            else
            {
                if (VirusHelper.VirusP2.viruslingNo == 0)
                {
                    VirusHelper.VirusP2.AddViruslings(2);
                }

                else
                {
                    VirusHelper.VirusP2.AddViruslings(VirusHelper.Virus.viruslingNo);
                }
            }  

            
        }

        public void Reverse(int player)
        {
            if (player == 1)
            {
                VirusHelper.Rotation *= -1;
            }
            else
            {
                VirusHelper.RotationP2 *= -1;
            }
        }

        public void OrbitUp(int player)
        {
            if (player == 1)
            {
                VirusHelper.RotationSpeed += 0.02f; // problematic!!
            }
            else
            {
                VirusHelper.RotationSpeedP2 += 0.02f; // problematic!!
            }
        }

        public void OrbitDown(int player)
        {
            if (player == 1)
            {
                VirusHelper.RotationSpeed -= 0.02f; // problematic!!
            }
            else
            {
                VirusHelper.RotationSpeedP2 -= 0.02f; // problematic!!
            }
        }

        public void Homing(int player)
        {
            if (player == 1)
            {
                VirusHelper.Virus.Homing();
            }
            else
            {
                VirusHelper.VirusP2.Homing();
            }
        }

        public void RadiusUp(int player)
        {
            if (player == 1)
            {
                VirusHelper.Radius1 += 10f;
            }
            else
            {
                VirusHelper.Radius1P2 += 10f;
            }
        }

        public void RadiusDown(int player)
        {
            if (player == 1)
            {
                VirusHelper.Radius1 -= 10f;
            }
            else
            {
                VirusHelper.Radius1P2 -= 10f;
            }
        }

        public void SpeedUp(int player)
        {
            if (player == 1)
            {
                VirusHelper.Virus.maxSpeed += 1;
            }
            else
            {
                VirusHelper.VirusP2.maxSpeed += 1;
            }
        }

        public void SpeedDown(int player)
        {
            if (player == 1)
            {
                VirusHelper.Virus.maxSpeed -= 1;
            }
            else
            {
                VirusHelper.VirusP2.maxSpeed -= 1;
            }
        }

        public void HealthUp(int player)
        {
            if (player == 1)
            {
                ScoreHelper.Lives += 1;
            }
            else
            {
                ScoreHelper.LivesP2 += 1;
            }
        }

        // ------------------ active ------------------

        public void Invincibility(int player)
        {
            if (player == 1)
            {
                VirusHelper.Virus.activePowerup = 1;
            }
            else
            {
                VirusHelper.VirusP2.activePowerup = 1;
            }
        }

        public void Pulse(int player)
        {
            if (player == 1)
            {
                VirusHelper.Virus.activePowerup = 2;
            }
            else
            {
                VirusHelper.VirusP2.activePowerup = 2;
            }
        }

        public void Stream(int player)
        {
            if (player == 1)
            {
                VirusHelper.Virus.activePowerup = 3;
            }
            else
            {
                VirusHelper.VirusP2.activePowerup = 3;
            }
        }

        public void Laser(int player)
        {
            if (player == 1)
            {
                VirusHelper.Virus.activePowerup = 4;
            }
            else
            {
                VirusHelper.VirusP2.activePowerup = 4;
            }
        }

        public void Freeze(int player)
        {
            if (player == 1)
            {
                VirusHelper.Virus.activePowerup = 5;
            }
            else
            {
                VirusHelper.VirusP2.activePowerup = 5;
            }
        }

        public void Antidote(int player)
        {
            if (player == 1)
            {
                VirusHelper.Virus.activePowerup = 6;
            }
            else
            {
                VirusHelper.VirusP2.activePowerup = 6;
            }
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
            //batch.DrawString(FontHelper.Fonts[2], names[type-1],Position + new Vector2(0, 0), Color.White);
            //
            if (pickup != 0)
            {
                if (pickup == 1)
                {
                    batch.Draw(textTex, new Vector2(850, 630) + textOffset, textRect, Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, layer);
                }
                else
                {
                    batch.Draw(textTex, new Vector2(850, 630) + textOffset, textRect, Color.Green, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, layer);
                }
            }
            if (used == false)
            {
                base.Draw(gameTime, batch, layer);
            }
        }

    }
}

