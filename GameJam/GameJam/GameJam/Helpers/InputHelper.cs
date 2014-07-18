using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;


using Microsoft.Xna.Framework.Media;

namespace GameJam
{
    public static class InputHelper
    {
        private static KeyboardState _currentKeyState;
        private static MouseState _currentMouseState;
        private static GamePadState _currentGamePadStateP1;
        private static GamePadState _currentGamePadStateP2;
        private static KeyboardState _previousKeyState;
        private static MouseState _previousMouseState;
        private static GamePadState _previousGamePadStateP1;
        private static GamePadState _previousGamePadStateP2;

        private static bool _keyLock = false;

        private static int _players = 1;

        #region Properties - Public Static

        public static KeyboardState CurrentKeyState
        {
            get { return _currentKeyState; }
            set
            {
                _previousKeyState = _currentKeyState;
                _currentKeyState = value;
            }
        }

        public static MouseState CurrentMouseState
        {
            get { return _currentMouseState; }
            set
            {
                _previousMouseState = _currentMouseState;
                _currentMouseState = value;
            }
        }

        public static GamePadState CurrentGamePadStatePlayer1
        {
            get { return _currentGamePadStateP1; }
            set
            {
                _previousGamePadStateP1 = _currentGamePadStateP1;
                _currentGamePadStateP1 = value;
            }
        }

        public static GamePadState CurrentGamePadStatePlayer2
        {
            get { return _currentGamePadStateP2; }
            set
            {
                _previousGamePadStateP2 = _currentGamePadStateP2;
                _currentGamePadStateP2 = value;
            }
        }

        public static bool KeyLock
        {
            get { return _keyLock; }
            set { _keyLock = value; }
        }

        public static int Players
        {
            get { return _players; }
            set { _players = value; }
        }

        public static KeyboardState PreviousKeyState
        {
            get { return _previousKeyState; }
        }

        public static MouseState PreviousMouseState
        {
            get { return _previousMouseState; }
        }

        public static GamePadState PreviousGamePadStatePlayer1
        {
            get { return _previousGamePadStateP1; }
        }

        public static GamePadState PreviousGamePadStatePlayer2
        {
            get { return _previousGamePadStateP2; }
        }

        #endregion

        #region Methods - Public Static

        public static bool WasButtonPressed(Keys key)
        {
            bool pressed = false;
            if (CurrentKeyState.IsKeyUp(key) && PreviousKeyState.IsKeyDown(key))
            {
                pressed = true;
            }
            return pressed;
        }

        public static bool WasPadButtonPressedP1(Buttons button)
        {
            bool pressed = false;
            if (CurrentGamePadStatePlayer1.IsButtonUp(button) && PreviousGamePadStatePlayer1.IsButtonDown(button))
            {
                pressed = true;
            }
            return pressed;
        }

        public static bool WasPadButtonPressedP2(Buttons button)
        {
            bool pressed = false;
            if (CurrentGamePadStatePlayer2.IsButtonUp(button) && PreviousGamePadStatePlayer2.IsButtonDown(button))
            {
                pressed = true;
            }
            return pressed;
        }

        public static bool IsButtonDown(Keys key)
        {
            bool down = false;

            if (CurrentKeyState.IsKeyDown(key))
            {
                down = true;
            }
            return down;
        }

        public static bool IsPadButtonDownP1(Buttons button)
        {
            bool down = false;

            if (CurrentGamePadStatePlayer1.IsButtonDown(button))
            {
                down = true;
            }
            return down;
        }

        public static bool IsPadButtonDownP2(Buttons button)
        {
            bool down = false;

            if (CurrentGamePadStatePlayer2.IsButtonDown(button))
            {
                down = true;
            }
            return down;
        }

        public static Vector2 LeftThumbstickDirectionP1
        {
            get { return CurrentGamePadStatePlayer1.ThumbSticks.Left;   }        
        }

        public static Vector2 LeftThumbstickDirectionP2
        {
            get { return CurrentGamePadStatePlayer2.ThumbSticks.Left; }
        }            

        public static bool WasMouseClicked()
        {
            bool clicked = false;
            if (CurrentMouseState.LeftButton == ButtonState.Released && PreviousMouseState.LeftButton == ButtonState.Pressed)
            {
                clicked = true;
            }
            return clicked;
        }

        public static void SetKeyboardState()
        {
            CurrentKeyState = Keyboard.GetState();
        }

        public static void SetMouseState()
        {
            CurrentMouseState = Mouse.GetState();
        }

        public static void SetGamePadStatePlayer1()
        {
            CurrentGamePadStatePlayer1 = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
        }

        public static void SetGamePadStatePlayer2()
        {
            CurrentGamePadStatePlayer2 = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.Two);
        }

        #endregion        
    }
}
