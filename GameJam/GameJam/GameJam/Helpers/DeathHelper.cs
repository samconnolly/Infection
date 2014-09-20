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
    public static class DeathHelper
    {
        private static List<SpriteBase> _deathList;
        private static List<SpriteBase> _usedItems = new List<SpriteBase> { };

        public static List<SpriteBase> KillCell
        {
            get { return _deathList; }
            set { _deathList = value; }
        }

        public static List<SpriteBase> UsedItems
        {
            get { return _usedItems; }
            set { _usedItems = value; }
        }
    }
}