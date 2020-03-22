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
    public class Score : DrawableGameComponent
    {
        #region variables

        Texture2D texture;

        SpriteBatch spriteBatch;

        Game1 game1;

        public Board board;

        Point position;

        char pointsType;

        public int numberOfPoints;

        #endregion

        #region Constructor
        public Score(Game1 game, int x, int y, char pointsType) : base(game)
        {
            this.texture = game.SpriteSheetMap;

            this.spriteBatch = game.SpriteBatch;

            position.Y = y;

            position.X = x;

            game1 = game;

            board = game1.Board;

            this.pointsType = pointsType;
        }
        #endregion

        #region Properties
        public Board Board
        {
            get
            {
                return board;
            }
        }

        #endregion

        #region Methods
        public override void Draw(GameTime gameTime)
        {
            Rectangle outRect = new Rectangle(position.X * Game1.outputTileSize, position.Y * Game1.outputTileSize, Game1.outputTileSize, Game1.outputTileSize);

            spriteBatch.Begin();

            switch (pointsType)
            {
                case '.':
                    spriteBatch.Draw(texture, destinationRectangle: outRect, sourceRectangle: new Rectangle(0, 1 * 35, 35, 35), color: Color.White);
                    break;
                case '?':
                    spriteBatch.Draw(texture, destinationRectangle: outRect, sourceRectangle: new Rectangle(15 * 35, 4 * 35, 35, 35), color: Color.White);
                    break;
                case ' ':
                    spriteBatch.Draw(texture, destinationRectangle: outRect, sourceRectangle: new Rectangle(16 * 35, 4 * 35, 35, 35), color: Color.White);
                    break;
            }

            spriteBatch.End();
        }
        #endregion

    }
}
