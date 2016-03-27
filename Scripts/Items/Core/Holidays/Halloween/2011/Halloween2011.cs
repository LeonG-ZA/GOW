using System;
using Server;
using Server.Items;

namespace Server.Misc
{
	public class Halloween2011 : GiftGiver
	{
		public static void Initialize()
		{
			GiftGiving.Register( new Halloween2011() );
		}

		public override DateTime Start{ get{ return new DateTime( 2011, 10, 29 ); } }
		public override DateTime Finish{ get{ return new DateTime( 2011, 11, 4 ); } }

		public override void GiveGift( Mobile mob )
		{
			TrickOrTreatBag2011 bag = new TrickOrTreatBag2011();
			
			switch ( GiveGift( mob, bag ) )
			{
				case GiftResult.Backpack:
					mob.SendMessage( 0x482, "Time to Trick or Treat! A bag has been placed in your backpack." );
					break;
				case GiftResult.BankBox:
					mob.SendMessage( 0x482, "Time to Trick or Treat! A bag has been placed in your bank box." );
					break;
			}
			GiftBoxOctogon box = new GiftBoxOctogon();
			box.Name = "Halloween 2011";
			box.Hue = Utility.RandomList( 43, 1175 );
			
			box.DropItem( new ShadowToken2011() );
			
			LargeDyingPlant2 plant1 = new LargeDyingPlant2();
			plant1.Name = "A large dying house plant killed by " + mob.Name;
			box.DropItem( plant1 );
			
			DyingPlant2 plant2 = new DyingPlant2();
			plant2.Name = "A dying house plant killed by " + mob.Name;
			box.DropItem( plant2 );
			
			CarvedPumpkin2 pump = new CarvedPumpkin2();
			pump.Name = "A spookie pumpkin carved by " + mob.Name;
			box.DropItem( pump );
			
			CarvedPumpkin pump1 = new CarvedPumpkin();
			pump1.Name = "A spookie pumpkin carved by " + mob.Name;
			box.DropItem( pump1 );
			
			switch ( GiveGift( mob, box ) )
			{
				case GiftResult.Backpack:
					mob.SendMessage( 0x482, "Happy Halloween from DS and Fraz GOW! A Halloween Gift Box has been placed in your backpack." );
					break;
				case GiftResult.BankBox:
                    mob.SendMessage(0x482, "Happy Halloween from DS and Fraz GOW! A Halloween Gift Box has been placed in your bank box.");
					break;
			}
			
		}
	}
}
