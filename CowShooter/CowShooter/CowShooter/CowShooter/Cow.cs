using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    class Cow : ICollisionObject
    {
        const float velocity = 1.0f;
        Vector2 cowPosition;
        CowManager manager;

        Texture2D texture;

        public Cow(CowManager manager)
        {
            cowPosition = new Vector2(0, 400);
            this.manager = manager;
            texture = manager.GetTexture(GetType());
        }


        public virtual void Update(GameTime gameTime)
        {
            cowPosition.X += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, cowPosition, Color.White);
        }

        public Rectangle getCollisionRectangle()
        {
            return new Rectangle((int)cowPosition.X, (int)cowPosition.Y, texture.Width, texture.Height);
        }

        public bool listenForGround()
        {
            return false;
        }

        public void NotifyOfCollision(ICollisionObject otherObject)
        {
            if (otherObject is Ammunition)
            {
                //TODO: Kill me :(
            }
        }

        public void NotifyGroundCollision()
        {
            //Don't care
        }
    }
}
