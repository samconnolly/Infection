using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameJam
{
    public class Line
    {
        public Vector2 start;
        public Vector2 end;
        public Vector2 vector;
        public int thickness;
        private List<Vector2> vertices = new List<Vector2> { };
        private Texture2D blank;
        public Color colour;
        private int res = 1;
        private int steps;
        private Vector2 vstep;

        public Line(Vector2 Start, Vector2 End, int Thickness, Color Colour)
        {
            start = Start;
            end = End;
            vector = end - start;
            vertices.Add(start);

            steps = (int)vector.Length() * res;
            vstep = new Vector2(vector.X / steps, vector.Y / steps);

            thickness = Thickness;
            colour = Colour;

            blank = new Texture2D(ViewPortHelper.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { colour });


            Update();
        }

        public void Update()
        {
            vertices = new List<Vector2> { };
            Vector2 pos = start;

            for (int x = 0; x < steps + 1; x++)
            {

                pos += vstep;


                if (vertices.Contains(pos) == false)
                {
                    for (int t=1; t < thickness+1; t++)
                    {
                        vertices.Add(pos + t*new Vector2(0,1));
                    }
                }
            }
        }

        public void Draw(SpriteBatch sbatch)
        {

            for (int x = 0; x < (vertices.Count()); x++)
            {
                sbatch.Draw(blank, vertices[x], null, colour, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            }
        }

    }
}
