using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CowShooter
{
    abstract class StoreItem
    {
        Texture2D texture;
        Rectangle clickRectangle;
        int meatCost;

        MeatStore meatStore;

        public StoreItem(int meatCost)
        {
            this.meatCost = meatCost;
        }

        public void SetClickRectangle(Rectangle clickRectangle)
        {
            this.clickRectangle = clickRectangle;
        }

        public void SetMeatStore(MeatStore meatStore)
        {
            this.meatStore = meatStore;
        }

        public void SetTexture()
        {
            texture = texture = meatStore.getTexture(GetType());
        }

        public bool CheckClick(Point mousePoisition)
        {
            if (clickRectangle.Contains(mousePoisition))
            {
                return true;
            }
            return false;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, clickRectangle, Color.White);
        }

        public int getCost()
        {
            return meatCost;
        }

        public abstract bool onBuy();
    }
}
