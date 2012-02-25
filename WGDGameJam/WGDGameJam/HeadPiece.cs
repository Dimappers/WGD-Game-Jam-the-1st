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
        Texture2D standardTailTexture;
        Game1 game;

        public HeadPiece(Texture2D headTexture, Texture2D standardPiece, Texture2D tailTexture, Game1 game)
            :base(headTexture, headTexture)
        {
            standardTexture = standardPiece;
            standardTailTexture = tailTexture;
            this.game = game;
        }

        public override void Update(GameTime gameTime, DirectionToMove moveDirection, Point newHeadPosition)
        {
            base.Update(gameTime, moveDirection, newHeadPosition);

            //TODO: Check if the next square is 


            if(game.GetMapManager().containsFood(location))
            {
                CowPiece newPiece = new CowPiece(standardTexture, standardTailTexture);
                AttachPiece(newPiece);
                game.GetMapManager().removeFood(location);
            }
        }
    }
}
