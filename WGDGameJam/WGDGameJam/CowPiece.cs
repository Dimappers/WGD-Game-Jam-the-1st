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
        const Vector2 squareSize = new Vector2(50, 50);
        const Vector2 middle = new Vector2(375, 275);
        protected Texture2D texture;
        protected CowPiece nextPiece;
        protected DirectionToMove lastMoveDirection;

        Point location;
        Point headPosition;

        public CowPiece(Texture2D texture)
        {
            this.texture = texture;
            headPosition = startPoint;
            nextPiece = null;
        }

        public void Update(GameTime gameTime, DirectionToMove moveDirection, Point newHeadPosition)
        {
            switch (moveDirection)
            {
                case DirectionToMove.down:
                    {
                        location.Y -= 1;
                        break;
                    }

                case DirectionToMove.up:
                    {
                        location.Y += 1;
                        break;
                    }

                case DirectionToMove.left:
                    {
                        location.X -= 1;
                        break;
                    }

                case DirectionToMove.right:
                    {
                        location.X += 1;
                        break;
                    }
            }

            if (newHeadPosition.X == -1)
            {
                headPosition = location;
            }
            else
            {
                headPosition = newHeadPosition;
            }

            if (nextPiece != null)
            {
                nextPiece.Update(gameTime, lastMoveDirection, headPosition);
            }

            lastMoveDirection = moveDirection;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Point offsetFromHead = new Point(location.X - headPosition.X, location.Y - headPosition.Y);
            Vector2 position = middle + new Vector2(squareSize.X * offsetFromHead.X, squareSize.Y * offsetFromHead.Y);
            spriteBatch.Draw(texture, position, Color.White);
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
