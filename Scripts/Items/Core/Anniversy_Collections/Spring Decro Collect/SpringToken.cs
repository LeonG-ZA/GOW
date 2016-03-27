using System; 
using Server; 
using Server.Gumps; 
using Server.Network;

namespace Server.Items
{

    public class SpringToken : Item
    {

        [Constructable]
        public SpringToken()
            : this(null)
        {
        }

        [Constructable]
        public SpringToken(String name)
            : base(10922)
        {
            Name = "Spring Decor Token";
            Stackable = false;
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public SpringToken(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001);
            }
            else
            {
                from.SendGump(new SpringTokenGump(from, this));
            }
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
    }
}
