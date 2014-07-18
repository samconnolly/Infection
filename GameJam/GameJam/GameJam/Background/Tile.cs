using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameJam
{
    public class Tile
    {
        private Texture2D _tileTexture;
        private string _asset;
        private Vector2 _position;
        private Rectangle _container;
        private Rectangle _rectangle;
        private List<Wall> _walls;
        
        public Tile(string asset, Vector2 pos)
        {
            _position = pos;
            _asset = asset;
            _walls = new List<Wall>();
        }

        public List<Wall> Walls
        {
            get { return this._walls; }
        }

        public Rectangle Container
        {
            get { return this._container; }
        }

        internal void LoadContent(SpriteBatch batch, Game game)
        {
            _tileTexture = game.Content.Load<Texture2D>(_asset);
            _container = new Rectangle((int)_position.X, (int)_position.Y, _tileTexture.Width, _tileTexture.Height);
            _rectangle = new Rectangle(0, 0, _tileTexture.Width, _tileTexture.Height);
        }

        internal void Draw(GameTime gameTime, SpriteBatch batch)
        {
            foreach (Wall wall in Walls)
            {
                wall.Draw(gameTime, batch);
            }

            batch.Draw(_tileTexture, _position, _rectangle, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.9f);            
        }

        internal Vector2 CheckCollisionWithWall(SpriteBase sprite)
        {
            Vector2 collided = Vector2.Zero;
            foreach (Wall wall in Walls)
            {
                collided = wall.CheckCollision(sprite);        
            }
            return collided;
        }
    }
}
