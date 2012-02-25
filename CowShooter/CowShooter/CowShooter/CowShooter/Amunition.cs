using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    class Amunition : ICollisionObject
    {
        Texture2D texture;
        bool flying;
        Vector2 velocity;

        const float dampening = 0.1f;

        public Rectangle collisionRectangle;

        public Amunition(Catapult catapult, Vector2 startVelocity)
        {

        }

        public Rectangle getCollisionRectangle()
        {
            return collisionRectangle;
        }

        public void NotifyOfCollision(ICollisionObject otherObject)
        {
            if (otherObject is Cow)
            {
                remove();
            }
            else if (true)
            {

            }
        }

        private void remove()
        {

        }

        

        public bool listenForGround()
        {
            return true;
        }


        public void NotifyGroundCollision()
        {
            velocity.Y = velocity.Y * -1 * dampening;
            if (velocity.Y < 0.01f)
            {
                velocity.Y = 0;
                flying = false;
            }
        }
    }
}
