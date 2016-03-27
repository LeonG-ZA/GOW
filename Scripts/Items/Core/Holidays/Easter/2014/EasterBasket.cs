using System;
using Server.Items;
using Server.Network;
using Server.HolidayConfiguration;

namespace Server.Items
{
	public class EasterBasket : BaseContainer
	{
		private int m_Year;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Year
		{
			get{ return m_Year; }
			set{ m_Year = value; InvalidateProperties();}
		}

		public override int DefaultGumpID{ get{ return 0x41; } }
		public override int DefaultDropSound{ get{ return 0x4F; } }

		public override Rectangle2D Bounds
		{
			get{ return new Rectangle2D( 35, 38, 110, 78 ); }
		}        
       			
		[Constructable]
		public EasterBasket() : this( Utility.RandomDyedHue() )
		{
		}		
		
		[Constructable]
		public EasterBasket( int hue ) : base( 0x990 )//Utility.RandomList( 0x36, 0x17, 0x120 ) )
		{
            Name = "Easter Basket";			
			Weight = 1.0;
			Hue = hue;
		}		

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Year == 0 )
				m_Year = HolidayConfig.m_CurrentYear;
			list.Add( "Easter: {0}", m_Year );
		}

		public EasterBasket( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
			writer.Write( m_Year );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			if ( version > 0 )
				m_Year = reader.ReadInt();
		}
	}
}
