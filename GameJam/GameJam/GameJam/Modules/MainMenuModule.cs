﻿using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using Microsoft.CSharp;
using System.Windows.Forms;
using System.Diagnostics;


namespace GameJam
{
    public class MainMenuModule : ModuleBase
    {
        //private Rectangle _drawRectangle;
        private Vector2 _position = new Vector2(0, 0);
        private Texture2D _background;
        private Rectangle _backRect;
        private bool _isPlayingMusic = false;
        private bool _isMuted = false;
        private Song music;

        private SpriteFont font;
        private List<Color> colours;
        private int selected = 0;
        private int max = 5;
        private int tree = 0;
        private bool mouseover = false;

        private Vector2 menuOffset = new Vector2(450, 350);
              
        public MainMenuModule(Game game)
            :base(game)
        {

        }

        #region ModuleBase Overrides


        internal override void Initialize()
        {
            IsMouseVisible = true;
        }

        internal override void LoadContent(SpriteBatch batch)
        {
            _background = this.Game.Content.Load<Texture2D>("Title screen copy");
            _backRect = new Rectangle(0, 0, _background.Width, _background.Height);

            //Load atmospheric music.
            music = this.Game.Content.Load<Song>("Controlled Chaos");

            // fonts
            font = this.Game.Content.Load<SpriteFont>("font");

        }

        internal override void UnloadContent()
        {
            
        }

        internal override void Update(GameTime gameTime, SpriteBatch batch)
        {
            //Play music if not playing already.
            if (!_isPlayingMusic)
            {
                MediaPlayer.Play(music);
                MediaPlayer.IsRepeating = true;
                _isPlayingMusic = true;
            }


            // menu control
            mouseover = false;

            if (InputHelper.WasButtonPressed(Microsoft.Xna.Framework.Input.Keys.Up) || InputHelper.WasPadThumbstickUpP1())
            {
                selected -= 1;
            }
            else if (InputHelper.WasButtonPressed(Microsoft.Xna.Framework.Input.Keys.Down) || InputHelper.WasPadThumbstickDownP1())
            {
                selected += 1;
            }
            else if (InputHelper.CurrentMouseState.X > menuOffset.X && InputHelper.CurrentMouseState.X < menuOffset.X + 200)
            {
                for (int i = 0; i < max + 1; i++)
                {
                    if (InputHelper.CurrentMouseState.Y >= menuOffset.Y + i * 50 && InputHelper.CurrentMouseState.Y < menuOffset.Y + (i + 1) * 50)
                    {
                        if (InputHelper.CurrentMouseState.Y != InputHelper.PreviousMouseState.Y)
                        {
                            selected = i;
                        }
                        mouseover = true;
                    }
                }
            }

            if (selected > max) { selected = 0; }
            if (selected < 0) { selected = max; }
            
            if ((InputHelper.WasMouseClicked() && mouseover == true) | InputHelper.WasButtonPressed(Microsoft.Xna.Framework.Input.Keys.Enter) || InputHelper.WasButtonPressed(Microsoft.Xna.Framework.Input.Keys.Space) 
                    || InputHelper.WasPadButtonPressedP1(Buttons.A))
            {
                if (tree == 0)
                {
                    if (selected == 0)
                    {
                        InputHelper.Players = 1;
                        tree = 1;
                        max = 2;
                    }
                    else if (selected == 1)
                    {
                        InputHelper.Players = 2;
                        tree = 1;
                        selected = 0;
                        max = 2;
                    }
                    else if (selected == 2)
                    {
                        tree = 2;
                        selected = 0;
                        max = 4;
                    }
                    else if (selected == 3)
                    {
                        tree = 3;
                        selected = 0;
                        max = 2;
                    }
                    else if (selected == 4)
                    {
                        GameStateManager.CurrentGameState = GameState.Highscore;
                        GameStateManager.HasChanged = true;
                    }
                    else if (selected == 5)
                    {
                        Game.Exit();
                    }
                }

                else if (tree == 1)
                {
                    if (selected == 0)
                    {
                        ScoreHelper.Hardcore = true;
                        GameStateManager.CurrentGameState = GameState.InGame;
                        GameStateManager.HasChanged = true;
                    }
                    else if (selected == 1)
                    {
                        ScoreHelper.Hardcore = false;
                        GameStateManager.CurrentGameState = GameState.InGame;
                        GameStateManager.HasChanged = true;
                    }
                    else
                    {
                        tree = 0;
                        selected = 0;
                        max = 5;
                    }
                    
                }

                else if (tree == 2)
                {
                    if (selected == 0)
                    {
                        ViewPortHelper.ToggleFullscreen();
                    }

                    else if (selected == 1)
                    {
                        float newVolume = MediaPlayer.Volume + 0.1f;
                        if (newVolume > 1.09999f)
                        {
                            newVolume = 0;
                        }
                        else if (newVolume > 1.0f)
                        {
                            newVolume = 1.0f;
                        }      
                        MediaPlayer.Volume = newVolume;
                    }
                    else if (selected == 2)
                    {
                        float newVolume = SoundEffectPlayer.Volume + 0.1f;
                        if (newVolume > 1.09999f)
                        {
                            newVolume = 0;
                        }
                        else if (newVolume > 1.0f)
                        {
                            newVolume = 1.0f;
                        }
                        SoundEffectPlayer.AdjustVolume(newVolume);
                    }
                    else if (selected == 3)
                    {
                        if (_isMuted)
                        {
                            MediaPlayer.Resume();
                            _isMuted = false;
                        }
                        else
                        {
                            MediaPlayer.Pause();
                            _isMuted = true;
                        }
                    }
                    else
                    {
                        tree = 0;
                        selected = 0;
                        max = 5;
                    }
                }

                else if (tree == 3)
                {
                    

                    if (selected == 0)
                    {
                        // controls!
                    }
                    else if (selected == 1)
                    {
                        if (InputHelper.ForceKeys == false)
                        {
                            InputHelper.ForceKeys = true;
                        }
                        else
                        {
                            InputHelper.ForceKeys = false;
                        }
                    }
                    else
                    {
                        tree = 0;
                        selected = 0;
                        max = 5;
                    }
                }

                
            }
            if (tree != 0)
            {
                if (InputHelper.WasPadButtonPressedP1(Buttons.B))
                {
                    tree = 0;
                    selected = 0;
                    max = 5;
                }
            }
            // exit game
            if (InputHelper.WasButtonPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Game.Exit();
            }
            
            // switch number of players
            //if (InputHelper.WasButtonPressed(Microsoft.Xna.Framework.Input.Keys.P) || InputHelper.WasPadButtonPressedP1(Buttons.Y))
            //{
            //    if (players == 1)
            //    {
            //        players = 2;
            //    }

            //    else
            //    {
            //        players = 1;
            //    }

            //    InputHelper.Players = players;
            //}

            // toggle fullscreen
            
            if (InputHelper.WasButtonPressed(Microsoft.Xna.Framework.Input.Keys.F) || InputHelper.WasPadButtonPressedP1(Buttons.X))
            {
                ViewPortHelper.ToggleFullscreen();                
            }

            // key assignments
            if (InputHelper.ForceKeys == true)
            {
                InputHelper.Keys = 1;
            }
            else if (InputHelper.CurrentGamePadStatePlayer1.IsConnected == false && InputHelper.CurrentGamePadStatePlayer2.IsConnected == false)
            {
                InputHelper.Keys = 1;
            }
            else if (InputHelper.CurrentGamePadStatePlayer1.IsConnected == false && InputHelper.CurrentGamePadStatePlayer2.IsConnected == true)
            {
                InputHelper.Keys = 1;
            }
            else if (InputHelper.CurrentGamePadStatePlayer1.IsConnected == true && InputHelper.CurrentGamePadStatePlayer2.IsConnected == false)
            {
                InputHelper.Keys = 2;
            }
            else
            {
                InputHelper.Keys = 0;
            }

            //Mute music
            if (InputHelper.WasButtonPressed(Microsoft.Xna.Framework.Input.Keys.M) || InputHelper.WasPadButtonPressedP1(Buttons.Back))
            {
                if (_isMuted)
                {
                    MediaPlayer.Resume();
                    _isMuted = false;
                }
                else
                {
                    MediaPlayer.Pause();
                    _isMuted = true;
                }
            }

            colours = new List<Color> { Color.White, Color.White, Color.White, Color.White, Color.White, Color.White };
            colours[selected] = Color.Black;
        }

