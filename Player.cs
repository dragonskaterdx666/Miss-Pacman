using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MsPacMan
{
    public class Player : DrawableGameComponent
    {

        #region Variables
        enum Direction
        {
            Up, Down, Right, Left
        }

        Dictionary<Direction, Vector2> spritePositions;

        Direction direction = Direction.Down;

        public int lives = 3;

        int frame = 0;

        public Point position, targetPosition, origin;
        Texture2D texture;
        SpriteFont arial;
        SpriteBatch spriteBatch;
        Board board;
        Game1 game;
        #endregion

        #region Constructors
        public Player(Game1 game, int x, int y) : base (game)
        {
            DrawOrder = 100; //Player should be the last object to draw

            position.Y = y * Game1.outputTileSize;
            position.X = x * Game1.outputTileSize;
            targetPosition = position;
            origin = targetPosition = position;

            texture = game.SpriteSheetPlayer;
            spriteBatch = game.SpriteBatch;
            board = game.Board;
            this.game = game;

            //Change of sprite according to the direction the player is moving
            //spritePositions = new Dictionary<Direction, Vector2>();

            //spritePositions[Direction.Right] = new Vector2(1, 1);
            //spritePositions[Direction.Left] = new Vector2(1, 0);
            //spritePositions[Direction.Up] = new Vector2(4, 1);
            //spritePositions[Direction.Down] = new Vector2(4, 0);
        }
        #endregion

        #region Methods


        public override void Update(GameTime gameTime)
        {
            if (targetPosition == position)
            {
                frame = 0;

                targetPosition = position;
                KeyboardState state = Keyboard.GetState();

                #region Player Movement Controls

                bool keyPressed = false;
                if (state.IsKeyDown(Keys.W))
                {
                    direction = Direction.Up;
                    keyPressed = true;

                }

                if (state.IsKeyDown(Keys.S))
                {
                    direction = Direction.Down;
                    keyPressed = true;

                }

                if (state.IsKeyDown(Keys.D))
                {
                    direction = Direction.Right;
                    keyPressed = true;

                }

                if (state.IsKeyDown(Keys.A))
                {
                    direction = Direction.Left;
                    keyPressed = true;
                }
                #endregion

                #region Player Movement Positions
                if (keyPressed == true)
                {

                    switch (direction)
                    {
                        case Direction.Up:
                            targetPosition.Y -= Game1.outputTileSize;
                            break;
                        case Direction.Down:
                            targetPosition.Y += Game1.outputTileSize;
                            break;
                        case Direction.Right:
                            targetPosition.X += Game1.outputTileSize;
                            break;
                        case Direction.Left:
                            targetPosition.X -= Game1.outputTileSize;
                            break;
                        default:
                            break;
                    }
                    if (board.board[targetPosition.X / Game1.outputTileSize,
                        targetPosition.Y / Game1.outputTileSize] != ' ')
                    {
                        targetPosition = position;
                    }
                }
                #endregion
            }

            else //position is not the same 
            {

                Vector2 vec = targetPosition.ToVector2() - position.ToVector2();
                vec.Normalize();
                position = (position.ToVector2() + vec).ToPoint();
                if ((position.X + position.Y) % 5 == 0) frame++;


                if (frame > 1) frame = -1;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(texture,

                new Rectangle(position, new Point(Game1.outputTileSize)),
                new Rectangle(

                    ((spritePositions[direction] + Vector2.UnitX * frame) * 16).ToPoint(),
                    new Point(16, 16)

                    ),
                Color.White

                );
            if (lives <= 0)
            {
                string gameOverText = "GAME OVER!";
                Vector2 stringSize = arial.MeasureString(gameOverText);
                Vector2 screenSize = new Vector2(game.graphics.PreferredBackBufferWidth, game.graphics.PreferredBackBufferHeight);
                Vector2 textPos = (screenSize - stringSize) / 2.0f;

                spriteBatch.DrawString(arial, gameOverText, textPos, Color.White);
            }


            spriteBatch.End();
        }

        public void Die()
        {
            lives--;
            position = targetPosition = origin;
            if (lives <= 0)
            {
                //R.I.P.

                foreach (DrawableGameComponent comp in game.Components)
                {
                    comp.Enabled = false;
                }
            }
        }
        #endregion
    }
}
