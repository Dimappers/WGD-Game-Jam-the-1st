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
        float groundLevel;

        public CollisionManager(float groundLevel)
        {
            ammo = new List<Ammunition>();
            cows = new List<Cow>();

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


    }
}
