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
        Texture2D hedgeVertTexture;
        Texture2D hedgeCornerTexturetl;
        Texture2D hedgeCornerTexturetr;
        Texture2D hedgeCornerTexturebl;
        Texture2D hedgeCornerTexturebr;
        Texture2D foodTexture;
        CowPiece headPiece;
        Random random = new Random();
        int size;

        public MapManager(int size, MapTextures mapTex, CowPiece head)
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
            createMap();
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

        private Square createHedge(int i, int j, HedgeType hedge)
        {
            switch(hedge)
            {
                case HedgeType.horizontal : {return new Square(i, j, hedgeTexture, Color.White, true, false); }
                case HedgeType.vertical : {return new Square(i, j, hedgeVertTexture, Color.White, true, false); }
                case HedgeType.corner_topleft : {return new Square(i, j, hedgeCornerTexturetl, Color.White, true, false);}
                case HedgeType.corner_topright : {return new Square(i, j, hedgeCornerTexturetr, Color.White, true, false);}
                case HedgeType.corner_bottomleft : {return new Square(i, j, hedgeCornerTexturebl, Color.White, true, false);}
                case HedgeType.corner_bottomright: {return new Square(i, j, hedgeCornerTexturebr, Color.White, true, false); }
                default: return null;
            }
        }
        private Square createFood(int i, int j) { return new Square(i, j, foodTexture, Color.Green, false, true); }
        private Square createGrass(int i, int j)
        {
            int r = random.Next(255);
            int g = random.Next(255);
            int b = random.Next(255); 
            return new Square(i, j, grassTexture, new Color(r,g,b), false, false);
        }

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
