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
        double age;

        CowManager manager;
        Texture2D texture;
        Vector2 position;
        Color colour = Color.White;

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
            if (age >= 15000) { manager.RemoveMeat(this); }
            else if (age >= 10000) { colour = Color.Green; }
            else if (age >= 5000) {colour = Color.LightGreen;}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, colour);
        }

        public float getLocation()
        {
            return position.X;
        }
    }
}
