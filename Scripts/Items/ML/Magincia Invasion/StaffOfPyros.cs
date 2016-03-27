using System;
using Server;

namespace Server.Items
{
	public class StaffOfPyros : BlackStaff
	{
		public override int LabelNumber{ get{ return 1079835; } } // Staff of Pyros
		public override int ArtifactRarity{ get{ return 12; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

        [Constructable]
        public StaffOfPyros()
        {
            Hue = 1161;
            Slayer = SlayerName.Silver;
            WeaponAttributes.MageWeapon = 30;
            Attributes.SpellChanneling = 1;
            Attributes.CastSpeed = 1;
            Attributes.WeaponDamage = 30;
        }

        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            phys = 0; 
            fire = 100; 
            cold = 0; 
            pois = 0;
            nrgy = 0;
            chaos = 0;
            direct = 0;
        }

		public StaffOfPyros( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( WeaponAttributes.MageWeapon == 0 )
				WeaponAttributes.MageWeapon = 30;

			if ( ItemID == 0xDF1 )
				ItemID = 0xDF0;
		}
	}
}