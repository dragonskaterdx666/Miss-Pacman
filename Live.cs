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

        private Board board;

        Point position;

        Score score;

        public Rectangle SourceDots = new Rectangle(16 * 35, 4 * 35, 35, 35);

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

        public Board Board => board;

        #endregion


        #region Methods

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle outRect = new Rectangle(position.X * Game1.outputTileSize, position.Y * Game1.outputTileSize, Game1.outputTileSize, Game1.outputTileSize);

            spriteBatch.Begin();

            spriteBatch.Draw(texture: game1.SpriteSheet, outRect, new Rectangle(8 * 16, 2 * 16, 16, 16), Color.White);

            spriteBatch.End();
        }
        #endregion
    }
}
