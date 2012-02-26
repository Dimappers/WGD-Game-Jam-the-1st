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
        const float velocity_h = 100.0f;
        public Vector2 cowPosition;
        CowManager manager;
        GameTime gameTime;
        Rectangle frameSize = new Rectangle(0, 0, 46, 36); //TODO: Make this the actual size of a cow
        public CollisionManager.OtherCowLocations otherCows;

        const int sizeOfJump = 50;
        const float velocity_v = -100.0f;

        public bool partOfPyramid;

        bool isJumping;
        bool isFalling;

        int health;
        HealthBar healthBar;

        bool justFinishedJumping;
        Vector2 startPoint;


        WallManager wallManager;

        int floorLevel = 400 - 32; //32 is height of cow_piece

        Texture2D texture;

        public bool isDead;
        Point nextStackPoint;
        Point lastStackPoint;
        CowStack cowStack;

        public Cow(CowManager manager, WallManager wallManager, CowStack cowStack, Texture2D h1Texture, Texture2D h2Texture, int health)
        {
            cowPosition = new Vector2(0, floorLevel);
            this.manager = manager;
            this.wallManager = wallManager;
            isDead = false;
            partOfPyramid = false;
            isJumping = false;
            isFalling = true;
            this.cowStack = cowStack;
            this.health = health;
            healthBar = new HealthBar(h1Texture, h2Texture, cowPosition, health);
        }
        /*public bool cowHasStopped()
        {
            return partOfPyramid && !(isFalling || isJumping);
        }*/
        public virtual void Update(GameTime gameTime)
        {
            if (texture == null)
            {
                texture = manager.GetTexture(GetType());
            }
            healthBar.Update(gameTime, cowPosition);

            this.gameTime = gameTime;

            if (!partOfPyramid)
            {
                if (cowPosition.X >= (600 - (12 * frameSize.Width)))
                {
                    //Part of pyramid
                    partOfPyramid = true;
                    nextStackPoint = cowStack.AddCowToCowStack(this);
                    lastStackPoint = new Point(0, 10);

                    takePyramidMove();
                }
                else
                {
                    Move(velocity_h, 0);
                }
            }
            else
            {
                takePyramidMove();
            }

            /*if (!partOfPyramid||isJumping||isFalling||justFinishedJumping) { Move(velocity_h, velocity_v); }
            else { PyramidMove(velocity_h, velocity_v); }*/
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, cowPosition, Color.White);
            healthBar.Draw(spriteBatch);
        }

        public Rectangle getCollisionRectangle()
        {
            return new Rectangle((int)cowPosition.X, (int)cowPosition.Y, frameSize.Width, frameSize.Height);
        }

        public Point getStackPosition()
        {
            return nextStackPoint;
        }

        public void takePyramidMove()
        {
            if (lastStackPoint != nextStackPoint)
            {
                Vector2 targetPosition = VectorFromStackPoint(nextStackPoint);
                if (nextStackPoint.Y == lastStackPoint.Y) //is horizontal move
                {
                    Move(velocity_h, 0);
                    if (cowPosition.X >= targetPosition.X)
                    {
                        //We have reached our destination
                        Point temp = lastStackPoint;
                        lastStackPoint = nextStackPoint;
                        nextStackPoint = cowStack.FinishedMove(this, nextStackPoint, temp);
                    } //else still going
                }
                //The move is an up and across
                else if (nextStackPoint.Y < lastStackPoint.Y)
                {
                    
                    if (targetPosition.Y < cowPosition.Y) //still going up
                    {
                        Move(0, velocity_v);

                    }
                    else
                    {
                        Move(velocity_h, 0);
                        if (cowPosition.X >= targetPosition.X)
                        {
                            //We have reached our destination
                            Point temp = lastStackPoint;
                            lastStackPoint = nextStackPoint;
                            nextStackPoint = cowStack.FinishedMove(this, nextStackPoint, temp);
                        } //else still going
                    }
                }
                else
                {
                    //This is a down move
                    Move(0, -velocity_v);
                    if (cowPosition.Y >= targetPosition.Y)
                    {
                        Point temp = lastStackPoint;
                        lastStackPoint = nextStackPoint;
                        nextStackPoint = cowStack.FinishedMove(this, nextStackPoint, temp);
                    } //else still moving downs
                }
            }
            else
            {
                nextStackPoint = cowStack.GetMove(nextStackPoint, this);
            }
            //else do no moving

        }

        public bool listenForGround()
        {
            return true;
        }

        public void NotifyOfCollision(ICollisionObject otherObject)
        {
            if (otherObject is Ammunition)
            {
                int d = ((Ammunition)otherObject).damage;
                health -= d;
                if (health <= 0)
                {
                    isDead = true;
                }
                else
                {
                    healthBar.takeDamage(d, gameTime);
                }
            }
        }

        public void NotifyGroundCollision()
        {
            isFalling = false;
        }
        /*public void JumpUp()
        {
            if (cowPosition.Y + frameSize.Height > startPoint.Y)
            {
                velocity_v = 10.0f;
                cowPosition.Y -= velocity_v * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //gravity();
            }
            else
            {
                isJumping = false;
                justFinishedJumping = true;
                velocity_v = 0.0f;
            }
        }*/
        /*private void gravity()
        {
            velocity_v -= 1.0f;
        }*/
        private void Move(float xd, float yd)
        {
            /*if (partOfPyramid)
            {
                if (isJumping)
                {
                    JumpUp();
                }
                else if (isFalling)
                {
                    Fall();
                }
                else
                {
                    cowPosition += new Vector2(xd, yd) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (otherCows != CollisionManager.OtherCowLocations.ThereIsNoCowToTheRight||wallInWay()) { justFinishedJumping = false; }
                }
            }
            else
            {
                if (cowPosition.X + (xd * gameTime.ElapsedGameTime.TotalSeconds) + getCollisionRectangle().Width >= WallManager.wallLocation
                    || otherCows == CollisionManager.OtherCowLocations.ThereIsACowNextToUsWithNoCowOnTop
                    || otherCows == CollisionManager.OtherCowLocations.ThereIsACowOnTopOfTheCowToTheRight)
                {
                    partOfPyramid = true;
                }
                else
                {
                    cowPosition += new Vector2(xd, yd) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }*/

            cowPosition += new Vector2(xd, yd) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        /*private void Fall() {
            isFalling = false;
            /*if (startPoint.Y < getCollisionRectangle().Top - getCollisionRectangle().Height)
            {
                velocity_v = -10.0f;
                cowPosition.Y += velocity_v * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //gravity();
            }
            else
            {
                isFalling = false;
                velocity_v = 0.0f;
            }*/
        //}*/
        /*private void PyramidMove(float xd, float yd)
        {
            switch (otherCows)
            {
                case CollisionManager.OtherCowLocations.ThereIsNoCowBelowCurrentCow: 
                    {
                        isFalling = true;
                        startPoint = cowPosition;
                        Fall(); 
                        break; 
                    }
                case CollisionManager.OtherCowLocations.ThereIsACowOnTopOfTheCowToTheRight: 
                    { 
                        break; 
                    }
                case CollisionManager.OtherCowLocations.ThereIsACowNextToUsWithNoCowOnTop:
                    { 
                        isJumping = true;
                        startPoint = cowPosition;
                        JumpUp(); 
                        break; 
                    }
                case CollisionManager.OtherCowLocations.ThereIsNoCowToTheRight:
                    {
                        if(!wallInWay())
                        {
                            Move(velocity_h,velocity_v);     
                        }
                        break; 
                    }
            }
        }
        private bool wallInWay()
        {
            return (belowWallHeight() && toRightOfWall());
        }
        private bool belowWallHeight() { return getCollisionRectangle().Bottom >= 400 - wallManager.wallHeight; } //TODO: Broken wall height - not using pixels
        private bool toRightOfWall() 
        {
            partOfPyramid = true;
            return getCollisionRectangle().Right >= WallManager.wallLocation; 
        }
        */
        public override string ToString()
        {
            return getCollisionRectangle().ToString();
        }

        public Vector2 VectorFromStackPoint(Point p)
        {
            float xPos = 600 - ((12 -p.X) * frameSize.Width);
            float yPos = 400 - ((11 - p.Y) * frameSize.Height);

            return new Vector2(xPos, yPos);
        }
    }
}
