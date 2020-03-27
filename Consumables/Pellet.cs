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
    public class Pellet : DrawableGameComponent
    {
        #region variables

        private Texture2D texture;

        private SpriteBatch spriteBatch;

        private Game1 game1;

        private Point position;

        public static bool powerPellet = false;

        int pelletValue = 50;

        static float timer;

        #endregion

        #region constructor

        public Pellet(Game1 game, int x, int y) : base(game)
        {
            position.X = x;

            position.Y = y;

            game1 = game;

            spriteBatch = game.SpriteBatch;

            texture = game.SpriteSheetMap;

        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
            //decreases the timer
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            //if the timer has a value lower than 0
            if (timer < 0)
            {
                //the effect of the power pellet wears out 
                powerPellet = false;
            }

            //area occupied by player 
            Rectangle playerPosition = new Rectangle(game1.Player.position, new Point(16));

            //area occupied by pellet
            Rectangle PelletArea = new Rectangle(((position.ToVector2()) * Game1.outputTileSize).ToPoint(), new Point(20));

            //if the pellet area is intersected by the player
            if (PelletArea.Intersects(playerPosition))
            {
                //timer starts
                timer = 60f;

                //power pellet effect kicks in
                powerPellet = true;

                //pellet is removed from the map
                game1.Components.Remove(this);

                game1.Pellets.Remove(this);

                //player gets 50 points for eating the pellet
                game1.Player.Score += pelletValue;


            }

        }

        /// <summary>
        /// Method to test wether the power pellet has been consumed or not (TO BE IMPROVED)
        /// </summary>
        public static void GetPelletStatus()
        {
            if (powerPellet == true)
            {
                powerPellet = true;
            }

        }
        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            Rectangle outRect = new Rectangle(position.X * Game1.outputTileSize, position.Y * Game1.outputTileSize, Game1.outputTileSize, Game1.outputTileSize);

            //pellets
            Rectangle PowerPellets = new Rectangle(15 * 35, 4 * 35, 35, 35);
            //draws the pellet
            spriteBatch.Draw(texture, outRect, PowerPellets, Color.White);


            spriteBatch.End();
        }
        #endregion

    }
}
