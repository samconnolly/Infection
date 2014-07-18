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
        private Texture2D _backgroundTexture;
        //private Dictionary<string, TileRef> _map;
        private Rectangle _drawRectangle;
        private Vector2 _position = new Vector2(0, 0);

        private const int TILE_WIDTH = 360;
        private const int TILE_HEIGHT = 240;

        public Background()
        {}

        internal void LoadContent(SpriteBatch batch, Game game)
        {
            _backgroundTexture = game.Content.Load<Texture2D>("Large Background");
            _drawRectangle = new Rectangle(0, 0, _backgroundTexture.Width, _backgroundTexture.Height);
        }

        internal void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.Draw(_backgroundTexture, _position, _drawRectangle, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.7f);
        }
    }
}
