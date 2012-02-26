using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    class Villager : ICollisionObject
    {
        Meat targetMeat;
        Vector2 position;
        Vector2 wheredYouComeFrom;
        Texture2D texture;
        Meat carryMeat;

        private bool hasMeat;
        private bool seekingFood;
        private Rectangle frame = new Rectangle(0, 0, 32, 32);

        bool isDead;
        bool isHome;

        bool movingLeft; //For patrolling

        CowManager cowManager;
        VillagerManager villagerManager;

        const float speed = 2.5f;

        public Villager(Texture2D villagerTexture, CowManager cowManager, VillagerManager villagerManager, Vector2 startingPosition)
        {
            wheredYouComeFrom = startingPosition;
            texture = villagerTexture;
            this.villagerManager = villagerManager;
            carryMeat = null;

            this.cowManager = cowManager;
            isDead = false;
            isHome = true;
            movingLeft = true;

            position = startingPosition;
        }

        public bool getIsDead()
        {
            return isDead;
        }

        public void setSeekingFood(bool seekFood) //if true villager will head to nearest food, pick it up and find another, if false will head home
        {
            seekingFood = seekFood;
            if (seekFood)
            {
                targetMeat = cowManager.NearestMeat(position.X);
                isHome = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            Vector2 target = AquireTarget();
            Vector2 directionToMove = target - position;
            if (directionToMove != Vector2.Zero)
            {
                directionToMove.Normalize();
                directionToMove *= speed;
            }
            
            if (position.X >= wheredYouComeFrom.X)
            {
                if (hasMeat)
                {
                    hasMeat = false;
                    //TODO: Tell manager have brought meat
                    villagerManager.DropOffMeat(carryMeat);
                    carryMeat = null;
                }

                if (!seekingFood)
                {
                    //We are home and we should stop moving
                    if (!isHome)
                    {
                        isHome = true;
                        villagerManager.notifyReturn(this);
                    }
                    directionToMove = Vector2.Zero;   
                }
            }
            if (directionToMove != Vector2.Zero)
            {
                if (isHome)
                {
                    isHome = false;
                }
            }
            position += directionToMove;
        }

        private Vector2 AquireTarget()
        {
            if (seekingFood)
            {
                if (hasMeat)
                {
                    //Return to home
                    return wheredYouComeFrom;
                }
                else
                {
                    if (targetMeat == null || targetMeat.getIsOff() || targetMeat.isGone())
                    {
                        //aquire new target
                        targetMeat = cowManager.NearestMeat(position.X);
                        if (targetMeat == null)
                        {
                            //wander round aimlessly
                            if (movingLeft)
                            {
                                if (position.X <= 100)
                                {
                                    movingLeft = false;
                                    return new Vector2(500, position.Y);
                                }
                                else
                                {
                                    return new Vector2(100, position.Y);
                                }
                            }
                            else
                            {
                                if (position.X >= 500)
                                {
                                    movingLeft = true;
                                    return new Vector2(100, position.Y);
                                }
                                else
                                {
                                    return new Vector2(500, position.Y);
                                }
                            }
                        }
                    }

                    return targetMeat.getLocation();
                }
            }
            else
            {
                return wheredYouComeFrom;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!hasMeat)
            {
                spriteBatch.Draw(texture, position, frame, Color.White);
            }
            else
            {
                Rectangle r = frame;
                r.Offset(32, 0);
                spriteBatch.Draw(texture, position, r, Color.White);
            }
        }

        public Rectangle getCollisionRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y - 100, frame.Width, frame.Height + 100);//add 100 so even villagers on bottom row get killed by wild bull
        }

        public bool listenForGround()
        {
            return false;
        }

        public void NotifyOfCollision(ICollisionObject otherObject)
        {
            if (otherObject is Meat)
            {
                if (!hasMeat)
                {
                    hasMeat = true;
                    carryMeat = (Meat)otherObject;
                    ((Meat)otherObject).pickupMeat();
                    targetMeat = null;
                }
            }
            else if (otherObject is Bull)
            {
                //kill the villager
                isDead = true;
            }
        }

        public void NotifyGroundCollision() { ; }
    }
}
