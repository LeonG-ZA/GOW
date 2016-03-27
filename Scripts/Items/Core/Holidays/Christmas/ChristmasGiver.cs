using System;
using Server;
using Server.Items;
using Server.HolidayConfiguration;

namespace Server.Misc
{
    public class Christmas : GiftGiver
	{
		public static void Initialize()
		{
			GiftGiving.Register( new Christmas() );
		}

        public override DateTime Start { get { return new DateTime(HolidayConfig.m_CurrentYear, HolidayConfig.ChristmasDecemberStart, HolidayConfig.ChristmasDay); } }
        public override DateTime Finish { get { return new DateTime(HolidayConfig.m_CurrentYear, HolidayConfig.ChristmasDecemberEnd, HolidayConfig.ChristmasDayEnd); } }

		public override void GiveGift( Mobile mob )
		{
            Bag bag = new Bag();

            bag.DropItem(new HappyHolidaysBag());
            bag.DropItem(new GiftBox2003());
            bag.DropItem(new GiftBox2004());
            bag.DropItem(new GiftBox2005());
            bag.DropItem(new HolidayTreeDeed());

            switch (GiveGift(mob, bag))
			{
				case GiftResult.Backpack:
                    mob.SendMessage(0x482, "Seasons Greetings from the team!  Gift items have been placed in your backpack.");
					break;
				case GiftResult.BankBox:
                    mob.SendMessage(0x482, "Seasons Greetings from the team!  Gift items have been placed in your bank box.");
					break;
			}
		}
	}
}
