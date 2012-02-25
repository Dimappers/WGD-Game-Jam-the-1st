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
        Texture2D hedgeVertTexture;
        Texture2D hedgeCornerTexturetl;
        Texture2D hedgeCornerTexturetr;
        Texture2D hedgeCornerTexturebl;
        Texture2D hedgeCornerTexturebr;
        Texture2D foodTexture;
        CowPiece headPiece;
        Random random = new Random();
        int size;
        Game1 game;

        public MapManager(int size, MapTextures mapTex, CowPiece head, Game1 game)
        {
            this.size = size;
            map = new Square[size,size];
            this.grassTexture = mapTex.grassTexture;
            this.hedgeTexture = mapTex.hedgeTexture;
            this.hedgeVertTexture = mapTex.hedgeVertTexture;
            this.hedgeCornerTexturetl = mapTex.hedgeCornerTexturetl;
            this.hedgeCornerTexturetr = mapTex.hedgeCornerTexturetr;
            this.hedgeCornerTexturebl = mapTex.hedgeCornerTexturebl;
            this.hedgeCornerTexturebr = mapTex.hedgeCornerTexturebr;
            this.foodTexture = mapTex.foodTexture;
            headPiece = head;
            this.game = game;
            createMap();
        }

        public Square getSquare(int i, int j) {return map[i,j];}
        public bool containsFood(Point location)
        {
            return map[location.X, location.Y].containsFood();
        }

        public void removeFood(Point location)
        {
            map[location.X, location.Y].takeFood();
            GenerateFood();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (!map[i, j].wall)
                    {
                        double randomnum = random.Next(200);
                        double factor = 1.0f/(game.score+1.0f);
                        int rounded = (int)Math.Round((double)(randomnum*factor));
                        if (rounded == 0)
                        {
                            int r = random.Next(255);
                            int g = random.Next(255);
                            int b = random.Next(255);
                            map[i, j].changeColour(new Color(r, g, b));
                        }
                    }
                }
            }
        }

        public void createMap()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    //Putting hedges around edges
                    if (i == 0 && j == 0) { map[i, j] = createHedge(i, j, HedgeType.corner_bottomright); }
                    else if (i == 0 && j == size - 1) { map[i, j] = createHedge(i, j, HedgeType.corner_topright); }
                    else if (i == size - 1 && j == 0) { map[i, j] = createHedge(i, j, HedgeType.corner_bottomleft); }
                    else if (i == size - 1 && j == size - 1) { map[i, j] = createHedge(i, j, HedgeType.corner_topleft); }
                    else if (i == 0 || i == size - 1) { map[i, j] = createHedge(i, j, HedgeType.vertical); }
                    else if (j == 0 || j == size - 1) { map[i, j] = createHedge(i, j, HedgeType.horizontal); }
                    else { map[i, j] = createGrass(i, j); }
                }
            }

            for (int i = 0; i < 10; ++i)
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

        public void Draw(SpriteBatch spriteBatch, Game1 game)
        {
            for(int i=0; i<size; i++)
            {
                for(int j=0; j<size; j++)
                {
                    map[i, j].Draw(spriteBatch, headPiece);
                }
            }
        }

        private Square createHedge(int i, int j, HedgeType hedge)
        {
            switch(hedge)
            {
                case HedgeType.horizontal : {return new Square(i, j, hedgeTexture, Color.White, true); }
                case HedgeType.vertical : {return new Square(i, j, hedgeVertTexture, Color.White, true); }
                case HedgeType.corner_topleft : {return new Square(i, j, hedgeCornerTexturetl, Color.White, true);}
                case HedgeType.corner_topright : {return new Square(i, j, hedgeCornerTexturetr, Color.White, true);}
                case HedgeType.corner_bottomleft : {return new Square(i, j, hedgeCornerTexturebl, Color.White, true);}
                case HedgeType.corner_bottomright: {return new Square(i, j, hedgeCornerTexturebr, Color.White, true); }
                default: return null;
            }
        }
        private Square createGrass(int i, int j){return new Square(i, j, grassTexture, Color.Green, false);}

        enum HedgeType{
            horizontal,
            vertical,
            corner_topleft,
            corner_bottomleft,
            corner_topright,
            corner_bottomright
        }
    }
}
