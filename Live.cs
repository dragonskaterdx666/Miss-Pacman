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
    public class Live : DrawableGameComponent
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

            texture = game.SpriteSheetPlayer;

        }
        #endregion

        #region Methods

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle SourceLives = new Rectangle(0, 1 * 32, 32, 32);

            Rectangle outRect = new Rectangle(position.X * Game1.outputTileSize, position.Y * Game1.outputTileSize, Game1.outputTileSize, Game1.outputTileSize);

            spriteBatch.Begin();

            spriteBatch.Draw(texture, outRect, SourceLives, Color.White);

            spriteBatch.End();
        }

        /// <summary>
        /// Counts the amount of lives on the list
        /// </summary>
        /// <returns></returns>
        public int LiveCount()
        {
            int lives = game1.Lives.Count();
            
            return lives;
        }

        #endregion
    }
}
