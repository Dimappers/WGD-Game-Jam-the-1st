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

        CowManager manager;
        Texture2D texture;
        Vector2 position;
        Color colour = Color.White;

        public Meat(CowManager manager, Texture2D texture, Vector2 position)
        {
            this.manager = manager;
            this.texture = texture;
            this.position = position;
            age = 0; 
        }

        public bool getIsOff()
        {
            return age >= 15000;
        }

        public void Update(GameTime gameTime)
        {
            age+=gameTime.ElapsedGameTime.TotalMilliseconds;
            if (age >= 15000) { manager.RemoveMeat(this); }
            else if (age >= 10000) { colour = Color.Green; }
            else if (age >= 5000) {colour = Color.LightGreen;}
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
            //Not checking for ground collisions
        }
    }
}
