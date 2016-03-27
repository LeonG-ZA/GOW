/*
 * Created by SharpDevelop.
 * User: Sharon
 * Date: 2/2/2006
 * Time: 8:19 AM
 * 
 * Ostard Eggs
 */
using System;
using Server;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public  class DaemonSpawn : Item
	{
	
		[Constructable]
		public DaemonSpawn() : base( 0x4695 )
		{
			Weight = 1.0;
			Name = "a pumpkin";		
			Movable = false;
			
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060662, "Carved By\tShazzy and Game Ova" );
		}

		public DaemonSpawn( Serial serial ) : base ( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 1 ) )
			{
				DaemonPumpkin dp = new DaemonPumpkin();
				dp.MoveToWorld( from.Location, from.Map );						
				this.Delete();
				from.SendMessage( "A killer pumpkin appears!" ); 
			}
			else
			{
				from.LocalOverheadMessage( MessageType.Regular, 906, 1019045 ); // I can't reach that.
			}
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

