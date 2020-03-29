using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace MsPacMan
{
    public class Player : DrawableGameComponent
    {
        enum Direction { Up, Down, Right, Left }



        #region Variables

        private Texture2D texture;

        private SpriteBatch spriteBatch;

        private SoundEffect deathSound;

        private SoundEffect pacmanChomp;

        private Board board;

        private Game1 game1;

        public Point position, targetPosition, origin;

        public bool allPointsCollected = false;

        public string filePath = Environment.CurrentDirectory + "/highscore.txt";

        //boolean variables

        public bool isPlayerAbleToGetNewLife = false;

        public bool isFruitAvailable = false;

        public bool isLifeEnabled = true;

        public bool isSecondLevelEnabled = false;

        public bool win = false;

        public int lives = 3;

        Dictionary<Direction, Vector2> spritePositions;

        Direction direction = Direction.Left;

        int frame = 0;

        float speed = 1.3F;

        SpriteFont minecraft;

        #endregion

        #region Constructors
        public Player(Game1 game, int x, int y) : base(game)
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

            pacmanChomp = game1.Content.Load<SoundEffect>("pacman_chomp");

            deathSound = game1.Content.Load<SoundEffect>("pacman_death");

            spritePositions = new Dictionary<Direction, Vector2>();

            spritePositions[Direction.Right] = new Vector2(5, 1);
            spritePositions[Direction.Left] = new Vector2(2, 1);
            spritePositions[Direction.Up] = new Vector2(2, 0);
            spritePositions[Direction.Down] = new Vector2(5, 0);

            minecraft = game.Content.Load<SpriteFont>("minecraft");
        }
        #endregion

        #region Properties

        /// <summary>
        /// Refers to the players current score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Refers to the highest score commited by the player
        /// </summary>
        public int HighScore { get; set; }

        #endregion

        #region Methods

        public override void Update(GameTime gameTime)
        {
            Rectangle pRect = new Rectangle(game1.Player.position, new Point(Game1.outputTileSize));

            if (Score > HighScore)
            {
                HighScore = Score;
                SetHighScore();
            }


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
                    pacmanChomp.Play();

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
            //corresponds to the position of the player
            Rectangle outRec = new Rectangle(position, new Point(Game1.outputTileSize));

            //position in sprite
            Rectangle sourceRec = new Rectangle(((spritePositions[direction] + (Vector2.UnitX * frame)) * 32).ToPoint(), new Point(32));

            spriteBatch.Begin();

            spriteBatch.Draw(texture, outRec, sourceRec, Color.White);

            //prints the score
            spriteBatch.DrawString(minecraft, $"{game1.Player.Score}", new Vector2(27 * Game1.outputTileSize, 1.12F * Game1.outputTileSize), Color.LightBlue);

            //prints the highscore
            spriteBatch.DrawString(minecraft, $"{game1.Player.HighScore}", new Vector2(15 * Game1.outputTileSize, 1.12F * Game1.outputTileSize), Color.LightBlue);

            if (lives <= 0)
            {
                this.SetHighScore();

                string gameOverText = "GAME OVER!";

                Vector2 stringSize = minecraft.MeasureString(gameOverText);

                Vector2 screenSize = new Vector2(game1.graphics.PreferredBackBufferWidth, game1.graphics.PreferredBackBufferHeight);

                Vector2 textPos = (screenSize - stringSize) / 2.0f;

                spriteBatch.DrawString(minecraft, gameOverText, textPos, Color.White);
            }

            if (CollectedAllPoints())
            {
                if (lives >= 0)
                {
                    this.SetHighScore();

                    string winText = "YOU WIN";

                    Vector2 stringSize = minecraft.MeasureString(winText);

                    Vector2 screenSize = new Vector2(game1.graphics.PreferredBackBufferWidth, game1.graphics.PreferredBackBufferHeight);

                    Vector2 textPos = (screenSize - stringSize) / 2.0f;

                    spriteBatch.DrawString(minecraft, winText, textPos, Color.White);

                    Win();

                }
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Function that's activated once the pacman is killed
        /// </summary>
        public void Die()
        {
            //sound effects
            deathSound.Play();

            //decrements lives
            lives--;

            //sets the pacman to the spawn
            position = targetPosition = origin;

            //removes the lives from the list and from the game components
            game1.Lives.Remove(game1.Live);

            game1.Components.Remove(game1.Live);

            isLifeEnabled = false;

            if (lives <= 0)
            {
                foreach (GameComponent comp in game1.Components)
                {
                    comp.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Function that verifies if all the points were collected
        /// </summary>
        /// <returns></returns>
        public bool CollectedAllPoints()
        {
            //gets the amount of dots in the map
            int amountOfDots = game1.Dots.Count();

            int amountOfPellets = game1.Pellets.Count();

            //gets the amount of point value on the map
            int totalDotPoints = amountOfDots * 10;

            int totalPelletPoints = amountOfPellets * 50;

            int total = totalDotPoints + totalPelletPoints + Cherry.cherryValue + Strawberry.strawberryValue + Upgrade.monalisaValue;

            int count = amountOfDots + amountOfPellets;
            //if the score is the same as all points caught then all the dots are collected
            if (count == 0)
                allPointsCollected = true;

            else
                allPointsCollected = false;

            return allPointsCollected;
        }

        /// <summary>
        /// Function that counts the amount of points the player has so it can assign a fruit
        /// </summary>
        public void ScoreCount()
        {
            int currentScore = game1.Player.Score;


            if (currentScore == 1000)
            {
                isFruitAvailable = true;

                game1.Cherries.Add(game1.Cherry);

                game1.Components.Add(game1.Cherry);

            }
            if (currentScore == 2000)
            {
                isFruitAvailable = true;

                game1.Strawberries.Add(game1.Strawberry);

                game1.Components.Add(game1.Strawberry);

            }
            if (currentScore == 3000)
            {
                isFruitAvailable = true;

                game1.Upgrades.Add(game1.Upgrade);

                game1.Components.Add(game1.Upgrade);

            }
            //if the player gets 10000 points he earns a life
            if (currentScore == 10000)
            {
                isPlayerAbleToGetNewLife = true;

                game1.Dots.Remove(game1.Dot);

                game1.Components.Remove(game1.Dot);

                game1.ExtraLives.Add(game1.ExtraLive);

                game1.Components.Add(game1.ExtraLive);

            }
        }

        /// <summary>
        /// Function that sets the highscore
        /// </summary>
        public void SetHighScore()
        {
            //new line to insert on the text file
            string line;

            int currentScore, highScore;

            //setting the variables to their current state
            currentScore = Score;

            highScore = HighScore;

            line = highScore.ToString();
            //comparing to get the highest score
            if (currentScore >= highScore)
            {
                highScore = currentScore;

                HighScore = highScore;

                var newHighScore = File.Create(filePath);

                newHighScore.Close();

                File.WriteAllText(filePath, line);

                newHighScore.Close();
            }
            else
            {
                HighScore = highScore;

            }

        }

        /// <summary>
        /// Function that's enabled once the player meets the winning conditions
        /// </summary>
        public void Win()
        {
            win = true;
            foreach (GameComponent comp in game1.Components)
            {
                comp.Enabled = false;
            }

            isSecondLevelEnabled = true;
        }

        #endregion
    }
}
