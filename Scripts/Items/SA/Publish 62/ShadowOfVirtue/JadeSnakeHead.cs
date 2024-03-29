﻿using System;
using Server;

namespace Server.Items
{
	public class JadeSnakeHead : BaseTalisman
	{
		public override int LabelNumber { get { return 1115647; } } // Jade Snake Head

        //public override int BasePoisonResistance { get { return 3; } }

		[Constructable]
		public JadeSnakeHead()
			: base( 0x2F59 )
		{
			Hue = 0x48A;

			Weight = 1.0;
			//AbsorptionAttributes.PoisonEater = 5; // TODO: enable eaters in talismans
			Attributes.RegenStam = 2;
			Attributes.LowerManaCost = 5;
			//Resistances.Poison = 3;
		}

		public JadeSnakeHead( Serial serial )
			: base( serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}