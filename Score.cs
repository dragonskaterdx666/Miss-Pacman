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

        public Rectangle PowerPellets = new Rectangle(15 * 35, 4 * 35, 35, 35);

        public Rectangle Upgrade = new Rectangle(13 * 16, 9 * 16, 16, 16);

        public Rectangle Dots = new Rectangle(16 * 35, 4 * 35, 35, 35);

        public Rectangle EmptySpace = new Rectangle(0, 1 * 35, 35, 35);

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
                    spriteBatch.Draw(texture, outRect, EmptySpace, Color.White);
                    break;
                case '?':
                    spriteBatch.Draw(texture, outRect, PowerPellets, Color.White);
                    break;
                case 'L':
                    spriteBatch.Draw(texture: game1.SpriteSheet, outRect, new Rectangle(8 * 16, 2 * 16, 16, 16), Color.White);
                    break;
                case 'M':
                    spriteBatch.Draw(texture: game1.SpriteSheet, outRect, Upgrade, Color.White);
                    break;
            }

            spriteBatch.End();
        }
        #endregion

    }
}
