//=================================================
//This script was created by Gizmo's Uo Quest Maker
//This script was created on 1/10/2013 12:13:59 AM
//=================================================
using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
	public class GuardJedrek : MondainQuester
	{
        //This npc is used in the DestroyObjectObjective Example Quest
        //Quest Item: BreakableCrate
        //Quest: Endthesupplyline
        public override bool DisallowAllMoves{ get{ return true; } }

		public override Type[] Quests
		{
			get{ return new Type[]
			{
				typeof( Endthesupplyline )
			};}
		}

		[Constructable]
		public GuardJedrek() : base("Guard Jedrek", "")
		{
			new Horse().Rider = this;
			InitBody();
		}
		public GuardJedrek(Serial serial) : base(serial)
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
			Female = false;
			Race = Race.Human;
			base.InitBody();
		}
		public override void InitOutfit()
		{
			AddItem( new PlateHelm() );
			AddItem( new PlateChest() );
			AddItem( new PlateGorget() );
			AddItem( new PlateArms() );
			AddItem( new PlateGloves() );
			AddItem( new PlateLegs() );
			AddItem( new Cloak() );
			AddItem( new VikingSword() );
			AddItem( new BronzeShield() );
			AddItem( new Boots() );
		}
	}
}
