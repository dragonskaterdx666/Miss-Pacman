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

        private Board board;

        private Point position;

        Score score;

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

        public override void Update(GameTime gameTime)
        {
            Rectangle playerPosition = new Rectangle(game1.player.position, new Point(16));

            Rectangle PelletArea = new Rectangle(((position.ToVector2()) * Game1.outputTileSize).ToPoint(), new Point(20));

            if (PelletArea.Intersects(playerPosition))
            {
                game1.Components.Remove(this);

                game1.Pellets.Remove(this);

                game1.Scores.Add(score);

            }
        }

        
        #endregion

    }
}
