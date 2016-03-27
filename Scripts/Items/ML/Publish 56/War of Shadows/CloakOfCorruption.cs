using System;
using Server;

namespace Server.Items
{
    [Flipable(0x1F03, 0x1F04)]
    public class CloakOfCorruption : BaseOuterTorso
    {
        [Constructable]
        public CloakOfCorruption()
            : base(12217, 12659)
        {
            Hue = 2406;
            Weight = 3.0;
        }

        public CloakOfCorruption(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1077499;
            }
        }// cloak of corruption

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}