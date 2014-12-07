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
        private List<int> spawnedItems = new List<int> { };

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
        Texture2D powerupTextTex;
        Texture2D heartTex;
        Texture2D circleTex;

        Vector2 dim = new Vector2(6, 5);

        Tuple<int, int, Texture2D,int> stats1;
        Tuple<int, int, Texture2D, int> stats2;
        //Tuple<int, int, Texture2D, int> stats3;
        List<Tuple<int, int, Texture2D, int>> stats;

        private float nanitePowerUp = 0.45f;
        private float physicsPowerUp = 0.25f;
        private float speedPowerUp = 0.25f;
        //private float healthPowerUp = 0.05f;

        int movement;
        int attack;

        Random random = new Random();

         public SpawnII(Texture2D GruntTexture, Texture2D ChargerTexture,
                                Texture2D SleeperTexture, Texture2D TurretTexture, Texture2D ArtilleyTexture,
                                        Texture2D missileTexture, Texture2D crossTexture, Texture2D bombTexture, Texture2D TextureSpawn,
                                            Texture2D powerupTexture, Texture2D powerupTextTexture, Texture2D heartTexture, Texture2D circleTexture)
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
            circleTex = circleTexture;

            powerupTex = powerupTexture;
            powerupTextTex = powerupTextTexture;
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

            return new Tuple<int, int, Texture2D, int>(movement, attack, ArtilleyTex,1);
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

            stats = new List<Tuple<int, int, Texture2D, int>> { stats1, stats2 };

            // group 3
            if (level > 2)
            {
                for (int i=0; i<(level-2); i++)
                {
                    Tuple<int, int, Texture2D, int> nextStats;
                    double type3 = random.NextDouble();

                    // Artillery
                    if (type3 < 0.25)
                    {
                        nextStats = GenerateArtillery();
                    }
                    // Turrets
                    else if (type3 < 0.5)
                    {
                        nextStats = GenerateTurrets();

                    }
                    // Specials
                    else if (type3 < 0.75)
                    {
                        nextStats = GenerateSpecials();

                    }
                    // Sleepers
                    else if (type3 < 0.9)
                    {
                        nextStats = GenerateSleepers();

                    }
                    // Grunts
                    else
                    {
                        nextStats = GenerateGrunts();
                    }

                    stats.Add(nextStats);
                }
            }


            // generate positions, fill spawn list

            List<SpriteBase> spawnList = new List<SpriteBase> {};
             
            foreach (Tuple<int, int, Texture2D, int> stat in stats)
            {
                int pos = random.Next(4);
                if (stat.Item2 == 3)
                {
                    missile = bombTex;
                }
                else
                {
                    missile = MissileTex;
                }

                Vector2 offset = new Vector2((float)(random.NextDouble() * 60.0 - 15), (float)(random.NextDouble() * 60.0 - 15));

                spawnList.Add(new EnemyGroup(stat.Item3, SpawnTex, spawnPoints[pos] + offset, stat.Item4, stat.Item1, stat.Item2, dim, missile, crossTex, circleTex));
            }  
            return spawnList;
        }

        public PowerUpBase SpawnPowerUp(Vector2 position)
        {
            int type = ChooseNormalPowerUp();    

                PowerUpBase powerUp = new PowerUpBase(powerupTex, heartTex, powerupTextTex, position, type);
                    
            return powerUp;
        }

        public PowerUpBase SpawnItem(Vector2 position)
        {
            int type = ChooseSpecialPowerUp();
            while (spawnedItems.Contains(type) == true | (ScoreHelper.Hardcore == true && type == 12))
            {
                type = ChooseSpecialPowerUp();
            }
                
            PowerUpBase powerUp = new PowerUpBase(powerupTex, heartTex, powerupTextTex, position, type);
            if (type >= 16 | type == 4 | type == 10)
            {
                spawnedItems.Add(type);
            }
       
            return powerUp;
        }

        public int ChooseNormalPowerUp()
        {
            int spawnChoice = 0;

            double chance = random.NextDouble();

            double choice = random.NextDouble();

            if (choice < nanitePowerUp)
            {
                // nanites
                double num = random.NextDouble()*100;

                if (num <= 50)
                {
                    spawnChoice = 1;
                }
                else if (num <= 50 + 25)
                {
                    spawnChoice = 2;
                }
                else if (num <= 50 + 25 + 15)
                {
                    spawnChoice = 5;
                }
                else
                {
                    spawnChoice = 6;
                }

            }
            else if (choice < nanitePowerUp + physicsPowerUp)
            {
                // physics
                
                double num = random.NextDouble()*100;
                

                if (num <= 16)
                {
                    spawnChoice = 7;
                }
                else if (num <= 16 + 25)
                {
                    spawnChoice = 8;
                }
                else if (num <= 16 + 25 + 16)
                {
                    spawnChoice = 9;
                }
                else if (num <= 16 + 25 + 16 + 25)
                {
                    spawnChoice = 11;
                }
                else
                {
                    spawnChoice = 12;
                }
            }
            else if (choice < nanitePowerUp + physicsPowerUp + speedPowerUp)
            {
                // speed
                double num = random.NextDouble() * 100;


                if (num <= 75)
                {
                    spawnChoice = 13;
                }                
                else
                {
                    spawnChoice = 14;
                }
            }
            else
            {
                // health up
                spawnChoice = 15;
            }

            return spawnChoice;
        }

        public int ChooseSpecialPowerUp()
        {
            int spawnChoice = 0;

            double chance = random.NextDouble();

            if (chance <= 0.16)
            {
                spawnChoice = 3;
            }
            else if (chance <= 0.23)
            {
                spawnChoice = 4;
            }
            else if (chance <= 0.30)
            {
                spawnChoice = 10;
            }
            else if (chance <= 0.37)
            {
                spawnChoice = 16;
            }
            else if (chance <= 0.49)
            {
                spawnChoice = 17;
            }
            else if (chance <= 0.61)
            {
                spawnChoice = 18;
            }
            else if (chance <= 0.68)
            {
                spawnChoice = 19;
            }
            else if (chance <= 0.75)
            {
                spawnChoice = 20;
            }
            else if (chance <= 0.79)
            {
                spawnChoice = 21;
            }
            else if (chance <= 0.86)
            {
                spawnChoice = 2;
            }
            else if (chance <= 0.93)
            {
                spawnChoice = 15;
            }
            else
            {
                spawnChoice = 13;
            }

            return spawnChoice;
        }


        //public List<SpriteBase> SpawnPowerUps(int n, int wave)

        //{
        //    // Power Up spawning
        //    Vector2 offset;
        //    List<int> positions = new List<int> { };
        //    for (int i = 0; i < n; i++)
        //    {
        //        int pos = random.Next(4);
        //        while (positions.Contains(pos))
        //        {
        //            pos += 1;
        //            if (pos >= n)
        //            {
        //                pos = 0;
        //            }
        //        }
        //        positions.Add(pos);
        //    }

        //    List<SpriteBase> spawnList = new List<SpriteBase> { };
        //    List<int> spawnTypeList = new List<int> { };

        //    int num = 0;
        //    foreach (int pos in positions)
        //    {
        //        int spawnChoice = ChoosePowerUps(num,wave);

        //        while (spawnTypeList.Contains(spawnChoice))
        //        {
        //            spawnChoice = ChoosePowerUps(num,wave);
        //        }

        //        offset = new Vector2((float)(random.NextDouble() * 60.0 - 15), (float)(random.NextDouble() * 60.0 - 15));
        //        spawnList.Add(new PowerUpBase(powerupTex, heartTex, powerupTextTex, spawnPoints[pos] + offset, spawnChoice));
        //        spawnTypeList.Add(spawnChoice);
        //        num += 1;
        //    }


        //    return spawnList;
        //}
    }
}
