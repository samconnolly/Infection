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


        Texture2D WBCTex;
        Texture2D WBCTexHit;
        Texture2D WBCTexSpawn;
        
        Texture2D RBCTex;
        Texture2D GBCTex;
        Texture2D BBCTex;
        Texture2D PBCTex;
        Texture2D OBCTex;
        Texture2D MissileTex;
        Texture2D bombTex;
        Texture2D crossTex;

        Texture2D proTex;
        Texture2D doubleTex;
        Texture2D reproTex;

        Random random = new Random();

        public SpawnII(Texture2D WBCTexture, Texture2D WBCTextureHit, Texture2D WBCTextureSpawn, 
                        Texture2D RBCTexture, Texture2D proTexture, Texture2D doubleTexture, Texture2D reproTexture,
                            Texture2D GBCTexture, Texture2D GBCTextureHit, Texture2D GBCTextureSpawn,
                                Texture2D BBCTexture, Texture2D BBCTextureHit, Texture2D BBCTextureSpawn, Texture2D missileTexture,
                                    Texture2D PBCTexture, Texture2D PBCTextureHit, Texture2D PBCTextureSpawn,
                                        Texture2D OBCTexture, Texture2D OBCTextureHit, Texture2D OBCTextureSpawn, Texture2D crossTexture, Texture2D bombTexture)
        {
            spawnPoints.Add(new Vector2(150,150));
            spawnPoints.Add(new Vector2(150,570));
            spawnPoints.Add(new Vector2(930,150));
            spawnPoints.Add(new Vector2(930,570));

            WBCTex = WBCTexture;
            WBCTexHit = WBCTextureHit;
            WBCTexSpawn = WBCTextureSpawn;
            
            RBCTex = RBCTexture;
            GBCTex = GBCTexture;
            BBCTex = BBCTexture;
            PBCTex = PBCTexture;
            OBCTex = OBCTexture;
            MissileTex = missileTexture;
            bombTex = bombTexture;
            crossTex = crossTexture;

            proTex = proTexture;
            doubleTex = doubleTexture;
            reproTex = reproTexture;
        }


        public List<SpriteBase> SpawnEnemies(int n)
        {
            List<SpriteBase> spawnList = new List<SpriteBase> {};

            for (int x = 0; x < n; x++)
            {
                int num = random.Next(3) + 2;
                
                int pos = random.Next(4);
                                
                Vector2 offset = new Vector2((float)(random.NextDouble() * 60.0 - 15), (float)(random.NextDouble() * 60.0 - 15));

                spawnList.Add(new EnemyGroup(WBCTex, WBCTexHit,WBCTexSpawn,spawnPoints[pos]+offset, num,1,3,MissileTex,crossTex));         
                
            }


            // Power Up spawning

            double chance = random.NextDouble();

            if (chance < 0.4f)
            {
                int choice = random.Next(3);
                Vector2 offset = new Vector2((float)(random.NextDouble() * 60.0 - 15), (float)(random.NextDouble() * 60.0 - 15));

                if (choice == 0)
                {
                    int pos = random.Next(4);

                    spawnList.Add(new Proliferate(proTex,spawnPoints[pos] + offset));
                }

                if (choice == 1)
                {
                    int pos = random.Next(4);

                    spawnList.Add(new DoubleUp(doubleTex, spawnPoints[pos] + offset));
                }

                if (choice == 2)
                {
                    int pos = random.Next(4);

                    spawnList.Add(new Reproduce(reproTex, spawnPoints[pos] + offset));
                }
            }

            return spawnList;
        }

        public List<SpriteBase> SpawnRed(int n)
        {
            List<SpriteBase> spawnList = new List<SpriteBase> { };

            for (int x = 0; x < n; x++)
            {
                int num = random.Next(2) + 2;
                                
                int pos = random.Next(4);

                Vector2 offset = new Vector2((float)(random.NextDouble() * 60.0 - 15), (float)(random.NextDouble() * 60.0 - 15));

                spawnList.Add(new RedBloodCellGroup(RBCTex, spawnPoints[pos] + offset, num));
                
            }

            return spawnList;
        }

    }
}
