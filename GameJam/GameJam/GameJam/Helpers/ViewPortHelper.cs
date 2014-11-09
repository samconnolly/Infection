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
    public static class ViewPortHelper
    {
        private static int _x;
        private static int _y;
        private static GraphicsDevice _graphicsDevice;
        private static GraphicsDeviceManager _graphicsDeviceManager;
        private static Game _game;
        private static float _xScale = 1.0f;
        private static float _yScale = 1.0f;
        private static int _xOffset;
        private static int _yOffset;
        private static int _screenWidth;
        private static int _screenHeight;
        private static int _windowedWidth;
        private static int _windowedHeight;
        private static float _aspectRatio;
        public static int _frameRate = 100;

        private static Song _currentSong;
                

        public static int X 
        {
            get { return _x; }  
        }

        public static int Y
        {
            get { return _y; }
        }

        public static void SetViewPort(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public static float XScale
        {
            get { return _xScale; }
        }

        public static float YScale
        {
            get { return _yScale; }
        }

        public static void SetDrawScale(float x, float y)
        {
            _xScale = x;
            _yScale = y;
        }

        public static int XOffset
        {
            get { return _xOffset; }
        }

        public static int YOffset
        {
            get { return _yOffset; }
        }

        public static void SetDrawOffset(int x, int y)
        {
            _xOffset = x;
            _yOffset = y;
        }

        public static int ScreenWidth
        {
            get { return _screenWidth; }
        }

        public static int ScreenHeight
        {
            get { return _screenHeight; }
        }

        public static void SetScreenSize(int x, int y)
        {
            _screenWidth = x;
            _screenHeight = y;
        }

        public static int WindowedWidth
        {
            get { return _windowedWidth; }
        }

        public static int WindowedHeight
        {
            get { return _windowedHeight; }
        }

        public static void SetWindowedSize(int x, int y)
        {
            _windowedWidth = x;
            _windowedHeight = y;
        }

        public static float AspectRatio
        {
            set { _aspectRatio = value; }
            get { return _aspectRatio; }
        }

        public static GraphicsDevice GraphicsDevice
        {
            set { _graphicsDevice = value; }
            get { return _graphicsDevice; }
        }

        public static GraphicsDeviceManager GraphicsDeviceManager
        {
            set { _graphicsDeviceManager = value; }
            get { return _graphicsDeviceManager; }
        }

        public static Game Game
        {
            set { _game = value; }
            get { return _game; }
        }

        public static int FrameRate
        {
            set { _frameRate = value; }
            get { return _frameRate; }
        }

        public static Song CurrentSong
        {
            set { _currentSong = value; }
            get { return _currentSong; }
        }

        public static void ToggleFullscreen()
        {
            if (GraphicsDeviceManager.IsFullScreen == true)
            {
                GraphicsDeviceManager.PreferredBackBufferHeight = WindowedHeight;
                GraphicsDeviceManager.PreferredBackBufferWidth = WindowedWidth;

                GraphicsDeviceManager.IsFullScreen = false;
                GraphicsDeviceManager.ApplyChanges();

                GraphicsDevice.Viewport = new Viewport(0, 0, WindowedWidth, WindowedHeight);
                SetViewPort(WindowedWidth, WindowedHeight);
                SetDrawScale(1, 1);
            }

            else
            {

                GraphicsDeviceManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                GraphicsDeviceManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

                GraphicsDeviceManager.IsFullScreen = true;
                GraphicsDeviceManager.ApplyChanges();

                GraphicsDevice.Viewport = new Viewport(XOffset, YOffset, ScreenWidth, ScreenHeight);
                SetViewPort(ScreenWidth, ScreenHeight);
                SetDrawScale((float)((double)ScreenWidth / (double)WindowedWidth),
                                                (float)((double)ScreenHeight / (double)WindowedHeight));
            }
        }


    }
}
