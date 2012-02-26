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
        public Vector2 cowPosition;
        CowManager manager;
        GameTime gameTime;
        Rectangle frameSize = new Rectangle(0, 0, 48, 36); //TODO: Make this the actual size of a cow

        const int sizeOfJump = 50;
        float velocity_v = 0.0f;

        public bool cowIsInFront;
        public bool cowHasStopped;

        bool isJumping;
        Vector2 startPoint;

        int floorLevel = 400 - 32; //32 is height of cow_piece

        Texture2D texture;

        public bool isDead;

        public Cow(CowManager manager)
        {
            cowPosition = new Vector2(0, floorLevel);
            this.manager = manager;
            isDead = false;
            cowIsInFront = false;
            cowHasStopped = false;
            isJumping = false;
            
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
            return new Rectangle((int)cowPosition.X, (int)cowPosition.Y, frameSize.Width, frameSize.Height);
        }

        public bool listenForGround()
        {
            return false;
        }

        public void NotifyOfCollision(ICollisionObject otherObject)
        {
            if (otherObject is Ammunition)
            {
                isDead = true;
            }
        }

        public void NotifyGroundCollision()
        {
            //Don't care
        }
        public void JumpUp()
        {
            if (cowPosition.Y > startPoint.Y - frameSize.Height)
            {
                cowPosition.Y -= 1;
                velocity_v = 1f;
            }
            else
            {
                if (cowPosition.Y + frameSize.Height >= startPoint.Y)
                {
                    isJumping = false;
                }
                else
                {
                    cowPosition.Y -= velocity_v;
                    gravity();

                    cowPosition.X += frameSize.Width/20;
                }
            }
            // velocity_v = (float)(Math.Sqrt((double)(2.0f * 9.8f * sizeOfJump)));
        }
        private void gravity()
        {
            velocity_v -= 0.1f;
        }
        private void Move(float xd, float yd)
        {
            if (cowIsInFront && !cowHasStopped)
            {
                if (!isJumping)
                {
                    startPoint = cowPosition;
                }
                JumpUp();
            }
            /*
             * Unnecessary Gravity?
            if (cowPosition.Y - yd * (float)gameTime.ElapsedGameTime.TotalSeconds >= floorLevel) { gravity(); }
            else { velocity_v = 0; } */
            if (cowPosition.X + xd + manager.GetTexture(GetType()).Width / 2 >= 600) 
            {
                cowHasStopped = true;
            }
            if (!cowHasStopped)
            {
                cowPosition += new Vector2(xd, yd) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
