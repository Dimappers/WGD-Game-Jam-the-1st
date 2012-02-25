using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WGDGameJam
{
    class WallHead : WallPiece
    {
        Point headPoint;
        Random random = new Random();
        DirectionToMove lastMovedDirection;

        public WallHead(int x, int y, Texture2D texture, Color colour, MapManager manager, int numberOfVertices) 
            : base(x,y,texture,colour,manager)
        {
            headPoint = new Point(x, y);
            Point lastPoint = new Point(x, y);
            WallPiece previous = this;
            Nullable<DirectionToMove> lastDirectionMoved = null;
            for (int verticesCreated = 0; verticesCreated <= numberOfVertices; ++verticesCreated)
            {
                //lastMovedDirection - this is the variable that tells the next piece what direction to go when moving
                //lastDirectionMoved - this is used in creation of the original wall only DON'T GET CONFUSED!!!
                DirectionToMove newDirectionToMove;
                do
                {
                    newDirectionToMove = (DirectionToMove)random.Next(4);
                } while (newDirectionToMove == lastDirectionMoved);
                
                
                //Reverse direction since if we create the wall upwards then the snake is facing downwards but only for first point
                if (lastDirectionMoved == null)
                {
                    switch (newDirectionToMove)
                    {
                        case DirectionToMove.down:
                            lastMovedDirection = DirectionToMove.up;
                            break;
                        case DirectionToMove.up:
                            lastMovedDirection = DirectionToMove.down;
                            break;
                        case DirectionToMove.left:
                            lastMovedDirection = DirectionToMove.right;
                            break;
                        case DirectionToMove.right:
                            lastMovedDirection = DirectionToMove.left;
                            break;
                    }
                }


                Console.WriteLine("Starting direction: " + lastMovedDirection.ToString());

                MapManager.HedgeType newWallHedgeType;
                if (newDirectionToMove == DirectionToMove.left || newDirectionToMove == DirectionToMove.right)
                {
                    newWallHedgeType = MapManager.HedgeType.horizontal;
                }
                else
                {
                    newWallHedgeType = MapManager.HedgeType.vertical;
                }

                if (lastDirectionMoved != null && lastPoint.X < manager.size && lastPoint.X > 0 && lastPoint.Y < manager.size && lastPoint.Y > 0)
                {
                    //Must create a corner piece
                    MapManager.HedgeType cornerType = newWallHedgeType;
                    switch (lastDirectionMoved)
                    {
                        case DirectionToMove.down:
                            switch (newDirectionToMove)
                            {
                                case DirectionToMove.left:
                                    cornerType = MapManager.HedgeType.corner_topleft;
                                    break;

                                case DirectionToMove.right:
                                    cornerType = MapManager.HedgeType.corner_topright;
                                    break;
                            }
                            break;
                        case DirectionToMove.up:
                            switch (newDirectionToMove)
                            {
                                case DirectionToMove.left:
                                    cornerType = MapManager.HedgeType.corner_bottomleft;
                                    break;

                                case DirectionToMove.right:
                                    cornerType = MapManager.HedgeType.corner_bottomright; //bottomright
                                    break;
                            }
                            break;

                        case DirectionToMove.left:
                            switch (newDirectionToMove)
                            {
                                case DirectionToMove.up:
                                    cornerType = MapManager.HedgeType.corner_topright;
                                    break;

                                case DirectionToMove.down:
                                    cornerType = MapManager.HedgeType.corner_bottomright;
                                    break;
                            }
                            break;

                        case DirectionToMove.right:
                            switch (newDirectionToMove)
                            {
                                case DirectionToMove.up:
                                    cornerType = MapManager.HedgeType.corner_topleft;
                                    break;

                                case DirectionToMove.down:
                                    cornerType = MapManager.HedgeType.corner_bottomleft;
                                    break;
                            }
                            break;
                    }

                    //Now make the relevant corner
                    manager.map[lastPoint.X, lastPoint.Y] = manager.createHedge(lastPoint.X, lastPoint.Y, cornerType);
                    previous.Attach((WallPiece)manager.map[lastPoint.X, lastPoint.Y]);
                    previous = (WallPiece)manager.map[lastPoint.X, lastPoint.Y];
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
                int length = random.Next(4, 6);
                for (int i = 1; i < length; ++i)
                {
                    Point pointToMakeWall = new Point(lastPoint.X + direction.X * i, lastPoint.Y + direction.Y * i);
                    if (isValid(pointToMakeWall.X,pointToMakeWall.Y) && pointToMakeWall != CowPiece.startPoint)
                    {
                        manager.map[pointToMakeWall.X, pointToMakeWall.Y] = manager.createHedge(pointToMakeWall.X, pointToMakeWall.Y, newWallHedgeType);
                        previous.Attach((WallPiece)manager.map[pointToMakeWall.X, pointToMakeWall.Y]);
                        previous = (WallPiece)manager.map[pointToMakeWall.X, pointToMakeWall.Y];
                    }
                }

                lastPoint = new Point(lastPoint.X + direction.X * length, lastPoint.Y + direction.Y * length);
                lastDirectionMoved = newDirectionToMove;
            }
        }
        public void MakeMove(DirectionToMove direction)
        {
            Console.WriteLine("Trying to move: " + direction.ToString());
            int newx = xlocation;
            int newy = ylocation;
            Texture2D textureToPassOn = null;
            switch (direction)
            {
                case DirectionToMove.down:
                {
                    if (isValid(xlocation, ylocation + 1))
                    { 
                        if (!mapManager.map[xlocation, ylocation + 1].isBlocking()) 
                        {
                            newx = xlocation; newy = ylocation + 1;
                            switch (lastMovedDirection)
                            {
                                case DirectionToMove.left:
                                    {
                                        //Top left
                                        textureToPassOn = mapManager.hedgeCornerTexturebr;
                                        break;
                                    }

                                case DirectionToMove.right:
                                    {
                                        Console.WriteLine("Using tr texture");
                                        textureToPassOn = mapManager.hedgeCornerTexturebl;
                                        break;
                                    }
                            }
                        }
                    }
                    break;
                }
                case DirectionToMove.up:
                {
                    if (isValid(xlocation, ylocation - 1))
                    { 
                        if (!mapManager.map[xlocation, ylocation - 1].isBlocking()) 
                        { 
                            newx = xlocation; newy = ylocation - 1;
                            switch (lastMovedDirection)
                            {
                                case DirectionToMove.left:
                                    {
                                        //Top left
                                        Console.WriteLine("Using bl texture");
                                        textureToPassOn = mapManager.hedgeCornerTexturetr;
                                        break;
                                    }

                                case DirectionToMove.right:
                                    {
                                        Console.WriteLine("Using br texture");
                                        textureToPassOn = mapManager.hedgeCornerTexturetl;
                                        break;
                                    }
                            }
                        } 
                    }
                    break;
                }
                case DirectionToMove.left :
                {
                    if (isValid(xlocation - 1, ylocation))
                    { 
                        if (!mapManager.map[xlocation -1 , ylocation].isBlocking()) 
                        { 
                            newx = xlocation - 1; newy = ylocation;
                            switch (lastMovedDirection)
                            {
                                case DirectionToMove.up:
                                    {
                                        Console.WriteLine("Using tr texture");
                                        textureToPassOn = mapManager.hedgeCornerTexturebl;
                                        break;
                                    }

                                case DirectionToMove.down:
                                    {
                                        Console.WriteLine("Using br texture");
                                        textureToPassOn = mapManager.hedgeCornerTexturetl; //should be br?
                                        break;
                                    }
                            }
                        } 
                    }
                    break;
                }
                case DirectionToMove.right :
                {
                    if (isValid(xlocation + 1, ylocation))
                    { 
                        if (!mapManager.map[xlocation + 1, ylocation].isBlocking()) 
                        { 
                            newx = xlocation + 1; newy = ylocation;
                            switch (lastMovedDirection)
                            {
                                case DirectionToMove.up:
                                    {
                                        textureToPassOn = mapManager.hedgeCornerTexturebr;
                                        Console.WriteLine("Using tl texture");
                                        break;
                                    }

                                case DirectionToMove.down:
                                    {
                                        textureToPassOn = mapManager.hedgeCornerTexturetr;
                                        Console.WriteLine("Using bl texture");
                                        break;
                                    }
                            }
                        }
                    }
                    break;
                }
            }

            if (newx != xlocation || newy != ylocation)
            {
                if (textureToPassOn == null)
                {
                    Console.WriteLine("Going straight");
                    switch (direction)
                    {
                        case DirectionToMove.left:
                        case DirectionToMove.right:
                            textureToPassOn = mapManager.hedgeTexture;
                            Console.WriteLine("Using horizontal");
                            break;

                        case DirectionToMove.up:
                        case DirectionToMove.down:
                            textureToPassOn = mapManager.hedgeVertTexture;
                            Console.WriteLine("Using vertical");
                            break;
                    }
                }

                switch (direction)
                {
                    case DirectionToMove.left:
                    case DirectionToMove.right:
                        texture = mapManager.hedgeTexture;
                        Console.WriteLine("Using horizontal");
                        break;

                    case DirectionToMove.up:
                    case DirectionToMove.down:
                        texture = mapManager.hedgeVertTexture;
                        Console.WriteLine("Using vertical");
                        break;
                }

                if (next != null) { next.Move(xlocation, ylocation, textureToPassOn); }
                else { mapManager.map[xlocation, ylocation] = mapManager.createGrass(xlocation, ylocation, false); }
                xlocation = newx;
                ylocation = newy;
                if (mapManager.map[newx, newy].containsFood()) { mapManager.GenerateFood(); }
                mapManager.map[newx, newy] = this;
                lastMovedDirection = direction;
            }
        }
        private bool isValid(int x, int y)
        {
            return (x > 0 && x < mapManager.size - 1 && y > 0 && y < mapManager.size - 1);
        }
    }
}
