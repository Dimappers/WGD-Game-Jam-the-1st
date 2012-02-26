using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    class CowManager
    {
        public List<Cow> activeCows;
        public List<Meat> meatsToRemove;
        public List<Meat> activeMeats;
        Dictionary<Type, Texture2D> cowTextures;

        const int minSpawnTime = 5;
        const int maxSpawnTime = 10;

        float timeTillNextCow;
        Random randomNumber;

        CollisionManager collisionManager;

        public CowManager(CollisionManager collisionManager)
        {
            this.collisionManager = collisionManager;
            activeCows = new List<Cow>();
            activeMeats = new List<Meat>();
            meatsToRemove = new List<Meat>();
            cowTextures = new Dictionary<Type, Texture2D>();
            randomNumber = new Random();
            timeTillNextCow = randomNumber.Next(minSpawnTime, maxSpawnTime);
            GenerateCow();
            activeMeats = new List<Meat>();

        }
        public void Update(GameTime gameTime)
        {
            if (timeTillNextCow < (float)gameTime.TotalGameTime.TotalSeconds)
            {
                GenerateCow();
                timeTillNextCow = (float)gameTime.TotalGameTime.TotalSeconds
                                        + randomNumber.Next(minSpawnTime, maxSpawnTime);
            }
            List<Cow> cowsToRemove = new List<Cow>();
            meatsToRemove = new List<Meat>();
            foreach(Cow cow in activeCows)
            {
                if (collisionManager.checkCowCollision(cow)) {
                    cow.cowIsInFront = true;
                }
                cow.Update(gameTime);
                if (cow.isDead)
                {
                    cowsToRemove.Add(cow);
                }
            }
            foreach (Cow cow in cowsToRemove)
            {
                RemoveCow(cow);
            }
            foreach (Meat meat in activeMeats)
            {
                meat.Update(gameTime);
            }
            foreach (Meat meat in meatsToRemove)
            {
                activeMeats.Remove(meat);
                collisionManager.removeOther(meat);
            }
            meatsToRemove = new List<Meat>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Cow cow in activeCows)
            {
                cow.Draw(spriteBatch);
            }
            foreach (Meat meat in activeMeats)
            {
                meat.Draw(spriteBatch);
            }
        }

        public void AddTexture(Type associatedType, Texture2D texture)
        {
            cowTextures.Add(associatedType, texture);
        }

        public Texture2D GetTexture(Type type)
        {
            return cowTextures[type];
        }

        private void GenerateCow()
        {
            Cow myCow;
            if (randomNumber.Next(5) == 0) { myCow = new Bull(this); }
            else { myCow = new Cow(this); }
            activeCows.Add(myCow);
            collisionManager.addCow(myCow);
        }

        public void RemoveCow(Cow removeCow)
        {
            activeCows.Remove(removeCow);
            Meat newMeat = new Meat(this, GetTexture(typeof(Meat)), removeCow.cowPosition);
            activeMeats.Add(newMeat);
            collisionManager.addOther(newMeat);
            collisionManager.removeCow(removeCow);
        }

        public void RemoveMeat(Meat removeMeat)
        {
            meatsToRemove.Add(removeMeat);
        }

        public Meat NearestMeat(float toWhere)
        {
            Meat nearestMeat = null;
            foreach (Meat meat in activeMeats)
            {
                if(nearestMeat==null) {nearestMeat = meat;}
                else if (Math.Abs(meat.getLocation().X - toWhere) < Math.Abs(nearestMeat.getLocation().X - toWhere)) { nearestMeat = meat; }
            }
            return nearestMeat;
        }
    }
}
