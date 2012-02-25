using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    class Meat
    {
        const int maxAge = 15000;
        double age;

        CowManager manager;
        Texture2D texture;
        Vector2 position;

        public Meat(CowManager manager, Texture2D texture, Vector2 position)
        {
            this.manager = manager;
            this.texture = texture;
            this.position = position;
            age = 0; 
        }

        public void Update(GameTime gameTime)
        {
            age+=gameTime.ElapsedGameTime.TotalMilliseconds;
            if (age >= maxAge) { manager.RemoveMeat(this); }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
