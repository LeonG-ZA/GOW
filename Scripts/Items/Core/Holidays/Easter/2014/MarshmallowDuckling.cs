using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Network;
using Server.HolidayConfiguration;
using Server.Misc;

namespace Server.Items
{
	[FlipableAttribute(39333, 39334)]
	public class MarshmallowDuckling : BaseFood
	{
		public override int LabelNumber { get { return 1123357; } } // Marshmallow Duckling

		private int m_Year;

		[CommandProperty(AccessLevel.Developer)]
		public int Year
		{
			get { return m_Year; }
			set { m_Year = value; InvalidateProperties(); }
		}

		[Constructable]
		public MarshmallowDuckling( ) : base(Utility.RandomMinMax(0, 1) == 0 ? 39333 : 39334)
		{
			Hue = Utility.RandomList( 44, 55, 77, 88, 111, 131 ); //Edit / Add your colors here
			Weight = 1;
			Stackable = false;

			if (m_Year == 0)
				m_Year = HolidayConfig.m_CurrentYear;
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (m_Year == 0)
				m_Year = HolidayConfig.m_CurrentYear;
			list.Add("Easter {0}", m_Year);
		}

		public MarshmallowDuckling(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)1); // version
			writer.Write(m_Year);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			if (version > 0)
				m_Year = reader.ReadInt();
		}
	}
}
