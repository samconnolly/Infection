using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameJam
{
    public static class FontHelper
    {
        private static List<SpriteFont> _fonts = new List<SpriteFont> { };

        public static List<SpriteFont> Fonts
        {
            get { return _fonts; }
            set { _fonts = value; }
        }

    }
}