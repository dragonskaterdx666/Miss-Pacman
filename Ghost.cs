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
    public class Ghost : DrawableGameComponent
    {
        enum Orientation { Horizontal, Vertical }

        #region variables

        private Texture2D texture;

        private SpriteBatch spriteBatch;

        private Game1 game1;

        public Board board;
        
        private char ghostType;
        
        private Orientation orientation;

        public Point position, targetPosition;

        int enemyLives = 1;

        int patrolSize;
        
        int patrolPosition = 0;
        
        int direction = 1;

        #endregion

        #region Constructor
        public Ghost(Game1 game, int x, int y, char ghostType) : base(game)
        {
            orientation = Game1.rnd.Next(2) > 0 ? Orientation.Horizontal : Orientation.Vertical;
            
            texture = game.SpriteSheet;
            
            spriteBatch = game.SpriteBatch;
            
            position.Y = y;
            
            position.X = x;

            targetPosition = position;
            
            game1 = game;
            
            board = game1.Board;
            
            this.ghostType = ghostType;

            patrolSize = 2 + Game1.rnd.Next(4);
        }
        #endregion

        #region Properties
        public Board Board => board;

        #endregion

        #region Methods


        public override void Update(GameTime gameTime)
        {
            Rectangle pRect = new Rectangle(game1.player.position, new Point(Game1.outputTileSize));
            Rectangle EnemyArea = new Rectangle(((position.ToVector2()) * Game1.outputTileSize).ToPoint(), new Point(Game1.outputTileSize));

            if (EnemyArea.Intersects(pRect)) 
            { 
                game1.player.Die(); 
            }

            if (position == targetPosition)
            {
                if (Math.Abs(patrolPosition) > patrolSize)
                    direction *= 1;

                targetPosition +=
                    orientation == Orientation.Horizontal
                    ? new Point(direction * Game1.outputTileSize, 0) : new Point(0, direction * Game1.outputTileSize);

                if (game1.Board.board[targetPosition.X / Game1.outputTileSize, targetPosition.Y / Game1.outputTileSize] == ' ')
                    patrolPosition++;
                else
                {
                    targetPosition = position;
                    direction = -direction;
                }

            }
            else
            {
                Vector2 dir = (targetPosition - position).ToVector2();

                dir.Normalize();

                position += dir.ToPoint();
            }
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
                    spriteBatch.Draw(texture, outRect, new Rectangle(0,2 * 16, 16, 15), Color.White);
                    break;
                case '2':
                    spriteBatch.Draw(texture, outRect, new Rectangle(0,3 * 16, 16, 15), Color.White);
                    break;
                case '3':
                    spriteBatch.Draw(texture, outRect, new Rectangle(0,4 * 16, 16, 15), Color.White);
                    break;
                case '+':
                    spriteBatch.Draw(texture, outRect, new Rectangle(0, 0, 16, 15), Color.White);
                    break;
            }

            spriteBatch.End();
        }

        public void Die()
        {
            enemyLives--;

            game1.Ghosts.Remove(this);

            if (enemyLives <= 0)
            {

                foreach (DrawableGameComponent gameComponent in game1.Components)
                {
                    gameComponent.Enabled = false;
                }
            }
        }

        #endregion
    }
}
