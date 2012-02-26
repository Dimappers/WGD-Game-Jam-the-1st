using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    public class CowManager
    {
        public List<Cow> activeCows;
        public List<Meat> meatsToRemove;
        public List<Meat> activeMeats;
        Dictionary<Type, Texture2D> cowTextures;

        Texture2D h1Texture;
        Texture2D h2Texture;

        const int minSpawnTime = 5;
        const int maxSpawnTime = 10;

        float timeTillNextCow;
        Random randomNumber;

        CollisionManager collisionManager;
        WallManager wallManager;

        CowStack cowStack;

        public CowManager(CollisionManager collisionManager, Texture2D h1Texture, Texture2D h2Texture)
        {
            this.collisionManager = collisionManager;
            activeCows = new List<Cow>();
            activeMeats = new List<Meat>();
            meatsToRemove = new List<Meat>();
            cowTextures = new Dictionary<Type, Texture2D>();
            randomNumber = new Random();
            timeTillNextCow = randomNumber.Next(minSpawnTime, maxSpawnTime);
            activeMeats = new List<Meat>();
            
            this.h1Texture = h1Texture;
            this.h2Texture = h2Texture;

        }
        public void SetWallManager(WallManager wallManager)
        {
            this.wallManager = wallManager;
            cowStack = new CowStack(wallManager);
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
            foreach(Cow cow in activeCows)
            {
                cow.otherCows = collisionManager.findCowCollisions(cow);
                cow.Update(gameTime);
                if (cow.isDead)
                {
                    cowsToRemove.Add(cow);
                }
            }
            foreach (Cow cow in cowsToRemove)
            {
                cowStack.removeDeadCow(cow.getStackPosition());
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
            if (randomNumber.Next(5) == 0) { myCow = new Bull(this, wallManager, cowStack, h1Texture, h2Texture); }
            else { myCow = new Cow(this, wallManager, cowStack, h1Texture, h2Texture, 1); }
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
