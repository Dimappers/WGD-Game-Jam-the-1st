using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CowShooter
{
    class CollisionManager
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
    }
}
