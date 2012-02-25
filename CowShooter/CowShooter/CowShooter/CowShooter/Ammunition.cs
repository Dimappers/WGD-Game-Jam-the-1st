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

        const float dampening = 0.55f;
        const float gravity = 5.0f;
        const float friction = 1.0f;

        Catapult catapult;
        public Ammunition(Catapult catapult, Vector2 startVelocity, Vector2 startPosition, Texture2D texture)
        {
            this.catapult = catapult;
            this.texture = texture;
            velocity = startVelocity;
            velocity.X = MathHelper.Clamp(velocity.X, 1.5f, 100f);
            position = startPosition;
            flying = true;
        }

        public void Update(GameTime gameTime)
        {
            if (flying)
            {
                velocity.Y -= gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (velocity.X > 0)
                {
                    velocity.X -= 0.5f * friction * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    velocity.X = 0;
                }
            }
            else
            {
                if (velocity.X > 0)
                {
                    velocity.X -= friction * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    velocity = Vector2.Zero;
                }
            }
            position -= velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        public Rectangle getCollisionRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
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
            if (velocity.Y < 0)
                velocity.Y *= -1;

            velocity.Y = velocity.Y * dampening;
            if (Math.Abs(velocity.Y) < 0.1f)
            {
                velocity.Y = 0;
                flying = false;
            }
        }
    }
}
