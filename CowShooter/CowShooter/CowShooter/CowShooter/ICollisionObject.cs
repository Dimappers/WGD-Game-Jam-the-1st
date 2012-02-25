using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace CowShooter
{
    interface ICollisionObject
    {
        public Rectangle getCollisionRectangle();

        public bool listenForGround();
        public void NotifyOfCollision(ICollisionObject otherObject);
        public void NotifyGroundCollision();
    }
}
