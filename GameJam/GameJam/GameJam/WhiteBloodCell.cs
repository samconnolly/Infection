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
    class WhiteBloodCell:SpriteBase
    {
        int hitPoints = 3;
        float dieChance = 0.1f;

        //Vector2 Velocity;
        float speed = 1.0f;
        float alertSpeed = 2.5f;
        float attackSpeed = 6.0f;
        public Vector2 groupCentre;
        public List<WhiteBloodCell> group;
        
        Random random;

        bool active = false;
        bool alert = false;
        bool attacking = false;
        bool retreating = false;
        bool benign = true;

        int spawnTimer = 0;
        int spawnTime = 1500;

        int frate = 60;
        int ftimer = 0;
        int frame = 0;
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

        int attackTimer = 0;

        List<Virusling> deathList = new List<Virusling> { };
        Virusling deadSpore = null;

        Vector2 attackAim = new Vector2(300,300);

        Texture2D normalTex;
        Texture2D hitTex;
        Texture2D spawnTex;

        public WhiteBloodCell(Texture2D texture,Texture2D hitTexture,Texture2D spawnTexture, Vector2 position):base(texture)
        {
            Position = position;
            Rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            random = new Random((int)(position.X*1000));

            float x = (float) random.NextDouble();
            float y = (float) Math.Sqrt(1 - Math.Pow(x,2));

            Velocity = new Vector2(x, y);

            normalTex = texture;
            hitTex = hitTexture;
            spawnTex = spawnTexture;
        }


        public override void Update(GameTime gameTime, SpriteBatch bactch)
        {
            
            // once spawned, do this...
            if (active == true)
            {

                // textures
                if (hit == true)
                {
                    hitTimer += gameTime.ElapsedGameTime.Milliseconds;

                    if (hitTimer >= hitTime)
                    {
                        Texture = normalTex;
                        hit = false;
                    }
                }

                // killing player

                if ((VirusHelper.VirusPosition - Position).Length() < 30)
                {
                    SoundEffectPlayer.PlaySquelch();
                    GameStateManager.CurrentGameState = GameState.GameOver;
                    GameStateManager.HasChanged = true;                    
                }

                //////////////////////////////////////
                if (InputHelper.Players == 2)
                {
                    if ((VirusHelper.VirusPositionP2 - Position).Length() < 30)
                    {
                        SoundEffectPlayer.PlaySquelch();
                        GameStateManager.CurrentGameState = GameState.GameOver;
                        GameStateManager.HasChanged = true;
                    }
                }
                ///////////////////////////////////////

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

                //////////////////////////////
                if (InputHelper.Players == 2)
                {
                    foreach (Virusling v in VirusHelper.ViruslingsP2)
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
                }

                //////////////////////

                foreach (Virusling v in deathList)
                {
                    VirusHelper.Viruslings.Remove(v);
                    VirusHelper.Virus.viruslingNo -= 1;
                }


                // movement, attacks

                if (attacking != true)
                {
                    ///////////////////////////////////
                    if (InputHelper.Players == 1)
                    {
                        attackAim = VirusHelper.VirusPosition;
                    }

                    else if (InputHelper.Players == 2)
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
                    ///////////////////////////////////
                }

                if ((Position - attackAim).Length() <= alertDistance && (Position - attackAim).Length() > attackDistance && attacking == false)
                {
                    alert = true;
                    benign = false;
                    attacking = false;
                    retreating = false;
                }

                else if ((Position - attackAim).Length() <= attackDistance)
                {
                    if (retreating == false && attacking == false)
                    {
                        alert = false;
                        benign = false;
                        attacking = true;
                        retreating = false;

                        attackTimer = (int)(attackTime - random.NextDouble() * 1000);
                    }
                }

                else
                {
                    alert = false;
                    benign = true;
                    attacking = false;
                    retreating = false;
                }



                if (alert)
                {
                    Velocity = attackAim - Position;
                    Vector2 norm = Velocity;
                    norm.Normalize();
                    Velocity = norm;
                    Velocity *= alertSpeed;
                }

                else if (attacking | retreating)
                {
                    if (attackTimer >= attackTime)
                    {
                        if ((Position - attackAim).Length() <= VirusHelper.Virus.Rectangle.Width / 2.0f)
                        {
                            attacking = false;
                            retreating = true;
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
                            Velocity *= attackSpeed;
                        }

                        else if (retreating == true)
                        {
                            Velocity = Position - attackAim;
                            Vector2 norm = Velocity;
                            norm.Normalize();
                            Velocity = norm;
                            Velocity *= attackSpeed;
                        }
                    }

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




                else if (benign)
                {
                    double theta = random.NextDouble() - 0.5;

                    Velocity = new Vector2((float)(Velocity.X * Math.Cos(theta)) - (float)(Velocity.Y * Math.Sin(theta)),
                                                (float)(Velocity.X * Math.Sin(theta)) + (float)(Velocity.Y * Math.Cos(theta)));

                    if ((groupCentre - Position).Length() > 50)
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
                }

                Vector2 away = Vector2.Zero;

                foreach (WhiteBloodCell gbc in group)
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

                Velocity += away;

                Position += Velocity * speed;

                if (hitPoints <= 0)
                {
                    dead = true;
                    ////////////////////////////////
                    if (hitBy == 1)
                    {
                        VirusHelper.Virus.AddViruslingsHere(1, Position);
                    }

                    else if (hitBy == 2)
                    {
                        VirusHelper.VirusP2.AddViruslingsHere(1, Position);
                    }
                    ///////////////////////////////
                    ScoreHelper.Score += 1;
                }
            }

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
                    frame += 1;
                    ftimer = 0;

                    if (frame > nframes)
                    {
                        frame = 0;
                    }
                }

                Rectangle = new Rectangle(frame * normalTex.Width,0,Rectangle.Width,Rectangle.Height);

                // spawn delay
                if (spawnTimer < spawnTime)
                {
                    spawnTimer += gameTime.ElapsedGameTime.Milliseconds;
                }

                else
                {
                    spawnTimer = 0;
                    active = true;
                    Rectangle = new Rectangle(0, 0, Rectangle.Width, Rectangle.Height);
                    Texture = normalTex;
                }
            }

            base.Update(gameTime, bactch);
        }

        

        public override void Draw(GameTime gameTime, SpriteBatch batch, float layer)
        {
                       
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
            Texture = hitTex;

            v.Bounce(Position,Velocity);

            if (v.damaging == true)
            {
                v.damaging = false;

                hitPoints -= 1;
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
