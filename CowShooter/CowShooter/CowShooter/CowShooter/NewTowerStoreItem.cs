using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CowShooter
{
    class NewTowerStoreItem : StoreItem
    {
        WallManager wallManager;
        public NewTowerStoreItem(WallManager wallManager)
            : base(5)
        {
            this.wallManager = wallManager;
        }

        public override bool onBuy()
        {
            return wallManager.addBlock();
        }

    }
}
