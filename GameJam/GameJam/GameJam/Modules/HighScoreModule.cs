using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJam
{
    public class HighScoreModule : ModuleBase
    {
        private SpriteFont _font;
        private SpriteFont _font2;
        private string _title = "0";
        private Vector2 _position;
        private Texture2D _finalTexture;
        private Texture2D _b_button;

        public HighScoreModule(Game game)
            :base(game)
        {

        }

        #region ModuleBase Overrides

        

        internal override void Initialize()
        {
            _title = ScoreHelper.Score.ToString();
            IsMouseVisible = true;
        }

        internal override void LoadContent(SpriteBatch batch)
        {
            //Final Texture
            _finalTexture = this.Game.Content.Load<Texture2D>("HighScore");
            _font = this.Game.Content.Load<SpriteFont>("font2");
            _font2 = this.Game.Content.Load<SpriteFont>("font");
            _b_button = this.Game.Content.Load<Texture2D>("b_button");
            Vector2 fontDim = _font.MeasureString(_title);
            _position = new Vector2(540, 20);

            if (ScoreHelper.Score > ScoreHelper.HighScores[ScoreHelper.HighScores.Count() - 1])
            {
                int n = 0;
                foreach (int s in ScoreHelper.HighScores)
                {
                    if (ScoreHelper.Score > s)
                    {
                        ScoreHelper.HighScores.Insert(n, ScoreHelper.Score);
                        ScoreHelper.HighScores.RemoveAt(ScoreHelper.HighScores.Count() - 1);
                        break;
                    }
                    n += 1;
                }
            }
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
            batch.DrawString(_font, "High Scores", _position + new Vector2(-100, 50), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

            int n = 0;
            foreach (int score in ScoreHelper.HighScores)
            {
                batch.DrawString(_font2, (n+1).ToString() + ". " + score.ToString(), _position + new Vector2(-250, 150 + n*40), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                n += 1;
            }

            batch.Draw(_finalTexture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            batch.Draw(_b_button, new Vector2(830, 610), new Rectangle(0, 0, _b_button.Width, _b_button.Height), Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.49f);
            batch.DrawString(_font2, "Back", new Vector2(900, 615), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
        }

        #endregion
    }
}
