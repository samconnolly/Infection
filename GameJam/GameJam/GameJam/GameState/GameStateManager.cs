using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameJam
{
    public static class GameStateManager
    {
        private static GameState _currentGameState;
        private static bool _hasChanged = false;

        public static GameState CurrentGameState
        {
            get { return _currentGameState; }
            set
            {
                _currentGameState = value;
            }
        }

        public static bool HasChanged
        {
            get { return _hasChanged; }
            set { _hasChanged = value; }
        }
    }
}
