using System;
using Server;
using Server.Prompts;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Multis;

namespace Server.Multis
{
	public class NewRenameBoatPrompt : Prompt
	{
        // What dost thou wish to name thy ship?
        public override int MessageCliloc { get { return 502580; } }

		private BaseShip m_Boat;

        public NewRenameBoatPrompt(BaseShip boat)
        {
            m_Boat = boat;
        }

		public override void OnResponse( Mobile from, string text )
		{
			m_Boat.EndRename( from, text );
		}
	}
}

namespace Server.Gumps
{
    public class RenameGalleonGump : Gump
    {
        public RenameGalleonGump(BaseShip boat, Mobile from)
            : base(150, 75)
        {
            m_Boat = boat;
            from.CloseGump(typeof(RenameGalleonGump));
            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;
            AddPage(0);
            AddBackground(0, 0, 316, 156, 2620);
            AddImage(10, 10, 0x910);
            AddLabel(85, 24, 69, "You Ships Name");
            AddLabel(85, 109, 69, "Please choose a new one!");
            AddAlphaRegion(51, 59, 214, 40);
            AddTextEntry(50, 67, 266, 50, 38, 1, "Type in here!");

            AddButton(30, 150, 0xF7, 0xF8, 1, GumpButtonType.Reply, 0); // Yes
        }

        private string GetString(RelayInfo info, int id)
        {
            TextRelay t = info.GetTextEntry(id);
            return (t == null ? null : t.Text.Trim());
        }

        private BaseShip m_Boat;

        public override void OnResponse(NetState state, RelayInfo info)
        {

            Mobile from = state.Mobile;

            string newName = GetString(info, 1);
            if (m_Boat != null)
            {
                newName = newName.Trim();
            }

            from.SendMessage(0X22, "Your Ship Name is now {0}.", newName);
            m_Boat.ShipName = newName;
        }
    }
}