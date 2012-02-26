using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    public class WallManager
    {
        const int floorLevel = 400;
        const int startingNumberOfBlocks = 5;
        public const float wallLocation = 600;

        public float wallHeight = 0;

        WallBlock[] blocks;
        Texture2D wallTexture;

        public WallManager(Texture2D wallTexture)
        {
            this.wallTexture = wallTexture;
            blocks = new WallBlock[startingNumberOfBlocks];
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = new WallBlock(wallTexture, createPositionVector(i));
                wallHeight += wallTexture.Height;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (WallBlock block in blocks)
            {
                block.Draw(spriteBatch);
            }
        }
        private Vector2 createPositionVector(int i)
        {
            return new Vector2(wallLocation, floorLevel - wallTexture.Height * (i + 1));
        }
        public void addBlock()
        {
            if (blocks.Length <= 10)
            {
                WallBlock[] temp = new WallBlock[blocks.Length + 1];
                for (int i = 0; i < blocks.Length; i++)
                {
                    temp[i] = blocks[i];
                }
                temp[blocks.Length] = new WallBlock(wallTexture, createPositionVector(blocks.Length));
                wallHeight += wallTexture.Height;
                blocks = temp;
            }
        }
        public void removeBlock()
        {
            WallBlock[] temp = new WallBlock[blocks.Length - 1];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = blocks[i];
            }
            wallHeight -= wallTexture.Height;
            blocks = temp;
        }
    }
}
