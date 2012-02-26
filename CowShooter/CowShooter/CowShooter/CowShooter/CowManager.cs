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
            cowTextures = new Dictionary<Type, Texture2D>();
            randomNumber = new Random();
            timeTillNextCow = randomNumber.Next(minSpawnTime, maxSpawnTime);
            GenerateCow();

        }
        public void Update(GameTime gameTime)
        {
            if (timeTillNextCow < (float)gameTime.TotalGameTime.TotalSeconds)
            {
                GenerateCow();
                timeTillNextCow = (float)gameTime.TotalGameTime.TotalSeconds
                                        + randomNumber.Next(minSpawnTime, maxSpawnTime);
            }
            foreach(Cow cow in activeCows)
            {
                if (collisionManager.checkCowCollision(cow)) {
                    cow.cowIsInFront = true;
                }
                cow.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Cow cow in activeCows)
            {
                cow.Draw(spriteBatch);
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
            Cow myCow = new Cow(this);
            activeCows.Add(myCow);
            collisionManager.addCow(myCow);
        }

        public void RemoveCow(Cow removeCow)
        {
            activeCows.Remove(removeCow);
        }
    }
}
