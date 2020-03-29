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
    public class Board : DrawableGameComponent
    {
        #region Variables
        public char[,] board;

        public int height, width;
        #endregion

        #region Constructor
        public Board(Game1 game, int width, int height) : base(game)
        {
            this.width = width;
            
            this.height = height;
            
            board = new char[width, height];
        }
        #endregion

    }
}
