using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameJam
{
    public struct TileRef
    {
        private int _x;
        private int _y;

        public TileRef(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X 
        { 
            get{return this._x;}
        }

        public int Y 
        { 
            get{return this._y;}
        }
    }
}
