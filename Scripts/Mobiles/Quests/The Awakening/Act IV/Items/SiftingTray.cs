using System;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Engines.Harvest;
using Server.Mobiles;
using Server.Regions;

namespace Server.Items
{
    [FlipableAttribute(0x1507, 0x1505)]
    public class SiftingTray : BaseHarvestTool, IUsesRemaining
	{
        public override HarvestSystem HarvestSystem { get { return Siftingharvest.System; } }

		[Constructable]
		public SiftingTray() : this( 50 )
		{
		}

		[Constructable]
        public SiftingTray(int uses)
            : base(0x1507)
        {
            UsesRemaining = uses;
            ShowUsesRemaining = true;
            Hue = 2500;
        }

        public override int LabelNumber { get { return 1153844; } }// A Sifting Tray

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Region.IsPartOf(typeof(Regions.SiftingTrayRegion)))
            {
                if (IsChildOf(from.Backpack) || Parent == from)
                    HarvestSystem.BeginHarvesting(from, this);
                else
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }  
            else
                from.SendLocalizedMessage(1152489);
        }
        public SiftingTray(Serial serial)
            : base(serial)
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
			ShowUsesRemaining = true;
		}
	}
}