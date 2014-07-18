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
    class Reproduce : PowerUpBase
    {
        public Reproduce(Texture2D texture, Vector2 position)
            : base(texture, position)
        {

        }

        public override void PowerUp()
        {
            VirusHelper.Virus.reproduce = true;
            base.PowerUp();
        }

    }
}


