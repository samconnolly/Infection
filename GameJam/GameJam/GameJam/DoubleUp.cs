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
    class DoubleUp : PowerUpBase
    {
        public DoubleUp(Texture2D texture, Vector2 position)
            : base(texture, position)
        {

        }

        public override void PowerUp()
        {
            if (VirusHelper.Virus.viruslingNo == 0)
            {
                VirusHelper.Virus.AddViruslings(2);
            }

            else
            {
                VirusHelper.Virus.AddViruslings(VirusHelper.Virus.viruslingNo);
            }

            base.PowerUp();
        }

    }
}

