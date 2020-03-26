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
    public class Ghost : DrawableGameComponent
    {
        enum Orientation { Horizontal, Vertical }

        enum GDirection { Up, Down, Left, Right }

        enum GhostTypes { Blue, Orange, Purple, Red }

        #region variables

        private Texture2D texture;

        private SpriteBatch spriteBatch;

        private Game1 game1;

        public Board board;
        
        private char ghostType;
        
        private Orientation orientation;

        public Point position, targetPosition;

        int enemyLives = 1;

        int patrolSize;
        
        int patrolPosition = 0;
        
        int direction = 1;

        Dictionary<GDirection, Vector2> ghostRed;

        Dictionary<GDirection, Vector2> ghostOrange;

        Dictionary<GDirection, Vector2> ghostPurple;

        Dictionary<GDirection, Vector2> ghostBlue;

        GDirection gDirection = GDirection.Up;

        int frame = 0;

        #endregion

        #region Constructor
        public Ghost(Game1 game, int x, int y, char ghostType) : base(game)
        {
            orientation = Game1.rnd.Next(2) > 0 ? Orientation.Horizontal : Orientation.Vertical;
            
            texture = game.SpriteSheet;
            
            spriteBatch = game.SpriteBatch;
            
            position.Y = y;
            
            position.X = x;

            targetPosition = position;//+ (new Point(0, -2 * 16));
            
            game1 = game;
            
            board = game1.Board;
            
            this.ghostType = ghostType;

            patrolSize = 2 + Game1.rnd.Next(4);

            #region ghost sprites

            ghostRed = new Dictionary<GDirection, Vector2>();

            ghostRed[GDirection.Right] = new Vector2(0, 0);
            ghostRed[GDirection.Left] = new Vector2(2, 0);
            ghostRed[GDirection.Up] = new Vector2(4, 0);
            ghostRed[GDirection.Down] = new Vector2(6, 0);


            ghostPurple = new Dictionary<GDirection, Vector2>();

            ghostPurple[GDirection.Right] = new Vector2(0, 4);
            ghostPurple[GDirection.Left] = new Vector2(2, 4);
            ghostPurple[GDirection.Up] = new Vector2(4, 4);
            ghostPurple[GDirection.Down] = new Vector2(6, 4);


            ghostBlue = new Dictionary<GDirection, Vector2>();

            ghostBlue[GDirection.Right] = new Vector2(0, 2);
            ghostBlue[GDirection.Left] = new Vector2(2, 2);
            ghostBlue[GDirection.Up] = new Vector2(4, 2);
            ghostBlue[GDirection.Down] = new Vector2(6, 2);


            ghostOrange = new Dictionary<GDirection, Vector2>();

            ghostOrange[GDirection.Right] = new Vector2(0, 3);
            ghostOrange[GDirection.Left] = new Vector2(2, 3);
            ghostOrange[GDirection.Up] = new Vector2(4, 3);
            ghostOrange[GDirection.Down] = new Vector2(6, 3);

            #endregion
        }


        #endregion

        #region Properties
        public Board Board => board;

        #endregion

        #region Methods


        public override void Update(GameTime gameTime)
        {
            Rectangle pRect = new Rectangle(game1.Player.position, new Point(Game1.outputTileSize));
            Rectangle EnemyArea = new Rectangle(((position.ToVector2()) * Game1.outputTileSize).ToPoint(), new Point(Game1.outputTileSize));

            if (EnemyArea.Intersects(pRect))
            {
                Pellet.GetPelletStatus();

                if (Pellet.powerPellet == true)
                {
                    this.Die();
                }
                else
                {
                    game1.Player.Die();
                }
                
            }

            if (position == targetPosition)
            {
                if (Math.Abs(patrolPosition) > patrolSize)
                    direction *= 1;

                targetPosition +=
                    orientation == Orientation.Horizontal
                    ? new Point(direction * Game1.outputTileSize, 0) : new Point(0, direction * Game1.outputTileSize);

                if ((game1.Board.board[targetPosition.X / Game1.outputTileSize,
                        targetPosition.Y / Game1.outputTileSize] == ' ') && (game1.Board.board[targetPosition.X / Game1.outputTileSize,
                        targetPosition.Y / Game1.outputTileSize] == '.') && game1.Board.board[targetPosition.X / Game1.outputTileSize,
                        targetPosition.Y / Game1.outputTileSize] == '?')
                    {
                    patrolPosition++;
                }
                else
                {
                    targetPosition = position;
                    direction = -direction;
                }

            }
            else
            {
                Vector2 dir = (targetPosition - position).ToVector2();
                dir.Normalize();
                position += dir.ToPoint();
            }
        }

        //Draws the different types of ghosts
        public override void Draw(GameTime gameTime)
        {
            #region ghosts
            Rectangle outRect = new Rectangle(position.X * Game1.outputTileSize, position.Y * Game1.outputTileSize, Game1.outputTileSize, Game1.outputTileSize);
            Rectangle sourceRedRec = new Rectangle(((ghostRed[gDirection] + (Vector2.UnitX * frame)) * 16).ToPoint(), new Point(15));
            Rectangle sourcePurpleRec = new Rectangle(((ghostPurple[gDirection] + (Vector2.UnitX * frame)) * 16).ToPoint(), new Point(15));
            Rectangle sourceBlueRec = new Rectangle(((ghostBlue[gDirection] + (Vector2.UnitX * frame)) * 16).ToPoint(), new Point(15));
            Rectangle sourceOrangeRec = new Rectangle(((ghostOrange[gDirection] + (Vector2.UnitX * frame)) * 16).ToPoint(), new Point(15));
            Rectangle sourcePelletRec1 = new Rectangle(8*16, 0, 16, 15);

            GhostTypes ghostTypes;

            spriteBatch.Begin();

            Pellet.GetPelletStatus();

            if (Pellet.powerPellet == false)
            {
                switch (ghostType)
                {
                    //allows for different types of ghosts
                    case '1':
                        spriteBatch.Draw(texture, outRect, sourceBlueRec, Color.White);
                        ghostTypes = GhostTypes.Blue;
                        break;
                    case '2':
                        spriteBatch.Draw(texture, outRect, sourceOrangeRec, Color.White);
                        ghostTypes = GhostTypes.Orange;
                        break;
                    case '3':
                        spriteBatch.Draw(texture, outRect, sourcePurpleRec, Color.White);
                        ghostTypes = GhostTypes.Purple;
                        break;
                    case '+':
                        spriteBatch.Draw(texture, outRect, sourceRedRec, Color.White);
                        ghostTypes = GhostTypes.Red;
                        break;
                }
            }
            else
            {
                switch (ghostType)
                {
                    //allows for different types of ghosts
                    case '1':
                        spriteBatch.Draw(texture, outRect, sourcePelletRec1, Color.White);
                        ghostTypes = GhostTypes.Blue;
                        break;
                    case '2':
                        spriteBatch.Draw(texture, outRect, sourcePelletRec1, Color.White);
                        ghostTypes = GhostTypes.Orange;
                        break;
                    case '3':
                        spriteBatch.Draw(texture, outRect, sourcePelletRec1, Color.White);
                        ghostTypes = GhostTypes.Purple;
                        break;
                    case '+':
                        spriteBatch.Draw(texture, outRect, sourcePelletRec1, Color.White);
                        ghostTypes = GhostTypes.Red;
                        break;

                        #endregion
                }
            }

           

            spriteBatch.End();
        }

        public void Die()
        {
            enemyLives--;

            game1.Ghosts.Remove(this);

            game1.Components.Remove(this);
        }

        #endregion
    }
}
