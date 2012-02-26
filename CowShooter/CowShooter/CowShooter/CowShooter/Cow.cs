using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    public class Cow : ICollisionObject
    {
        const float velocity_h = 10.0f;
        public Vector2 cowPosition;
        CowManager manager;
        GameTime gameTime;
        Rectangle frameSize = new Rectangle(0, 0, 48, 36); //TODO: Make this the actual size of a cow
        public CollisionManager.OtherCowLocations otherCows;

        const int sizeOfJump = 50;
        float velocity_v = 0.0f;

        public bool partOfPyramid;
        public bool cowHasStopped;
        public Cow jumpTo = null;
        public Cow nextTo = null;

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
            partOfPyramid = false;
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
            if (!partOfPyramid||isJumping) { Move(velocity_h, velocity_v); }
            else { PyramidMove(velocity_h, velocity_v); }
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
        }
        private void gravity()
        {
            velocity_v -= 0.1f;
        }
        private void Move(float xd, float yd)
        {
            if (partOfPyramid && !cowHasStopped)
            {
                if (!isJumping)
                {
                    startPoint = cowPosition;
                    isJumping = true;
                }
                JumpUp();
            }
            if (cowPosition.X + xd + manager.GetTexture(GetType()).Width / 2 >= 600) 
            {
                cowHasStopped = true;
            }
            if (!cowHasStopped)
            {
                cowPosition += new Vector2(xd, yd) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        private void Fall() { /*TODO: write this*/}
        private void PyramidMove(float xd, float yd)
        {
            switch (otherCows)
            {
                case CollisionManager.OtherCowLocations.notBelow: { Fall(); break; }
                case CollisionManager.OtherCowLocations.alsoJumpTo: { cowHasStopped = true; break; }
                case CollisionManager.OtherCowLocations.onlyNextTo: { isJumping = true; JumpUp(); break; }
                case CollisionManager.OtherCowLocations.noCows: { Move(1.0f, 0); break; }
            }
        }
    }
}
