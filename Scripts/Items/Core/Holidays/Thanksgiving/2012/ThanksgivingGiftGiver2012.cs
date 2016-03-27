using System;
using Server.Items;
using Server.HolidayConfiguration;

namespace Server.Misc
{
    public class ThanksgivingGiftGiver2012 : GiftGiver
    {
        public override DateTime Start
        {
            get
            {
                return new DateTime(HolidayConfig.m_CurrentYear, HolidayConfig.ThanksgivingNovemberStart, HolidayConfig.ThanksgivingDay);
            }
        }
        public override DateTime Finish
        {
            get
            {
                return new DateTime(HolidayConfig.m_CurrentYear, HolidayConfig.ThanksgivingNovemberEnd, HolidayConfig.ThanksgivingDayEnd);
            }
        }
        public static void Initialize()
        {
            GiftGiving.Register(new ThanksgivingGiftGiver2012());
        }

        public override void GiveGift(Mobile mob)
        {
            GiftBox box = new GiftBox();

            box.DropItem(new MistletoeDeed());
            box.DropItem(new PileOfGlacialSnow());
            box.DropItem(new LightOfTheWinterSolstice());

            int random = Utility.Random(100);

            if (random < 60)
                box.DropItem(new DecorativeTopiary());
            else if (random < 84)
                box.DropItem(new FestiveCactus());
            else
                box.DropItem(new SnowyTree());

            switch ( this.GiveGift(mob, box) )
            {
                case GiftResult.Backpack:
                    mob.SendMessage(0x482, "Happy Holidays from the team!  Gift items have been placed in your backpack.");
                    break;
                case GiftResult.BankBox:
                    mob.SendMessage(0x482, "Happy Holidays from the team!  Gift items have been placed in your bank box.");
                    break;
            }
        }
    }
}