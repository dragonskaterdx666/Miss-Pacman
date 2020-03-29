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

    public class Game1 : Game
    {
        #region Variables

        private Player p;

        private Live l;

        private ExtraLive extraLive;

        private Dot d;

        private Cherry c;

        private Strawberry strawberry;

        private Upgrade u;

        private Pellet pellet;

        #region lists
        private List<Dot> dotList;

        private List<Pellet> pelletList;

        private List<Ghost> ghostList;

        private List<Live> liveList;

        private List<ExtraLive> extraLiveList;

        private List<Upgrade> upgradeList;

        private List<Cherry> cherriesList;

        private List<Strawberry> strawberryList;
        #endregion

        private Texture2D spriteSheet, spriteSheetPlayer, spriteSheetMap;

        private SpriteBatch spriteBatch;

        private SoundEffect beginning;

        private Board board;

        //random variables
        public static Random rnd = new Random();

        public Random randomX = new Random();

        public Random randomY = new Random();

        //board related variables
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

        public ExtraLive ExtraLive => extraLive;

        public Dot Dot => d;

        public Cherry Cherry => c;

        public Strawberry Strawberry => strawberry;

        public Upgrade Upgrade => u;

        public Pellet Pellet => pellet;

        public Board Board => board;

        public List<Dot> Dots => dotList;
        public List<Pellet> Pellets => pelletList;
        public List<Ghost> Ghosts => ghostList;
        public List<Live> Lives => liveList;
        public List<ExtraLive> ExtraLives => extraLiveList;
        public List<Upgrade> Upgrades => upgradeList;
        public List<Cherry> Cherries => cherriesList;

        public List<Strawberry> Strawberries => strawberryList;

        string file;

        int n = 3;
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

            file = File.ReadAllText(Player.filePath);

            Player.HighScore = Int32.Parse(file);

            beginning.Play();

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

            beginning = Content.Load<SoundEffect>("pacman_beginning");

            //calls the function responsible for initializing the level
            LoadLevel();

            if(p.isSecondLevelEnabled)
            {
                LoadSecondLevel();
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            if(p.isSecondLevelEnabled)
            {
                LoadLevel();
            }
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
                        #region HORIZONTAL
                        //HORIZONTAL LINES
                        /*bottom but mid top*/
                        case 'T':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(0, 5 * 35, 35, 35), color: Color.White);
                            break;
                        /*top but mid bottom*/
                        case 'A':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(0, 6 * 35, 35, 35), color: Color.White);
                            break;
                        #endregion

                        #region ROUNDED CORNERS
                        //ROUNDED CORNERS 
                        /*top left corner*/
                        case '0':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(8 * 35, 5 * 35, 35, 35), color: Color.White);
                            break;
                            /*top right corner*/
                        case 'B':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(11 * 35, 5 * 35, 35, 35), color: Color.White);
                            break;
                         /*bottom left cornerS*/
                        case 'H':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(4 * 35, 6 * 35, 35, 35), color: Color.White);
                            break;
                            /*bottom right corner*/
                        case 'D':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(7 * 35, 6 * 35, 35, 35), color: Color.White);
                            break;
                        #endregion

                        #region CORNERS
                        case 'X':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(10 * 35, 6 * 35, 35, 35), color: Color.White);
                            break;                       
                        case 'Y':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(6 * 35, 5 * 35, 35, 35), color: Color.White);
                            break;                      
                        case 'Q':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(5 * 35, 5 * 35, 35, 35), color: Color.White);
                            break;
                        /*top left corner*/
                        case 'P':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(9 * 35, 6 * 35, 35, 35), color: Color.White);
                            break;
                        #endregion

                        #region VERTICAL
                        //VERTICAL LINES
                        /*left side vertical lines*/
                        case 'R':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(3 * 35, 6 * 35, 35, 35), color: Color.White);
                            break;
                        /*right side vertical lines*/
                        case 'V':
                            spriteBatch.Draw(texture: SpriteSheetMap, destinationRectangle: outRect, sourceRectangle: new Rectangle(2 * 35, 6 * 35, 35, 35), color: Color.White);
                            break;
                        #endregion

                        #region MIDDLE SECTION
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
                        #endregion

                        #region SPELLING AND WORDS
                        //SPELLING HIGHSCORE, SCORE AND LIVES USING THE SPRITESHEET
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
                        case '»':
                            spriteBatch.Draw(texture: spriteSheetMap, outRect, new Rectangle(8 * 35, 2 * 35, 35, 35), Color.White);
                            break;
                        case '«':
                            spriteBatch.Draw(texture: spriteSheetMap, outRect, new Rectangle(9 * 35, 2 * 35, 35, 35), Color.White);
                            break;
                        #endregion

                        #region CORNER ASSET
                        //ASSETS SHOWN
                        case 'M':
                            spriteBatch.Draw(texture: spriteSheet, outRect, new Rectangle(13 * 16, 9 * 16, 16, 16), Color.White);
                            break;
                            #endregion
                    }

                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion

        /// <summary>
        /// Loads the first level of the game
        /// </summary>
        public void LoadLevel()
        {
            //this reads the file from content
            string[] file = File.ReadAllLines(Content.RootDirectory + "/level1.txt");

            int y, x, i, j, rndPosX, rndPosY, rndPosX2, rndPosY2, y2, x2, x3;

            Random backUpRndY = new Random();

            Random randomX2 = new Random();

            Random randomY2 = new Random();

            x2 = 6; x3 = 22;

            y2 = backUpRndY.Next(9, 24);

            rndPosX = randomX.Next(2, 30);

            rndPosY = randomY.Next(4, 29);

            rndPosX2 = randomX2.Next(2, 30);

            rndPosY2 = randomY2.Next(4, 29);

            char filePosition;

            char randomPos;

            boardWidth = file[0].Length;

            boardHeight = file.Length;

            //initializing lists

            ghostList = new List<Ghost>();

            dotList = new List<Dot>();

            pelletList = new List<Pellet>();

            liveList = new List<Live>();

            upgradeList = new List<Upgrade>();

            cherriesList = new List<Cherry>();

            extraLiveList = new List<ExtraLive>();

            strawberryList = new List<Strawberry>();

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

                    if (filePosition == '1')
                    {
                        int i1;

                        for (i1 = 0; i1 < 4; i1++)
                        {
                            Ghost ghost = new Ghost(this, j, i, i1);

                            ghostList.Add(ghost);

                            Components.Add(ghost);
                        }

                        //this removes the enemy and adds a space
                        Board.board[j, i] = ' ';
                    }
                    else if (filePosition == '?')
                    {
                        pellet = new Pellet(this, j, i);

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
                    else if (filePosition == ' ')
                    {
                        d = new Dot(this, j, i);

                        dotList.Add(d);

                        Components.Add(d);
                    }
                    else if (filePosition == 'L')
                    {
                        l = new Live(this, j, i);

                        liveList.Add(l);

                        Components.Add(l);
                    }
                    else if (filePosition == '*')
                    {
                        randomPos = file[rndPosX][rndPosY];

                        //to avoid appearing in assets
                        if (randomPos == ' ')
                        {
                            //randomly assigns the cherry spot
                            c = new Cherry(this, rndPosX, rndPosY);

                            //randomly assigns the upgrade spot
                            u = new Upgrade(this, rndPosX2, rndPosY2);

                            strawberry = new Strawberry(this, rndPosX2, rndPosY);
                        }
                        else
                        {
                            //randomly assigns the cherry spot
                            c = new Cherry(this, x2, y2);

                            //randomly assigns the upgrade spot
                            u = new Upgrade(this, x3, y2);

                            strawberry = new Strawberry(this, x2, y2);
                        }

                        if (p.Score != 10000)
                        {
                            d = new Dot(this, j, i);

                            dotList.Add(d);

                            Components.Add(d);
                        }

                    }
                    else if (filePosition == '£')
                    {
                        if ((p.Score >= 10000) && p.isPlayerAbleToGetNewLife == true)
                        {
                            extraLive = new ExtraLive(this, j, i);

                            extraLiveList.Add(extraLive);

                            Components.Add(extraLive);
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

        /// <summary>
        /// L
        /// </summary>
        public void LoadSecondLevel()
        {
            //this reads the file from content
            string[] file = File.ReadAllLines(Content.RootDirectory + "/level2.txt");

            #region VARIABLES INIT
            //instanciating variables
            int y, x, i, j, rndPosX, rndPosY, rndPosX2, colunaE, colunaD;

            rndPosX = randomX.Next(4, 27);

            rndPosY = randomY.Next(4, 23);

            rndPosX2 = randomX.Next(5, 27);

            colunaE = 7;

            colunaD = 26;

            char filePosition;

            char randomPos;

            boardWidth = file[0].Length;

            boardHeight = file.Length;
            #endregion

            #region LISTS INIT
            //initializing lists

            ghostList = new List<Ghost>();

            dotList = new List<Dot>();

            pelletList = new List<Pellet>();

            liveList = new List<Live>();

            upgradeList = new List<Upgrade>();

            cherriesList = new List<Cherry>();

            extraLiveList = new List<ExtraLive>();

            strawberryList = new List<Strawberry>();
            #endregion

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

            //WRITTING IN BOARD
            for (i = 0; i < boardHeight; i++)
            {
                for (j = 0; j < boardWidth; j++)
                {
                    filePosition = file[i][j];

                    if (filePosition == '1')
                    {
                        int i1;

                        for (i1 = 0; i1 < 4; i1++)
                        {
                            Ghost ghost = new Ghost(this, j, i, i1);

                            ghostList.Add(ghost);

                            Components.Add(ghost);
                        }

                        //this removes the enemy and adds a space
                        Board.board[j, i] = ' ';
                    }
                    else if (filePosition == '?')
                    {
                        pellet = new Pellet(this, j, i);

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
                    else if (filePosition == ' ')
                    {

                        d = new Dot(this, j, i);

                        dotList.Add(d);

                        Components.Add(d);

                    }
                    else if (filePosition == '*')
                    {
                        randomPos = file[rndPosX][rndPosY];

                        //to avoid appearing in assets
                        if (randomPos == ' ')
                        {
                            //randomly assigns the cherry spot
                            c = new Cherry(this, rndPosY, rndPosX);

                            rndPosX = randomX.Next(5, 29);

                            rndPosY = randomY.Next(5, 23);

                            //randomly assigns the upgrade spot
                            u = new Upgrade(this, colunaE, rndPosX);

                            rndPosX = randomX.Next(5, 29);

                            strawberry = new Strawberry(this, colunaD, rndPosX);
                        }

                        if (p.Score != 10000)
                        {
                            d = new Dot(this, j, i);

                            dotList.Add(d);

                            Components.Add(d);
                        }

                    }
                    else if (filePosition == 'L')
                    {
                        l = new Live(this, j, i);

                        liveList.Add(l);

                        Components.Add(l);
                    }
                    else if (filePosition == '£')
                    {
                        if ((p.Score >= 10000) && p.isPlayerAbleToGetNewLife == true)
                        {
                            extraLive = new ExtraLive(this, j, i);

                            extraLiveList.Add(extraLive);

                            Components.Add(extraLive);
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
