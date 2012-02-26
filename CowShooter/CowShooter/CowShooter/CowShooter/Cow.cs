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

        int health;
        HealthBar healthBar;


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
            this.cowStack = cowStack;
            this.health = health;
            healthBar = new HealthBar(h1Texture, h2Texture, cowPosition, health);
        }

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
        }

        private void Move(float xd, float yd)
        {
            cowPosition += new Vector2(xd, yd) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

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
