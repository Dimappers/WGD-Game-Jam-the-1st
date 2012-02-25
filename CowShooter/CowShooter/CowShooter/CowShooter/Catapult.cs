using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CowShooter
{
    class Catapult
    {
        Vector2 positionToSpawn = new Vector2(10, 10);
        Rectangle draggablePosition = new Rectangle(0,0, 50,50);
        const int widthOfLine = 5;
        
        Texture2D catapultTexture;
        Texture2D lineTexture;
        bool isBeingDragged;
        MouseState oldMouseState, newMouseState;
        Vector2 draggedToPoint;

        public Catapult(Texture2D catapultTexture, Texture2D lineTexture)
        {
            this.catapultTexture = catapultTexture;
            this.lineTexture = lineTexture;
            oldMouseState = Mouse.GetState();
            newMouseState = oldMouseState;
        }

        public void Update(GameTime gameTime)
        {
            newMouseState = Mouse.GetState();

            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                onPress();
            }
            else if (newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                onRelease();
            }

            if (isBeingDragged)
            {
                //TODO: Distance check
                draggedToPoint = new Vector2(newMouseState.X, newMouseState.Y);
            }

            oldMouseState = newMouseState;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(catapultTexture, positionToSpawn, Color.White);

            if (isBeingDragged)
            {
                drawLine(draggedToPoint, spriteBatch);
            }
        }

        private void onPress()
        {
            if (draggablePosition.Contains(newMouseState.X, newMouseState.Y))
            {
                isBeingDragged = true;
                draggedToPoint = new Vector2(newMouseState.X, newMouseState.Y);
            }
        }

        private void onRelease()
        {
            isBeingDragged = false;


            Vector2 fireTrajectory = draggedToPoint - new Vector2(draggablePosition.Center.X, draggablePosition.Center.Y);
            Console.WriteLine("Fire trajectory: " + fireTrajectory.ToString());
        }

        private void drawLine(Vector2 endPosition, SpriteBatch spriteBatch)
        {
            Vector2 startPosition = new Vector2(draggablePosition.Center.X, draggablePosition.Center.Y);
            float length = Vector2.Distance(startPosition, endPosition);
            float theta = (float)Math.Atan((double)((endPosition.X - startPosition.X) / (endPosition.Y - startPosition.Y)));
            float alpha = (float)(2 * Math.PI) - theta;
            spriteBatch.Begin();
            spriteBatch.Draw(lineTexture, new Rectangle((int)startPosition.X, (int)startPosition.Y, widthOfLine, (int)length), null, Color.White, alpha, new Vector2(lineWidth / 2, 0), SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }


}
