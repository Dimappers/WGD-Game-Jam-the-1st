using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    class WallBlock
    {
        Texture2D texture;
        Vector2 position;
        public WallBlock(Texture2D texture, Vector2 position)
        { 
            this.texture = texture; 
            this.position = position; 
        }
        public void Draw(SpriteBatch spriteBatch) 
        { 
            spriteBatch.Draw(texture, position, Color.White); 
        }
    }
}
