using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBTerMurTinker : SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBTerMurTinker()
		{
		}

        public override IShopSellInfo SellInfo { get { return this.m_SellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return this.m_BuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( "Making Valuables With Basket Weaving", typeof( BasketWeavingBook ), 10625, 20, 0xFBE, 0 ) );
				// TODO (SA): Statuette Engraving Tool
				Add( new GenericBuyInfo( typeof( AudChar ), 33, 20, 0x403B, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
			}
		}
	}
}