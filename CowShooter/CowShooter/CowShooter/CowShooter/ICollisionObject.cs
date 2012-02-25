using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace CowShooter
{
    interface ICollisionObject
    {
        Rectangle getCollisionRectangle();

        bool listenForGround();
        void NotifyOfCollision(ICollisionObject otherObject);
        void NotifyGroundCollision();
    }
}
