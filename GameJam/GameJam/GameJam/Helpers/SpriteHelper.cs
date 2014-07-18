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
    public static class SpriteHelper
    {
        private static Background _currentBackground;

        public static Background CurrentBackground
        {
            set { _currentBackground = value; }
        }

        public static Vector2 CheckWallCollisions(SpriteBase sprite)
        {
            Vector2 newPos = Vector2.Zero;

            //Locate sprite.
            //Tile location = (from t in _currentBackground.Tiles
                         //    where t.Container.Contains(sprite.CollisionRectangle)
                          //   select t).FirstOrDefault();

            //If location ascertained.
           // if (location != null)
          //  {
            //    newPos = location.CheckCollisionWithWall(sprite);         
          //  }

            return newPos;
        }
    }
}
