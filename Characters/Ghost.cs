using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace MsPacMan
{
    public class Ghost : DrawableGameComponent
    {
        //enumeradores
        enum Orientation { Horizontal, Vertical }

        enum Direction { Up, Down, Left, Right }

        #region variables

        TimeSpan Time = new TimeSpan();

        private Texture2D texture;

        private SpriteBatch spriteBatch;

        private SoundEffect eatghostSound;

        private Game1 game1;

        private int ghostType;

        private Board board;

        private Orientation orientation;

        public Point position, targetPosition, origin;

        int enemyLives = 4;

        int patrolSize;

        int patrolPosition = 0;

        int direction = 1;

        float timer;

        int frame = 0;

        public static int ghostValue = 200;

        Dictionary<Direction, Vector2> ghostColor;

        Dictionary<Direction, Point> Surroundings;

        Direction gDirection = Direction.Up;

        #endregion

        #region Constructor
        public Ghost(Game1 game, int x, int y, int ghostType) : base(game)
        {
            orientation = Game1.rnd.Next(2) > 0 ? Orientation.Horizontal : Orientation.Vertical;

            texture = game.SpriteSheet;

            spriteBatch = game.SpriteBatch;

            this.ghostType = ghostType;

            position.Y = y;

            position.X = x;

            targetPosition = position;

            game1 = game;

            board = game1.Board;

            origin = position;

            targetPosition = origin + new Point(0, -3);

            patrolSize = 2 + Game1.rnd.Next(4);

            eatghostSound = game1.Content.Load<SoundEffect>("pacman_eatghost");

            Surroundings = new Dictionary<Direction, Point>
            {
                [Direction.Up] = new Point(0, -1),
                [Direction.Down] = new Point(0, 1),
                [Direction.Left] = new Point(-1, 0),
                [Direction.Right] = new Point(1, 0),
            };

            ghostColor = new Dictionary<Direction, Vector2>
            {
                [Direction.Right] = new Vector2(0, ghostType),
                [Direction.Left] = new Vector2(2, ghostType),
                [Direction.Up] = new Vector2(4, ghostType),
                [Direction.Down] = new Vector2(6, ghostType),
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

            ChasePattern(ghostType);

            if (EnemyArea.Intersects(pRect))
            {
                Pellet.GetPelletStatus();

                if (Pellet.powerPellet)
                {
                    Die();
                }

                else
                {
                    game1.Player.Die();
                }

            }
        }

        //Draws the different types of ghosts
        public override void Draw(GameTime gameTime)
        {
            Rectangle outRect = new Rectangle(position.X * Game1.outputTileSize, position.Y * Game1.outputTileSize, Game1.outputTileSize, Game1.outputTileSize);

            Rectangle sourceRec = new Rectangle(((ghostColor[gDirection] + Vector2.UnitX * frame) * 16).ToPoint(), new Point(15));

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
        /// Functions that kills the ghost and respawns it
        /// </summary>
        public void Die()
        {
            //plays death sound
            eatghostSound.Play();

            enemyLives--;

            //determines the order of the ghost
            int n = 4 - enemyLives;

            //assigns the value of the ghost
            AssignGhostValue(n);

            //REMOVES THE GHOSTS FROM THE LIST AND THE COMPONENTS
            game1.Ghosts.Remove(this);

            game1.Components.Remove(this);

            //RESPAWNER
            if (Time.TotalSeconds == 10f)
            {
                Respawn();
            }

        }

        /// <summary>
        /// Assigns the value of the ghost upon consumpton based on n
        /// </summary>
        /// <param name="n">the number of order on which the player has ate the ghost</param>
        public void AssignGhostValue(int n)
        {
            ghostValue = ghostValue * n;

            game1.Player.Score += ghostValue;
        }

        /// <summary>
        /// Assign the different ghost patterns to each one 
        /// </summary>
        /// <param name="ghostType">type of ghost, comes from the constructor</param>
        public void ChasePattern(int ghostType)
        {
            int ghosType = ghostType;

            int blinky = 0, pinky = 1, inky = 2, clyde = 3;

            if (ghosType == blinky)
            {
                ChaseAggressive();
            }
            else if (ghosType == pinky)
            {
                ChaseAmbush();
            }
            else if (ghosType == inky)
            {
                ChasePatrol();
            }
            else if (ghosType == clyde)
            {
                ChaseRandom();
            }
        }

        /// <summary>
        /// Chase pattern for blinky (the agressive stalker of pacman)
        /// </summary>
        public void ChaseAggressive()
        {
            //Blinky the red ghost is very aggressive in its approach while chasing Pac - Man and will follow Pac-Man once located

            float dist = Vector2.Distance(position.ToVector2(), targetPosition.ToVector2());

            if (dist > 1)
            {
                dist -= position.X;
            }
            if (position == targetPosition)
            {

                if (Math.Abs(patrolPosition) > patrolSize)
                    direction *= 1;

                // move horizontally or vertically one unit
                targetPosition += orientation == Orientation.Horizontal
                    ? new Point(0, direction)
                    : new Point(direction, 0);

                if (game1.Board.board[targetPosition.X, targetPosition.Y] == ' ' ||
                    game1.Board.board[targetPosition.X, targetPosition.Y] == '.')
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

                if ((position.X + position.Y) % 4 == 0)
                    frame++;
                if (frame > 1) frame = -1;
            }

        }

        /// <summary>
        /// Cyan Ghost Pattern
        /// </summary>
        public void ChaseAmbush()
        {
            //Pinky the pink ghost will attempt to ambush Pac-Man by trying to get in front of him and cut him off

            if (position == targetPosition)
            {

                if (Math.Abs(patrolPosition) > patrolSize)
                    direction *= 1;

                // move horizontally or vertically one unit
                targetPosition += orientation == Orientation.Horizontal
                    ? new Point(direction, 0)
                    : new Point(0, direction);

                if (game1.Board.board[targetPosition.X, targetPosition.Y] == ' ' ||
                    game1.Board.board[targetPosition.X, targetPosition.Y] == '.')
                {
                    // increment patrol Position
                    patrolPosition++;
                }
                else
                {
                    targetPosition.X = 3;
                    targetPosition.Y = 29;
                    targetPosition = position;
                    direction = -direction;
                }

            }
            else
            {
                Vector2 dir = (targetPosition - position).ToVector2();
                dir.Normalize();
                position += dir.ToPoint();

                if ((position.X + position.Y) % 4 == 0)
                    frame++;
                if (frame > 1) frame = -1;
            }
        }

        /// <summary>
        /// Blue Ghost Pattern
        /// </summary>
        public void ChasePatrol()
        {
            //Inky the cyan ghost will patrol an area and is not very predictable in this mode
            if (position == targetPosition)
            {

                if (Math.Abs(patrolPosition) > patrolSize)
                    direction *= 1;

                // move horizontally or vertically one unit
                targetPosition += orientation == Orientation.Horizontal
                    ? new Point(0, direction)
                    : new Point(direction, 0);

                if (game1.Board.board[targetPosition.X, targetPosition.Y] == ' ' ||
                    game1.Board.board[targetPosition.X, targetPosition.Y] == '.')
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

                if ((position.X + position.Y) % 4 == 0)
                    frame++;
                if (frame > 1) frame = -1;
            }
        }

        /// <summary>
        /// Orange Ghost Pattern 
        /// </summary>
        public void ChaseRandom()
        {
            //Clyde the orange ghost is moving in a random fashion and seems to stay out of the way of Pac-Man
            if (position == targetPosition)
            {

                if (Math.Abs(patrolPosition) > patrolSize)
                    direction *= 1;

                // move horizontally or vertically one unit
                targetPosition += orientation == Orientation.Horizontal
                    ? new Point(direction, 0)
                    : new Point(0, direction);

                if (game1.Board.board[targetPosition.X, targetPosition.Y] == ' ' ||
                    game1.Board.board[targetPosition.X, targetPosition.Y] == '.')
                {
                    // increment patrol Position
                    patrolPosition++;
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

                if ((position.X + position.Y) % 4 == 0)
                    frame++;
                if (frame > 1) frame = -1;
            }
        }
        /// <summary>
        /// Respawns the ghosts after frightned stage
        /// </summary>
        public void Respawn()
        {
            game1.Ghosts.Add(this);

            game1.Components.Add(this);


        }

        #endregion
    }
}
