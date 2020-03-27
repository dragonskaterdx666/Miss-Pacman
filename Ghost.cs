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
        //enumeradores
        enum Orientation { Horizontal, Vertical }

        enum GDirection { Up, Down, Left, Right }

        enum GhostTypes { Blue, Orange, Purple, Red }

        #region variables

        private Texture2D texture;

        private SpriteBatch spriteBatch;

        private Game1 game1;

        public Board board;

        private Orientation orientation;

        public Point position, targetPosition;

        int enemyLives = 4;

        int patrolSize;

        int patrolPosition = 0;

        int direction = 1;

        int frame = 0;

        int ghostValue = 200;

        Dictionary<GDirection, Vector2> ghostColor;

        Dictionary<GDirection, Point> Surroundings;

        GDirection gDirection = GDirection.Up;

        #endregion

        #region Constructor
        public Ghost(Game1 game, int x, int y, int ghostType) : base(game)
        {
            orientation = Game1.rnd.Next(2) > 0 ? Orientation.Horizontal : Orientation.Vertical;

            texture = game.SpriteSheet;

            spriteBatch = game.SpriteBatch;

            position.Y = y;

            position.X = x;

            targetPosition = position - new Point(0, 1 * 16);

            game1 = game;

            board = game1.Board;

            patrolSize = 2 + Game1.rnd.Next(4);

            Surroundings = new Dictionary<GDirection, Point>
            {
                [GDirection.Up] = new Point(0, -1),
                [GDirection.Down] = new Point(0, 1),
                [GDirection.Left] = new Point(-1, 0),
                [GDirection.Right] = new Point(0, 1),
            };

            ghostColor = new Dictionary<GDirection, Vector2>
            {
                [GDirection.Right] = new Vector2(0, ghostType),
                [GDirection.Left] = new Vector2(2, ghostType),
                [GDirection.Up] = new Vector2(4, ghostType),
                [GDirection.Down] = new Vector2(6, ghostType),
            };

        }


        #endregion

        #region Properties
        public Board Board => board;

        #endregion

        #region Methods


        public override void Update(GameTime gameTime)
        {
            Rectangle pRect = new Rectangle(game1.Player.position, new Point(Game1.outputTileSize));

            Rectangle EnemyArea = new Rectangle(((position.ToVector2()) * Game1.outputTileSize).ToPoint(), new Point(Game1.outputTileSize));

            if (EnemyArea.Intersects(pRect))
            {
                Pellet.GetPelletStatus();

                if (Pellet.powerPellet == true)
                {
                    this.Die();
                }

                else
                {
                    game1.Player.Die();
                }

            }

            if (position == targetPosition)
            {

                if (Math.Abs(patrolPosition) > patrolSize)
                    direction *= -1;

                // move horizontally or vertically one unit
                targetPosition += orientation == Orientation.Horizontal
                    ? new Point(direction * Game1.outputTileSize, 0)
                    : new Point(0, direction * Game1.outputTileSize);

                if (game1.Board.board[targetPosition.X / Game1.outputTileSize,
                    targetPosition.Y / Game1.outputTileSize] == '#' && game1.Board.board[targetPosition.X / Game1.outputTileSize,
                        targetPosition.Y / Game1.outputTileSize] == ' ' && game1.Board.board[targetPosition.X / Game1.outputTileSize,
                        targetPosition.Y / Game1.outputTileSize] == '.')
                {
                    // increment patrol Position
                    patrolPosition += direction;
                }
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

            Rectangle sourceRec = new Rectangle(((ghostColor[gDirection] + (Vector2.UnitX * frame)) * 16).ToPoint(), new Point(15));

            Rectangle sourcePelletRec = new Rectangle(8 * 16, 0, 16, 15);

            spriteBatch.Begin();

            Pellet.GetPelletStatus();

            if (!Pellet.powerPellet)
            {
                spriteBatch.Draw(texture, outRect, sourceRec, Color.White);
            }
            else
            {
                spriteBatch.Draw(texture, outRect, sourcePelletRec, Color.White);
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Deals with ghost movement
        /// </summary>
        public void GhostMovement()
        {











        }

        public void Die()
        {
            enemyLives--;

            int n = 4 - enemyLives;

            AssignGhostValue(n);

            game1.Ghosts.Remove(this);

            game1.Components.Remove(this);
        }

        public void AssignGhostValue(int n)
        {
            ghostValue = ghostValue * n;

            game1.Player.Score += ghostValue;
        }

        #endregion
    }
}
