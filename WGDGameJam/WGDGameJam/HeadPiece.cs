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

        public HeadPiece(Texture2D headTexture, Texture2D standardPiece)
            :base(headTexture, headTexture)
        {
            standardTexture = standardPiece;
        }
    }
}
