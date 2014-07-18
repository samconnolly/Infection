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
    class Circle
    {
        public Vector2 position;
        public float radius;
        private List<Vector2> vertices;
        private Texture2D blank;
        public int thickness = 3;
        private int res;
        public Color color;

        public Circle(Vector2 Position, float Radius, int Thickness, Color Color)
        {
            position = Position;
            radius = Radius;
            thickness = Thickness;

            color = Color;

            blank = new Texture2D(ViewPortHelper.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { color });

            vertices = new List<Vector2> { };

            res = (int)(radius * 20);

            for (int i = 0; i < res; i++)
            {
                double th = i * ((Math.PI * 2.0) / res);

                for (int j = 0; j < thickness; j++)
                {
                    vertices.Add(position + new Vector2((float)((radius + j) * Math.Sin(th)), (float)((radius + j) * Math.Cos(th))));
                }
            }
        }

        public void Update()
        {
            vertices = new List<Vector2> { };

            res = (int)(radius * 8);

            for (int i = 0; i < res; i++)
            {
                double th = i * ((Math.PI * 2.0) / res);

                for (int j = 0; j < thickness; j++)
                {
                    vertices.Add(position + new Vector2((float)((radius + j) * Math.Sin(th)), (float)((radius + j) * Math.Cos(th))));
                }
            }
        }

        public void Draw(SpriteBatch sbatch)
        {

            for (int x = 0; x < (vertices.Count()); x++)
            {
                sbatch.Draw(blank, vertices[x], null, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            }
        }

    }
}