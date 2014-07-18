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
    public class Virus:SpriteBase
    {
        
        //Vector2 Velocity;
        Vector2 acceleration;
        public Vector2 direction;
        float maxSpeed;
        float accelerationMagnitude;
        float deccelerationMagnitude;
        bool accing;
        public int viruslingNo;
        List<Virusling> viruslingList;
        Random random;
        Texture2D miniTex;
        Texture2D tex;

        // power ups

        public bool reproduce = false;
        private int reproductionTime = 5000;
        private int reproductionTimer = 0;
        private int reproductionCount = 0;
        private int reproductionTotal = 15;

        // player
        int player;

        public Virus(Texture2D texture, Texture2D miniTexture, Vector2 position, int Player = 1)
            : base(texture)
        {
            Position = position;
            Rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Velocity = new Vector2(0,0);
            acceleration = new Vector2(0,0);
            maxSpeed = 4.0f;
            accelerationMagnitude = 15.0f;
            deccelerationMagnitude = 4.0f;
            accing = false;
            viruslingNo = 2;
            viruslingList = new List<Virusling> { };
            random = new Random();
            miniTex = miniTexture;
            tex = texture;

            player = Player;

            AddViruslings(viruslingNo);

            

        }

        
        public void AddViruslings(int n)
        {
            for (int x = 0; x < n; x++)
            {
                // create an offset vector outside the virus
                double length = random.NextDouble() + 1.1;
                Vector2 edge = new Vector2(Rectangle.Width / 2.0f,Rectangle.Width / 2.0f);

                Vector2 startOffset = edge * (float) length;

                double theta = random.NextDouble() * Math.PI * 2.0;

                startOffset = new Vector2((float)(startOffset.X * Math.Cos(theta)) - (float)(startOffset.Y * Math.Sin(theta)),
                                        (float)(startOffset.X * Math.Sin(theta)) + (float)(startOffset.Y * Math.Cos(theta)));

                if (player == 1)
                {
                    viruslingList.Add(new Virusling(miniTex, Position - startOffset));
                }

                else if (player == 2)
                {
                    viruslingList.Add(new Virusling(miniTex, Position - startOffset,2));
                }

                viruslingNo += 1;
            }
        }

        public void AddViruslingsHere(int n, Vector2 here)
        {
            for (int x = 0; x < n; x++)
            {
                // create an offset vector outside the virus
                double length = random.NextDouble() + 0.5;
                Vector2 edge = new Vector2(15.0f, 15.0f);

                Vector2 startOffset = edge * (float)length;

                double theta = random.NextDouble() * Math.PI * 2.0;

                startOffset = new Vector2((float)(startOffset.X * Math.Cos(theta)) - (float)(startOffset.Y * Math.Sin(theta)),
                                        (float)(startOffset.X * Math.Sin(theta)) + (float)(startOffset.Y * Math.Cos(theta)));

                viruslingList.Add(new Virusling(miniTex, here - startOffset,player));

                viruslingNo += 1;
            }
        }

        public override void Update(GameTime gameTime, SpriteBatch batch)
        {
            // power ups

            if (reproduce == true)
            {
                reproductionTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (reproductionTimer >= reproductionTime)
                {
                    reproductionTimer = 0;
                    AddViruslings(1);
                    reproductionCount += 1;

                    if (reproductionCount >= reproductionTotal)
                    {
                        reproduce = false;
                    }
                }
            }
               


            acceleration = Vector2.Zero; // reset acc'n

            // check for key presses

            accing = false;

            if (InputHelper.IsButtonDown(Keys.Up) || InputHelper.IsButtonDown(Keys.W))
            {
                acceleration += new Vector2(0, -1);
                accing = true;
            }

            if (InputHelper.IsButtonDown(Keys.Down) || InputHelper.IsButtonDown(Keys.S))
            {
                acceleration += new Vector2(0, 1);
                accing = true;
            }

            if (InputHelper.IsButtonDown(Keys.Left) || InputHelper.IsButtonDown(Keys.A))
            {
                acceleration += new Vector2(-1, 0);
                accing = true;
            }

            if (InputHelper.IsButtonDown(Keys.Right) || InputHelper.IsButtonDown(Keys.D))
            {
                acceleration += new Vector2(1,0 );
                accing = true;
            }

            // check for button presses

            if (player == 1)
            {
                if (InputHelper.LeftThumbstickDirectionP1.Length() > 0)
                {
                    acceleration = new Vector2(InputHelper.LeftThumbstickDirectionP1.X, -InputHelper.LeftThumbstickDirectionP1.Y);
                    accing = true;
                }
            }

            else if (player == 2)
            {
                if (InputHelper.LeftThumbstickDirectionP2.Length() > 0)
                {
                    acceleration = new Vector2(InputHelper.LeftThumbstickDirectionP2.X, -InputHelper.LeftThumbstickDirectionP2.Y);
                    accing = true;
                }
            }

            // slow down if not no key is down
            if (accing == false && Velocity != Vector2.Zero)
            {
                acceleration = -Velocity;
                acceleration.Normalize();
                acceleration *= deccelerationMagnitude;
            }
            
            // accelerate up to full speed if key is down
            if (acceleration != Vector2.Zero && accing)
            {                         
                    acceleration.Normalize();
                    acceleration *= accelerationMagnitude;                
            }

            // adjust Velocity

            Velocity += acceleration*gameTime.ElapsedGameTime.Milliseconds/1000.0f;

            if (Velocity.Length() > maxSpeed)
            {
                Vector2 norm = Velocity;
                norm.Normalize();
                Velocity = norm;
                Velocity *= maxSpeed;
            }            

            // adjust position

            Position += Velocity;

            if (Velocity.Length() > 0.001f)
            {
                direction = Velocity;
                direction.Normalize();
            }

            // ------------ viruslings ---------------------
            
            //if (InputHelper.WasButtonPressed(Keys.V))
            //{
            //    AddViruslings(1);
            //}



            if (player == 1)
            {
                VirusHelper.VirusPosition = Position;
                VirusHelper.VirusVelocity = Velocity;
                VirusHelper.Virus = this;
                VirusHelper.Viruslings = viruslingList;
            }

            else if (player == 2)
            {
                VirusHelper.VirusPositionP2 = Position;
                VirusHelper.VirusVelocityP2 = Velocity;
                VirusHelper.VirusP2 = this;
                VirusHelper.ViruslingsP2 = viruslingList;
            }

            foreach (Virusling virusling in viruslingList)
            {                
                virusling.Update(gameTime, batch);
            }

            base.Update(gameTime, batch);


        }

        public override void Draw(GameTime gameTime, SpriteBatch batch, float layer)
        {
                        
            base.Draw(gameTime, batch, layer);

            foreach (Virusling virusling in viruslingList)
            {
                virusling.Draw(gameTime, batch, layer);
            }
        }

    }
}
