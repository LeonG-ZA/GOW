using System;
using Server;
using Server.Items;
using Server.HolidayConfiguration;

namespace Server.Misc
{
	public class Easter : GiftGiver
	{
		public static void Initialize()
		{
			GiftGiving.Register( new Easter() );
		}

        public override DateTime Start { get { return new DateTime(HolidayConfig.m_CurrentYear, HolidayConfig.EasterAprilStart, HolidayConfig.EasterDayStart); } }
        public override DateTime Finish { get { return new DateTime(HolidayConfig.m_CurrentYear, HolidayConfig.EasterAprilEnd, HolidayConfig.EasterDayEnd); } }

		public override void GiveGift( Mobile mob )
		{
			EasterBasket basket = new EasterBasket();
			basket.Hue = Utility.RandomList( 0x36, 0x17, 0x120 ) ;

			basket.DropItem( new RaisedEasterBunny() );
			basket.DropItem( new EasterJellyBeans() );

			switch ( GiveGift( mob, basket ) )
			{
				case GiftResult.Backpack:
					mob.SendLocalizedMessage( 1154717 );//Happy Easter! We have placed a gift for you in your backpack.
					break;
				case GiftResult.BankBox:
					mob.SendLocalizedMessage( 1154718 );//Happy Easter! We have placed a gift for you in your bank box.
					break;
			}
		}
	}
}
