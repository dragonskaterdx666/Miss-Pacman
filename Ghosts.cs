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
    public class Ghosts : DrawableGameComponent
    {

        #region variables

        Texture2D texture;

        SpriteBatch spriteBatch;

        Game1 game1;

        public Board board;

        Point position;
        
        char ghostType;

        #endregion

        #region Constructor
        public Ghosts(Game1 game, int x, int y, char ghostType) : base(game)
        {
            this.texture = game.SpriteSheetAssets;
            
            this.spriteBatch = game.SpriteBatch;
            
            position.Y = y;
            
            position.X = x;
            
            game1 = game;
            
            board = game1.Board;
            
            this.ghostType = ghostType;
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


        public override void Update(GameTime gameTime)
        {
            Rectangle pRect = new Rectangle(game1.player.position, new Point(Game1.outputTileSize));
            Rectangle eRect = new Rectangle(position, new Point(Game1.outputTileSize));

            if (eRect.Intersects(pRect)) { game1.player.Die(); }


            //if (position == targetPosition)
            //{
            //    if (Math.Abs(patrolPosition) > patrolSize)
            //        direction *= 1;

            //    targetPosition +=
            //        orientation == Orientation.Horizontal
            //        ? new Point(direction * Game1.outputTileSize, 0) : new Point(0, direction * Game1.outputTileSize);

            //    if (game.Board.board[targetPosition.X / Game1.outputTileSize,
            //        targetPosition.Y / Game1.outputTileSize] == ' ')
            //    {
            //        patrolPosition++;
            //    }
            //    else
            //    {
            //        targetPosition = position;
            //        direction = -direction;
            //    }

            //}
            //else
            //{
            //    Vector2 dir = (targetPosition - position).ToVector2();
            //    dir.Normalize();
            //    position += dir.ToPoint();
            //}
        }

        //Draws the different types of ghosts
        public override void Draw(GameTime gameTime)
        {
            
            Rectangle outRect = new Rectangle(position.X * Game1.outputTileSize, position.Y * Game1.outputTileSize, Game1.outputTileSize, Game1.outputTileSize);
            
            spriteBatch.Begin();

            switch (ghostType)
            {
                //allows for different types of ghosts
                case '1':
                    spriteBatch.Draw(texture, outRect, new Rectangle(0,2 * 16, 16, 16), Color.White);
                    break;
                case '2':
                    spriteBatch.Draw(texture, outRect, new Rectangle(0,3 * 16, 16, 16), Color.White);
                    break;
                case '3':
                    spriteBatch.Draw(texture, outRect, new Rectangle(0,4 * 16, 16, 16), Color.White);
                    break;
            }

            spriteBatch.End();
        }
        #endregion
    }
}
