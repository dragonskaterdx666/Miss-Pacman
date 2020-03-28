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
    public class Upgrade : DrawableGameComponent
    {
        #region variables

        private Texture2D texture;

        private SpriteBatch spriteBatch;

        private SoundEffect eatFruitSound;

        private Game1 game1;

        private Point position;

        public static int monalisaValue = 5000;

        public Rectangle SourceUpgrade = new Rectangle(13 * 16, 9 * 16, 16, 16);

        #endregion

        #region Constructor
        public Upgrade(Game1 game, int x, int y) : base(game)
        {
            position.X = x;

            position.Y = y;

            game1 = game;

            spriteBatch = game.SpriteBatch;

            texture = game.SpriteSheet;

            eatFruitSound = game1.Content.Load<SoundEffect>("pacman_eatfruit");
        }
        #endregion

        #region Properties

        #endregion

        #region Methods

        public override void Update(GameTime gameTime)
        {
            Rectangle playerPosition = new Rectangle(game1.Player.position, new Point(16));

            Rectangle UpgradeArea = new Rectangle(((position.ToVector2()) * Game1.outputTileSize).ToPoint(), new Point(15));

            if (UpgradeArea.Intersects(playerPosition))
            {
                eatFruitSound.Play();

                game1.Components.Remove(this);

                game1.Upgrades.Remove(this);

                game1.Player.Score += 5000;

                game1.Player.ScoreCount();

            }
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle outRect = new Rectangle(position.X * Game1.outputTileSize, position.Y * Game1.outputTileSize, Game1.outputTileSize, Game1.outputTileSize);

            spriteBatch.Begin();

            spriteBatch.Draw(texture, outRect, SourceUpgrade, Color.White);

            spriteBatch.End();
        }
        #endregion

    }
}
