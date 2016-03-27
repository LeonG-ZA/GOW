using System;

namespace Server.Items
{
    public class MatchHS : Item
    {
        [Constructable]
        public MatchHS() : this(1)
        {
        }

        [Constructable]
        public MatchHS(int amount) : base(0xF6B)
        {
            Hue = 542;
            Amount = amount;
            Stackable = true;
            Weight = 1.0;
        }

        public MatchHS(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1096648;
            }
        }// Match

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                if (Amount > 1)
                    Consume(1);
                else
                    Delete();

                from.AddToBackpack(new LitMatchHS(DateTime.UtcNow));
                from.SendLocalizedMessage(1116114); //You ignite the match.
            }
            else
                from.SendLocalizedMessage(1060640); //The item must be in your backpack to use it.
        }

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