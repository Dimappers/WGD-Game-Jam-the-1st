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
        Texture2D hedgeCrossTexture;
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


            for(int i = 0; i <5; ++i)
                GenerateWallShape();   
        }

        private void GenerateFood()
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

        private void GenerateWallShape()
        {
            int numberOfVertices = random.Next(1,4); //number of times wall changes direction - may want to tie this in to psychedelic factor
            Nullable<DirectionToMove> lastDirectionMoved = null;
            Point lastPoint = new Point(random.Next(size), random.Next(size));
            for(int verticesCreated = 0; verticesCreated <= numberOfVertices; ++verticesCreated)
            {
                DirectionToMove newDirectionToMove;
                do {
                    newDirectionToMove = (DirectionToMove)random.Next(4);
                } while(newDirectionToMove == lastDirectionMoved);

                HedgeType newWallHedgeType;
                if(newDirectionToMove == DirectionToMove.left || newDirectionToMove == DirectionToMove.right)
                {
                    newWallHedgeType = HedgeType.horizontal;
                }
                else
                {
                    newWallHedgeType = HedgeType.vertical;
                }

                if (lastDirectionMoved != null && lastPoint.X < 20 && lastPoint.X >= 0 && lastPoint.Y < 20 && lastPoint.Y >= 0)
                {
                    //Must create a corner piece
                    HedgeType cornerType = newWallHedgeType;
                    switch(lastDirectionMoved)
                    {
                        case DirectionToMove.down:
                            switch(newDirectionToMove)
                            {
                                case DirectionToMove.left:
                                    cornerType = HedgeType.corner_topleft;
                                    break;

                                case DirectionToMove.right:
                                    cornerType = HedgeType.corner_topright;
                                    break;
                            }
                            break;
                        case DirectionToMove.up:
                            switch(newDirectionToMove)
                            {
                                case DirectionToMove.left:
                                    cornerType = HedgeType.corner_bottomleft; 
                                    break;

                                case DirectionToMove.right:
                                    cornerType = HedgeType.corner_bottomright; //bottomright
                                    break;
                            }
                            break;

                        case DirectionToMove.left:
                            switch(newDirectionToMove)
                            {
                                case DirectionToMove.up:
                                    cornerType = HedgeType.corner_topright;
                                    break;

                                case DirectionToMove.down:
                                    cornerType = HedgeType.corner_bottomright;
                                    break;
                            }
                            break;
                            
                        case DirectionToMove.right:
                            switch(newDirectionToMove)
                            {
                                case DirectionToMove.up:
                                    cornerType = HedgeType.corner_topleft;
                                    break;

                                case DirectionToMove.down:
                                    cornerType = HedgeType.corner_bottomleft;
                                    break;
                            }
                            break;
                    }

                    //Now make the relevant corner
                    map[lastPoint.X, lastPoint.Y] = createHedge(lastPoint.X, lastPoint.Y, cornerType);
                }

                Point direction = Point.Zero;
                switch (newDirectionToMove)
                {
                    case DirectionToMove.left:
                        direction = new Point(-1, 0);
                        break;

                    case DirectionToMove.right:
                        direction = new Point(1, 0);
                        break;

                    case DirectionToMove.up:
                        direction = new Point(0, -1);
                        break;

                    case DirectionToMove.down:
                        direction = new Point(0, 1);
                        break;
                }
                int length = random.Next(2, 6);
                for (int i = 1; i < length; ++i)
                {
                    Point pointToMakeWall = new Point(lastPoint.X + direction.X * i, lastPoint.Y + direction.Y * i);
                    if (pointToMakeWall.X < 19 && pointToMakeWall.X > 0 && pointToMakeWall.Y < 19 && pointToMakeWall.Y > 0 && pointToMakeWall != CowPiece.startPoint)
                    {
                        if (map[pointToMakeWall.X, pointToMakeWall.Y].isBlocking())
                        {
                            map[pointToMakeWall.X, pointToMakeWall.Y] = createHedge(pointToMakeWall.X, pointToMakeWall.Y, HedgeType.crisscross);
                        }
                        else
                        {
                            map[pointToMakeWall.X, pointToMakeWall.Y] = createHedge(pointToMakeWall.X, pointToMakeWall.Y, newWallHedgeType);
                        }
                    }
                }

                lastPoint = new Point(lastPoint.X + direction.X * length, lastPoint.Y + direction.Y * length);
                lastDirectionMoved = newDirectionToMove;
            }
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
                case HedgeType.crisscross: { return new Square(i, j, hedgeCrossTexture, Color.White, true); }
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
            corner_bottomright,
            crisscross
        }
    }
}
