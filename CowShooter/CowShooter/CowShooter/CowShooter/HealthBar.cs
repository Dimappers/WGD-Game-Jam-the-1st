using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    public class HealthBar
    {

        Texture2D bar1Texture;
        Vector2 bar1Position;

        Texture2D bar2Texture;
        Vector2 bar2Position;

        int totalHealth;
        int currentHealth;

        int healthWidth;
        const int baseHealthWidth = 26;

        bool isShowing;

        float timeSinceLastHit;

        public HealthBar(Texture2D h1Texture, Texture2D h2Texture, Vector2 cowPosition, int totalHealth)
        {
            bar1Texture = h1Texture;
            bar2Texture = h2Texture;

            this.totalHealth = totalHealth;
            currentHealth = totalHealth;
            healthWidth = baseHealthWidth;

            bar1Position = new Vector2();
            bar2Position = new Vector2();

            updatePosition(cowPosition);
            isShowing = false;
            //Constructor stuff
        }

        public void Update(GameTime gameTime, Vector2 cowPosition)
        {
            if (isShowing)
            {
                if (gameTime.TotalGameTime.TotalSeconds > timeSinceLastHit + 3)
                {
                    isShowing = false;
                    timeSinceLastHit = -1;
                }
            }
            updatePosition(cowPosition);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isShowing)
            {
                spriteBatch.Draw(bar1Texture, bar1Position, Color.White);
                spriteBatch.Draw(bar2Texture, new Rectangle((int)bar2Position.X, (int)bar2Position.Y, healthWidth, bar2Texture.Height), 
                    null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.0f);

            }
            // Draw the stuff
        }

        public void takeDamage(int damage, GameTime gameTime)
        {
            currentHealth -= damage;
            if (currentHealth < 0) {
                currentHealth = 0;
            }


            timeSinceLastHit = (float)gameTime.TotalGameTime.TotalSeconds;
            isShowing = true;

            healthWidth = (int)((float)baseHealthWidth * ((float)currentHealth / (float)totalHealth));
            // Takes damage
        }

        public void updatePosition(Vector2 cowPosition)
        {
            bar1Position.Y = cowPosition.Y - bar1Texture.Height;
            bar1Position.X = cowPosition.X + 10;

            bar2Position.Y = bar1Position.Y + 2;
            bar2Position.X = bar1Position.X + 2;
        }

    }
}
