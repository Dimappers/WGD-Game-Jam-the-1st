using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    public class Meat : ICollisionObject
    {
        double age;
        const float groundLevel = 400;
        const float gravityScaleFactor = 0.05f;

        CowManager manager;
        Texture2D texture;
        Vector2 position;
        Color colour = Color.White;
        public int value;

        bool meatPickedUp;
        bool onGround;
        float yVelocity;

        public Meat(CowManager manager, Texture2D texture, Vector2 position)
        {
            value = 3;
            this.manager = manager;
            this.texture = texture;
            this.position = position;
            age = 0;
            meatPickedUp = false;
            onGround = false;
        }

        public bool getIsOff()
        {
            return age >= 15000;
        }

        public void Update(GameTime gameTime)
        {
            if (!onGround)
            {
                yVelocity += 9.8f * gravityScaleFactor;
                position.Y += yVelocity;

                if (position.Y + texture.Height > groundLevel)
                {
                    NotifyGroundCollision();
                }
            }

                
            age+=gameTime.ElapsedGameTime.TotalMilliseconds;
            if (age >= 15000) { manager.RemoveMeat(this); }
            else if (age >= 10000) { 
                colour = Color.Green;
                value = 1;
            }
            else if (age >= 5000) {
                colour = Color.LightGreen;
                value = 2;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, colour);
        }

        public Vector2 getLocation()
        {
            return position;
        }

        public void pickupMeat()
        {
            manager.RemoveMeat(this);
            meatPickedUp = true;
        }

        public bool isGone()
        {
            return meatPickedUp;
        }

        public Rectangle getCollisionRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public bool listenForGround()
        {
            return false;
        }

        public void NotifyOfCollision(ICollisionObject otherObject)
        {
            //Don't really care - the cow deletes it if necessary
        }

        public void NotifyGroundCollision()
        {
            yVelocity = 0;
            onGround = true;
            //Not checking for ground collisions
        }
    }
}
