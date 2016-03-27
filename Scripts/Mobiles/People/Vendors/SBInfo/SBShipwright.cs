using System;
using System.Collections.Generic;
using Server.Multis;

namespace Server.Mobiles
{
    public class SBShipwright : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBShipwright()
        {
        }

        public override IShopSellInfo SellInfo
        {
            get
            {
                return this.m_SellInfo;
            }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get
            {
                return this.m_BuyInfo;
            }
        }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                if (Core.HS)
                {
                    Add(new GenericBuyInfo("1041205", typeof(NewSmallBoatDeed), 10177, 20, 0x14F2, 0));
                    Add(new GenericBuyInfo("1041205", typeof(NewSmallDragonBoatDeed), 15157, 20, 0x14F2, 0));
                    Add(new GenericBuyInfo("1041205", typeof(NewMediumBoatDeed), 17157, 20, 0x14F2, 0));
                    Add(new GenericBuyInfo("1041205", typeof(NewMediumDragonBoatDeed), 19257, 20, 0x14F2, 0));
                    Add(new GenericBuyInfo("1041205", typeof(NewLargeBoatDeed), 29257, 20, 0x14F2, 0));
                    Add(new GenericBuyInfo("1041205", typeof(NewLargeDragonBoatDeed), 35257, 20, 0x14F2, 0));
                }
                else
                {
                    Add(new GenericBuyInfo("1041205", typeof(SmallBoatDeed), 10177, 20, 0x14F2, 0));
                    Add(new GenericBuyInfo("1041206", typeof(SmallDragonBoatDeed), 10177, 20, 0x14F2, 0));
                    Add(new GenericBuyInfo("1041207", typeof(MediumBoatDeed), 11552, 20, 0x14F2, 0));
                    Add(new GenericBuyInfo("1041208", typeof(MediumDragonBoatDeed), 11552, 20, 0x14F2, 0));
                    Add(new GenericBuyInfo("1041209", typeof(LargeBoatDeed), 12927, 20, 0x14F2, 0));
                    Add(new GenericBuyInfo("1041210", typeof(LargeDragonBoatDeed), 12927, 20, 0x14F2, 0));
                }
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                //You technically CAN sell them back, *BUT* the vendors do not carry enough money to buy with
            }
        }
    }
}