using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CowShooter
{
    class NewVillagerStoreItem : StoreItem
    {
        public NewVillagerStoreItem(VillagerManager villagerManager)
            :base(10)
        {

        }

        public override bool onBuy()
        {
            return true;   
        }
    }
}
