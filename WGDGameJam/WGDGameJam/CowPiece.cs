using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WGDGameJam
{
    class CowPiece
    {
        const Point startPoint = new Point(50, 50);
        protected Texture2D texture;

        Point location;
        Point headPosition;

        public CowPiece(Texture2D texture)
        {
            this.texture = texture;
            headPosition = startPoint;
        }



        public void Update(GameTime gameTime, DirectionToMove moveDirection, Point newHeadPosition)
        {
            headPosition = newHeadPosition;
            switch (moveDirection)
            {
                case DirectionToMove.down:
                    {

                    }

            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, 
        }
    }

    enum DirectionToMove
    {
        up,
        down,
        left,
        right
    }
}
