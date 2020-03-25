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

        private Board board;

        Point position;

        char pointsType;

        public int numberOfPoints = 0;

        #endregion

        #region Constructor
        public Score(Game1 game, int x, int y, char pointsType) : base(game)
        {
            texture = game.SpriteSheetMap;

            spriteBatch = game.SpriteBatch;

            position.Y = y;

            position.X = x;

            game1 = game;

            board = game1.Board;

            this.pointsType = pointsType;
        }
        #endregion

        #region Properties
        public Board Board => board;

        #endregion

        #region Methods
        public override void Draw(GameTime gameTime)
        {
            Rectangle outRect = new Rectangle(position.X * Game1.outputTileSize, position.Y * Game1.outputTileSize, Game1.outputTileSize, Game1.outputTileSize);
            
            

            Rectangle Upgrade = new Rectangle(13 * 16, 9 * 16, 16, 16);

            Rectangle EmptySpace = new Rectangle(0, 1 * 35, 35, 35);
            
            spriteBatch.Begin();

            switch (pointsType)
            {
                case '.':
                    spriteBatch.Draw(texture, outRect, EmptySpace, Color.White);
                    break;

            }

            spriteBatch.End();
        }
        #endregion

    }
}
