using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    public class CollisionManager
    {
        /*List<Ammunition> ammo;
        List<Cow> cows;*/
        List<ICollisionObject> otherObjects;
        float groundLevel;

        public CollisionManager(float groundLevel)
        {
            /*ammo = new List<Ammunition>();
            cows = new List<Cow>();*/
            otherObjects = new List<ICollisionObject>();

            this.groundLevel = groundLevel;
        }

        public void addCow(Cow cow)
        {
            otherObjects.Add(cow);
        }

        public void addAmmo(Ammunition ammo)
        {
            otherObjects.Add(ammo);
        }

        public void addOther(ICollisionObject other)
        {
            otherObjects.Add(other);
        }

        public void checkCollision()
        {
            foreach (ICollisionObject objectA in otherObjects)
            {
                foreach (ICollisionObject objectB in otherObjects)
                {
                    if (objectA != objectB)
                    {
                        if(objectA.getCollisionRectangle().Intersects(objectB.getCollisionRectangle()))
                        {
                            objectA.NotifyOfCollision(objectB);
                            objectB.NotifyOfCollision(objectA);
                        }
                    }
                }
                if (objectA.listenForGround())
                {
                    if (objectA.getCollisionRectangle().Bottom > groundLevel)
                    {
                        objectA.NotifyGroundCollision();
                    }
                }
            }
        }

        public OtherCowLocations findCowCollisions(Cow cow)
        {
            Rectangle cowRectangle = cow.getCollisionRectangle(); 
            bool hasCowBelow = false;
            bool hasCowInJumpToPosition = false;
            bool hasCowNextToWithNoCowOnTop = false;
            bool notStoodOnGround = cowRectangle.Bottom < 400 - cowRectangle.Height ;
            foreach (ICollisionObject otherCowObject in otherObjects)
            {             
                Cow otherCow = otherCowObject as Cow;
                if(otherCow != null)
                {
                    if (!otherCow.Equals(cow) && otherCow.partOfPyramid)
                    {
                        if (notStoodOnGround
                            && new Rectangle(
                                    cowRectangle.X, 
                                    cowRectangle.Y + (int)(1.5*(float)cowRectangle.Height), 
                                    cowRectangle.Width, 
                                    cowRectangle.Height/2)
                                .Intersects(otherCow.getCollisionRectangle()))
                        {
                            //hasCowBelow = true;
                        }
                        if (new Rectangle(
                                    cowRectangle.X + cowRectangle.Width/2, 
                                    cowRectangle.Y - (int)(1.5*(float)cowRectangle.Height), 
                                    cowRectangle.Width, 
                                    cowRectangle.Height)
                                .Intersects(otherCow.getCollisionRectangle()))
                        {
                            //looks to right & up to check for cow
                            hasCowInJumpToPosition = true;
                        }
                        else if (new Rectangle(
                                    cowRectangle.X + cowRectangle.Width/2, 
                                    cowRectangle.Y, 
                                    cowRectangle.Width, 
                                    cowRectangle.Height)
                                .Intersects(otherCow.getCollisionRectangle()))
                        {
                            //just looks to right
                            hasCowNextToWithNoCowOnTop = true;
                        }
                    }
                }
            }
            if(notStoodOnGround&&!hasCowBelow)
            {
                //return OtherCowLocations.ThereIsNoCowBelowCurrentCow;
            }
            if(hasCowInJumpToPosition)
            {
                return OtherCowLocations.ThereIsACowOnTopOfTheCowToTheRight; 
            }
            if(hasCowNextToWithNoCowOnTop) 
            {
                return OtherCowLocations.ThereIsACowNextToUsWithNoCowOnTop;
            }
            return OtherCowLocations.ThereIsNoCowToTheRight;
        }

        public void removeCow(Cow cow)
        {
            otherObjects.Remove(cow);
        }

        public void removeAmmo(Ammunition ammoToRemove)
        {
            otherObjects.Remove(ammoToRemove);
        }

        public void removeOther(ICollisionObject other)
        {
                    otherObjects.Remove(other);
        }
        
        public enum OtherCowLocations
        {
            ThereIsACowNextToUsWithNoCowOnTop,
            ThereIsACowOnTopOfTheCowToTheRight,
            ThereIsNoCowBelowCurrentCow,
            ThereIsNoCowToTheRight
        }
    }
}
