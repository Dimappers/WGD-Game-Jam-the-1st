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
        Vector2 heading;
        Meat targetMeat;
        Vector2 position;
        Vector2 wheredYouComeFrom;

        Texture2D texture;

        private bool hasMeat;
        private bool seekingFood;
        private Rectangle frame = new Rectangle(0, 0, 32, 32);

        CowManager cowManager;

        const float speed = 2.5f;

        public Villager(Texture2D villagerTexture, CowManager cowManager, Vector2 startingPosition)
        {
            wheredYouComeFrom = startingPosition;
            texture = villagerTexture;

            this.cowManager = cowManager;
        }

        public void setSeekingFood(bool seekFood) //if true villager will head to nearest food, pick it up and find another, if false will head home
        {
            seekingFood = seekFood;
            if (seekFood)
            {
                targetMeat = cowManager.NearestMeat(position.X);
            }

        }

        public void Update(GameTime gameTime)
        {
            Vector2 target = AquireTarget();
            Vector2 directionToMove = target - position;
            directionToMove.Normalize();
            directionToMove *= speed;

            if (position.X > wheredYouComeFrom.X)
            {
                if (hasMeat)
                {
                    hasMeat = false;
                    //TODO: Tell manager have brought meat
                }

                if (!seekingFood)
                {
                    //We are home and we should stop moving
                    directionToMove = Vector2.Zero;
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
                    if (targetMeat == null || targetMeat.getIsOff())
                    {
                        //aquire new target
                        targetMeat = cowManager.NearestMeat(position.X);
                        if (targetMeat == null)
                        {
                            //wander round aimlessly
                            return new Vector2(new Random().Next(500), position.Y);
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
            return new Rectangle((int)position.X, (int)position.Y, frame.Width, frame.Height);
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
                    ((Meat)otherObject).pickupMeat();
                    targetMeat = null;
                }
            }
        }

        public void NotifyGroundCollision() { ; }
    }
}
