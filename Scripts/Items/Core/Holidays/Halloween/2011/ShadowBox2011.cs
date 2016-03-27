using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ShadowBox2011 : GiftBoxRectangle
	{
		public override string DefaultName
		{
			get { return "Halloween 2011 Gift Box"; }
		}

		[Constructable]
		public ShadowBox2011() : this( 1 )
		{
			Movable = true;
			Hue = 1175;
		}

		[Constructable]
		public ShadowBox2011( int amount )
		{
			DropItem( new FireDemonStatueDeed() );
            DropItem(new SpikeColumnDeed());
			DropItem( new SpikePostDeed() );

            DropItem(new ShadowAltarDeed());
			
		    DropItem(new ShadowBannerDeed());

			switch ( Utility.Random( 3 ))
			{
                case 0: DropItem(new ObsidianPillarDeed()); break;
                case 1: DropItem(new ObsidianRockDeed()); break;
                case 2: DropItem(new ShadowPillarAddon()); break;
			}
                      	

		}
		
		public ShadowBox2011( Serial serial ) : base( serial )
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
	}
}
