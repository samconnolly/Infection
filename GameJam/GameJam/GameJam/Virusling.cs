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
    public class Virusling : SpriteBase
    {
                
        Vector2 acceleration;
        
        float accelerationMagnitude;
    
        public Vector2 target;

        float radius;
        float err;
        Random random;

        int damageCounter = 0;
        int damageTime = 0;
        public bool damaging = true;

        int shootTimer = 0;
        int shootTime = 100;
        bool shoot = false;

        int blastTimer = 0;
        int blastTime = 100;
        bool blast = false;

        public int player = 1;

        public Virusling(Texture2D texture, Vector2 startPosition, int Player = 1)
            : base(texture)
        {
            Position = startPosition;
            Rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            
            acceleration = new Vector2(0, 0);
            target = new Vector2(300, 300);

            accelerationMagnitude = 0.08f;

            radius = 40.0f;
            random = new Random();

            err = (1600 - random.Next(800)) * 0.001f;

            Velocity = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f);

            player = Player;
        }        


        public override void Update(GameTime gameTime, SpriteBatch batch)
        {            

            // avoid too much damage

            if (damaging == false)
            {
                damageCounter += gameTime.ElapsedGameTime.Milliseconds;

                if (damageCounter >= damageTime)
                {
                    damageCounter = 0;
                    damaging = true;
                }
            }

            // movement

            if (player == 1)
            {
                target = VirusHelper.VirusPosition;
            }

            else if (player == 2)
            {
                target = VirusHelper.VirusPositionP2;
            }
            //target = new Vector2(ViewPortHelper.X/2.0f, ViewPortHelper.Y/2.0f);


            Vector2 diff = (target - Position);

            acceleration = Vector2.Zero;

            if (diff.Length() > radius)
            {
                Velocity = 0.99f * Velocity;
            }

            if (diff.Length() <= radius)
            {
                Velocity = new Vector2((float)(diff.X * Math.Cos(Math.PI / 2.0)) - (float)(diff.Y * Math.Sin(Math.PI / 2.0)),
                                            (float)(diff.X * Math.Sin(Math.PI / 2.0)) + (float)(diff.Y * Math.Cos(Math.PI / 2.0))) * 0.05f * err;               
            }

            else if (diff.Length() < radius * 2.0f)
            {
                acceleration = -0.5f*Velocity ;
                acceleration += diff;
                acceleration.Normalize();
                acceleration *= accelerationMagnitude * 5.0f;

                Velocity += acceleration;
            }

            else if (diff.Length() > 100)// && diff.Length() < 200)
            {
                acceleration = -Velocity ;
                acceleration += diff;
                acceleration.Normalize();
                acceleration *= accelerationMagnitude * 3.0f;

                Velocity += acceleration;
            }

            //else if (diff.Length() > 200)
            //{
            //    acceleration = -Velocity;
            //    acceleration += diff;
            //    acceleration.Normalize() ;
            //    acceleration *= accelerationMagnitude * 2.0f;

            //    Velocity += acceleration;
            //}

            if ((diff).Length() <= (VirusHelper.Virus.Rectangle.Width / 2.0f) * Scale && blast == false)
            {
                if (player == 1)
                {
                    Velocity = VirusHelper.VirusVelocity;
                    Velocity -= diff * 0.02f;
                }
                else if (player == 2)
                {
                    Velocity = VirusHelper.VirusVelocityP2;
                    Velocity -= diff * 0.02f;
                }

            }

            //-- outwards ---
            if (player == 1)
            {
                if ((InputHelper.WasButtonPressed(Keys.Q) || InputHelper.WasPadButtonPressedP1(Buttons.B)) && diff.Length() < 100f)
                {
                    blast = true;
                    blastTimer = 0;
                }
            }
            else if (player == 2)
            {
                if ((InputHelper.WasButtonPressed(Keys.Q) || InputHelper.WasPadButtonPressedP2(Buttons.B)) && diff.Length() < 100f)
                {
                    blast = true;
                    blastTimer = 0;
                }
            }

            if (blast == true && (blastTimer < blastTime | diff.Length() <= radius))
            {
                blastTimer += gameTime.ElapsedGameTime.Milliseconds;
          
                
                Vector2 dir = diff;
                dir.Normalize();
                               
                Velocity = -dir * 15; 
            }

            else
            {
                blast = false;
            }
            
            //--- directionwise ---
            if (player == 1)
            {
                if ((InputHelper.WasButtonPressed(Keys.E) || InputHelper.WasPadButtonPressedP1(Buttons.A)) && diff.Length() < 100f)
                {
                    shoot = true;
                    shootTimer = 0;
                }
            }
            else if (player == 2)
            {
                if ((InputHelper.WasButtonPressed(Keys.E) || InputHelper.WasPadButtonPressedP2(Buttons.A)) && diff.Length() < 100f)
                {
                    shoot = true;
                    shootTimer = 0;
                }
            }

            if (shoot == true && (shootTimer < shootTime | diff.Length() <= radius))
            {
                shootTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (player == 1)
                {
                    if (VirusHelper.VirusVelocity.Length() > 0.1f)
                    {
                        Velocity = VirusHelper.Virus.direction * 15;
                    }
                    else
                    {
                        blast = false;
                    }
                }

                else if (player == 2)
                {
                    if (VirusHelper.VirusVelocityP2.Length() > 0.1f)
                    {
                        Velocity = VirusHelper.VirusP2.direction * 15;
                    }
                    else
                    {
                        blast = false;
                    }
                }
                                
            }

            else
            {
                shoot = false;
            }

            // getting hit by purple's energy blast

            foreach (var group in CellsHelper.Cells)
            {
                if (group is PurpleBloodCellGroup)
                {
                    PurpleBloodCellGroup cellGroup = group as PurpleBloodCellGroup;

                    foreach (PurpleBloodCell cell in cellGroup.group)
                    {
                        if (cell.firing == true)
                        {
                            if ((Position - cell.Position).Length() < cell.circle.radius)
                            {
                                Vector2 bounceDir = (Position - cell.Position);
                                bounceDir.Normalize();
                                Velocity += bounceDir*10;
                                //Bounce(bounceDir, Vector2.Zero);
                            }
                        }
                    }
                }
            }

            Position += Velocity;
            


            base.Update(gameTime, batch);
        }
    }
}