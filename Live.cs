using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MsPacMan
{
    public class Live: DrawableGameComponent
    {
        #region variables

        Texture2D texture;

        SpriteBatch spriteBatch;

        Game1 game1;

        Point position;

        #endregion

        #region Constructor
        public Live(Game1 game, int x, int y) : base(game)
        {
            position.X = x;

            position.Y = y;

            game1 = game;

            spriteBatch = game.SpriteBatch;

            texture = game.SpriteSheet;

        }
        #endregion

        #region Properties


        #endregion


        #region Methods

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle SourceLives = new Rectangle(10 * 16, 2 * 16, 16, 16);

            Rectangle outRect = new Rectangle(position.X * Game1.outputTileSize, position.Y * Game1.outputTileSize, Game1.outputTileSize, Game1.outputTileSize);

            spriteBatch.Begin();

            spriteBatch.Draw(texture: game1.SpriteSheet, outRect, SourceLives, Color.White);

            spriteBatch.End();
        }

        public void LiveCount()
        {

        }
        #endregion
    }
}
