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
    public class Dot: DrawableGameComponent
    {
        #region variables

        private Texture2D texture;

        private SpriteBatch spriteBatch;

        private Game1 game1;

        private Board board;

        private Point position;

        private Player playerScore;

        int dotValue = 10;        

        public Rectangle SourceDots = new Rectangle(16 * 35, 4 * 35, 35, 35);

        #endregion

        #region Constructor
        public Dot(Game1 game,int x, int y): base(game)
        {
            position.X = x;

            position.Y = y;

            game1 = game;

            spriteBatch = game.SpriteBatch;

            texture = game.SpriteSheetMap;

        }
        #endregion

        #region Properties

        public Player currentScore => playerScore;

        #endregion

        #region Methods

        public override void Update(GameTime gameTime)
        {
            Rectangle playerPosition = new Rectangle(game1.player.position, new Point(16));
            
            Rectangle DotArea = new Rectangle(((position.ToVector2()) * Game1.outputTileSize).ToPoint(), new Point(8));

            if (DotArea.Intersects(playerPosition))
            {
                game1.Components.Remove(this);

                game1.Dots.Remove(this);

                //currentScore.numberOfPoints += dotValue;

                //game1.Scores.Add(currentScore);

            }
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle outRect = new Rectangle(position.X * Game1.outputTileSize, position.Y * Game1.outputTileSize, Game1.outputTileSize, Game1.outputTileSize);

            spriteBatch.Begin();

            spriteBatch.Draw(texture, outRect, SourceDots, Color.White);

            spriteBatch.End();
        }
        #endregion
    }
}
