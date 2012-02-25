using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WGDGameJam
{
    public class WallPiece : Square
    {
        public WallPiece next;
        public WallPiece(int x, int y, Texture2D texture, Color colour, MapManager manager) 
            : base(x,y,texture,colour,true,manager)
        {}

        public void Attach(WallPiece wall) {
            next = wall;
        }

        public void Move(int x, int y, Texture2D shapeToDraw)
        {
            if (next != null) { next.Move(xlocation, ylocation, texture); }
            else { mapManager.map[xlocation, ylocation] = mapManager.createGrass(xlocation, ylocation, false); }
            this.xlocation = x;
            this.ylocation = y;
            mapManager.map[xlocation, ylocation] = this;
            texture = shapeToDraw;
        }
    }
}
