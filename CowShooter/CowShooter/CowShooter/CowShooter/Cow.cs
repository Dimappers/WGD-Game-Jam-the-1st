using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    class Cow
    {
        const float velocity = 1.0f;
        Vector2 cowPosition;
        CowManager manager;

        public Cow(CowManager manager)
        {
            cowPosition = new Vector2(0, 400);
            this.manager = manager;
        }


        public virtual void Update(GameTime gameTime)
        {
            cowPosition.X += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(manager.GetTexture(GetType()), cowPosition, Color.White);
        }
    }
}
