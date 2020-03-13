using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MsPacMan
{
    public class Game1 : Game
    {
        #region Variables

        GraphicsDeviceManager graphics;
        
        SpriteBatch spriteBatch;
        
        Texture2D spriteSheet;

        #endregion

        #region Constuctor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        #endregion

        #region Properties
        public Texture2D SpriteSheet
        {
            get
            {
                return spriteSheet;
            }
        }
        public SpriteBatch SpriteBatch 
        { 
            get 
            { 
                return spriteBatch; 
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
            spriteSheet = Content.Load<Texture2D>("mspacman");

            spriteSheet = Content.Load<Texture2D>("sprites");

            spriteSheet = Content.Load<Texture2D>("sprites2");

           
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
            GraphicsDevice.Clear(Color.CornflowerBlue);


            base.Draw(gameTime);
        }
        #endregion

        public void LoadLevel()
        {
            //this reads the file from content
            string[] file = File.ReadAllLines(Content.RootDirectory + "/map.txt");
        }
        #endregion

    }

}
