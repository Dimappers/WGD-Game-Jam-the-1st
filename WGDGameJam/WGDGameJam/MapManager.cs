using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WGDGameJam
{
    public class MapManager
    {
        Square[,] map;
        Texture2D grassTexture;
        Texture2D hedgeTexture;
        Texture2D foodTexture;
        CowPiece headPiece;
        int size;

        Random random = new Random();

        public MapManager(int size, Texture2D grassTexture, Texture2D hedgeTexture, Texture2D foodTexture, CowPiece head)
        {
            this.size = size;
            map = new Square[size,size];
            this.grassTexture = grassTexture;
            this.hedgeTexture = hedgeTexture;
            this.foodTexture = foodTexture;
            headPiece = head;
            createMap();
        }

        public bool containsFood(Point location)
        {
            return map[location.X, location.Y].containsFood();
        }

        public void removeFood(Point location)
        {
            map[location.X, location.Y].takeFood();
            GenerateFood();
        }

        public void createMap()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == 0 || j == 0) { map[i, j] = createHedge(i,j); }
                    else if (i == 4) { map[i, j] = new Square(i, j, hedgeTexture, Color.Green, false); }
                    else { map[i, j] = createGrass(i,j); }
                }
            }

            for (int i = 0; i < 50; ++i)
            {
                GenerateFood();
            }
        }

        public void GenerateFood()
        {
            int xPos, yPos;
            do
            {
                xPos = random.Next(size);
                yPos = random.Next(size);
                //TODO: Check no snake here
            }  while (map[xPos, yPos].isBlocking());

            map[xPos, yPos].giveFood(foodTexture);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i=0; i<size; i++)
            {
                for(int j=0; j<size; j++)
                {
                    map[i, j].Draw(spriteBatch, headPiece);
                }
            }
        }

        private Square createHedge(int i, int j) { return new Square(i, j, hedgeTexture, Color.Green, true); }
        private Square createGrass(int i, int j) { return new Square(i, j, grassTexture, Color.Green, false); }
    }
}
