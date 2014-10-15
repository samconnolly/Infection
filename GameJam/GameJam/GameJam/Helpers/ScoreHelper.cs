﻿using System;
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
        private static int _lives2 = 3;
        private static List<int> _highscores = new List<int> {0,0,0,0,0,0,0,0,0,0};
        private static List<Virus> _livePlayers = new List<Virus> { };
        private static List<Virus> _deadPlayers = new List<Virus> { };

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

        public static int LivesP2
        {
            get { return _lives2; }
            set { _lives2 = value; }
        }

        public static List<Virus> DeadPlayers
        {
            get { return _deadPlayers; }
            set { _deadPlayers = value; }
        }

         public static List<Virus> LivePlayers
        {
            get { return _livePlayers; }
            set { _livePlayers = value; }
        }

        public static void PlayerHit(Virus player)
        {
            if (VirusHelper.Virus.invincible == false)
            {
                player.hit = true;

                if (_hardcore == true)
                {
                    if (InputHelper.Players == 1)
                    {
                        SoundEffectPlayer.PlaySquelch();
                        GameStateManager.CurrentGameState = GameState.GameOver;
                        GameStateManager.HasChanged = true;
                    }
                    else if (InputHelper.Players == 2)
                    {
                        if (player.player == 1 && VirusHelper.VirusP2.dead == false)
                        {
                            _deadPlayers.Add(VirusHelper.Virus);
                            SoundEffectPlayer.PlaySquelch();
                            player.dead = true;
                        }
                        else if (player.player == 2 && VirusHelper.Virus.dead == false)
                        {
                            _deadPlayers.Add(VirusHelper.VirusP2);
                            SoundEffectPlayer.PlaySquelch();
                            player.dead = true;
                            
                        }
                        else
                        {
                            SoundEffectPlayer.PlaySquelch();
                            GameStateManager.CurrentGameState = GameState.GameOver;
                            GameStateManager.HasChanged = true;
                        }
                    }
                }

                else
                {
                    SoundEffectPlayer.PlaySquelch();

                    if (player.player == 1)
                    {
                        _lives -= 1;
                    }
                    else
                    {
                        _lives2 -= 1;
                    }

                    if (InputHelper.Players == 1 && _lives == 0)
                    {                    
                        GameStateManager.CurrentGameState = GameState.GameOver;
                        GameStateManager.HasChanged = true;
                    }
                    else if (InputHelper.Players == 2 && _lives == 0 && _lives2 == 0)
                    {
                        GameStateManager.CurrentGameState = GameState.GameOver;
                        GameStateManager.HasChanged = true;
                    }
                    else if (InputHelper.Players == 2 && _lives == 0)
                    {
                        _deadPlayers.Add(VirusHelper.Virus);
                    }
                    else if (InputHelper.Players == 2 && _lives2 == 0)
                    {
                        _deadPlayers.Add(VirusHelper.VirusP2);
                    }
                }

                if ( _livePlayers.Count() < 1)
                {
                     SoundEffectPlayer.PlaySquelch();
                     GameStateManager.CurrentGameState = GameState.GameOver;
                     GameStateManager.HasChanged = true;
                }
            }
        }

    }
}
