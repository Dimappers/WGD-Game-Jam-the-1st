using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WGDGameJam
{
    class Square
    {
        Texture2D texture;
        Color colour;
        private bool blocking;
        private bool food;
        int xlocation;
        int ylocation;

        public Square(int x, int y, Texture2D texture, Color colour, bool blocking, bool food)
        {
            this.texture = texture;
            this.colour = colour;
            this.blocking = blocking;
            this.food = food;
            xlocation = x;
            ylocation = y;
        }

        public bool isBlocking()
        {
            return blocking;
        }
        public bool containsFood()
        {
            return food;
        }
        public void Draw(SpriteBatch spriteBatch, CowPiece head)
        {
            Vector2 position = new Vector2((- head.headPosition.X + xlocation) * 50, (- head.headPosition.Y + ylocation) * 50);
            spriteBatch.Draw(texture, position, colour); 
        }
    }
}
