using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace MsPacMan
{
    public class Dot : DrawableGameComponent
    {
        #region variables

        private Texture2D texture;

        private SpriteBatch spriteBatch;

        private Game1 game1;

        private Point position;

        public int dotValue = 10;

        #endregion

        #region Constructor
        public Dot(Game1 game, int x, int y) : base(game)
        {
            position.X = x;

            position.Y = y;

            game1 = game;

            spriteBatch = game.SpriteBatch;

            texture = game.SpriteSheetMap;

        }
        #endregion

        #region Properties

        #endregion

        #region Methods

        public override void Update(GameTime gameTime)
        {
            Rectangle playerPosition = new Rectangle(game1.Player.position, new Point(16));

            Rectangle DotArea = new Rectangle(((position.ToVector2()) * Game1.outputTileSize).ToPoint(), new Point(8));

            if (DotArea.Intersects(playerPosition))
            {
                game1.Components.Remove(this);

                game1.Dots.Remove(this);

                game1.Player.Score += dotValue;

                //confirms if the player is meeting the score condition to win
                game1.Player.ScoreCount();

            }
        }

        /// <summary>
        /// Draws the dots in game
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            Rectangle SourceDots = new Rectangle(16 * 35, 4 * 35, 35, 35);

            Rectangle outRect = new Rectangle(position.X * Game1.outputTileSize, position.Y * Game1.outputTileSize, Game1.outputTileSize, Game1.outputTileSize);

            spriteBatch.Begin();

            spriteBatch.Draw(texture, outRect, SourceDots, Color.White);

            spriteBatch.End();
        }
        #endregion
    }
}
