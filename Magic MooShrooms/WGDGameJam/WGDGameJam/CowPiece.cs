﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WGDGameJam
{
    public class CowPiece
    {
        public static Point startPoint = new Point(5, 5);
        Vector2 squareSize = new Vector2(50, 50);
        Vector2 middle = new Vector2(375, 275);

        protected Texture2D texture;
        protected Texture2D tailTexture;
        protected CowPiece nextPiece;
        protected DirectionToMove lastMoveDirection;
        protected bool isTail;

        protected Point location;
        public Point headPosition;

        Game1 game;

        public CowPiece(Texture2D texture, Texture2D tailTexture, Game1 game)
        {
            this.texture = texture;
            this.tailTexture = tailTexture;
            headPosition = startPoint;
            nextPiece = null;
            isTail = false;
            location = startPoint;
            this.game = game;
        }

        public CowPiece(Texture2D texture, Texture2D tailTexture, bool isTail, Game1 game)
            :this(texture, tailTexture, game)
        {
            this.isTail = isTail;
        }

        public void AttachPiece(CowPiece newPiece)
        {
            if (nextPiece == null)
            {
                nextPiece = newPiece;
                Point newLocation = Point.Zero;
                switch (lastMoveDirection)
                {
                    case DirectionToMove.down:
                        newLocation = new Point(location.X, location.Y - 1);
                        break;

                    case DirectionToMove.up:
                        newLocation = new Point(location.X, location.Y + 1);
                        break;

                    case DirectionToMove.left:
                        newLocation = new Point(location.X + 1, location.Y);
                        break;

                    case DirectionToMove.right:
                        newLocation = new Point(location.X - 1, location.Y);
                        break;
                }

                newPiece.Move(newLocation);
                newPiece.SetAsTail();
                newPiece.lastMoveDirection = lastMoveDirection;
                isTail = false;
                newPiece.headPosition = headPosition;
            }
            else
            {
                nextPiece.AttachPiece(newPiece);
            }
        }

        public void Move(Point newLocation)
        {
            location = newLocation;
        }

        public void SetAsTail()
        {
            isTail = true;
        }

        protected bool isDirectionOpposite(DirectionToMove a, DirectionToMove b)
        {
            switch (a)
            {
                case DirectionToMove.right:
                    return b == DirectionToMove.left;

                case DirectionToMove.left:
                    return b == DirectionToMove.right;

                case DirectionToMove.up:
                    return b == DirectionToMove.down;

                case DirectionToMove.down:
                    return b == DirectionToMove.up;
            }

            return false;
        }

        public virtual void Update(GameTime gameTime, DirectionToMove moveDirection, Point newHeadPosition)
        {
            if (isDirectionOpposite(moveDirection, lastMoveDirection))
            {
                moveDirection = lastMoveDirection;
            }
            if (isTail) { game.GetMapManager().getSquare(location.X, location.Y).SetBlocking(false); }
            switch (moveDirection)
            {
                case DirectionToMove.down:
                    {
                        location.Y += 1;
                        break;
                    }

                case DirectionToMove.up:
                    {
                        location.Y -= 1;
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
            Vector2 position = middle + new Vector2(squareSize.X * offsetFromHead.X, squareSize.Y * offsetFromHead.Y) + new Vector2(400, 300);

            Texture2D texToDraw;
            if (isTail)
            {
                texToDraw = tailTexture;
            }
            else
            {
                texToDraw = texture;
            }
            float rotation = 0.0f;
            switch(lastMoveDirection)
            {
                case DirectionToMove.down:
                    rotation = (float)(3.0f * Math.PI / 2.0f);
                    break;

                case DirectionToMove.right:
                    rotation = (float)Math.PI;
                    break;

                case DirectionToMove.up:
                    rotation = (float)(Math.PI / 2.0f);
                    break;

            }
            spriteBatch.Draw(texToDraw, position, null, Color.White, rotation, new Vector2(texToDraw.Width/2 + 0.5f, texToDraw.Height/2 + 0.5f),1.0f, SpriteEffects.None, 0.0f);

            if (nextPiece != null)
            {
                nextPiece.Draw(gameTime, spriteBatch);
            }
        }
    }

    public enum DirectionToMove
    {
        up,
        down,
        left,
        right
    }
}
