using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace WGDGameJam
{
    class HeadPiece : CowPiece
    {
        Texture2D standardTexture;

        KeyboardState oldState;
        KeyboardState newState;

        DirectionToMove direction;


        public HeadPiece(Texture2D headTexture, Texture2D standardPiece)
            :base(headTexture, headTexture)
        {
            standardTexture = standardPiece;
            oldState = Keyboard.GetState();
            newState = oldState;
        }

        public void Update(GameTime gameTime)
        {
            newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Right) && oldState.IsKeyUp(Keys.Right)) { direction = DirectionToMove.right; }
            else if (newState.IsKeyDown(Keys.Left) && oldState.IsKeyUp(Keys.Left)) { direction = DirectionToMove.left; }
            else if (newState.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up)) { direction = DirectionToMove.up; }
            else if (newState.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down)) { direction = DirectionToMove.down; }

            oldState = newState;

            base.Update(gameTime, direction, new Point(-1,-1));
        }
    }
}
