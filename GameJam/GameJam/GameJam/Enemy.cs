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
    class Enemy:SpriteBase
    {
        int hitPoints = 10;
        float dieChance = 0.1f;

        //Vector2 Velocity;
        float slow = 1.0f;
        float medium = 2.5f;
        float fast = 6.0f;
        public Vector2 groupCentre;
        public List<Enemy> group;
        
        Random random;

        Vector2 startPos;

        bool active = false;
        bool alert = false;
        bool attacking = false;
        bool retreating = false;
        bool benign = true;
        public bool firing = false;
        public bool shock = false;

        int spawnTimer = 0;
        int spawnTime = 1500;

        int frate;
        int ftimer = 0;
        int sframe = 0;
        int nframes = 3;

        int alertDistance = 300;
        int attackDistance = 150;
        int retreatDistance = 100;
        int attackTime = 3000;

        int hitTime = 300;
        int hitTimer = 0;
        bool hit = false;
        int hitBy = 1;

        public bool dead = false;

        public int movement;
        public int attack;

        private int attackTimer = 0;

        List<Virusling> deathList = new List<Virusling> { };
        Virusling deadSpore = null;

        Vector2 attackAim = new Vector2(300,300);

        Texture2D normalTex;
        Texture2D spawnTex;
        Texture2D missileTex;
        Texture2D crossTex;

        public Circle circle;

        float fireSpeed = 10.0f;
        int fireTimeMin = 3000;
        int fireTimeMax = 5000;
        int fireTime = 4000;
        int fireTimer = 0;

        int blinkTimer = 0;
        int blinkMin = 1000;
        int blinkMax = 3000;
        int blinkTime = 200;
        int blink = 0;
        bool blinking = false;

        private float fallSpeed = 6.0f;

        private int colours = 5;
        private int colour = 0;
        private int frames = 1;
        private int frame = 0;
        
        public Enemy(Texture2D texture, Texture2D spawnTexture, Vector2 position, int movementType, int attackType,
            Vector2 sheetDimensions, int skinColour, Texture2D missileTexture = null, Texture2D crossTexture = null)
            : base(texture)
        {
            Position = position;
            startPos = Position;
            random = new Random((int)(position.X*1000));

            float x = (float) random.NextDouble();
            float y = (float) Math.Sqrt(1 - Math.Pow(x,2));

            Velocity = new Vector2(x, y);

            normalTex = texture;
            spawnTex = spawnTexture;
            missileTex = missileTexture;
            crossTex = crossTexture;

            movement = movementType;
            attack = attackType;

            colours = (int)sheetDimensions.Y;
            frames = (int)sheetDimensions.X;
            SheetSize = new Vector2(frames, colours);

            attackTimer = (int)(attackTime - random.NextDouble() * 1000);
            fireTime = (int)(random.NextDouble() * (fireTimeMax - fireTimeMin) + fireTimeMin);

            blink = random.Next(blinkMin, blinkMax);

            colour = skinColour;

            circle = new Circle(Vector2.Zero, 2, 2, Color.White);

            frate = ViewPortHelper.FrameRate;
            Scale = 0.2f;
        }


        public void Fire()
        {
            CellsHelper.AddCells.Add(new Missile(missileTex, this.Position + new Vector2(Rectangle.Width / 2 * Scale, Rectangle.Height / 2 * Scale), attackAim, fireSpeed));
        }

        public void Bomb()
        {
            CellsHelper.AddCells.Add(new Bomb(missileTex, crossTex, attackAim, fallSpeed));
        }

        public void Wave()
        {
            if (firing == false)
            {
                firing = true;
                circle.position = Position + new Vector2(Rectangle.Width / 2 * Scale, Rectangle.Height / 2 * Scale);
                circle.radius = (normalTex.Width/frames) / 2;
            }
        }

        public bool LaserCollision(int player)
        {
            bool hit = false;
            Vector2 start;
            Vector2 vector;
            float yLow;
            float yHigh;
            int side;
            int pside;
            Vector2 end;

            if (player == 1)
            {
                start = (VirusHelper.Virus.Position + VirusHelper.Virus.laserOffset);
                vector = new Vector2(VirusHelper.Virus.laserRect.Width * (float)Math.Cos(VirusHelper.Virus.laserRot),
                                           VirusHelper.Virus.laserRect.Width * (float)Math.Sin(VirusHelper.Virus.laserRot));

                end = vector + start;
                float gradient = vector.Y / vector.X;

                yLow = (Position.X - VirusHelper.Virus.Position.X) * gradient + VirusHelper.Virus.Position.Y + VirusHelper.Virus.laserOffset.Y;
                yHigh = (Position.X + Rectangle.Width * Scale - VirusHelper.Virus.Position.X) * gradient + VirusHelper.Virus.Position.Y + VirusHelper.Virus.laserOffset.Y;

            }

            else
            {
                start = (VirusHelper.VirusP2.Position + VirusHelper.VirusP2.laserOffset);
                vector = new Vector2(VirusHelper.VirusP2.laserRect.Width * (float)Math.Cos(VirusHelper.VirusP2.laserRot),
                                           VirusHelper.VirusP2.laserRect.Width * (float)Math.Sin(VirusHelper.VirusP2.laserRot));
                end = vector + start;
                float gradient = vector.Y / vector.X;

                yLow = (Position.X - VirusHelper.VirusP2.Position.X) * gradient + VirusHelper.VirusP2.Position.Y + VirusHelper.VirusP2.laserOffset.Y;
                yHigh = (Position.X + Rectangle.Width * Scale - VirusHelper.VirusP2.Position.X) * gradient + VirusHelper.VirusP2.Position.Y + VirusHelper.VirusP2.laserOffset.Y;

            }

            if (end.X > start.X)
            {
                side = 1;
            }
            else
            {
                side = 0;
            }

            if (Position.X > start.X)
            {
                pside = 1;
            }
            else
            {
                pside = 0;
            }
            
            if (((Position.Y < yLow && Position.Y + Rectangle.Height*Scale > yLow)
                 | (Position.Y < yHigh && Position.Y + Rectangle.Height*Scale > yHigh)) && side == pside)
            {
                hit = true;
            }

            return hit;
        }

        public override void Update(GameTime gameTime, SpriteBatch bactch)
        {            
            // once spawned, do this... standard behaviour
            if (active == true)
            {

                //------------- textures --------------------------------------------------------------------------------------------------------------
                if (hit == true)
                {
                    hitTimer += gameTime.ElapsedGameTime.Milliseconds;

                    if (hitTimer >= hitTime)
                    {
                        frame = 0;
                        hit = false;
                    }
                }

                // blinking!

                blinkTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (blinking == false && blinkTimer >= blink)
                {
                    blinking = true;
                    frame = 1;
                    blink = random.Next(blinkMin, blinkMax);
                    blinkTimer = 0;
                }

                if (blinking == true && blinkTimer >= blinkTime)
                {
                    blinking = false;
                    blinkTimer = 0;
                    frame = 0;
                }

                // ---------- killing player ----------------------------------------------------------------------------------------------------------

                foreach (Virus virus in ScoreHelper.LivePlayers)
                {
                    if ((virus.Position - Position).Length() < (virus.width) * virus.Scale)
                    {
                        ScoreHelper.PlayerHit(virus);
                        dead = true;                       
                    }
                }
                
                //---------- getting hit by spores ----------------------------------------------------------------------------------------

                deathList = new List<Virusling> { };
                deadSpore = null;

                foreach (Virus virus in ScoreHelper.LivePlayers)
                {
                    foreach (Virusling v in virus.viruslingList)
                    {
                        if ((v.Position - this.Position ).Length() < (Rectangle.Height*Scale / 2.0f + v.Rectangle.Width*v.Scale / 2.0f))
                        {
                            deadSpore = Hit(v);
                        }

                        if (deadSpore != null)
                        {
                            deathList.Add(deadSpore);
                        }
                    }
                }

                foreach (Virusling v in deathList)
                {
                    VirusHelper.Viruslings.Remove(v);
                    VirusHelper.Virus.viruslingNo -= 1;
                }

                // ----- getting hit by laser --------
                foreach (Virus virus in ScoreHelper.LivePlayers)
                {
                    if (virus.laser == true)
                    {
                        if (LaserCollision(virus.player) == true)
                        {
                            LaserHit(virus.player);
                        }
                    }
                }

                //==============================================================================================================================================
                //------------- movement, attacks --------------------------------------------------------------------------------------------------------------
                //==============================================================================================================================================

                //----- attacks -------------
                
                // set attack aim
                if (attacking != true)
                {
                    if (InputHelper.Players == 1)
                    {
                        attackAim = VirusHelper.VirusPosition;
                    }

                    else if (InputHelper.Players == 2)
                    {
                        if (ScoreHelper.LivePlayers.Count() > 1)
                        {
                            float p1Dist = (Position - VirusHelper.VirusPosition).Length();
                            float p2Dist = (Position - VirusHelper.VirusPositionP2).Length();

                            if (p1Dist < p2Dist)
                            {
                                attackAim = VirusHelper.VirusPosition;
                            }

                            else
                            {
                                attackAim = VirusHelper.VirusPositionP2;
                            }
                        }
                        else
                        {
                            if (ScoreHelper.LivePlayers.Count() > 0)
                            {
                                attackAim = ScoreHelper.LivePlayers[0].Position;
                            }
                        }
                    }

                }

                // no attack
                if (attack == 1)
                {
                    // do nothing!
                }

                // rush - charge at player
                else if (attack == 2)
                {

                    // set off alert
                    if ((Position - attackAim).Length() <= alertDistance && (Position - attackAim).Length() > attackDistance && attacking == false)
                    {
                        alert = true;
                        benign = false;
                        attacking = false;
                        retreating = false;
                        frame = 2;
                    }

                    // set off attack
                    else if ((Position - attackAim).Length() <= attackDistance)
                    {
                        if (retreating == false && attacking == false)
                        {
                            alert = false;
                            benign = false;
                            attacking = true;
                            retreating = false;

                            frame = 4;

                            attackTimer = (int)(attackTime - random.NextDouble() * 1000);
                        }
                    }

                    // set benign
                    else
                    {
                        if (alert == true | attacking == true | retreating == true)
                        {
                            startPos = Position;
                        }

                        alert = false;
                        benign = true;
                        attacking = false;
                        retreating = false;

                        frame = 0;
                    }

                    // alert movement
                    if (alert)
                    {
                        Velocity = attackAim - Position;
                        Vector2 norm = Velocity;
                        norm.Normalize();
                        Velocity = norm;
                        Velocity *= medium;
                    }

                    // attack/retreat movement
                    else if (attacking | retreating)
                    {
                        // attack
                        if (attackTimer >= attackTime)
                        {
                            foreach (Virus virus in ScoreHelper.LivePlayers)
                            {
                                if ((Position - attackAim).Length() <= virus.Rectangle.Width * virus.Scale / 2.0f)
                                {
                                    attacking = false;
                                    retreating = true;
                                }
                            }

                            if ((Position - attackAim).Length() >= retreatDistance && retreating == true)
                            {
                                retreating = false;
                                attacking = true;
                                attackTimer = 0;
                            }

                            if (attacking == true)
                            {
                                Velocity = attackAim - Position;
                                Vector2 norm = Velocity;
                                norm.Normalize();
                                Velocity = norm;
                                Velocity *= fast;
                            }

                            else if (retreating == true)
                            {
                                Velocity = Position - attackAim;
                                Vector2 norm = Velocity;
                                norm.Normalize();
                                Velocity = norm;
                                Velocity *= fast;
                            }
                        }


                        // retreat
                        else
                        {
                            double theta = random.NextDouble() - 0.5;

                            Velocity = new Vector2((float)(Velocity.X * Math.Cos(theta)) - (float)(Velocity.Y * Math.Sin(theta)),
                                                        (float)(Velocity.X * Math.Sin(theta)) + (float)(Velocity.Y * Math.Cos(theta)));

                            if ((attackAim - Position).Length() > attackDistance)
                            {
                                Vector2 extra = (attackAim - Position);
                                extra.Normalize();
                                Velocity += extra * 1.0f;
                                Vector2 norm = Velocity;
                                norm.Normalize();
                                Velocity = norm;
                            }

                            else if ((attackAim - Position).Length() < 35)
                            {
                                Vector2 extra = (Position - attackAim);
                                extra.Normalize();
                                Velocity += extra * 1.0f;
                                Vector2 norm = Velocity;
                                norm.Normalize();
                                Velocity = norm;
                            }
                        }

                        attackTimer += gameTime.ElapsedGameTime.Milliseconds;

                    }
                }

                // Bomb
                else if (attack == 3)
                {
                    fireTimer += gameTime.ElapsedGameTime.Milliseconds;

                    if (fireTimer > fireTime)
                    {
                        fireTimer = 0;
                        Bomb();
                        fireTime = (int)(random.NextDouble() * (fireTimeMax - fireTimeMin) + fireTimeMin);
                    }
                }

                else if (attack == 4)
                {
                    fireTimer += gameTime.ElapsedGameTime.Milliseconds;

                    if (fireTimer > fireTime)
                    {
                        fireTimer = 0;
                        Wave();
                        fireTime = (int)(random.NextDouble() * (fireTimeMax - fireTimeMin) + fireTimeMin);
                    }
                }

                // missile
                else if (attack == 5)
                {
                    fireTimer += gameTime.ElapsedGameTime.Milliseconds;

                    if (fireTimer > fireTime)
                    {
                        fireTimer = 0;
                        Fire();
                        fireTime = (int)(random.NextDouble() * (fireTimeMax - fireTimeMin) + fireTimeMin);
                    }

                }

                else if (attack == 6)
                {
                    fireTimer += gameTime.ElapsedGameTime.Milliseconds;

                    if (fireTimer > fireTime)
                    {
                        fireTimer = 0;
                        Wave();
                        fireTime = (int)(random.NextDouble() * (fireTimeMax - fireTimeMin) + fireTimeMin);
                        shock = true;
                    }
                }

                // ------- standard movement --------------------

                if (benign)
                {
                    // static
                    if (movement == 1)
                    {
                        Velocity = Vector2.Zero;
                    }

                    // slow standard - move towards player
                    else if (movement == 2)
                    {
                        Velocity = attackAim - Position;
                        Vector2 norm = Velocity;
                        norm.Normalize();
                        Velocity = norm;
                        Velocity *= slow;
                    }

                    // fast standard - move towards player
                    else if (movement == 3)
                    {

                        Velocity = attackAim - Position;
                        Vector2 norm = Velocity;
                        norm.Normalize();
                        Velocity = norm;
                        Velocity *= medium;
                    }

                    // erratic
                    else if (movement == 4)
                    {

                        if ((groupCentre - Position).Length() > 150)
                        {
                            Vector2 extra = (groupCentre - Position);
                            extra.Normalize();
                            Velocity += extra * 1.0f;
                            Vector2 norm = Velocity;
                            norm.Normalize();
                            Velocity = norm;
                        }

                        if ((new Vector2(ViewPortHelper.X / 2, ViewPortHelper.Y / 2) - Position).Length() > 350)
                        {
                            Vector2 extra = (new Vector2(ViewPortHelper.X / 2, ViewPortHelper.Y / 2) - Position);
                            extra.Normalize();
                            Velocity += extra * 1.0f;
                            Vector2 norm = Velocity;
                            norm.Normalize();
                            Velocity = norm;
                        }


                        double theta = random.NextDouble() - 0.5;

                        Velocity = new Vector2((float)(Velocity.X * Math.Cos(theta)) - (float)(Velocity.Y * Math.Sin(theta)),
                                                    (float)(Velocity.X * Math.Sin(theta)) + (float)(Velocity.Y * Math.Cos(theta)));
                    }

                    // pulse - slight movement around a point
                    else if (movement == 5)
                    {

                        if ((startPos - Position).Length() > 100)
                        {
                            Vector2 extra = (groupCentre - Position);
                            extra.Normalize();
                            Velocity += extra * 1.0f;
                            Vector2 norm = Velocity;
                            norm.Normalize();
                            Velocity = norm;
                        }

                        if ((new Vector2(ViewPortHelper.X / 2, ViewPortHelper.Y / 2) - Position).Length() > 350)
                        {
                            Vector2 extra = (new Vector2(ViewPortHelper.X / 2, ViewPortHelper.Y / 2) - Position);
                            extra.Normalize();
                            Velocity += extra * 1.0f;
                            Vector2 norm = Velocity;
                            norm.Normalize();
                            Velocity = norm;
                        }


                        double theta = random.NextDouble() - 0.5;

                        Velocity = new Vector2((float)(Velocity.X * Math.Cos(theta)) - (float)(Velocity.Y * Math.Sin(theta)),
                                                    (float)(Velocity.X * Math.Sin(theta)) + (float)(Velocity.Y * Math.Cos(theta)));

                    }
                }

                // ----- keeping the cells apart ----------

                if (movement != 1)
                {
                    Vector2 away = Vector2.Zero;

                    foreach (Enemy gbc in group)
                    {
                        float dist = (Position - gbc.Position).Length();

                        if (dist > 0 && dist < 30)
                        {
                            Vector2 add = (Position - gbc.Position);
                            add.Normalize();
                            away += add;
                        }
                    }
                    if (away.Length() > 0)
                    {
                        away.Normalize();
                    }

                    Velocity += away*slow;
                }

                if (CellsHelper.Freeze == false)
                {
                    Position += Velocity;
                }

                // ------- death of the cell ------------------------

                if (hitPoints <= 0)
                {
                    dead = true;
                    
                    if (hitBy == 1)
                    {
                        VirusHelper.Virus.AddViruslingsHere(1, Position);
                    }

                    else if (hitBy == 2)
                    {
                        VirusHelper.VirusP2.AddViruslingsHere(1, Position);
                    }
                    
                    ScoreHelper.Score += 1;
                }
                
                XFrame = frame;
                YFrame = colour;

            }


            // ---------------- spawning warning ---------------------------------------------------
            else
            {
                // textures
                Texture = spawnTex;

                if (ftimer < frate)
                {
                    ftimer += gameTime.ElapsedGameTime.Milliseconds;
                }

                else
                {
                    sframe += 1;
                    ftimer = 0;

                    if (sframe > nframes)
                    {
                        sframe = 0;
                    }
                }

                Rectangle = new Rectangle(sframe * spawnTex.Width / 3, 0, spawnTex.Width / 3, spawnTex.Height);
                Scale = 1.0f;

                // spawn delay
                if (spawnTimer < spawnTime)
                {
                    spawnTimer += gameTime.ElapsedGameTime.Milliseconds;
                    SheetSize = new Vector2(3, 1);
                }

                else
                {
                    spawnTimer = 0;
                    active = true;
                    Texture = normalTex;
                    Rectangle = new Rectangle(0, 0, Texture.Width / frames, Texture.Height / colours);
                    Scale = 0.2f;
                    SheetSize = new Vector2(frames, colours);
                    XFrame = frame;
                    YFrame = colour;
                }

            }

            if (firing == true)
            {
                circle.radius += gameTime.ElapsedGameTime.Milliseconds / 4.0f;
                circle.Update();

                if (circle.radius > 300)
                {
                    firing = false;
                    shock = false;
                }
            }
            
            base.Update(gameTime, bactch);
        }

        

        public override void Draw(GameTime gameTime, SpriteBatch batch, float layer)
        {
            if (firing == true)
            {
                circle.Draw(batch);
            }
                                               
           base.Draw(gameTime, batch, layer);
            
        }

        public Virusling Hit(Virusling v)
        {
            SoundEffectPlayer.PlaySquelch();

            hit = true;
            hitTimer = 0;
            ////////////////
            hitBy = v.player;
            ////////////////
            frame = 5;

            v.Bounce(Position,Velocity);

            if (v.damaging == true)
            {
                v.damaging = false;

                hitPoints -= 1;
            }

            if (random.NextDouble() < dieChance)
            {
                //return v;
                return null;
            }

            else
            {
                return null;
            }
        }

        public void LaserHit(int player)
        {
            SoundEffectPlayer.PlaySquelch();

            hit = true;
            hitTimer = 0;
            ////////////////
            hitBy = player;
            ////////////////
            frame = 5;

            hitPoints -= 1;            
        }
    }
}
