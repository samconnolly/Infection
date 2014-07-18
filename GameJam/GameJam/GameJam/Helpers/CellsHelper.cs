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
    }
}
