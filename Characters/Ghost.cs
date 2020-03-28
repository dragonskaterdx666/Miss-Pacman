using System;
using System.Collections.Generic;
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

        enum GDirection { Up, Down, Left, Right }

        #region variables

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

        int frame = 0;

        public static int ghostValue = 200;

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

            this.ghostType = ghostType;

            position.Y = y;

            position.X = x;

            targetPosition = position;

            game1 = game;

            board = game1.Board;

            origin = targetPosition = position;

            patrolSize = 2 + Game1.rnd.Next(4);

            eatghostSound = game1.Content.Load<SoundEffect>("pacman_eatghost");

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

            ChasePattern(ghostType);

            if (EnemyArea.Intersects(pRect))
            {
                Pellet.GetPelletStatus();

                if (Pellet.powerPellet)
                {
                    this.Die();
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

        public void Die()
        {
            eatghostSound.Play();

            enemyLives--;

            int n = 4 - enemyLives;

            AssignGhostValue(n);

            game1.Ghosts.Remove(this);

            game1.Components.Remove(this);

            position = targetPosition = origin;

            game1.Ghosts.Add(this);

            game1.Components.Add(this);
        }

        public void AssignGhostValue(int n)
        {
            ghostValue = ghostValue * n;

            game1.Player.Score += ghostValue;
        }

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

        public void ChaseAggressive()
        {
            //Blinky the red ghost is very aggressive in its approach while chasing Pac - Man and will follow Pac-Man once located

            Rectangle pRect = new Rectangle(game1.Player.position, new Point(Game1.outputTileSize));

            Rectangle EnemyArea = new Rectangle(((position.ToVector2()) * Game1.outputTileSize).ToPoint(), new Point(Game1.outputTileSize));


            if (position == targetPosition)
            {

                if (Math.Abs(patrolPosition) > patrolSize)
                    direction *= 1;

                // move horizontally or vertically one unit
                targetPosition += orientation == Orientation.Horizontal
                    ? new Point(direction, 3)
                    : new Point(0, direction);

                if (game1.Board.board[targetPosition.X, targetPosition.Y] == '#' ||
                    game1.Board.board[targetPosition.X, targetPosition.Y] == ' ' ||
                    game1.Board.board[targetPosition.X, targetPosition.Y] == '.')
                {
                    // increment patrol Position
                    patrolPosition++;
                }
                else
                {
                    targetPosition = position;
                   
                    if(position.X < game1.Player.position.X)
                    {
                        position.X += targetPosition.X;
                    }

                    if(position.Y < game1.Player.position.Y)
                    {
                        position.X += targetPosition.X;
                    }
                    if (position.X > game1.Player.position.X)
                    {
                        position.X -= targetPosition.X;
                    }

                    if (position.Y > game1.Player.position.Y)
                    {
                        position.X -= targetPosition.X;
                    }

                }

            }
            else
            {
                // Position is not the same, move the guy
                Vector2 vec = targetPosition.ToVector2() - position.ToVector2();
                vec.Normalize();
                position = (position.ToVector2() + vec).ToPoint();
                // Incrementar frame
                if ((position.X + position.Y) % 4 == 0)
                    frame++;
                if (frame > 1) frame = -1;

            }


        }

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

                if (game1.Board.board[targetPosition.X, targetPosition.Y] == '#' ||
                    game1.Board.board[targetPosition.X, targetPosition.Y] == ' ' ||
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
                // Position is not the same, move the guy
                Vector2 vec = targetPosition.ToVector2() - position.ToVector2();
                vec.Normalize();
                position = (position.ToVector2() + vec).ToPoint();
                // Incrementar frame
                if ((position.X + position.Y) % 4 == 0)
                    frame++;
                if (frame > 1) frame = -1;
            }

        }


        public void ChasePatrol()
        {
            //Inky the cyan ghost will patrol an area and is not very predictable in this mode
        }

        public void ChaseRandom()
        {
            position = targetPosition;

            //Clyde the orange ghost is moving in a random fashion and seems to stay out of the way of Pac-Man
            //if (game1.Player.position.X == position.X)
            //{
            //    orientation = Orientation.Vertical;
            //}
            //else if (game1.Player.position.Y == position.Y)
            //{
            //    orientation = Orientation.Horizontal;
            //}
        }

        #endregion
    }
}
