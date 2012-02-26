using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CowShooter
{
    class NewVillagerStoreItem : StoreItem
    {
        VillagerManager villagerManager;
        public NewVillagerStoreItem(VillagerManager villagerManager)
            :base(10)
        {
            this.villagerManager = villagerManager;
        }

        public override bool onBuy()
        {
            return villagerManager.CreateVillager();
        }
    }
}
