using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Gumps
{
	public class ConfirmSnowSculptureGump : Gump
	{
		private SnowSculptureDeed m_Deed;
		private Type[] m_Selected;

		private enum Buttons
		{
			Cancel,
			Okay
		}

		public ConfirmSnowSculptureGump( SnowSculptureDeed deed, Type[] selected ) : base( 60, 36 )
		{
			
			m_Deed = deed;
			m_Selected = selected;

			AddPage( 0 );

			AddBackground( 0, 0, 291, 99, 0x13BE );
			AddImageTiled( 5, 6, 280, 20, 0xA40 );
			AddHtmlLocalized( 9, 8, 280, 20, 501544, 0x7FFF, false, false ); // Accepting item.  
			AddImageTiled( 5, 31, 280, 40, 0xA40 );
			AddHtmlLocalized( 9, 35, 272, 40, 0x7FFF, false, false );
			AddButton( 180, 73, 0xFB7, 0xFB8, (int) Buttons.Okay, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 215, 75, 100, 20, 1011036, 0x7FFF, false, false ); // OKAY
			AddButton( 5, 73, 0xFB1, 0xFB2, (int) Buttons.Cancel, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 40, 75, 100, 20, 1060051, 0x7FFF, false, false ); // CANCEL
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_Deed == null || m_Deed.Deleted )
				return;

			switch ( info.ButtonID )
			{
				case (int) Buttons.Okay:
					
					Item item = null;

					foreach ( Type type in m_Selected )
					{
						try
						{
							item = Activator.CreateInstance( type ) as Item;
						}
						catch ( Exception ex )
						{
							Console.WriteLine( ex.Message );
							Console.WriteLine( ex.StackTrace );
						}				

						if ( item != null )
						{
							m_Deed.Delete();
							sender.Mobile.AddToBackpack( item );
						}
					}

					break;
				case (int) Buttons.Cancel:
					sender.Mobile.SendGump( new SnowSculptureGump( m_Deed ) );
					break;
			}
		}
	}
}
