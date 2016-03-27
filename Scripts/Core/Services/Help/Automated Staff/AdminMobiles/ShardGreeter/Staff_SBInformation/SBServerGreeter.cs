using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Items.Staff;
using Server.ContextMenus;
using Server.Multis;
using Server.Spells;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Accounting;
using Server.Misc;
using Server.Network;


namespace Server.Mobiles
{
    public class SBServerGreeter : SBInfo
    {
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

        public SBServerGreeter()
        {
        }

        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(RobeOfEntitlement), 0, 20, 0x2683, 0));
                Add(new GenericBuyInfo(typeof(CommUnit), 0, 20, 0x1F07, 0));
                Add(new GenericBuyInfo(typeof(StaffOfAnnulment), 0, 20, 0x13F8, 0));
                Add(new GenericBuyInfo(typeof(LensesOfResist), 0, 20, 0x2FB8, 0));
                Add(new GenericBuyInfo(typeof(CollarOfVisibility), 0, 20, 0x1F08, 0));
                Add(new GenericBuyInfo(typeof(RingOfReduction), 0, 20, 0x1F09, 0));
                Add(new GenericBuyInfo(typeof(BraceletOfEthics), 0, 20, 0x1F06, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(RobeOfEntitlement), 0);
                Add(typeof(CommUnit), 0);
                Add(typeof(StaffOfAnnulment), 0);
                Add(typeof(LensesOfResist), 0);
                Add(typeof(CollarOfVisibility), 0);
                Add(typeof(RingOfReduction), 0);
                Add(typeof(BraceletOfEthics), 0);
            }
        }
    }
}