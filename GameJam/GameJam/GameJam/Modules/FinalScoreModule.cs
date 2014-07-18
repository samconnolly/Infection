using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJam
{
    public class FinalScoreModule : ModuleBase
    {
        private SpriteFont _font;
        private string _title = "0";
        private Vector2 _position;
        private Texture2D _finalTexture;

        public FinalScoreModule(Game game)
            :base(game)
        {

        }

        #region ModuleBase Overrides

        public override bool IsMouseVisible
        {
            get { return false; }
        }

        internal override void Initialize()
        {
            _title = ScoreHelper.Score.ToString();
        }

        internal override void LoadContent(SpriteBatch batch)
        {
            //Final Texture
            _finalTexture = this.Game.Content.Load<Texture2D>("Final Score");
            _font = this.Game.Content.Load<SpriteFont>("font2");
            Vector2 fontDim = _font.MeasureString(_title);
            _position = new Vector2((ViewPortHelper.X / (ViewPortHelper.XScale*2.0f) - (fontDim.X / 2)), ViewPortHelper.Y / (ViewPortHelper.YScale*2.0f));
        }

        internal override void UnloadContent()
        {
            
        }

        internal override void Update(GameTime gameTime, SpriteBatch batch)
        {
            if (InputHelper.WasButtonPressed(Keys.R) || InputHelper.WasPadButtonPressedP1(Buttons.A))
            {
                GameStateManager.CurrentGameState = GameState.InGame;
                GameStateManager.HasChanged = true;
            }
            if (InputHelper.WasButtonPressed(Keys.Escape) || InputHelper.WasPadButtonPressedP1(Buttons.B))
            {
                GameStateManager.CurrentGameState = GameState.MainMenu;
                GameStateManager.HasChanged = true;
            }
        }

        internal override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.DrawString(_font, _title, _position, Color.Black,0.0f,Vector2.Zero,1.0f,SpriteEffects.None,0.0f);
            batch.Draw(_finalTexture, new Vector2(0, 0), Color.White);
        }

        #endregion
    }
}
