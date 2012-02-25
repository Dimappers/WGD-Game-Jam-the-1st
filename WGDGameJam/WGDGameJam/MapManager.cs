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
        public Square[,] map;
        public Texture2D grassTexture;
        public Texture2D hedgeTexture;
        public Texture2D hedgeVertTexture;
        public Texture2D hedgeCornerTexturetl;
        public Texture2D hedgeCornerTexturetr;
        public Texture2D hedgeCornerTexturebl;
        public Texture2D hedgeCornerTexturebr;
        public Texture2D hedgeCrossTexture;
        public Texture2D foodTexture;
        CowPiece headPiece;
        Random random = new Random();
        public int size;
        Game1 game;
        WallPiece[] wallpieces;

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
            this.hedgeCrossTexture = mapTex.hedgeCrossTexture;
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
                        Color newColour = crazyFloor();
                        if (newColour != Color.Black) { map[i, j].changeColour(newColour); }
                    }
                }
            }
        }
        private Color crazyFloor()
        {
            double randomnum = random.Next(200);
            double factor = 1.0f / (3.0f * game.score + 1.0f);
            int rounded = (int)Math.Round((double)(randomnum * factor));
            if (rounded != 0) {return Color.Black;}
            int r = random.Next(255);
            int g = random.Next(255);
            int b = random.Next(255);
            return new Color(r, g, b);
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
                    else { map[i, j] = createGrass(i, j, true); }
                }
            }

            for (int i = 0; i < 10; ++i)
            {
                GenerateFood();
            }

            wallpieces = new WallPiece[15];
            for(int i = 0; i <wallpieces.Length; ++i)
                wallpieces[i] = GenerateWallShape();   
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

        private WallPiece GenerateWallShape()
        {
            int numberOfVertices = random.Next(1,4); //number of times wall changes direction - may want to tie this in to psychedelic factor
            Point lastPoint;
            do { lastPoint = new Point(1 + random.Next(size - 1), 1 + random.Next(size - 1)); }
            while (map[lastPoint.X, lastPoint.Y].isBlocking());
            return new WallHead(lastPoint.X, lastPoint.Y, hedgeTexture, Color.Brown, this, numberOfVertices);
        }
        public void MoveWalls()
        {
            int boolean;
            for (int i = 0; i < wallpieces.Length; i++)
            {
                boolean = random.Next((int)(0/*8*/ / (1 + game.score)));
                if (boolean == 0)
                {
                    int directionnumber = random.Next(4);
                    DirectionToMove direction = DirectionToMove.up;
                    switch (directionnumber)
                    {
                        case 0: direction = DirectionToMove.down; break;
                        case 1: direction = DirectionToMove.right; break;
                        case 2: direction = DirectionToMove.left; break;
                    }
                    ((WallHead)wallpieces[i]).MakeMove(direction);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for(int i=0; i<size; i++)
            {
                for(int j=0; j<size; j++)
                {
                    map[i, j].Draw(spriteBatch, headPiece, gameTime);
                }
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    map[i, j].drawFood(spriteBatch, headPiece, gameTime);
                }
            }
        }

        public WallPiece createHedge(int i, int j, HedgeType hedge)
        {
            switch(hedge)
            {
                case HedgeType.horizontal : {return new WallPiece(i, j, hedgeTexture, Color.Brown, this); }
                case HedgeType.vertical: { return new WallPiece(i, j, hedgeVertTexture, Color.Brown, this); }
                case HedgeType.corner_topleft: { return new WallPiece(i, j, hedgeCornerTexturetl, Color.Brown, this); }
                case HedgeType.corner_topright: { return new WallPiece(i, j, hedgeCornerTexturetr, Color.Brown, this); }
                case HedgeType.corner_bottomleft: { return new WallPiece(i, j, hedgeCornerTexturebl, Color.Brown, this); }
                case HedgeType.corner_bottomright: { return new WallPiece(i, j, hedgeCornerTexturebr, Color.Brown, this); }
                case HedgeType.crisscross: { return new WallPiece(i, j, hedgeCrossTexture, Color.Brown, this); }
                default: return null;
            }
        }
        public Square createGrass(int i, int j, bool reset)
        {
            Color newColour;
            //for when resetting map to make everything green
            if(reset) {return new Square(i, j, grassTexture, Color.Green, false, this);}
            //for when colouring a square that was a wall
            else if (map[i, j].colour == Color.Brown) 
            {
                newColour = crazyFloor();
                if (newColour == Color.Black) { newColour = Color.Green; }
            }
            //other times
            else
            {
                newColour = crazyFloor();
                if (newColour == Color.Black) { newColour = map[i, j].colour; }
            }
            return new Square(i, j, grassTexture, newColour, false, this);
        }

        public Game1 getGame()
        {
            return game;

        }

        public enum HedgeType{
            horizontal,
            vertical,
            corner_topleft,
            corner_bottomleft,
            corner_topright,
            corner_bottomright,
            crisscross
        }

        public void reset()
        {
            createMap();
        }
    }
}
