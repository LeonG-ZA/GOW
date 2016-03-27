using System;
using Server;

namespace Server.Items
{
	public class Planesword : Longsword
	{
		[Constructable]
		public Planesword()
		{
			Hue = 0x47E;
			Attributes.SpellChanneling = 1;
			WeaponAttributes.HitLightning = 10;
		}

        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            phys = 50; fire = 0; cold = 0; nrgy = 50; chaos = 0; direct = 0;
            pois = 0;
        }

		public Planesword( Serial serial ) : base( serial )
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
		}
	}
}