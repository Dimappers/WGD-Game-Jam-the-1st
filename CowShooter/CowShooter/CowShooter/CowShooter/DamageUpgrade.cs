using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CowShooter
{
    class DamageUpgrade : StoreItem
    {
        Catapult catapult;
        public DamageUpgrade(Catapult catapult)
            : base(3)
        {
            this.catapult = catapult;
        }

        public override bool onBuy()
        {
            catapult.upgradeDamage(1);
            return true;
        }
    }
}
