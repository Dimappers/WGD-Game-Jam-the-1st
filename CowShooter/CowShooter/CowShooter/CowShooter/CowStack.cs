using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace CowShooter
{
    public class CowStack
    {
        Cow[,] Stack;

        WallManager wallManager;
        readonly Point startPoint = new Point(0, 10);

        public CowStack(WallManager wallManager)
        {
            Stack = new Cow[12, 11];
            this.wallManager = wallManager;
        }

        public Point AddCowToCowStack(Cow newCow)
        {
            Stack[startPoint.X, startPoint.Y] = newCow;
            return GetMove(startPoint, newCow);
        }

        public Point FinishedMove(Cow cow, Point reached, Point last)
        {
            //Stack[last.X, last.Y] = null;
            //Stack[reached.X, reached.Y] = cow;
            return GetMove(reached, cow);
        }

        public Point GetMove(Point position, Cow cow)
        {
            Point returnPoint;
            if (position.Y + 1 < 11 && Stack[position.X, position.Y + 1] == null)
            {
                returnPoint = new Point(position.X, position.Y + 1); //move down move
            }
                //Precondition: it is either on the ground or on a cow
            else if (position.X + 1 < 12 && Stack[position.X + 1, position.Y] == null)
            {
                returnPoint = new Point(position.X + 1, position.Y); //move foward move
            }
                //Precondition: (either on the ground or on a cow) & space to the right is full or the space to the right is the wall
            else if (position.X + 1 < 12)
            {
                //Precondition: The space to the right is full of cow (crucially not wall)//Shouldn't ever go to high
                if (Stack[position.X + 1, position.Y - 1] == null)
                {
                    returnPoint = new Point(position.X + 1, position.Y - 1); //move up and across move
                }
                else //two cows on top 
                {
                    returnPoint = position; //stop move
                }
            }
            else //at the wall & (either on the ground or on a cow) 
            {
                if (position.Y <= 10 - wallManager.wallHeight) //then we are above the wall
                {
                    returnPoint = new Point(-1, -1); //climb over wall move TODO
                }
                else //we are stuck behind the wall
                {
                    returnPoint = position; //At wall move
                }
            }

            Stack[position.X, position.Y] = null;
            if(returnPoint.X!=-1||returnPoint.Y!=-1) {Stack[returnPoint.X, returnPoint.Y] = cow;}

            return returnPoint;
        }

        public void removeDeadCow(Point p)
        {
            if (p.X != -1 || p.Y != -1) { Stack[p.X, p.Y] = null; }
        }

        public void Reset()
        {
            Stack = new Cow[12, 11];
        }
    }
}
