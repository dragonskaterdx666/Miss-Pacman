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

        private Player p;

        private Live l;

        private List<Dot> dotList;

        private List<Pellet> pelletList;

        private List<Ghost> ghostList;

        private List<Live> liveList;

        private List<Upgrade> upgradeList;

        private List<Cherry> cherriesList;

        private Texture2D spriteSheet, spriteSheetPlayer, spriteSheetMap;

        private SpriteBatch spriteBatch;

        private Board board;

        public static Random rnd = new Random();

        public int boardWidth, boardHeight;

        public const int outputTileSize = 25;       

        public GraphicsDeviceManager graphics;

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
        public Texture2D SpriteSheet => spriteSheet;
        public Texture2D SpriteSheetPlayer => spriteSheetPlayer;        
        public Texture2D SpriteSheetMap => spriteSheetMap;

        //property refering to the textures to create the assets
        public SpriteBatch SpriteBatch => spriteBatch;

        public Player Player => p;

        public Live Live => l;

        public Board Board => board;

        public List<Dot> Dots => dotList;
        public List<Pellet> Pellets => pelletList;
        public List<Ghost> Ghosts => ghostList;
        public List<Live> Lives => liveList;
        public List<Upgrade> Upgrades => upgradeList;
        public List<Cherry> Cherries => cherriesList;

        #endregion

        #region Methods 

        #region Methods protected in the Class
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
            spriteSheetPlayer = Content.Load<Texture2D>("sprites");

            spriteSheet = Content.Load<Texture2D>("sprites2");

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

                        case 'X':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(10 * 35, 6 * 35, 35, 35), color: Color.White);
                            break;
                        /*bottom right corner*/
                        case 'D':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(7 * 35, 6 * 35, 35, 35), color: Color.White);
                            break;

                        case 'Y':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(6 * 35, 5 * 35, 35, 35), color: Color.White);
                            break;
                        /*bottom left cornerS*/
                        case 'H':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(4 * 35, 6 * 35, 35, 35), color: Color.White);
                            break;

                        case 'Q':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(5 * 35, 5 * 35, 35, 35), color: Color.White);
                            break;
                            /*top left corner*/
                        case 'P':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(9 * 35, 6 * 35, 35, 35), color: Color.White);
                            break;
                        case '0':
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
                        /*right side*/
                        case '4':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(16 * 35, 1 * 35, 35, 35), color: Color.White);
                            break;
                        case '5':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(17 * 35, 1 * 35, 35, 35), color: Color.White);
                            break;
                        case '6':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(18 * 35, 1 * 35, 35, 35), color: Color.White);
                            break;
                        case '7':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(17 * 35, 2 * 35, 35, 35), color: Color.White);
                            break;
                        case '8':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(16 * 35, 2 * 35, 35, 35), color: Color.White);
                            break;
                        case '9':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(20 * 35, 2 * 35, 35, 35), color: Color.White);
                            break;
                        case 'N':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(18 * 35, 2 * 35, 35, 35), color: Color.White);
                            break;
                        case '-':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(19 * 35, 2 * 35, 35, 35), color: Color.White);
                            break;
                        case 'O':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(18 * 35, 0, 35, 35), color: Color.White);
                            break;
                        case 'Ç':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(19 * 35, 0, 35, 35), color: Color.White);
                            break;
                        case '.':
                            spriteBatch.Draw(texture: spriteSheetMap, outRect, new Rectangle(0, 0, 35, 35), Color.White);
                            break;
                        //SPELLING HIGHSCORE
                        case '%':
                            spriteBatch.Draw(texture: spriteSheetMap, outRect, new Rectangle(0, 2 * 35, 35, 35), Color.White);
                            break;
                        case '(':
                            spriteBatch.Draw(texture: spriteSheetMap, outRect, new Rectangle(1 * 35, 2 * 35, 35, 35), Color.White);
                            break;
                        case ')':
                            spriteBatch.Draw(texture: spriteSheetMap, outRect, new Rectangle(2 * 35, 2 * 35, 35, 35), Color.White);
                            break;
                        case '$':
                            spriteBatch.Draw(texture: spriteSheetMap, outRect, new Rectangle(3 * 35, 2 * 35, 35, 35), Color.White);
                            break;
                        case '=':
                            spriteBatch.Draw(texture: spriteSheetMap, outRect, new Rectangle(4 * 35, 2 * 35, 35, 35), Color.White);
                            break;
                        case 'º':
                            spriteBatch.Draw(texture: spriteSheetMap, outRect, new Rectangle(5 * 35, 2 * 35, 35, 35), Color.White);
                            break;
                        case '´':
                            spriteBatch.Draw(texture: spriteSheetMap, outRect, new Rectangle(6 * 35, 2 * 35, 35, 35), Color.White);
                            break;
                        case '~':
                            spriteBatch.Draw(texture: spriteSheetMap, outRect, new Rectangle(7 * 35, 2 * 35, 35, 35), Color.White);
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
            string[] file = File.ReadAllLines(Content.RootDirectory + "/level1.txt");

            int y, x, i, j;

            char filePosition;

            boardWidth = file[0].Length;

            boardHeight = file.Length;

            //initializing lists
            
            ghostList = new List<Ghost>();

            dotList = new List<Dot>();

            pelletList = new List<Pellet>();

            liveList = new List<Live>();

            upgradeList = new List<Upgrade>();

            cherriesList = new List<Cherry>();

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
           
            for (i = 0; i < boardHeight; i++)
            {
                for (j = 0; j < boardWidth; j++)
                {
                    filePosition = file[i][j];

                    if (filePosition == '1' || filePosition == '2' || filePosition == '3' || filePosition == '+')
                    {
                        Ghost ghost = new Ghost(this, j, i, filePosition);

                        ghostList.Add(ghost);
                        
                        Components.Add(ghost);

                        //this removes the enemy and adds a space
                        Board.board[j, i] = ' ';
                    }
                    else if(filePosition == '?')
                    {

                        Pellet pellet = new Pellet(this, j, i);

                        pelletList.Add(pellet);

                        Components.Add(pellet);
                    }
                    
                    else if (filePosition == 'S')
                    {
                        p = new Player(this, j, i);

                        Components.Add(p);

                        p.Score = 0;

                        Board.board[j, i] = ' '; 
                    }
                    else if(filePosition == ' ')
                    {
                        Dot dot = new Dot(this, j, i);

                        dotList.Add(dot);

                        Components.Add(dot);
                    }
                    else if(filePosition == 'L')
                    {
                        l = new Live(this, j, i);
                        
                        liveList.Add(l);

                        Components.Add(l);
                    }
                    else if(filePosition == 'M')
                    {
                        Upgrade upgrade = new Upgrade(this, j, i);

                        upgradeList.Add(upgrade);

                        Components.Add(upgrade);
                    }
                    else if(filePosition == '*')
                    {
                        if(Player.CollectedAllDots() == true)
                        {
                            Cherry cherry = new Cherry(this, j, i);

                            cherriesList.Add(cherry);

                            Components.Add(cherry);
                        }
                        else
                        {
                            Dot dot = new Dot(this, j, i);

                            dotList.Add(dot);

                            Components.Add(dot);
                        }
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
