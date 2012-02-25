﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WGDGameJam
{
    public class Square
    {
        Texture2D texture;
        public Color colour;
        private bool blocking;
        private Texture2D food;
        protected int xlocation;
        protected int ylocation;
        public bool wall;
        public MapManager mapManager;
        public Square(int x, int y, Texture2D texture, Color colour, bool blocking, MapManager manager)
        {
            this.texture = texture;
            this.colour = colour;
            this.blocking = blocking;
            this.food = null;
            xlocation = x;
            ylocation = y;
            wall = blocking;
            this.mapManager = manager;
        }

        public bool isBlocking()
        {
            return blocking;
        }
        public void SetBlocking(bool blocking) { this.blocking = blocking; }
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
        public void changeColour(Color colour) { this.colour = colour; }
        public void Draw(SpriteBatch spriteBatch, CowPiece head)
        {
            Vector2 position = new Vector2((-head.headPosition.X + xlocation) * 50, (-head.headPosition.Y + ylocation) * 50) + new Vector2(350, 250);
            spriteBatch.Draw(texture, position, colour); 
            if (food != null)
            {
                spriteBatch.Draw(food, position, Color.White);
            }
            
        }
    }
}
