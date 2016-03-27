using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class SBCarpets : SBInfo
	{
		private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBCarpets()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 127, 500, 0x56B8, 0, new object[]{ 0x56B8 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 127, 500, 0x56B9, 0, new object[]{ 0x56B9 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 127, 500, 0x56BA, 0, new object[]{ 0x56BA } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 127, 500, 0x56BB, 0, new object[]{ 0x56BB } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 127, 500, 0x56BC, 0, new object[]{ 0x56BC } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 127, 500, 0x56BD, 0, new object[]{ 0x56BD } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 127, 500, 0x56BE, 0, new object[]{ 0x56BE } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 127, 500, 0x56BF, 0, new object[]{ 0x56BF } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 127, 500, 0x56C0, 0, new object[]{ 0x56C0 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 127, 500, 0x56C1, 0, new object[]{ 0x56C1 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 127, 500, 0x56C2, 0, new object[]{ 0x56C2 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 127, 500, 0x56C3, 0, new object[]{ 0x56C3 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 127, 500, 0x56C4, 0, new object[]{ 0x56C4 } ) );
				
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 137, 500, 0x56C5, 0, new object[]{ 0x56C5 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 137, 500, 0x56C6, 0, new object[]{ 0x56C6 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 137, 500, 0x56C7, 0, new object[]{ 0x56C7 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 137, 500, 0x56C8, 0, new object[]{ 0x56C8 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 137, 500, 0x56C9, 0, new object[]{ 0x56C9 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 137, 500, 0x56CA, 0, new object[]{ 0x56CA } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 137, 500, 0x56CB, 0, new object[]{ 0x56CB } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 137, 500, 0x56CC, 0, new object[]{ 0x56CC } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 137, 500, 0x56CD, 0, new object[]{ 0x56CD } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 137, 500, 0x56CE, 0, new object[]{ 0x56CE } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 137, 500, 0x56CF, 0, new object[]{ 0x56CF } ) );
				
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 262, 500, 0x56D0, 0, new object[]{ 0x56D0 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 262, 500, 0x56D1, 0, new object[]{ 0x56D1 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 262, 500, 0x56D2, 0, new object[]{ 0x56D2 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 262, 500, 0x56D3, 0, new object[]{ 0x56D3 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 262, 500, 0x56D4, 0, new object[]{ 0x56D4 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 262, 500, 0x56D5, 0, new object[]{ 0x56D5 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 262, 500, 0x56D6, 0, new object[]{ 0x56D6 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 262, 500, 0x56D7, 0, new object[]{ 0x56D7 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 262, 500, 0x56D8, 0, new object[]{ 0x56D8 } ) );
				
			
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 377, 500, 0x56D9, 0, new object[]{ 0x56D9 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 377, 500, 0x56DA, 0, new object[]{ 0x56DA } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 377, 500, 0x56DB, 0, new object[]{ 0x56DB } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 377, 500, 0x56DC, 0, new object[]{ 0x56DC } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 377, 500, 0x56DD, 0, new object[]{ 0x56DD } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 377, 500, 0x56DE, 0, new object[]{ 0x56DE } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 377, 500, 0x56DF, 0, new object[]{ 0x56DF } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 377, 500, 0x56E0, 0, new object[]{ 0x56E0 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 377, 500, 0x56E1, 0, new object[]{ 0x56E1 } ) );
				
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 387, 500, 0x56E2, 0, new object[]{ 0x56E2 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 387, 500, 0x56E3, 0, new object[]{ 0x56E3 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 387, 500, 0x56E4, 0, new object[]{ 0x56E4 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 387, 500, 0x56E5, 0, new object[]{ 0x56E5 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 387, 500, 0x56E6, 0, new object[]{ 0x56E6 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 387, 500, 0x56E7, 0, new object[]{ 0x56E7 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 387, 500, 0x56E8, 0, new object[]{ 0x56E8 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 387, 500, 0x56E9, 0, new object[]{ 0x56E9 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 387, 500, 0x56EA, 0, new object[]{ 0x56EA } ) );
				
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 627, 500, 0x56EB, 0, new object[]{ 0x56EB } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 627, 500, 0x56EC, 0, new object[]{ 0x56EC } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 627, 500, 0x56ED, 0, new object[]{ 0x56ED } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 627, 500, 0x56EE, 0, new object[]{ 0x56EE } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 627, 500, 0x56EF, 0, new object[]{ 0x56EF } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 627, 500, 0x56F0, 0, new object[]{ 0x56F0 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 627, 500, 0x56F1, 0, new object[]{ 0x56F1 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 627, 500, 0x56F2, 0, new object[]{ 0x56F2 } ) );
				Add( new GenericBuyInfo( typeof( DecorativeCarpet ), 627, 500, 0x56F3, 0, new object[]{ 0x56F3 } ) );
				
				
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{

				Add( typeof( DecorativeCarpet ), 25 );

			}
		}
	}
}
