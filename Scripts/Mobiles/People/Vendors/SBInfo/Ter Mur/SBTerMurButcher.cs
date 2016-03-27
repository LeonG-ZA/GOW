using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBTerMurButcher : SBInfo
	{
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBTerMurButcher()
		{
		}

        public override IShopSellInfo SellInfo { get { return this.m_SellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return this.m_BuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( GargishDagger ), 20, 20, 0x902, 0 ) );
				Add( new GenericBuyInfo( typeof( ButcherKnife ), 15, 20, 0x13F6, 0 ) );
				Add( new GenericBuyInfo( typeof( RawRibs ), 7, 20, 0x9F1, 0 ) );
				Add( new GenericBuyInfo( typeof( RawLambLeg ), 5, 20, 0x1609, 0 ) );
				Add( new GenericBuyInfo( typeof( RawBird ), 3, 20, 0x9B9, 0 ) );
				Add( new GenericBuyInfo( typeof( RawChickenLeg ), 2, 20, 0x1607, 0 ) );
				Add( new GenericBuyInfo( typeof( Sausage ), 17, 20, 0x9C0, 0 ) );
				Add( new GenericBuyInfo( typeof( Ham ), 20, 20, 0x9C9, 0 ) );
                Add(new GenericBuyInfo(typeof(SliceOfBacon), 3, 20, 0x979, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				Add( typeof( RawRibs ), 3 );
				Add( typeof( RawLambLeg ), 2 );
				Add( typeof( RawChickenLeg ), 1 );
				Add( typeof( RawBird ), 1 );
                Add(typeof(SliceOfBacon), 1);
				Add( typeof( Sausage ), 8 );
				Add( typeof( Ham ), 10 );
				Add( typeof( GargishDagger ), 10 );
			}
		}
	}
}