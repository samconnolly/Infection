using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameJam
{
    public static class CellsHelper
    {
        private static List<SpriteBase> _cellList;
        private static List<SpriteBase> _addList = new List<SpriteBase> { };
        private static bool _freeze = false;
        private static bool _antidote = false;

        public static List<SpriteBase> Cells
        {
            get { return _cellList; }
            set { _cellList = value; }
        }

        public static List<SpriteBase> AddCells
        {
            get { return _addList; }
            set { _addList = value; }
        }

        public static bool Freeze
        {
            get { return _freeze; }
            set { _freeze = value; }
        }

        public static bool Antidote
        {
            get { return _antidote; }
            set { _antidote = value; }
        }
    }
}
