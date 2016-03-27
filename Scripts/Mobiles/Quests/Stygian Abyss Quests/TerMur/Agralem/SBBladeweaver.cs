using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBBladeweaver : SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBBladeweaver()
		{
		}

        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
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
				Add( new GenericBuyInfo( typeof( SoulGlaive ), 71, 20, 0x406B, 0 ) );
				Add( new GenericBuyInfo( typeof( Cyclone ), 52, 20, 0x406C, 0 ) );
				Add( new GenericBuyInfo( typeof( Boomerang ), 27, 20, 0x4067, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( SoulGlaive ), 35 );
				Add( typeof( Cyclone ), 25 );
				Add( typeof( Boomerang ), 14 );
			}
		}
	}
}