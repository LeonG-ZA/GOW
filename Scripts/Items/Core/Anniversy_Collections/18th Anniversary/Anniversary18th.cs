using System;
using Server;
using Server.Items;

namespace Server.Misc
{
    public class Anniversary18thGiftGiver : GiftGiver
	{
		public static void Initialize()
		{
            GiftGiving.Register(new Anniversary18thGiftGiver());
		}

		public override DateTime Start { get { return new DateTime( 2015, 10, 10); } }
		public override DateTime Finish { get { return new DateTime( 2016, 2, 23); } }

		public override void GiveGift( Mobile mob )
		{
            Container anniversybag = new AnniversyBag18th();
            anniversybag.DropItem(new SunDial());
            anniversybag.DropItem(new DecoratedCommemorativePlate());
            //anniversybag.DropItem(new RandomRecipe18th());
            //anniversybag.DropItem(new AnniversyCard18th());

            switch (GiveGift(mob, anniversybag))
			{
				case GiftResult.Backpack:
					mob.SendLocalizedMessage( 1156142 ); // Happy 18th Anniversary! We have placed a gift for you in your backpack.
					break;
				case GiftResult.BankBox:
					mob.SendLocalizedMessage( 1156143 ); // Happy 18th Anniversary! We have placed a gift for you in your bank box. 
					break;
			}
		}
	}
}