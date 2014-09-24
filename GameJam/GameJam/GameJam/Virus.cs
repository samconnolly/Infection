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
        public float maxSpeed;
        float accelerationMagnitude;
        float deccelerationMagnitude;
        bool accing;
        public int viruslingNo;
        List<Virusling> viruslingList;
        public List<Virusling> deathList;

        Random random;
        Texture2D miniTex;
        Texture2D tex;
        Texture2D eyeTex;
        Texture2D powerupTex;
        Rectangle powerupRect;
        List<int> powerupList = new List<int> { 3, 10, 8, 7, 0, 6 };
        bool powerupActive = false;
        bool powerupTiming = false;
        int powerupTimer = 0;
        int powerupTime = 500;

        // power ups

        public int activePowerup = 0;

        public bool reproduce = false;
        private int reproductionTime = 5000;
        private int reproductionTimer = 0;
        private int reproductionCount = 0;
        private int reproductionTotal = 10;
        
        public bool invincible = false;
        public int invinceTimer = 0;
        public int invinceTime = 20000;

        public bool homing = false;
        public int homingTimer = 0;
        public int homingTime = 20000;

        private int freezeTimer = 0;
        private int freezeTime = 3000;

        public bool laser = false;
        private int laserTimer = 0;
        private int laserTime = 500;
        private Texture2D laserTex;
        public Rectangle laserRect;
        public float laserRot = 0.0f;
        public Vector2 laserOffset;

        // player
        int player;

        //test circles
        
        Circle circle1;
        Circle circle2;
        Circle circle3;
        Circle circle4;
        bool circles = true;

        // frames
        private int dir = 0;
        private int directions = 3;
        private int colour = 4;
        private int colours = 7;
        public int width;
        public int height;
        
        private int eyeColours = 6;
        public int eyeColour = 0;
        private Rectangle eyeRect;
        private int eyeWidth;
        private int eyeHeight;
        private List<Vector2> eyeOffset = new List<Vector2> { new Vector2(-0, -235), new Vector2(-230, -235), new Vector2(-125, -235) };
        
        public Virus(Texture2D texture, Texture2D miniTexture,Texture2D eyeTexture, Texture2D laserTexture, Texture2D powerupTexture, Vector2 position, int Player = 1)
            : base(texture)
        {
            Scale = 0.1f;

            Position = position;
            width = texture.Width/directions;
            height = texture.Height/colours;
            SheetSize = new Vector2(directions, colours);
            Velocity = new Vector2(0,0);
            acceleration = new Vector2(0,0);
            maxSpeed = 4.0f;
            accelerationMagnitude = 15.0f;
            deccelerationMagnitude = 4.0f;
            accing = false;
            viruslingNo = 2;
            viruslingList = new List<Virusling> { };
            deathList = new List<Virusling> { };
            random = new Random();
            miniTex = miniTexture;
            tex = texture;
            powerupTex = powerupTexture;
            powerupRect = new Rectangle(0, 0, powerupTex.Width / 11, powerupTex.Height);

            laserTex = laserTexture;
            laserRect = new Rectangle(0, 0, laserTex.Width, laserTex.Height);

            eyeTex = eyeTexture;
            eyeWidth = eyeTex.Width / directions;
            eyeHeight= eyeTex.Height / eyeColours;            
            eyeRect = new Rectangle(0, 0, eyeWidth,eyeHeight);

            player = Player;

            AddViruslings(viruslingNo);

            circle1 = new Circle(this.Position, VirusHelper.radius1, 2, Color.Red);
            circle2 = new Circle(this.Position, VirusHelper.radius2, 2, Color.Red);
            circle3 = new Circle(this.Position, VirusHelper.radius3, 2, Color.Red);
            circle4 = new Circle(new Vector2(730,60), 34, 2, Color.White);

            if (Velocity.Y < 0)
            {
                laserRot = (float)Math.Atan((double)(-Velocity.X / Velocity.Y));
            }
            else
            {
                laserRot = (float)Math.Atan((double)(-Velocity.X / Velocity.Y)) + (float)Math.PI;
            }

        }

        public void Invincible()
        {
            invincible = true;
            invinceTimer = 0;
        }

        public void Homing()
        {
            homing = true;
            homingTimer = 0;
        }

        public void AddViruslings(int n)
        {
            for (int x = 0; x < n; x++)
            {
                // create an offset vector outside the virus
                double length = random.NextDouble() + 1.1;
                Vector2 edge = new Vector2(Rectangle.Width * Scale / 2.0f, Rectangle.Width * Scale / 2.0f);

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
            // ----- power ups -------

            // -- passive --

            // repoduce
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
            
            // -- active --

            // button presses 

            if (player == 1)
            {
                if (((InputHelper.WasButtonPressed(Keys.Space) && InputHelper.Keys == player) ||
                        (InputHelper.WasPadButtonPressedP1(Buttons.A) && InputHelper.Keys != player)))
                {
                    // invincibility
                    if (activePowerup == 1)
                    {
                        Invincible();
                    }

                    // pulse
                    if (activePowerup == 2)
                    {
                        powerupActive = true;
                        powerupTiming = true;

                        foreach (Virusling v in viruslingList)
                        {
                            v.Pulse();
                        }
                    }

                    // stream
                    if (activePowerup == 3)
                    {
                        powerupActive = true;
                        powerupTiming = true;

                        foreach (Virusling v in viruslingList)
                        {
                            v.Stream();
                        }
                    }

                    // laser
                    if (activePowerup == 4)
                    {
                        laser = true;
                        laserTimer = 0;
                    }

                    // freeze
                    if (activePowerup == 5)
                    {
                        CellsHelper.Freeze = true;
                        freezeTimer = 0;
                    }

                    // antidote
                    if (activePowerup == 6)
                    {
                        powerupActive = true;
                        powerupTiming = true;

                        CellsHelper.Antidote = true;
                    }
                }
            }
            else if (player == 2)
            {
                if (((InputHelper.WasButtonPressed(Keys.Space) && InputHelper.Keys == player) ||
                        (InputHelper.WasPadButtonPressedP1(Buttons.A) && InputHelper.Keys != player)))
                {
                    // invincibility
                    if (activePowerup == 1)
                    {
                        Invincible();
                    }

                    // pulse
                    if (activePowerup == 2)
                    {
                        foreach (Virusling v in viruslingList)
                        {
                            v.Pulse();

                        }
                    }

                    // stream
                    if (activePowerup == 3)
                    {
                        foreach (Virusling v in viruslingList)
                        {
                            v.Stream();
                        }
                    }

                    // laser
                    if (activePowerup == 4)
                    {
                        laser = true;
                        laserTimer = 0;
                    }

                    // freeze
                    if (activePowerup == 5)
                    {
                        CellsHelper.Freeze = true;
                        freezeTimer = 0;
                    }

                    // antidote
                    if (activePowerup == 6)
                    {
                        CellsHelper.Cells = new List<SpriteBase> { };
                    }
                }
            }

            // invincible

            if (invincible == true)
            {
                invinceTimer += gameTime.ElapsedGameTime.Milliseconds;
                powerupActive = true;

                if (invinceTimer > invinceTime)
                {
                    invincible = false;
                    powerupActive = false;
                }
            }

            // homing

            if (homing == true)
            {
                homingTimer += gameTime.ElapsedGameTime.Milliseconds;
                
                if (homingTimer > homingTime)
                {
                    homing = false;
                }
            }

            // -- laser --
                
            if (laser == true)
            {
                // rotation
                if (Velocity.Length() > 0.05f)
                {
                    if (Velocity.Y < 0)
                    {
                        laserRot = (float)Math.Atan((double)(-Velocity.X / Velocity.Y)) - (float)(Math.PI / 2);
                    }
                    else
                    {
                        laserRot = (float)Math.Atan((double)(-Velocity.X / Velocity.Y)) + (float)(Math.PI / 2);
                    }
                }

                // offset

                if (0.0f <= laserRot && laserRot < (float)(Math.PI / 2))
                {
                    laserOffset = new Vector2(0, -laserRect.Height/2);// -new Vector2(0, Rectangle.Height * Scale);
                }

                else if ((float)(Math.PI / 2) <= laserRot && laserRot < (float)(Math.PI))
                {
                    laserOffset = new Vector2(laserRect.Height/2, 0);
                }

                else if ((float)(Math.PI) <= laserRot && laserRot < (float)((3*Math.PI) / 2))
                {
                    laserOffset = new Vector2(0, laserRect.Height/2);
                }

                else
                {
                    laserOffset = new Vector2(-laserRect.Height/2, 0);
                }

                // firing

                powerupActive = true;
                laserTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (laserTimer > laserTime)
                {
                    laser = false;
                    powerupActive = false;
                }
            }

            // -- freeze --

            if (CellsHelper.Freeze == true)
            {
                freezeTimer += gameTime.ElapsedGameTime.Milliseconds;
                powerupActive = true;

                if (freezeTimer > freezeTime)
                {
                    CellsHelper.Freeze = false;
                    powerupActive = false;
                }
            }

            if (powerupTiming == true)
            {
                powerupTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (powerupTimer >= powerupTime)
                {
                    powerupTiming = false;
                    powerupActive = false;
                    powerupTimer = 0;
                }
            }
            if (powerupActive == false)
            {
                circle4.color = Color.White;
                circle4.Update();
            }
            else
            {
                circle4.color = Color.Red;
                circle4.Update();
            }

            acceleration = Vector2.Zero; // reset acc'n

            // ============= check for key presses =============================

            accing = false;

            if (InputHelper.Keys == player)
            {

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
                    acceleration += new Vector2(1, 0);
                    accing = true;
                }
            }
            // ============= check for button presses =============================

            else if (player == 1)
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

            // ================================================================================

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

            // CHEAT
            if (InputHelper.WasButtonPressed(Keys.V))
            {
                AddViruslings(1);
            }

            // MOD ============================

            if (InputHelper.WasButtonPressed(Keys.Enter))
            {
                activePowerup += 1;

                if (activePowerup > 6)
                {
                    activePowerup = 0;
                }
            }

            if (InputHelper.IsButtonDown(Keys.D1))
            {


                if (InputHelper.IsButtonDown(Keys.LeftShift))
                {
                    VirusHelper.Radius1 -= 1;
                }

                else
                {
                    VirusHelper.Radius1 += 1;
                }
            }

            if (InputHelper.IsButtonDown(Keys.D2))
            {


                if (InputHelper.IsButtonDown(Keys.LeftShift))
                {
                    VirusHelper.Radius2 -= 1;
                }

                else
                {
                    VirusHelper.Radius2 += 1;
                }
            }

            if (InputHelper.IsButtonDown(Keys.D3))
            {


                if (InputHelper.IsButtonDown(Keys.LeftShift))
                {
                    VirusHelper.Radius3 -= 1;
                }

                else
                {
                    VirusHelper.Radius3 += 1;
                }
            }

            if (InputHelper.IsButtonDown(Keys.D4))
            {


                if (InputHelper.IsButtonDown(Keys.LeftShift))
                {
                    VirusHelper.InnerSlow -= 0.1f;
                }

                else
                {
                    VirusHelper.InnerSlow += 0.1f;
                }
            }

            if (InputHelper.IsButtonDown(Keys.D5))
            {


                if (InputHelper.IsButtonDown(Keys.LeftShift))
                {
                    VirusHelper.OuterSlow -= 0.1f;
                }

                else
                {
                    VirusHelper.OuterSlow += 0.1f;
                }
            }

            if (InputHelper.IsButtonDown(Keys.D6))
            {


                if (InputHelper.IsButtonDown(Keys.LeftShift))
                {
                    VirusHelper.InnerAccn -= 0.01f;
                }

                else
                {
                    VirusHelper.InnerAccn += 0.01f;
                }
            }

            if (InputHelper.IsButtonDown(Keys.D7))
            {


                if (InputHelper.IsButtonDown(Keys.LeftShift))
                {
                    VirusHelper.OuterAccn -= 0.1f;
                }

                else
                {
                    VirusHelper.OuterAccn += 0.1f;
                }
            }

            if (InputHelper.IsButtonDown(Keys.D8))
            {


                if (InputHelper.IsButtonDown(Keys.LeftShift))
                {
                    VirusHelper.OuterOuterAccn -= 0.1f;
                }

                else
                {
                    VirusHelper.OuterOuterAccn += 0.1f;
                }
            }

            if (InputHelper.IsButtonDown(Keys.D9))
            {


                if (InputHelper.IsButtonDown(Keys.LeftShift))
                {
                    VirusHelper.OuterOuterOuterAccn -= 0.1f;
                }

                else
                {
                    VirusHelper.OuterOuterOuterAccn += 0.1f;
                }
            }

            if (InputHelper.IsButtonDown(Keys.D0))
            {


                if (InputHelper.IsButtonDown(Keys.LeftShift))
                {
                    VirusHelper.OuterOuterSlow -= 0.1f;
                }

                else
                {
                    VirusHelper.OuterOuterSlow += 0.1f;
                }
            }

            if (InputHelper.WasButtonPressed(Keys.Add))
            {
                VirusHelper.Rotation *= -1;
            }

            if (InputHelper.IsButtonDown(Keys.F1))
            {
                if (InputHelper.IsButtonDown(Keys.LeftShift))
                {
                    this.Scale *= 0.99f;
                }

                else
                {
                    this.Scale *= 1.01f;
                }
            }

            // =================================

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


            // get rid of dead viruslings

            
            foreach (Virusling v in deathList)
            {
                VirusHelper.Viruslings.Remove(v);
                VirusHelper.Virus.viruslingNo -= 1;
            }

            foreach (Virusling virusling in viruslingList)
            {                
                virusling.Update(gameTime, batch);
            }

            if (circles == true)
            {
                circle1.position = Position;
                circle2.position = Position;
                circle3.position = Position;

                circle1.radius = VirusHelper.Radius1;
                circle2.radius = VirusHelper.Radius2;
                circle3.radius = VirusHelper.Radius3;

                circle1.Update();
                circle2.Update();
                circle3.Update();
            }

            // frames

            if (Velocity.X > 0 && Math.Abs(Velocity.X) > Math.Abs(Velocity.Y) && 
                ((InputHelper.Players == player && InputHelper.IsButtonDown(Keys.Right))
                | (InputHelper.LeftThumbstickDirectionP1).X > 0))
            {
                dir = 1;
            }
            else if (Velocity.X < 0 && Math.Abs(Velocity.X) > Math.Abs(Velocity.Y) &&
                ((InputHelper.Players == player && InputHelper.IsButtonDown(Keys.Left))
                | (InputHelper.LeftThumbstickDirectionP1).X < 0))
            {
                dir = 0;
            }
            else
            {
                dir = 2;
            }

            XFrame = dir;
            YFrame = colour;

            eyeRect.X = dir * eyeWidth;
            eyeRect.Y = eyeColour * eyeHeight;

            if (activePowerup != 0)
            {
                powerupRect.X = (powerupList[activePowerup - 1]) * powerupRect.Width;
            }

            base.Update(gameTime, batch);


        }

        public override void Draw(GameTime gameTime, SpriteBatch batch, float layer)
        {
            batch.Draw(eyeTex, (Position - ((DrawOffset + eyeOffset[dir]) * Scale)), eyeRect, Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, layer + 0.01f);
                                    
            base.Draw(gameTime, batch, layer);

            foreach (Virusling virusling in viruslingList)
            {
                virusling.Draw(gameTime, batch, layer);
            }

            if (laser == true)
            {
                batch.Draw(laserTex, Position + laserOffset, laserRect, Color.White, laserRot, Vector2.Zero, 1.0f, SpriteEffects.None, layer+0.01f);//LAZER TEX !!!!!!!!
            }

            if (circles == true)
            {
                circle1.Draw(batch);
                circle2.Draw(batch);
                circle3.Draw(batch);            
            }

            circle4.Draw(batch);
            // HUD - power up



            if (activePowerup != 0)
            {
                batch.Draw(powerupTex, new Vector2(700, 30), powerupRect, Color.White, 0.0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0.01f);
            }
        }

    }
}
