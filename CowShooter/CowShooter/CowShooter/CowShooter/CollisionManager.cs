using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CowShooter
{
    class CollisionManager
    {
        List<Ammunition> ammo;
        List<Cow> cows;
        List<ICollisionObject> otherObjects;
        float groundLevel;

        public CollisionManager(float groundLevel)
        {
            ammo = new List<Ammunition>();
            cows = new List<Cow>();
            otherObjects = new List<ICollisionObject>();

            this.groundLevel = groundLevel;
        }

        public void addCow(Cow cow)
        {
            cows.Add(cow);
        }

        public void addAmmo(Ammunition ammo)
        {
            this.ammo.Add(ammo);
        }

        public void addOther(ICollisionObject other)
        {
            otherObjects.Add(other);
        }

        public void checkCollision()
        {
            foreach (Ammunition ammoShot in ammo)
            {
                foreach (Cow c in cows)
                {
                    if (c.getCollisionRectangle().Intersects(ammoShot.getCollisionRectangle()))
                    {
                        c.NotifyOfCollision(ammoShot);
                        ammoShot.NotifyOfCollision(c);
                    }
                }

                foreach (ICollisionObject other in otherObjects)
                {
                    if (ammoShot.getCollisionRectangle().Intersects(other.getCollisionRectangle()))
                    {
                        ammoShot.NotifyOfCollision(other);
                        other.NotifyOfCollision(ammoShot);
                    }
                    foreach (Cow c in cows)
                    {
                        if (other.getCollisionRectangle().Intersects(c.getCollisionRectangle()))
                        {
                            other.NotifyOfCollision(c);
                            c.NotifyOfCollision(other);
                        }
                    }

                    foreach (ICollisionObject another in otherObjects)
                    {
                        if (another != other)
                        {
                            if (another.getCollisionRectangle().Intersects(other.getCollisionRectangle()))
                            {
                                another.NotifyOfCollision(other);
                                other.NotifyOfCollision(another);
                            }
                        }
                    }
                }
                

                if (ammoShot.getCollisionRectangle().Bottom > groundLevel)
                {
                    ammoShot.NotifyGroundCollision();
                }
            }
        }

        public bool checkCowCollision(Cow cow)
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
        }

        public void removeCow(Cow cow)
        {
            cows.Remove(cow);
        }

        public void removeAmmo(Ammunition ammoToRemove)
        {
            ammo.Remove(ammoToRemove);
        }

        public void removeOther(ICollisionObject other)
        {
            otherObjects.Remove(other);
        }
    }
}
