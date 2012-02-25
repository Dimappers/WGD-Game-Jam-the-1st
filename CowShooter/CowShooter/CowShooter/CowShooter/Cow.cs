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
        const float velocity_h = 10.0f;
        Vector2 cowPosition;
        CowManager manager;
        GameTime gameTime;

        const int sizeOfJump = 50;
        float velocity_v = 0.0f;

        int floorLevel = 400 - 32; //32 is height of cow_piece

        Texture2D texture;

        public Cow(CowManager manager)
        {
            cowPosition = new Vector2(0, floorLevel); //32 is height of cow_piece
            this.manager = manager;
            
        }
        public virtual void Update(GameTime gameTime)
        {
            if (texture == null)
            {
                texture = manager.GetTexture(GetType());
            }
            this.gameTime = gameTime;
            Move(velocity_h, velocity_v);
            if(velocity_v!=0)Console.WriteLine("h: " + velocity_h + " v: " + velocity_v);
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
        public void JumpUp()
        {
            velocity_v = (float)(Math.Sqrt((double)(2.0f * 9.8f * sizeOfJump)));
        }
        private void gravity()
        {
            velocity_v -= 0.1f*9.8f;
        }
        private void Move(float xd, float yd)
        {
            if (cowPosition.Y - yd * (float)gameTime.ElapsedGameTime.TotalSeconds >= floorLevel) { gravity(); }
            else { velocity_v = 0; }
            if (cowPosition.X + xd + manager.GetTexture(GetType()).Width / 2 < 600) 
            { 
                cowPosition += new Vector2(xd, yd) * (float)gameTime.ElapsedGameTime.TotalSeconds; 
            }
        }
    }
}
