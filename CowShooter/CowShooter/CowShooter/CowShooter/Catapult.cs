using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CowShooter
{
    public class Catapult
    {
        
        Vector2 positionToSpawn = new Vector2(668, 100);
        Rectangle draggablePosition = new Rectangle(660, 84, 32, 32);
        const int widthOfLine = 5;
        const float maxDistance = 150;
        const float sharpestDownAngle = (9.0f * (float)Math.PI) / 8.0f;
        const float powerScale = 0.05f;
        
        Texture2D catapultTexture;
        Texture2D lineTexture;
        Texture2D ammoTexture;
        bool isBeingDragged;
        MouseState oldMouseState, newMouseState;
        Vector2 draggedToPoint;
        CollisionManager collisionManager;
        List<Ammunition> ammo;
        float lastSetAngle;
        int damage;


        public Catapult(Texture2D catapultTexture, Texture2D lineTexture, Texture2D ammoTexture, CollisionManager collisionManager)
        {
            this.catapultTexture = catapultTexture;
            this.lineTexture = lineTexture;
            this.ammoTexture = ammoTexture;
            oldMouseState = Mouse.GetState();
            newMouseState = oldMouseState;
            this.collisionManager = collisionManager;
            // Setup the list of shot ammo
            ammo = new List<Ammunition>();
            lastSetAngle = 0.0f;
            damage = 1;
        }

        public void Reset()
        {
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

            List<Ammunition> deadAmmo = new List<Ammunition>();
            foreach (Ammunition a in ammo)
            {
                a.Update(gameTime);
                if (a.isDead)
                {
                    deadAmmo.Add(a);
                }
            }

            foreach (Ammunition a in deadAmmo)
            {
                ammo.Remove(a);
                collisionManager.removeAmmo(a);
            }
            oldMouseState = newMouseState;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Ammunition a in ammo)
            {
                a.Draw(spriteBatch);
            }

            if (isBeingDragged)
            {
                drawLine(draggedToPoint, spriteBatch);
            }
            else
            {
                spriteBatch.Draw(catapultTexture, positionToSpawn, null, Color.White, (3.0f * (float)Math.PI) / 2.0f + ((float)Math.PI / 2.0f) + lastSetAngle, /*Vector2.Zero*/new Vector2(68, 34f), 1.0f, SpriteEffects.None, 0.0f);
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

                Vector2 fireTrajectory = (draggedToPoint - new Vector2(draggablePosition.Center.X, draggablePosition.Center.Y)) * powerScale;
                Console.WriteLine("Fire trajectory: " + fireTrajectory.ToString());
                Ammunition newAmmo = new Ammunition(this, fireTrajectory, new Vector2(draggablePosition.Center.X, draggablePosition.Center.Y), ammoTexture, damage);
                ammo.Add(newAmmo);
                collisionManager.addAmmo(newAmmo);
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

            if (alpha < sharpestDownAngle)
                alpha = sharpestDownAngle;

            spriteBatch.Draw(lineTexture, new Rectangle((int)startPosition.X, (int)startPosition.Y, widthOfLine, (int)length), null, Color.White, alpha, new Vector2(widthOfLine / 2, 0), SpriteEffects.None, 0);
            spriteBatch.Draw(catapultTexture, positionToSpawn, null, Color.White, (3.0f * (float)Math.PI) / 2.0f + ((float)Math.PI / 2.0f) + (float)Math.Atan(yDist / xDist), /*Vector2.Zero*/new Vector2(68,34f), 1.0f, SpriteEffects.None, 0.0f);
            lastSetAngle = (float)Math.Atan(yDist / xDist);
        }

        public void upgradeDamage(int upgradeAmount)
        {
            damage += upgradeAmount;
        }
    }
}
