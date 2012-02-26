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

        public OtherCowLocations findCowCollisions(Cow cow)
        {
            Rectangle cowRectangle = cow.getCollisionRectangle(); 
            bool below = false;
            bool jumpTo = false;
            bool nextTo = false;
            foreach (Cow otherCow in cows)
            {
                if (!otherCow.Equals(cow)&&otherCow.GetType()==typeof(Cow))
                {
                    if (!new Rectangle(cowRectangle.X, cowRectangle.Y+cowRectangle.Height, cowRectangle.Width, cowRectangle.Height).Intersects(otherCow.getCollisionRectangle()))
                    {
                        below = true;
                    }
                    else if (new Rectangle(cowRectangle.X + cowRectangle.Width, cowRectangle.Y - cowRectangle.Height, cowRectangle.Width, cowRectangle.Height).Intersects(otherCow.getCollisionRectangle()))
                    {
                        jumpTo = true;
                    }
                    else if(new Rectangle(cowRectangle.X + cowRectangle.Width, cowRectangle.Y, cowRectangle.Width, cowRectangle.Height).Intersects(otherCow.getCollisionRectangle()))
                    {
                        nextTo = true;
                    }
                }
            }
            if(below) {return OtherCowLocations.notBelow;}
            if(jumpTo) {return OtherCowLocations.alsoJumpTo; }
            if(nextTo) {return OtherCowLocations.onlyNextTo;}
            return OtherCowLocations.noCows;
        }

        public void removeCow(Cow cow)
        {
            cows.Remove(cow);
        }

        public void removeAmmo(Ammunition ammoToRemove)
        {
            ammo.Remove(ammoToRemove);
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
