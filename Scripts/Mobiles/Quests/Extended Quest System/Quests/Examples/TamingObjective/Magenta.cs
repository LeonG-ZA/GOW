//=================================================
//This script was created by Gizmo's Uo Quest Maker
//This script was created on 1/11/2013 2:26:28 AM
//=================================================
using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
	public class Magenta : MondainQuester
	{
        public override bool DisallowAllMoves{ get{ return true; } }

		public override Type[] Quests
		{
			get{ return new Type[]
			{
				typeof( BearCares )
			};}
		}

		[Constructable]
		public Magenta() : base("Magenta", "The Forest Tender")
		{
			new FrenziedOstard().Rider = this;
			InitBody();
		}
		public Magenta(Serial serial) : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void InitBody()
		{
			InitStats(100, 100, 100);
			Female = true;
			Race = Race.Human;
			base.InitBody();
		}
		public override void InitOutfit()
		{
			AddItem( new TallStrawHat() );
			AddItem( new StuddedChest() );
			AddItem( new SilverNecklace() );
			AddItem( new StuddedArms() );
			AddItem( new StuddedGloves() );
			AddItem( new StuddedLegs() );
			AddItem( new Cloak() );
			AddItem( new SkinningKnife() );
			AddItem( new Lantern() );
			AddItem( new Boots() );
		}
	}
}
