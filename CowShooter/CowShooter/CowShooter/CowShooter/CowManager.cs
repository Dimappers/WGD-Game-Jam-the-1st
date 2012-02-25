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
        List<Cow> activeCows;
        Dictionary<Type, Texture2D> cowTextures;

        public CowManager()
        {
            activeCows = new List<Cow>();
            cowTextures = new Dictionary<Type, Texture2D>();
        }

        public void Update(GameTime gameTime)
        {
            foreach(Cow cow in activeCows)
            {
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

        public void AddTexture(Type asociatedType, Texture2D texture)
        {
            cowTextures.Add(asociatedType, texture);
        }

        public Texture2D GetTexture(Type type)
        {
            return cowTextures[type];
        }

        private void GenerateStartingCows()
        {
            Cow myCow = new Cow(this);
            activeCows.Add(myCow);
        }
    }
}
