using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WGDGameJam
{
    class MapManager
    {
        Square[,] map;
        Texture2D grassTexture;
        Texture2D hedgeTexture;
        Texture2D foodTexture;
        CowPiece headPiece;
        int size;

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

        public void createMap()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == 0 || j == 0) { map[i, j] = createHedge(i,j); }
                    else if (i == 4) { map[i, j] = new Square(i, j, hedgeTexture, Color.Green, false, false); }
                    else { map[i, j] = createGrass(i,j); }
                }
            }
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

        private Square createHedge(int i, int j) { return new Square(i, j, hedgeTexture, Color.Green, true, false); }
        private Square createFood(int i, int j) { return new Square(i, j, foodTexture, Color.Green, false, true); }
        private Square createGrass(int i, int j) { return new Square(i, j, grassTexture, Color.Green, false, false); }
    }
}
