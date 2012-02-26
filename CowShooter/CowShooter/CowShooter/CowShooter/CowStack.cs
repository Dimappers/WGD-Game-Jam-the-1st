using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace CowShooter
{
    class CowStack
    {

        Cow[,] cowstack;

        CowStack()
        {
            cowstack = new Cow[10,10];
            //TODO THE COW STACK
        }

        public Point addCow(Cow cow)
        {
            int cowHeight = 0;
            Point position = new Point();
            for (int i = 0; i < 10; i++)
            {
                if (cowstack[i, cowHeight] == null)
                {
                    continue;
                }
                else if (cowstack[i, cowHeight + 1] == null)
                {
                    cowHeight++;
                    continue;
                }
                else
                {
                    cowstack[i - 1, cowHeight] = cow;
                    position = new Point(i - 1, cowHeight);
                }
            }
            return position;
        }
    }
}
