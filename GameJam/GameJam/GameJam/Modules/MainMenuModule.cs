using System;
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
        private int players = 1;
              
        public MainMenuModule(Game game)
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

            if (InputHelper.WasButtonPressed(Microsoft.Xna.Framework.Input.Keys.Space) || InputHelper.WasPadButtonPressedP1(Buttons.A))
            {
                GameStateManager.CurrentGameState = GameState.InGame;
                GameStateManager.HasChanged = true;
            }

            // exit game
            if (InputHelper.WasButtonPressed(Microsoft.Xna.Framework.Input.Keys.Escape) || InputHelper.WasPadButtonPressedP1(Buttons.B))
            {
                Game.Exit();
            }

            // switch number of players
            if (InputHelper.WasButtonPressed(Microsoft.Xna.Framework.Input.Keys.P) || InputHelper.WasPadButtonPressedP1(Buttons.Y))
            {
                if (players == 1)
                {
                    players = 2;
                }

                else
                {
                    players = 1;
                }

                InputHelper.Players = players;
            }

            // toggle fullscreen
            
            if (InputHelper.WasButtonPressed(Microsoft.Xna.Framework.Input.Keys.F) || InputHelper.WasPadButtonPressedP1(Buttons.X))
            {
                if (ViewPortHelper.GraphicsDeviceManager.IsFullScreen == true)
                {
                    ViewPortHelper.GraphicsDeviceManager.PreferredBackBufferHeight = ViewPortHelper.WindowedHeight;
                    ViewPortHelper.GraphicsDeviceManager.PreferredBackBufferWidth = ViewPortHelper.WindowedWidth;

                    ViewPortHelper.GraphicsDeviceManager.IsFullScreen = false;
                    ViewPortHelper.GraphicsDeviceManager.ApplyChanges();

                    ViewPortHelper.GraphicsDevice.Viewport = new Viewport(0, 0, ViewPortHelper.WindowedWidth, ViewPortHelper.WindowedHeight);
                    ViewPortHelper.SetViewPort(ViewPortHelper.WindowedWidth, ViewPortHelper.WindowedHeight);
                    ViewPortHelper.SetDrawScale(1, 1);
                }

                else
                {

                    ViewPortHelper.GraphicsDeviceManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    ViewPortHelper.GraphicsDeviceManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

                    ViewPortHelper.GraphicsDeviceManager.IsFullScreen = true;
                    ViewPortHelper.GraphicsDeviceManager.ApplyChanges();
                                        
                    ViewPortHelper.GraphicsDevice.Viewport = new Viewport(ViewPortHelper.XOffset, ViewPortHelper.YOffset, ViewPortHelper.ScreenWidth, ViewPortHelper.ScreenHeight);
                    ViewPortHelper.SetViewPort(ViewPortHelper.ScreenWidth, ViewPortHelper.ScreenHeight);
                    ViewPortHelper.SetDrawScale((float)((double)ViewPortHelper.ScreenWidth / (double)ViewPortHelper.WindowedWidth),
                                                    (float)((double)ViewPortHelper.ScreenHeight / (double)ViewPortHelper.WindowedHeight));
                }

                
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
        }

        internal override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.Draw(_background, new Vector2(0, 0), _backRect, Color.White,0.0f,Vector2.Zero,1.0f,SpriteEffects.None,0.5f);
            batch.DrawString(font, "Number of Players: " + players.ToString() + " (Hit 'P' to change)", new Vector2(200, 600), Color.White,0.0f,Vector2.Zero,1.0f,SpriteEffects.None,0.0f);
        }

        #endregion
    }
}
