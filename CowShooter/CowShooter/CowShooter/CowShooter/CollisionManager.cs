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

        /*public bool checkCowCollision(Cow cow)
        {
            foreach (Cow otherCow in cows)
            {
                if (!otherCow.Equals(cow))
                {
                    if (cow.getCollisionRectangle().Intersects(otherCow.getCollisionRectangle()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }*/

        public OtherCowLocations findCowCollisions(Cow cow)
        {
            Rectangle cowRectangle = cow.getCollisionRectangle(); 
            bool below = false;
            bool jumpTo = false;
            bool nextTo = false;
            foreach (ICollisionObject otherCowObject in otherObjects)
            {             
                Cow otherCow = otherCowObject as Cow;
                if(otherCow != null)
                {
                    if (!otherCow.Equals(cow)&&otherCow.partOfPyramid)
                    {
                        if (new Rectangle(cowRectangle.X, cowRectangle.Y + cowRectangle.Height, cowRectangle.Width, cowRectangle.Height).Intersects(otherCow.getCollisionRectangle()))
                        {
                            below = true;
                        }
                        else if (new Rectangle(cowRectangle.X + cowRectangle.Width, cowRectangle.Y - cowRectangle.Height, cowRectangle.Width, cowRectangle.Height).Intersects(otherCow.getCollisionRectangle()))
                        {
                            jumpTo = true;
                        }
                        else if (new Rectangle(cowRectangle.X + cowRectangle.Width, cowRectangle.Y, cowRectangle.Width, cowRectangle.Height).Intersects(otherCow.getCollisionRectangle()))
                        {
                            nextTo = true;
                        }
                    }
                }
            }
            if(below)
            {
                return OtherCowLocations.notBelow;
            }
            if(jumpTo)
            {
                return OtherCowLocations.alsoJumpTo; 
            }
            if(nextTo) 
            {
                return OtherCowLocations.onlyNextTo;
            }
            return OtherCowLocations.noCows;
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
            onlyNextTo,
            alsoJumpTo,
            notBelow,
            noCows
        }
    }
}
