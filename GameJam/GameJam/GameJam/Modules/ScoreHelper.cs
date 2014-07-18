using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameJam
{
    public static class ScoreHelper
    {
        private static int _score;

        public static int Score
        {
            get { return _score; }
            set { _score = value; }
        }

    }
}
