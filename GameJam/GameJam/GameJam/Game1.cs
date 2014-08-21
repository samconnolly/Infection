using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameJam
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private ModuleBase _currentModule;
        private SpriteBatch spriteBatch;

        private GameModule _pausedGameModule;

        private float aspectRatio = (3.0f / 2.0f);
        private int windowedWidth = 1080;
        private int windowedHeight = 720;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = windowedHeight;
            graphics.PreferredBackBufferWidth = windowedWidth;
            
            graphics.IsFullScreen = false; // initial fullscreen?
        }

        public ModuleBase CurrentModule
        {
            get { return this._currentModule; }
            set { this._currentModule = value; }
        }

        protected override void Initialize()
        {
            ViewPortHelper.SetViewPort(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            ViewPortHelper.GraphicsDevice = GraphicsDevice;
            ViewPortHelper.GraphicsDeviceManager = graphics;
            ViewPortHelper.Game = this;

            // find maximum size of render field with 3:2 aspect ratio when full screen

            int width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int height = (int)(width / aspectRatio);

            if (height > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                width = (int)(height * aspectRatio);
            }

            float xScale = 1f;
            float yScale = 1f;

            int xOffset = (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - width) / 2.0);
            int yOffset = (int)((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - height) / 2.0);

            ViewPortHelper.SetScreenSize(width, height);
            ViewPortHelper.SetDrawScale(xScale, yScale);
            ViewPortHelper.SetDrawOffset(xOffset, yOffset);
            ViewPortHelper.SetWindowedSize(windowedWidth, windowedHeight);

            GameStateManager.CurrentGameState = GameState.MainMenu;
            GameStateManager.HasChanged = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);            
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            //Set the new keyboardstate.
            InputHelper.SetKeyboardState();
            InputHelper.SetMouseState();
            InputHelper.SetGamePadStatePlayer1();
            InputHelper.SetGamePadStatePlayer2();

            //Workout which GameState you are in and load the right module.
            if (GameStateManager.HasChanged)
            {
                switch (GameStateManager.CurrentGameState)
                {
                    case GameState.MainMenu:
                        this.CurrentModule = new MainMenuModule(this);
                        this.CurrentModule.Initialize();
                        this.CurrentModule.LoadContent(spriteBatch);
                        break;
                    case GameState.Highscore:
                        this.CurrentModule = new HighScoreModule(this);
                        this.CurrentModule.Initialize();
                        this.CurrentModule.LoadContent(spriteBatch);
                        break;
                    case GameState.InGame:
                        //If the game is new load new game module.
                        if (_pausedGameModule == null)
                        {
                            this.CurrentModule = new GameModule(this);
                            this.CurrentModule.Initialize();
                            this.CurrentModule.LoadContent(spriteBatch);
                        }
                        else
                        {
                            //If the game was paused re-load the last game module.
                            this.CurrentModule = _pausedGameModule;
                            _pausedGameModule = null;
                        }
                        break;
                    case GameState.GameOver:
                        this.CurrentModule = new FinalScoreModule(this);
                        this.CurrentModule.Initialize();
                        this.CurrentModule.LoadContent(spriteBatch);
                        break;
                }

                //Set the mosue visibility.
                this.IsMouseVisible = this.CurrentModule.IsMouseVisible;             
            }

            //Reset the haschanged flag.
            if (GameStateManager.HasChanged)
            {
                GameStateManager.HasChanged = false;
            }

            //Update the current module.
            this.CurrentModule.Update(gameTime, spriteBatch);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // clear graphics device
            GraphicsDevice.Clear(Color.Black);

            // draw stuff
            if (graphics.IsFullScreen == true)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Matrix.CreateScale(ViewPortHelper.XScale, ViewPortHelper.YScale, 1.0f));
            }

            if (graphics.IsFullScreen == false)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            }

            this.CurrentModule.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
