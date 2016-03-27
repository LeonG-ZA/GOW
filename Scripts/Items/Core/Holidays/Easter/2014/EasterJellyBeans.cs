using System;
using Server.Items;
using Server.Network;
using Server.HolidayConfiguration;

namespace Server.Items
{
	public class EasterJellyBeans : BaseFood
	{
		public override int LabelNumber 
		{ 
			get 
			{ 
				if ( Hue == 1140 )
					return 1154715; //Chocolate Jellybeans
				else
					return 1096932; // jellybeans
			} 
		}

		public override bool DisplayWeight { get { return false; } }

		private int m_Year;

		[CommandProperty( AccessLevel.Developer )]
		public int Year
		{
			get{ return m_Year; }
			set{ m_Year = value; InvalidateProperties();}
		}

		[Constructable]
		public EasterJellyBeans() : base( 0x468C )
		{
			Weight = 1.0;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Year == 0 )
                m_Year = HolidayConfig.m_CurrentYear;
			list.Add( "Easter: {0}", m_Year );
		}

		public EasterJellyBeans( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
