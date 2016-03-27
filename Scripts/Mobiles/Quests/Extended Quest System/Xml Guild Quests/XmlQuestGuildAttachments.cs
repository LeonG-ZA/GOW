using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlQuestGuildBlacksmith : XmlQuestGuild
    {
        // a serial constructor is REQUIRED
        public XmlQuestGuildBlacksmith(ASerial serial)
            : base(serial)
        {
        }

        [Attachable]
        public XmlQuestGuildBlacksmith() : base("Blacksmith", "Marks of the Forge")
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            // version 0

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            // version 0
        }
    }

    public class XmlQuestGuildTailor : XmlQuestGuild
    {
        // a serial constructor is REQUIRED
        public XmlQuestGuildTailor(ASerial serial)
            : base(serial)
        {
        }

        [Attachable]
        public XmlQuestGuildTailor()
            : base("Tailor", "Marks of the Cloth")
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            // version 0

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            // version 0
        }
    }
}
