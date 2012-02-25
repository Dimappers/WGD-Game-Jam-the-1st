using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    class Ammunition : ICollisionObject
    {
        Texture2D texture;
        bool flying;
        Vector2 velocity;
        Vector2 position;

        const float dampening = 0.1f;
        const float gravity = 1.0f;

        public Rectangle collisionRectangle;
        Catapult catapult;
        public Ammunition(Catapult catapult, Vector2 startVelocity, Vector2 startPosition)
        {
            this.catapult = catapult;
            velocity = startVelocity;
            position = startPosition;
        }

        public void Update(GameTime gameTime)
        {
            velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
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
