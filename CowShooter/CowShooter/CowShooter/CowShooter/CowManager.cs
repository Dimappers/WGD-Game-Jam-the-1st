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
        public List<Meat> activeMeats;
        public List<Meat> meatsToRemove;
        Dictionary<Type, Texture2D> cowTextures;

        CollisionManager collisionManager;

        public CowManager(CollisionManager collisionManager)
        {
            this.collisionManager = collisionManager;
            activeCows = new List<Cow>();
            activeMeats = new List<Meat>();
            meatsToRemove = new List<Meat>();
            cowTextures = new Dictionary<Type, Texture2D>();

            GenerateCow();
            activeCows.ElementAt<Cow>(0).JumpUp();
            
            
            GenerateCow();

        }
        public void Update(GameTime gameTime)
        {
            foreach(Cow cow in activeCows)
            {
                cow.Update(gameTime);
            }
            foreach (Meat meat in activeMeats)
            {
                meat.Update(gameTime);
            }
            foreach (Meat meat in meatsToRemove)
            {
                activeMeats.Remove(meat);
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
            Cow myCow = new Cow(this);
            activeCows.Add(myCow);
            collisionManager.addCow(myCow);
        }

        public void RemoveCow(Cow removeCow)
        {
            activeCows.Remove(removeCow);
            activeMeats.Add(new Meat(this, GetTexture(typeof(Meat)), removeCow.cowPosition));
        }

        public void RemoveMeat(Meat removeMeat)
        {
            meatsToRemove.Add(removeMeat);
        }
    }
}
