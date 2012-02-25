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
        private Texture2D food;
        int xlocation;
        int ylocation;

        public Square(int x, int y, Texture2D texture, Color colour, bool blocking)
        {
            this.texture = texture;
            this.colour = colour;
            this.blocking = blocking;
            this.food = null;
            xlocation = x;
            ylocation = y;
        }

        public bool isBlocking()
        {
            return blocking;
        }
        public bool containsFood()
        {
            return food != null;
        }

        public void giveFood(Texture2D food)
        {
            this.food = food;
        }

        public void takeFood()
        {
            this.food = null;
        }

        public void Draw(SpriteBatch spriteBatch, CowPiece head)
        {
            Vector2 position = new Vector2((- head.headPosition.X + xlocation) * 50, (- head.headPosition.Y + ylocation) * 50);
            spriteBatch.Draw(texture, position, colour); 
            if (food != null)
            {
                spriteBatch.Draw(food, position + new Vector2(12, 12), Color.White);
            }
            
        }
    }
}
