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
        Point startPoint = new Point(50, 50);
        Vector2 squareSize = new Vector2(50, 50);
        Vector2 middle = new Vector2(375, 275);

        protected Texture2D texture;
        protected Texture2D tailTexture;
        protected CowPiece nextPiece;
        protected DirectionToMove lastMoveDirection;
        protected bool isTail;

        Point location;
        public Point headPosition;

        public CowPiece(Texture2D texture, Texture2D tailTexture)
        {
            this.texture = texture;
            this.tailTexture = tailTexture;
            headPosition = startPoint;
            nextPiece = null;
            isTail = false;
        }

        public CowPiece(Texture2D texture, Texture2D tailTexture, bool isTail)
            :this(texture, tailTexture)
        {
            this.isTail = isTail;
        }

        public void AttachPiece(CowPiece newPiece)
        {
            if (nextPiece == null)
            {
                nextPiece = newPiece;
                isTail = false;
            }
            else
            {
                nextPiece.AttachPiece(newPiece);
            }
        }

        public virtual void Update(GameTime gameTime, DirectionToMove moveDirection, Point newHeadPosition)
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

        
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Point offsetFromHead = new Point(location.X - headPosition.X, location.Y - headPosition.Y);
            Vector2 position = middle + new Vector2(squareSize.X * offsetFromHead.X, squareSize.Y * offsetFromHead.Y);
            Texture2D texToDraw;
            if (isTail)
            {
                texToDraw = tailTexture;
            }
            else
            {
                texToDraw = texture;
            }
            spriteBatch.Draw(texToDraw, position, Color.White);

            if (nextPiece != null)
            {
                nextPiece.Draw(gameTime, spriteBatch);
            }
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
