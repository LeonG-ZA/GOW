using System;
using Server.Items;

namespace Server.Items
{
	public class VEPlateGloves : PlateGloves
	{
		public override int LabelNumber{ get{ return 1150237; } } //Gauntlets of Virtuous Epiphany
		
		public override SetItem SetID{ get{ return SetItem.Epiphany; } }
		public override int Pieces{ get{ return 6; } }
		
		public override int BasePhysicalResistance{ get{ return 7; } }
		public override int BaseFireResistance{ get{ return 7; } }
		public override int BaseColdResistance{ get{ return 7; } }
		public override int BasePoisonResistance{ get{ return 7; } }
		public override int BaseEnergyResistance{ get{ return 7; } }
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		[Constructable]
		public VEPlateGloves() : base()
		{
			SetHue = 0xA34;		
			
			Attributes.BonusMana = 1;
			ArmorAttributes.MageArmor = 1;
			
			SetAttributes.BonusHits = 1;
			SetAttributes.RegenHits = 1;
			SetAttributes.RegenMana = 6;			
			SetAttributes.DefendChance = 20;
			
			SetPhysicalBonus = 28;
			SetFireBonus = 28;
			SetColdBonus = 28;
			SetPoisonBonus = 28;
			SetEnergyBonus = 28;
		}

		public VEPlateGloves( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 ); // version
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
		}
	}
}