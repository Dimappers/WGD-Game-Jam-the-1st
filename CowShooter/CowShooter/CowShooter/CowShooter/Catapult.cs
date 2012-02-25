﻿using System;
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
        
        Vector2 positionToSpawn = new Vector2(600, 50);
        Rectangle draggablePosition = new Rectangle(648, 51, 32, 32);
        const int widthOfLine = 5;
        const float maxDistance = 150;
        const float powerScale = 0.05f;
        
        Texture2D catapultTexture;
        Texture2D lineTexture;
        Texture2D ammoTexture;
        bool isBeingDragged;
        MouseState oldMouseState, newMouseState;
        Vector2 draggedToPoint;

        List<Ammunition> ammo;

        public Catapult(Texture2D catapultTexture, Texture2D lineTexture, Texture2D ammoTexture)
        {
            this.catapultTexture = catapultTexture;
            this.lineTexture = lineTexture;
            this.ammoTexture = ammoTexture;
            oldMouseState = Mouse.GetState();
            newMouseState = oldMouseState;

            // Setup the list of shot ammo
            ammo = new List<Ammunition>();
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
                if (newMouseState.X < draggablePosition.Center.X)
                {
                    draggedToPoint.X = draggablePosition.Center.X;
                }
            }

            foreach (Ammunition a in ammo)
            {
                a.Update(gameTime);
            }

            oldMouseState = newMouseState;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(catapultTexture, positionToSpawn, Color.White);
            foreach (Ammunition a in ammo)
            {
                a.Draw(spriteBatch);
            }

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
            if (isBeingDragged)
            {
                isBeingDragged = false;


                Vector2 fireTrajectory = (draggedToPoint -
                            new Vector2(draggablePosition.Center.X, draggablePosition.Center.Y))
                            * powerScale;
                Console.WriteLine("Fire trajectory: " + fireTrajectory.ToString());

                ammo.Add(new Ammunition(this, fireTrajectory,
                                            new Vector2(draggablePosition.Center.X, draggablePosition.Center.Y),
                                            ammoTexture));
            }
        }

        private void drawLine(Vector2 endPosition, SpriteBatch spriteBatch)
        {
            Vector2 startPosition = new Vector2(draggablePosition.Center.X, draggablePosition.Center.Y);
            float length = Vector2.Distance(startPosition, endPosition);
            if (length > maxDistance)
            {
                length = maxDistance;
            }
            float alpha = (3.0f * (float)Math.PI) / 2.0f;
            float xDist = endPosition.X - startPosition.X;
            float yDist = endPosition.Y - startPosition.Y;
            alpha += (float)Math.Atan(yDist / xDist);

            spriteBatch.Draw(lineTexture, new Rectangle((int)startPosition.X, (int)startPosition.Y, widthOfLine, (int)length), null, Color.White, alpha, new Vector2(widthOfLine / 2, 0), SpriteEffects.None, 0);
        }
    }
}
