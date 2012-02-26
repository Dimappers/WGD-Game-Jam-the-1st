using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    public class Ammunition : ICollisionObject
    {
        Texture2D texture;
        bool flying;
        Vector2 velocity;
        Vector2 position;
        
        public int damage;

        const float dampening = 0.55f;
        const float gravity = 5.0f;
        const float friction = 1.0f;

        public Boolean isDead;
        float timeTillDeath;

        Catapult catapult;
        float angle;

        public Ammunition(Catapult catapult, Vector2 startVelocity, Vector2 startPosition, Texture2D texture, int damage)
        {
            this.catapult = catapult;
            this.texture = texture;
            velocity = startVelocity;
            velocity.X = MathHelper.Clamp(velocity.X, 1.5f, 100f);
            position = startPosition;
            this.damage = damage;
            timeTillDeath = -10f;
            isDead = false;
            flying = true;

            angle = (float)Math.Atan2(startVelocity.Y, startVelocity.X);
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
                angle = (float)Math.Atan2(velocity.Y, velocity.X);
            }
            else
            {
                // Handles killing off the ammo 
                if (timeTillDeath < -5f)
                {
                    timeTillDeath = (float)gameTime.TotalGameTime.TotalSeconds;
                }
                else if ((float)gameTime.TotalGameTime.TotalSeconds > timeTillDeath+4f)
                {
                    remove();
                }

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
            checkOutOfBounds();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, angle,Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
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
            isDead = true;
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

        private void checkOutOfBounds()
        {
            if ((int)position.X + texture.Width < 0)
            {
                remove();
            }
        }
    }
}
