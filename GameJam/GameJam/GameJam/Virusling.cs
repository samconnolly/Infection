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
        float radius2;
        float radius3;

        float islow;
        float oslow;
        float ooslow;

        float iaccn;
        float oaccn;
        float ooaccn;
        float oooaccn;

        float repel;

        int rot;
        float rotSpeed;

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

        int homingDist = 100;
        float homingSpeed = 10;

        public int player = 1;

        private int colour = 5;
        private int colours = 6;
        private int width;
        private int height;

        public Virusling(Texture2D texture, Vector2 startPosition, int Player = 1)
            : base(texture)
        {
            Scale = 0.1f;

            Position = startPosition;
            width = texture.Width;
            height = texture.Height / colours;
            SheetSize = new Vector2(1, colours);
            
            acceleration = new Vector2(0, 0);
            target = new Vector2(300, 300);

            accelerationMagnitude = 0.08f;

            radius = VirusHelper.radius1;
            radius2 = VirusHelper.radius2;
            radius3 = VirusHelper.radius3;

            islow = VirusHelper.InnerSlow;
            oslow = VirusHelper.OuterSlow;
            ooslow = VirusHelper.OuterOuterSlow;

            iaccn = VirusHelper.InnerAccn;
            oaccn = VirusHelper.OuterAccn;
            ooaccn = VirusHelper.OuterOuterAccn;
            oooaccn = VirusHelper.OuterOuterOuterAccn;

            repel = VirusHelper.Repel;

            random = new Random();
            
            Velocity = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f);

            player = Player;
            if (player == 2)
            {
                colour = 4;
            }
        }   
     
        public void Pulse()
        {
            blast = true;
            blastTimer = 0;
        }

        public void Stream()
        {
            shoot = true;
            shootTimer = 0;
        }


        public override void Update(GameTime gameTime, SpriteBatch batch)
        {
            if (player == 1)
            {
                radius = VirusHelper.Radius1;
                radius2 = VirusHelper.Radius2;
                radius3 = VirusHelper.Radius3;

                islow = VirusHelper.InnerSlow;
                oslow = VirusHelper.OuterSlow;
                ooslow = VirusHelper.OuterOuterSlow;

                iaccn = VirusHelper.InnerAccn;
                oaccn = VirusHelper.OuterAccn;
                ooaccn = VirusHelper.OuterOuterAccn;
                oooaccn = VirusHelper.OuterOuterOuterAccn;

                repel = VirusHelper.Repel;
                rot = VirusHelper.Rotation;
            }
            else
            {
                radius = VirusHelper.Radius1P2;
                radius2 = VirusHelper.Radius2P2;
                radius3 = VirusHelper.Radius3P2;

                islow = VirusHelper.InnerSlowP2;
                oslow = VirusHelper.OuterSlowP2;
                ooslow = VirusHelper.OuterOuterSlowP2;

                iaccn = VirusHelper.InnerAccnP2;
                oaccn = VirusHelper.OuterAccnP2;
                ooaccn = VirusHelper.OuterOuterAccnP2;
                oooaccn = VirusHelper.OuterOuterOuterAccnP2;

                repel = VirusHelper.RepelP2;
                rot = VirusHelper.RotationP2;
            }

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

            //----------------------------------------------------------------------------------------------------------------
            // ===================== movement ================================================================================
            //----------------------------------------------------------------------------------------------------------------

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
            radius = VirusHelper.Radius1;


            // ===== outside orbit ==============
            if (diff.Length() > radius2)
            {
                Velocity *= islow;
                acceleration -= iaccn * Velocity;

            }
            
            // ==== inside orbit ===============
            if (diff.Length() <= radius2)
            {
                acceleration = Vector2.Zero;
                //err = (1600 - random.Next(800)) * 0.001f;
                err = 1.2f - (float)random.NextDouble()*0.4f;
                rot = VirusHelper.Rotation;
                rotSpeed = VirusHelper.RotationSpeed;
                Velocity = rot * new Vector2((float)(diff.X * Math.Cos(Math.PI / 2.0)) - (float)(diff.Y * Math.Sin(Math.PI / 2.0)),
                                            (float)(diff.X * Math.Sin(Math.PI / 2.0)) + (float)(diff.Y * Math.Cos(Math.PI / 2.0))) *1.5f* rotSpeed *err;
                //if (player == 1)
                //{
                //    Velocity += VirusHelper.Virus.Velocity*0.2f;
                //}
                //else if (player == 2)
                //{
                //    Velocity += VirusHelper.VirusP2.Velocity;
                //}

                Velocity += diff * Velocity.Length() * 0.001f;
            }

            // === inside radius ============
            else if (diff.Length() < radius3)
            {
                //acceleration = -islow * Velocity;
                acceleration += diff;
                acceleration.Normalize();
                acceleration *= accelerationMagnitude * oaccn;

                Velocity += acceleration;
            }

            // === inside inner radius ============
            //else if (diff.Length() < radius3 && diff.Length() > radius2)
            //{
            //    acceleration = -oslow * Velocity;
            //    acceleration += diff;
            //    acceleration.Normalize();
            //    acceleration *= accelerationMagnitude * oaccn;

            //    Velocity += acceleration;
            //}

            // ==== limiting radius ============
            else if (diff.Length() > radius3)
            {
                //acceleration = - ooslow * Velocity ;
                acceleration += diff;
                acceleration.Normalize();
                acceleration *= accelerationMagnitude * oooaccn;

                Velocity += acceleration;
            }

            if (diff.Length() < radius)
            {
                Vector2 outward;
                outward = diff;
                outward.Normalize();
                Velocity -= outward * ooaccn;
            }

            // ===== Collision with player =====

            if ((diff).Length() <= (VirusHelper.Virus.width / 2.0 * VirusHelper.Virus.Scale) && blast == false)
            {
                if (player == 1)
                {
                    Velocity = VirusHelper.VirusVelocity;
                    Velocity -= diff * repel;
                }
                else if (player == 2)
                {
                    Velocity = VirusHelper.VirusVelocityP2;
                    Velocity -= diff * repel;
                }

            }

            //----------------------------------------------------------------------------------------------------------------
            // ========= Abilities ===========================================================================================
            //----------------------------------------------------------------------------------------------------------------
                        
            //-- pulse ---

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
            
            //--- stream ---           

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

            // -- homing --

            if ((player == 1 && VirusHelper.Virus.homing == true) ||
                    (player == 2 && VirusHelper.VirusP2.homing == true))
            {
                foreach (var enemies in CellsHelper.Cells)
                {
                    if (enemies is EnemyGroup)
                    {
                        foreach (Enemy enemy in (enemies as EnemyGroup).group)
                        {
                            Vector2 close = enemy.Position - Position;

                            if (close.Length() <= homingDist)
                            {
                                close.Normalize();;
                                
                                Velocity = close*homingSpeed;
                            }
                        }
                    }
                }
            }

            // getting hit by purple's energy blast

            foreach (var group in CellsHelper.Cells)
            {                
                if (group is EnemyGroup)
                {
                    EnemyGroup enemyGroup = group as EnemyGroup;

                    foreach (Enemy cell in enemyGroup.group)
                    {
                        if (cell.firing == true)
                        {
                            if ((Position - cell.Position).Length() < cell.circle.radius)
                            {
                                Vector2 bounceDir = (Position - cell.Position);
                                bounceDir.Normalize();
                                Velocity += bounceDir * 10;
                                //Bounce(bounceDir, Vector2.Zero);

                                // chance to kill if shocked
                                if (cell.shock == true)
                                {
                                    double die = random.NextDouble();

                                    if (die <= 0.1)
                                    {
                                        VirusHelper.Virus.deathList.Add(this);
                                    }
                                }

                            }
                        }
                    }
                }
            }

            // colour
            YFrame = colour;
            

            Position += Velocity;

            base.Update(gameTime, batch);
        }
    }
}