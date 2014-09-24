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
    class SpawnII
    {
        List<Vector2> spawnPoints = new List<Vector2> { };


        Texture2D ChargerTex;
        
        Texture2D GruntTex;
        Texture2D SleeperTex;
        Texture2D ArtilleyTex;
        Texture2D TurretTex;

        Texture2D SpawnTex;
        Texture2D MissileTex;
        Texture2D bombTex;
        Texture2D crossTex;
        Texture2D missile;

        Texture2D powerupTex;
        Texture2D heartTex;
        int radiusCount = 0;
        int speedCount = 0;
        int orbitCount = 0;

        Vector2 dim = new Vector2(6, 5);

        Tuple<int, int, Texture2D,int> stats1;
        Tuple<int, int, Texture2D, int> stats2;
        Tuple<int, int, Texture2D, int> stats3;

        private float veryCommon = 0.50f;
        private float common = 0.25f;
        private float uncommon = 0.14f;
        private float rare = 0.10f;
        //private float veryRare = 0.01f;

        int movement;
        int attack;

        Random random = new Random();

        public SpawnII(Texture2D GruntTexture, Texture2D ChargerTexture,
                                Texture2D SleeperTexture, Texture2D TurretTexture, Texture2D ArtilleyTexture,
                                        Texture2D missileTexture, Texture2D crossTexture, Texture2D bombTexture, Texture2D TextureSpawn,
                                            Texture2D powerupTexture, Texture2D heartTexture)
        {
            spawnPoints.Add(new Vector2(150,150));
            spawnPoints.Add(new Vector2(150,570));
            spawnPoints.Add(new Vector2(930,150));
            spawnPoints.Add(new Vector2(930,570));

            ChargerTex = ChargerTexture;
            SpawnTex = TextureSpawn;
            
            GruntTex = GruntTexture;
            SleeperTex = SleeperTexture;
            ArtilleyTex = ArtilleyTexture;
            TurretTex = TurretTexture;
            
            MissileTex = missileTexture;
            bombTex = bombTexture;
            crossTex = crossTexture;

            powerupTex = powerupTexture;
            heartTex = heartTexture;
        }

        public Tuple<int,int,Texture2D,int> GenerateGrunts()
        {
            double num = random.NextDouble() * random.NextDouble() * random.NextDouble() * random.NextDouble()*8 * 5 + 5;
            num = random.NextDouble() * random.NextDouble() * random.NextDouble() * random.NextDouble() * 8 * 5 + 5;
            num = random.NextDouble() * random.NextDouble() * random.NextDouble() * random.NextDouble() * 8 * 5 + 5;
            num = random.NextDouble() * random.NextDouble() * random.NextDouble() * random.NextDouble() * 8 * 5 + 5;

            int n = (int)num;

            if (n < 5) { n = 5; }
            if (n > 10) { n = 10; }
            
            attack = 1;

            double move = random.NextDouble();

            if (move < 0.5)
            {
                movement = 2;
            }
            else
            {
                movement = 3;
            }

            return new Tuple<int, int, Texture2D,int> ( movement, attack, GruntTex,n);
        }

        public Tuple<int, int, Texture2D, int> GenerateChargers()
        {
            int n;

            if (random.NextDouble() < 0.5)
            {
                n = 3;
            }
            else
            {
                if (random.NextDouble() < 0.75)
                {
                     n = 2;
                }
                else
                {
                     n = 4;
                }
            }
            
            attack = 2;

            double move = random.NextDouble();

            if (move < 0.6)
            {
                movement = 4;
            }
            else if (move < 0.8)
            {
                movement = 3;
            }
            else
            {
                movement = 2;
            }

            return new Tuple<int, int, Texture2D, int>(movement, attack, ChargerTex,n);
        }

        public Tuple<int, int, Texture2D, int> GenerateSleepers()
         {
             return new Tuple<int, int, Texture2D, int>(1, 2, SleeperTex,1);
         }

        public Tuple<int, int, Texture2D, int> GenerateArtillery()
        {
            int n;

            if (random.NextDouble() < 0.5)
            {
                n = 2;
            }
            else
            {
                if (random.NextDouble() < 0.75)
                {
                    n = 1;
                }
                else
                {
                    n = 3;
                }
            }

            attack = 3;

            double move = random.NextDouble();

            if (move < 0.5)
            {
                movement = 1;
            }
            else
            {
                movement = 5;
            }

            return new Tuple<int, int, Texture2D, int>(movement, attack, ArtilleyTex,n);
        }


        public Tuple<int, int, Texture2D, int> GenerateTurrets()
        {
            int n;

            if (random.NextDouble() < 0.5)
            {
                n = 2;
            }
            else
            {
                if (random.NextDouble() < 0.75)
                {
                    n = 1;
                }
                else
                {
                    n = 3;
                }
            }

            attack = 5;

            double move = random.NextDouble();

            if (move < 0.4)
            {
                movement = 1;
            }
            else if (move < 0.7)
            {
                movement = 5;
            }
            else if (move < 0.85)
            {
                movement = 2;
            }
            else
            {
                movement = 4;
            }

            return new Tuple<int, int, Texture2D, int>(movement, attack, TurretTex,n);
        }

        public Tuple<int, int, Texture2D, int> GenerateSpecials()
        {
            double att = random.NextDouble();

            if (att < 0.5)
            {
                attack = 4;
            }
            else
            {
                attack = 6;
            }

            double move = random.NextDouble();

            if (move < 0.5)
            {
                movement = 1;
            }            
            else
            {
                movement = 5;
            }

            return new Tuple<int, int, Texture2D, int>(movement, attack, ChargerTex,1);
        }

        public List<SpriteBase> SpawnEnemies(int level)
        {
            // generate types and numbers

            // group 1
            double type1 = random.NextDouble();
            
            // grunts
            if (type1 < 0.75)
            {
                stats1 = GenerateGrunts();
            }
            // chargers
            else
            {
               stats1 = GenerateChargers();
                
            }

            // group 2
            double type2 = random.NextDouble();

            // chargers
            if (type2 < 0.25)
            {
                stats2 = GenerateChargers();
            }
            // sleepers
            else if (type2 < 0.5)
            {
                stats2 = GenerateSleepers();

            }
            // artillery
            else if (type2 < 0.75)
            {
                stats2 = GenerateArtillery();

            }
            // turrets
            else
            {
                stats2 = GenerateTurrets();
            }


            // group 3
            if (level > 2)
            {
                double type3 = random.NextDouble();

                // Artillery
                if (type3 < 0.25)
                {
                    stats3 = GenerateArtillery();
                }
                // Turrets
                else if (type3 < 0.5)
                {
                    stats3 = GenerateTurrets();

                }
                // Specials
                else if (type3 < 0.75)
                {
                    stats3 = GenerateSpecials();

                }
                // Sleepers
                else if (type3 < 0.9)
                {
                    stats3 = GenerateSleepers();

                }
                // Grunts
                else
                {
                    stats3 = GenerateGrunts();
                }
            }


            // generate positions, fill spawn list

            List<SpriteBase> spawnList = new List<SpriteBase> {};
             
            // group 1               
            int pos = random.Next(4);
                                
            Vector2 offset = new Vector2((float)(random.NextDouble() * 60.0 - 15), (float)(random.NextDouble() * 60.0 - 15));

            spawnList.Add(new EnemyGroup(stats1.Item3, SpawnTex, spawnPoints[pos] + offset, stats1.Item4, stats1.Item1, stats1.Item2, dim, missile, crossTex));

            // group 2
            pos = random.Next(4);
            if (stats2.Item2 == 3)
            {
                missile = bombTex;
            }
            else
            {
                missile = MissileTex;
            }

            offset = new Vector2((float)(random.NextDouble() * 60.0 - 15), (float)(random.NextDouble() * 60.0 - 15));

            spawnList.Add(new EnemyGroup(stats2.Item3, SpawnTex, spawnPoints[pos] + offset, stats2.Item4, stats2.Item1, stats2.Item2, dim, missile, crossTex));

            // group 3
            if (level > 2)
            {
                pos = random.Next(4);
                if (stats3.Item2 == 3)
                {
                    missile = bombTex;
                }
                else
                {
                    missile = MissileTex;
                }

                offset = new Vector2((float)(random.NextDouble() * 60.0 - 15), (float)(random.NextDouble() * 60.0 - 15));

                spawnList.Add(new EnemyGroup(stats3.Item3, SpawnTex, spawnPoints[pos] + offset, stats3.Item4, stats3.Item1, stats3.Item2, dim, missile, crossTex));
            }            
            return spawnList;
        }

        public List<SpriteBase> SpawnPowerUps()

        {
            // Power Up spawning
            Vector2 offset;
            int pos = random.Next(4);
            double chance = random.NextDouble();
            List<SpriteBase> spawnList = new List<SpriteBase> { };

            double choice = random.NextDouble();
            offset = new Vector2((float)(random.NextDouble() * 60.0 - 15), (float)(random.NextDouble() * 60.0 - 15));

            if (choice < veryCommon)
            {
                // very common
                int num = random.Next(2);

                if (num == 0)
                {
                    spawnList.Add(new PowerUpBase(powerupTex, heartTex,spawnPoints[pos],1));
                }
                else
                {
                    spawnList.Add(new PowerUpBase(powerupTex, heartTex,spawnPoints[pos],2));
                }

            }
            else if (choice < veryCommon + common)
            {
                // common
                if (ScoreHelper.Hardcore == false)
                {
                    int num = random.Next(3);

                    if (num == 0)
                    {
                        if (speedCount < 2)
                        {
                            spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 13));
                            speedCount += 1;
                        }
                        else
                        {
                            spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 13));
                            speedCount += 1;
                        }
                    }
                    else if (num == 1)
                    {
                        if (speedCount > -2)
                        {
                            spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 14));
                            speedCount -= 1;
                        }
                        else
                        {
                            spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 13));
                            speedCount += 1;
                        }
                    }
                    else
                    {
                        spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 15));
                    }
                }
                else
                {
                    int num = random.Next(2);

                    if (num == 0)
                    {
                        spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 13));
                    }
                    else
                    {
                        spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 14));
                    }
                }
            }
            else if (choice < veryCommon + common + uncommon)
            {
                // uncommon
                int num = random.Next(10);

                if (num == 0)
                {
                    spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 3));
                }
                else if (num == 1)
                {
                    spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 5));
                }
                else if (num == 2)
                {
                    spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 6));
                }
                else if (num == 3)
                {
                    spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 7));
                }
                else if (num == 4)
                {
                    if (orbitCount < 2)
                    {
                        spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 8));
                        orbitCount += 1;
                    }
                    else
                    {
                        spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 9));
                        orbitCount -= 1;
                    }
                }
                else if (num == 5)
                {
                    if (orbitCount > -2)
                    {
                        spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 9));
                        orbitCount -= 1;
                    }
                    else
                    {
                        spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 8));
                        orbitCount += 1;
                    }
                }
                else if (num == 6)
                {
                    if (radiusCount < 2)
                    {
                        spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 11));
                        radiusCount += 1;
                    }
                    else
                    {
                        spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 12));
                        radiusCount -= 1;
                    }
                }
                else if (num == 7)
                {
                    if (radiusCount > -2)
                    {
                        spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 12));
                        radiusCount -= 1;
                    }
                    else
                    {
                        spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 11));
                        radiusCount += 1;
                    }
                }
                else if (num == 8)
                {
                    spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 17));
                }
                else
                {
                    spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 18));
                }
            }
            else if (choice < veryCommon + common + uncommon + rare)
            {
                // rare
                int num = random.Next(10);

                if (num == 0)
                {
                    spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 4));
                }
                else if (num == 1)
                {
                    spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 10));
                }
                else if (num == 2)
                {
                    spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 16));
                }
                else if (num == 3)
                {
                    spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 19));
                }
                else
                {
                    spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 20));
                }
            }
            else
            {
                // very rare
                spawnList.Add(new PowerUpBase(powerupTex, heartTex, spawnPoints[pos], 21));
            }

            return spawnList;
        }        
    }
}
