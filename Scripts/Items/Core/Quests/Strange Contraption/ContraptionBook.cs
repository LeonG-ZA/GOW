using System;
using Server;
using Server.Items;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
    public class ContraptionBook : Item
    {
        
        [Constructable]
        public ContraptionBook()
            : base(0x0FF4)
        {

            Movable = false;
            Weight = 1.0;
        }

        public ContraptionBook(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 1))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return;
            }
            else

            from.SendLocalizedMessage(1055141);//This device is used to purify water. For it to work, three key ingredients are required: a plague beast core, some moonfire brew, and a fragment of obsidian.
        }
    }
}
