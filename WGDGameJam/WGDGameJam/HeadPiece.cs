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
        bool dead;

        public HeadPiece(Texture2D headTexture, Texture2D standardPiece, Texture2D tailTexture, Game1 game)
            :base(headTexture, headTexture, game)
        {
            standardTexture = standardPiece;
            standardTailTexture = tailTexture;
            this.game = game;
            dead = false;
        }

        public bool isDead()
        {
            return dead;
        }

        public override void Update(GameTime gameTime, DirectionToMove moveDirection, Point newHeadPosition)
        {
            base.Update(gameTime, moveDirection, newHeadPosition);
            if (game.GetMapManager().getSquare(location.X,location.Y).isBlocking())
            {
                dead = true;
            }
            else if (game.GetMapManager().containsFood(location))
            {
                CowPiece newPiece = new CowPiece(standardTexture, standardTailTexture, game);
                AttachPiece(newPiece);
                game.GetMapManager().removeFood(location);
                game.addToScore();
            }
            game.GetMapManager().getSquare(location.X, location.Y).SetBlocking(true);
        }
    }
}
