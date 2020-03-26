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
        enum Direction { Up, Down, Right, Left }

        #region Variables

        private Texture2D texture;
        
        private SpriteBatch spriteBatch;
        
        private Board board;
        
        private Game1 game1;

        public Point position, targetPosition, origin;

        public bool allDotsCollected = false;
       
        public int lives = 3;
        
        Dictionary<Direction, Vector2> spritePositions;

        Direction direction = Direction.Left;

        int frame = 0;

        float speed = 2.0f;

        SpriteFont arial;
          
        #endregion

        #region Constructors
        public Player(Game1 game, int x, int y) : base (game)
        {
            //Player should be the last object to draw
            DrawOrder = 100; 
            
            this.game1 = game;
            
            position.Y = y * Game1.outputTileSize;
            
            position.X = x * Game1.outputTileSize;
            
            targetPosition = position;
            
            origin = targetPosition = position;

            texture = game1.SpriteSheetPlayer;
            
            spriteBatch = game1.SpriteBatch;
            
            board = game1.Board;
            
            spritePositions = new Dictionary<Direction, Vector2>();

            spritePositions[Direction.Right] = new Vector2(5, 1);
            spritePositions[Direction.Left] = new Vector2(2, 1);
            spritePositions[Direction.Up] = new Vector2(2, 0);
            spritePositions[Direction.Down] = new Vector2(5, 0);

            arial = game.Content.Load<SpriteFont>("arial");

            //spritePositions[Direction.Right] = new Vector2(3.2f, 1);
            //spritePositions[Direction.Left] = new Vector2(0, 1);
            //spritePositions[Direction.Up] = new Vector2(2, 0);
            //spritePositions[Direction.Down] = new Vector2(5.2f, 0);
        }
        #endregion

        #region Properties

        public int Score { get; set; }

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
                    if ((board.board[targetPosition.X / Game1.outputTileSize,
                        targetPosition.Y / Game1.outputTileSize] != ' ') && (board.board[targetPosition.X / Game1.outputTileSize,
                        targetPosition.Y / Game1.outputTileSize] != '.') && (board.board[targetPosition.X / Game1.outputTileSize,
                        targetPosition.Y / Game1.outputTileSize] != '?') && (board.board[targetPosition.X / Game1.outputTileSize,
                        targetPosition.Y / Game1.outputTileSize] != '*'))
                    {
                        targetPosition = position;
                    }
                }
                #endregion
            }
            // if the position is not the same
            else  
            {
                Vector2 vec = targetPosition.ToVector2() - position.ToVector2();
               
                vec.Normalize();
                
                position = (position.ToVector2() + vec).ToPoint();
                
                if ((position.X + position.Y) % 6 == 0) frame++;


                if (frame > 0) frame = -2;
            }
        }

        //Draws pacman on the different positions
        public override void Draw(GameTime gameTime)
        {
            Rectangle outRec = new Rectangle(position, new Point(Game1.outputTileSize));

            Rectangle sourceRec = new Rectangle(((spritePositions[direction] + (Vector2.UnitX * frame)) * 32).ToPoint(), new Point(32));

            spriteBatch.Begin();

            spriteBatch.Draw(texture, outRec, sourceRec, Color.White);

            spriteBatch.DrawString(arial, $"{game1.Player.Score}", new Vector2(27 * Game1.outputTileSize, 1.3F * Game1.outputTileSize), Color.White);

            if (lives <= 0)
            {
                string gameOverText = "GAME OVER!";
                
                Vector2 stringSize = arial.MeasureString(gameOverText);
                
                Vector2 screenSize = new Vector2(game1.graphics.PreferredBackBufferWidth, game1.graphics.PreferredBackBufferHeight);
                
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

                foreach (DrawableGameComponent gameComponent in game1.Components)
                {
                    gameComponent.Enabled = false;
                }
            }
        }

        public bool CollectedAllDots()
        {
            int amountOfDots = game1.Dots.Count;

            int totalDotPoints = amountOfDots * 10;

            if(Score == totalDotPoints)
                allDotsCollected = true;
            
            else
                allDotsCollected = false;
            
            return allDotsCollected;
        }

        public void ScoreCount()
        {
            int i;

            int amountOfDots = game1.Dots.Count;

            int totalDotPoints = amountOfDots * 10;

            for (i = 0; i < totalDotPoints; i++)
            {
                //if the player gets 1000 points he earns a life
                if(game1.Player.Score == 1000)
                {
                    Live live = new Live(game1, game1.boardWidth, game1.boardHeight);

                    game1.Lives.Add(live);
                }
                if(game1.Player.Score == 2000)
                {
                    Live live = new Live(game1, game1.boardWidth, game1.boardHeight);

                    game1.Lives.Add(live);
                }
                if(game1.Player.Score == 3000)
                {
                    Live live = new Live(game1, game1.boardWidth, game1.boardHeight);

                    game1.Lives.Add(live);
                }
            }

        }
        #endregion
    }
}
