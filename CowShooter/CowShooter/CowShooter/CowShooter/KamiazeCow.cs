using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    class KamikazeCow : Cow
    {
        const int health = 1;
        public KamikazeCow(CowManager manager, WallManager wallManager, CowStack cowStack, Texture2D h1Texture, Texture2D h2Texture) : base(manager, wallManager, cowStack, h1Texture, h2Texture, health) {}

        public bool checkWallCollision()
        {
            if (cowPosition.X + frameSize.Width >= 590)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (texture == null)
            {
                texture = manager.GetTexture(GetType());
            }
            healthBar.Update(gameTime, cowPosition);

            this.gameTime = gameTime;
            Move(velocity_h, 0);
        }
                
    }
}
