using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CowShooter
{
    class MeatStore
    {
        int meatCount;
        bool displayingStoreFront;

        StoreItem[,] storeItems;
        Dictionary<Type, Texture2D> shopItemTextures;
        SpriteFont font;
        Texture2D storeBackground;
        int nextPos;

        public MeatStore(SpriteFont font, Texture2D storeBackground)
        {
            meatCount = 0;
            storeItems = new StoreItem[4, 2];
            nextPos = 0;
            shopItemTextures = new Dictionary<Type, Texture2D>();
            this.font = font;
            this.storeBackground = storeBackground;
        }

        public void addMeat(int count)
        {
            meatCount += count;
        }

        public void spendMeat(StoreItem item)
        {
            if (meatCount >= item.getCost())
            {
                if (item.onBuy())
                {
                    meatCount -= item.getCost();
                }
            }
        }

        public int getMeatCount()
        {
            return meatCount;
        }

        public void Reset()
        {
            meatCount = 0;
        }

        public void toggleStore()
        {
            displayingStoreFront = !displayingStoreFront;
        }

        public void Update(MouseState currentMouseState, MouseState lastMouseState)
        {
            if (displayingStoreFront)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    for (int i = 0; i < nextPos; ++i)
                    {
                        Point nextLocation = getArrayPosition(i);
                        if (storeItems[nextLocation.X, nextLocation.Y].CheckClick(new Point(currentMouseState.X, currentMouseState.Y)))
                        {
                            spendMeat(storeItems[nextLocation.X, nextLocation.Y]);
                            break;
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (displayingStoreFront)
            {
                spriteBatch.Draw(storeBackground, new Rectangle(100,50, 546, 344), Color.White);
                for (int i = 0; i < nextPos; ++i)
                {
                    Point nextLocation = getArrayPosition(i);
                    storeItems[nextLocation.X, nextLocation.Y].Draw(spriteBatch);
                }
            }

            spriteBatch.DrawString(font, "Meat: " + meatCount, new Vector2(50, 50), Color.White);
        }

        Point getArrayPosition(int location)
        {
            if (location < 8)
            {
                return new Point(location % 4, (int)((float)location / 4.0f));
            }
            return new Point(-1, -1);
        }

        public void addTexture(Type itemType, Texture2D texture)
        {
            shopItemTextures.Add(itemType, texture);
        }

        public Texture2D getTexture(Type itemType)
        {
            return shopItemTextures[itemType];
        }

        public void addStoreItem(StoreItem storeItem) 
        {
            Point nextLocation = getArrayPosition(nextPos);
            storeItems[nextLocation.X, nextLocation.Y] = storeItem;
            storeItem.SetClickRectangle(new Rectangle(110 + (nextLocation.X * 133), 60 +  (nextLocation.Y * 128), 128, 128));
            storeItem.SetMeatStore(this);
            storeItem.SetTexture(); 
            nextPos++;
        }
    }
}
