using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameJam
{
    public static class ScoreHelper
    {
        private static bool _hardcore = true;
        private static int _score;
        private static int _lives = 3;
        private static List<int> _highscores = new List<int> {0,0,0,0,0,0,0,0,0,0};

        public static int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public static List<int> HighScores
        {
            get { return _highscores; }
            set { _highscores = value; }
        }

        public static bool Hardcore
        {
            get { return _hardcore; }
            set { _hardcore = value; }
        }

        public static int Lives
        {
            get { return _lives; }
            set { _lives = value; }
        }

        public static void PlayerHit()
        {
            if (_hardcore == true)
            {
                SoundEffectPlayer.PlaySquelch();
                GameStateManager.CurrentGameState = GameState.GameOver;
                GameStateManager.HasChanged = true;
            }

            else
            {
                _lives -= 1;

                if (_lives == 0)
                {
                    SoundEffectPlayer.PlaySquelch();
                    GameStateManager.CurrentGameState = GameState.GameOver;
                    GameStateManager.HasChanged = true;
                }
            }
        }

    }
}
