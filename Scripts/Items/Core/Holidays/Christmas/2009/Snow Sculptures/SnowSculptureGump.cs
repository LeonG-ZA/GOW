using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Gumps
{
    public class SnowSculptureGump : Gump
	{
        private SnowSculptureDeed m_Deed;

		public SnowSculptureGump( SnowSculptureDeed deed ) : base( 150, 50 )
		{
			m_Deed = deed;

				AddBackground( 0, 0, 350, 500, 0xA28 );

				AddItem( 90, 52, 0x4578 );
				AddButton( 70, 35, 0x868, 0x869, 1, GumpButtonType.Reply, 0 ); //seahorse

				AddItem( 220, 52, 0x456E );
				AddButton( 185, 35, 0x868, 0x869, 2, GumpButtonType.Reply, 0 ); // pegasus

				AddItem( 90, 243, 0x457A );
				AddButton( 70, 226, 0x868, 0x869, 3, GumpButtonType.Reply, 0 ); // mermaid

				AddItem( 220, 243, 0x457C );
				AddButton( 185, 226, 0x868, 0x869, 4, GumpButtonType.Reply, 0 ); // gryphon

			}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( m_Deed.Deleted || info.ButtonID == 0 || info.ButtonID > 4 )
					return;

			List<Type> types = new List<Type>();
			
			switch ( info.ButtonID )
			{
				case 1: types.Add(typeof(SeahorseSnowSculpture)); break;//Seahorse 
				case 2: types.Add(typeof(PegasusSnowSculpture)); break;//pegasus 
				case 3: types.Add(typeof(MermaidSnowSculpture)); break;//mermaid
                case 4: types.Add(typeof(GryphonSnowSculpture)); break;//gryphon 

			}

			if ( types.Count > 0 )
			{
                sender.Mobile.CloseGump(typeof(ConfirmSnowSculptureGump));
                sender.Mobile.SendGump(new ConfirmSnowSculptureGump(m_Deed, types.ToArray() ));
			}
			else
				sender.Mobile.SendLocalizedMessage( 501311 ); // This option is currently disabled, while we evaluate it for game balance.
		}
	}
}
