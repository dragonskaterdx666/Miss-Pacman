using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MsPacMan
{
    public class Player /*: DrawableGameComponent*/
    {

        //Variables
        enum Direction
        {
            Up, Down, Right, Left
        }

        Texture2D texture;
        SpriteBatch spriteBatch;
        Game1 game;

        #region Methods
        public Player(Game1 game, int x, int y) 
        {
            //DrawOrder = 100; //Player should be the last object to draw
        }

        //public override void Update()
        //{

        //}

        #endregion
    }
}
