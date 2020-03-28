using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace MsPacMan
{
    public class Cherry : DrawableGameComponent
    {

        #region variables

        private Texture2D texture;

        private SpriteBatch spriteBatch;

        private SoundEffect eatFruitSound;

        private Game1 game1;

        private Point position;

        public static int cherryValue = 100;

        #endregion

        #region Constructor
        public Cherry(Game1 game, int x, int y) : base(game)
        {
            position.X = x;

            position.Y = y;

            game1 = game;

            spriteBatch = game.SpriteBatch;

            texture = game.SpriteSheet;

            eatFruitSound = game1.Content.Load<SoundEffect>("pacman_eatfruit");

        }
        #endregion

        #region Methods

        public override void Update(GameTime gameTime)
        {
            Rectangle playerPosition = new Rectangle(game1.Player.position, new Point(16));

            Rectangle CherryArea = new Rectangle((position.ToVector2() * Game1.outputTileSize).ToPoint(), new Point(11));

            if (CherryArea.Intersects(playerPosition))
            {
                eatFruitSound.Play();

                game1.Cherries.Remove(this);

                game1.Components.Remove(this);

                game1.Player.Score += cherryValue;
            }
        }

        /// <summary>
        /// Draws the cherries in game
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            Rectangle sourceCherry = new Rectangle(0, 7 * 16, 16, 15);

            Rectangle outRect = new Rectangle(position.X * Game1.outputTileSize, position.Y * Game1.outputTileSize, Game1.outputTileSize, Game1.outputTileSize);

            spriteBatch.Begin();

            spriteBatch.Draw(texture, outRect, sourceCherry, Color.White);

            spriteBatch.End();
        }
        #endregion
    }
}
