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
    public class Bomb : SpriteBase
    {
        private int fallTime;
        private int fallTimer = 0;
        private int boomTime = 1000;
        private Texture2D tex;
        private Texture2D crossTex;

        public Bomb(Texture2D texture,Texture2D CrossTexture, Vector2 target, int falltime)
            : base(texture)
        {
            tex = texture;
            crossTex = CrossTexture;
            fallTime = falltime;

            this.Texture = crossTex;
            this.Position = target;
        }

        public override void Update(GameTime gameTime, SpriteBatch bactch)
        {
            fallTimer += gameTime.ElapsedGameTime.Milliseconds;

            
            if (fallTimer > fallTime + boomTime)
            {
                DeathHelper.KillCell.Add(this);
            }

            else if (fallTimer > fallTime)
            {
                this.Texture = tex;
            }

            if ((this.Position - VirusHelper.VirusPosition).Length() < tex.Width && fallTimer > fallTime)
            {
                SoundEffectPlayer.PlaySquelch();
                GameStateManager.CurrentGameState = GameState.GameOver;
                GameStateManager.HasChanged = true;
            }
            else if (InputHelper.Players == 2)
            {
                if ((this.Position - VirusHelper.VirusPositionP2).Length() < tex.Width && fallTimer > fallTime)
                {
                    SoundEffectPlayer.PlaySquelch();
                    GameStateManager.CurrentGameState = GameState.GameOver;
                    GameStateManager.HasChanged = true;
                }
            }
        }
    }
}

