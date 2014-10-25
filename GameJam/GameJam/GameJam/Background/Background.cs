using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameJam
{
    public class Background
    {
        private Texture2D _backgroundTexture1;
        private Texture2D _backgroundTexture2;
        private Texture2D _backgroundTexture3;
        private Texture2D _backgroundTexture4;
        private Texture2D _backgroundTexture5;
        private List<Texture2D> _backgrounds;
        //private Dictionary<string, TileRef> _map;
        private Rectangle _drawRectangle;
        private Vector2 _position = new Vector2(0, 0);

        private const int TILE_WIDTH = 360;
        private const int TILE_HEIGHT = 240;

        public Background()
        {}

        internal void LoadContent(SpriteBatch batch, Game game)
        {
            _backgroundTexture1 = game.Content.Load<Texture2D>("level_1");
            _backgroundTexture2 = game.Content.Load<Texture2D>("level_2");
            _backgroundTexture3 = game.Content.Load<Texture2D>("level_3");
            _backgroundTexture4 = game.Content.Load<Texture2D>("level_4");
            _backgroundTexture5 = game.Content.Load<Texture2D>("level_5");

            _backgrounds = new List<Texture2D> { _backgroundTexture1, _backgroundTexture2, _backgroundTexture3, _backgroundTexture4, _backgroundTexture5 };

            _drawRectangle = new Rectangle(0, 0, _backgroundTexture1.Width, _backgroundTexture1.Height);
        }

        internal void Draw(GameTime gameTime, SpriteBatch batch, int level)
        {
            if (level < 1)
            {
                level = 1;
            }
            else if (level > 5)
            {
                level = 5;
            }
            batch.Draw(_backgrounds[level - 1], _position, _drawRectangle, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.7f);
        }
    }
}
