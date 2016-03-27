using System;
using System.Collections.Generic;
using Server.Items;
using Server.Multis.Deeds;
using Server.T2AConfiguration;

namespace Server.Mobiles
{
    public class SBArchitect : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBArchitect()
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
                this.Add(new GenericBuyInfo("1041280", typeof(InteriorDecorator), 10001, 20, 0xFC1, 0));

                if (T2AConfig.T2AArchitectEnabled)
                {
                    this.Add(new GenericBuyInfo("1041280", typeof(StonePlasterHouseDeed), 10001, 20, 0xFC1, 0));
                    this.Add(new GenericBuyInfo("1041280", typeof(FieldStoneHouseDeed), 10001, 20, 0xFC1, 0));
                    this.Add(new GenericBuyInfo("1041280", typeof(SmallBrickHouseDeed), 10001, 20, 0xFC1, 0));
                    this.Add(new GenericBuyInfo("1041280", typeof(WoodHouseDeed), 10001, 20, 0xFC1, 0));
                    this.Add(new GenericBuyInfo("1041280", typeof(WoodPlasterHouseDeed), 10001, 20, 0xFC1, 0));
                    this.Add(new GenericBuyInfo("1041280", typeof(ThatchedRoofCottageDeed), 10001, 20, 0xFC1, 0));
                    this.Add(new GenericBuyInfo("1041280", typeof(BrickHouseDeed), 10001, 20, 0xFC1, 0));
                    this.Add(new GenericBuyInfo("1041280", typeof(TwoStoryWoodPlasterHouseDeed), 10001, 20, 0xFC1, 0));
                    this.Add(new GenericBuyInfo("1041280", typeof(TwoStoryStonePlasterHouseDeed), 10001, 20, 0xFC1, 0));
                    this.Add(new GenericBuyInfo("1041280", typeof(TowerDeed), 10001, 20, 0xFC1, 0));
                    this.Add(new GenericBuyInfo("1041280", typeof(KeepDeed), 10001, 20, 0xFC1, 0));
                    this.Add(new GenericBuyInfo("1041280", typeof(CastleDeed), 10001, 20, 0xFC1, 0));
                }
                else
                    if (Core.AOS)
                        this.Add(new GenericBuyInfo("1060651", typeof(HousePlacementTool), 627, 20, 0x14F6, 0));

            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                this.Add(typeof(InteriorDecorator), 5000);

                if (Core.AOS)
                    this.Add(typeof(HousePlacementTool), 301);
            }
        }
    }
}