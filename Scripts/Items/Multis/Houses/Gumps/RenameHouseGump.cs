using System;
using Server.Prompts;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Multis;

namespace Server.Prompts
{
    public class RenamePrompt : Prompt
    {
        private readonly BaseHouse m_House;

        // What dost thou wish the sign to say?
        public override int MessageCliloc { get { return 501302; } }

        public RenamePrompt(BaseHouse house)
        {
            this.m_House = house;
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (this.m_House.IsFriend(from))
            {
                if (this.m_House.Sign != null)
                    this.m_House.Sign.Name = text;

                from.SendMessage("Sign changed.");
            }
        }
    }
}