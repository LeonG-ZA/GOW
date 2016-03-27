using System;
using Server;
using Server.Items;

namespace Server.Misc
{
	public class SpringDecor2015 : GiftGiver
	{
		public static void Initialize()
		{
            GiftGiving.Register(new SpringDecor2015());
		}

		public override DateTime Start{ get{ return new DateTime( 2015, 3, 20 ); } }
		public override DateTime Finish{ get{ return new DateTime( 2015, 3, 31 ); } }

		public override void GiveGift( Mobile mob )
		{
            TallRoundBasket basket = new TallRoundBasket();
            basket.Hue = Utility.RandomList(1193, 1196);

            basket.DropItem(new SpringToken());
            basket.DropItem(new SpringToken());
            basket.DropItem(new SpringToken());

            switch (GiveGift(mob, basket))
			{
				case GiftResult.Backpack:
					mob.SendMessage( 0x482, "Spring Is here!  Token items have been placed in your backpack." );
					break;
				case GiftResult.BankBox:
                    mob.SendMessage(0x482, "Spring Is here!  Token items have been placed in your bank box.");
					break;
			}
		}
	}
}