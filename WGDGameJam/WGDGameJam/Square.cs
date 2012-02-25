using System;
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
        Color colour;
        private bool blocking;
        private Texture2D food;
        int xlocation;
        int ylocation;
        public bool wall;
        MapManager mapManager;
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
        public void Draw(SpriteBatch spriteBatch, CowPiece head, GameTime gameTime)
        {
            Vector2 position = new Vector2((-head.headPosition.X + xlocation) * 50, (-head.headPosition.Y + ylocation) * 50) + new Vector2(350, 250) + new Vector2(400,300);
            spriteBatch.Draw(texture, position, null, colour, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f); 
        }

        public void drawFood(SpriteBatch spriteBatch, CowPiece head, GameTime gameTime)
        {
            if (food != null)
            {
                Game1 g = mapManager.getGame();
                float sizeOfScale = Math.Min(g.score * 0.05f, 0.5f);
                float scale = (float)Math.Cos(((float)gameTime.TotalGameTime.TotalMilliseconds / 1000.0f) * Math.Log(Math.Log(g.score + 1))) * sizeOfScale;
                if (g.score < 2)
                {
                    scale = 0.0f;
                }
                scale += 1.0f;
                Vector2 position = new Vector2((-head.headPosition.X + xlocation) * 50, (-head.headPosition.Y + ylocation) * 50) + new Vector2(350, 250) + new Vector2(400,300);
                float scaledWidth = food.Width * scale;
                float scaledHeight = food.Height * scale;
                Rectangle destRect = new Rectangle((int)(position.X - ((scaledWidth -food.Width) / 2.0f)), (int)(position.Y - ((scaledHeight - food.Height) / 2.0f)), (int)scaledWidth, (int)scaledHeight);
                spriteBatch.Draw(food, destRect, Color.White);
                //spriteBatch.Draw(food, position, null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            }
        }
    }
}