        internal override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.Draw(_background, new Vector2(0, 0), _backRect, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

            if (tree == 0)
            {
                batch.DrawString(font, "New Game", menuOffset, colours[0], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.DrawString(font, "Multiplayer", menuOffset + new Vector2(0, 50), colours[1], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.DrawString(font, "Options", menuOffset +  new Vector2(0, 100), colours[2], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.DrawString(font, "Controls", menuOffset +  new Vector2(0, 150), colours[3], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.DrawString(font, "Leaderboard", menuOffset + new Vector2(0, 200), colours[4], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.DrawString(font, "Exit", menuOffset + new Vector2(0, 250), colours[5], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            }
            else if (tree == 1)
            {
                batch.DrawString(font, "Hardcore", menuOffset + new Vector2(0, 0), colours[0], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.DrawString(font, "Easy", menuOffset + new Vector2(0, 50), colours[1], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.DrawString(font, "Back", menuOffset + new Vector2(0, 100), colours[2], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            }

            else if (tree == 2)
            {
                batch.DrawString(font, "Toggle Fullscreen", menuOffset + new Vector2(0, 0), colours[0], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.DrawString(font, "Music Volume:" + ((int)(MediaPlayer.Volume * 100)).ToString(), menuOffset + new Vector2(0, 50), colours[1], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.DrawString(font, "SFX Volume:" + ((int)(SoundEffectPlayer.Volume * 100)).ToString(), menuOffset + new Vector2(0, 100), colours[2], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.DrawString(font, "Mute Music", menuOffset + new Vector2(0, 150), colours[3], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.DrawString(font, "Back", menuOffset + new Vector2(0, 200), colours[4], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            }
            else if (tree == 3)
            {
                // get info
                string p1 = "pad";
                string p2 = "pad";
                if (InputHelper.Keys == 1)
                {
                    p1 = "keys";
                }
                else if (InputHelper.Keys == 2)
                {
                    p2 = "keys";
                }

                // Display info
                batch.DrawString(font, "Pad 1 Connected - " + InputHelper.CurrentGamePadStatePlayer1.IsConnected.ToString() + "       "
                                        + "Pad 2 Connected - " + InputHelper.CurrentGamePadStatePlayer2.IsConnected.ToString(), menuOffset + new Vector2(-250, 0),
                                            Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

                batch.DrawString(font, "P1 - " + p1.ToString() + "       " + "P2 - " + p2.ToString(), menuOffset + new Vector2(-50, 50), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

                // options
                batch.DrawString(font, "Show Controls", menuOffset + new Vector2(0, 150), colours[0], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.DrawString(font, "Toggle keys or pad", menuOffset + new Vector2(0, 200), colours[1], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                batch.DrawString(font, "Back", menuOffset + new Vector2(0, 250), colours[2], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

            }

        }

        #endregion
    }
}
