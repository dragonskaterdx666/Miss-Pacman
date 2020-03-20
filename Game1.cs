using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MsPacMan
{
    
    public class Game1 : Game
    {
        #region Variables
        
        public const int outputTileSize = 25;       

        public GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        Texture2D spriteSheet, spriteSheetPlayer, spriteSheetMap, spriteSheetAssets;

        Board board;

        List<Ghosts> ghosts;

        public int boardWidth, boardHeight;

        #endregion

        #region Constuctor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        #endregion

        #region Properties

        //Properties referring to the assets used on the game
        public Texture2D SpriteSheet
        {
            get
            {
                return spriteSheet;
            }
        }
        public Texture2D SpriteSheetPlayer
        {
            get
            {
                return spriteSheetPlayer;
            }
        }
        public Texture2D SpriteSheetMap
        {
            get
            {
                return spriteSheetMap;
            }
        }
        public Texture2D SpriteSheetAssets
        {
            get
            {
                return spriteSheetAssets;
            }
        }


        //property refering to the textures to create the assets
        public SpriteBatch SpriteBatch
        {
            get
            {
                return spriteBatch;
            }
        }
        public Board Board
        {
            get
            {
                return board;
            }
        }

        #endregion

        #region Methods 

        #region Methods within the Class
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic related content.  
        /// Calling base.Initialize will enumerate through any components and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //creating the spritesheets that are going to be used
            spriteSheet = Content.Load<Texture2D>("sprites");

            spriteSheetPlayer = Content.Load<Texture2D>("mspacman");

            spriteSheetAssets = Content.Load<Texture2D>("sprites2");

            spriteSheetMap = Content.Load<Texture2D>("mappa");

            //calls the function responsible for initializing the level
            LoadLevel();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            int x, y;

            GraphicsDevice.Clear(Color.TransparentBlack);

            spriteBatch.Begin();

            //drawing the map
            for (x = 0; x < boardWidth; x++)
            {
                for (y = 0; y < boardHeight; y++)
                {
                    Rectangle outRect = new Rectangle(x * outputTileSize, y * outputTileSize, outputTileSize, outputTileSize);

                    switch (Board.board[x, y])
                    {
                        //HORIZONTAL LINES
                        /*top*/
                        case 'T':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(0, 5 * 35, 35, 35), color: Color.White);
                            break;
                        /*bottom*/
                        case 'A':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(0, 6 * 35, 35, 35), color: Color.White);
                            break;

                            //CORNERS 
                            /*top right corner*/
                        case 'B':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(11 * 35, 5 * 35, 35, 35), color: Color.White);
                            break;
                           /*bottom right corner*/
                        case 'D':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(7 * 35, 6 * 35, 35, 35), color: Color.White);
                            break;                       
                            /*bottom left corner*/
                        case 'Q':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(4 * 35, 6 * 35, 35, 35), color: Color.White);
                            break;
                            /*top left corner*/
                        case 'P':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(8 * 35, 5 * 35, 35, 35), color: Color.White);
                            break;

                            //VERTICAL LINES
                            /*left side vertical lines*/
                        case 'R':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(3 * 35, 6 * 35, 35, 35), color: Color.White);
                            break;
                            /*right side vertical lines*/
                       case 'V':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(2 * 35, 6 * 35, 35, 35), color: Color.White);
                            break;
                            //special corners for more rounded places
                            /*left side*/
                        case 'X':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(22 * 35, 0, 35, 35), color: Color.White);
                            break;
                            /*right side*/
                        case 'Y':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(23 * 35, 0, 35, 35), color: Color.White);
                            break;
                        //GHOST SPAWN
                        /*middle top left corner*/
                        case '4':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(29 * 35, 0, 35, 35), color: Color.White);
                            break;
                        /*middle top right corner*/
                        case '5':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(28 * 35, 0, 35, 35), color: Color.White);
                            break;
                        /*middle bottom left corner*/
                        case '6':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(31 * 35, 0, 35, 35), color: Color.White);
                            break;
                        /*middle bottom right corner*/
                        case '7':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(30 * 35, 0, 35, 35), color: Color.White);
                            break;
                        /*middle left vertical lines*/
                        case '8':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(18 * 35, 1 * 35, 35, 35), color: Color.White);
                            break;
                        /*middle right vertical lines*/
                        case '9':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(19 * 35, 1 * 35, 35, 35), color: Color.White);
                            break;
                        /*middle bottom horizontal lines*/
                        case 'K':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(27 * 35, 1 * 35, 35, 35), color: Color.White);
                            break;
                        /*middle top horizontal lines*/
                        case 'W':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(28 * 35, 1 * 35, 35, 35), color: Color.White);
                            break;
                        //this represents the non specified letters or numbers on the switch case and by default adds the color of the background
                        default:
                            spriteBatch.Draw(spriteSheetMap, outRect, new Rectangle(0, 2 * 35, 35, 35), Color.White);
                            break;
                    }

                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion

        public void LoadLevel()
        {
            //this reads the file from content
            string[] file = File.ReadAllLines(Content.RootDirectory + "/map.txt");

            int y, x, i, j;

            boardWidth = file[0].Length;

            boardHeight = file.Length;

            //board takes in as size arguments the board size and the board width
            board = new Board(this, boardWidth, boardHeight);
            Components.Add(board);

            for (y = 0; y < boardHeight; y++)
            {
                for (x = 0; x < boardWidth; x++)
                {
                    Board.board[x, y] = file[y][x];
                }
            }
            ghosts = new List<Ghosts>();

            for (i = 0; i < boardHeight; i++)
            {
                for (j = 0; j < boardWidth; j++)
                {
                    if (file[i][j] == 'G' || file[i][j] == '1' || file[i][j] == '2' || file[i][j] == '3' || file[i][j] == 'E' || file[i][j] == 'C' )
                    {
                        Ghosts ghost = new Ghosts(this, j, i, file[i][j]);

                        ghosts.Add(ghost);
                        
                        Components.Add(ghost);

                        //this removes the enemy and adds a space
                        Board.board[j, i] = ' ';
                    }
                    else
                    {
                        Board.board[j, i] = file[i][j];
                    }
                }
            }

            //Set Preferred Window Size
            graphics.PreferredBackBufferWidth = boardWidth * outputTileSize;
            graphics.PreferredBackBufferHeight = (boardHeight + 1) * outputTileSize;
            graphics.ApplyChanges();
        }
        #endregion

    }

}
