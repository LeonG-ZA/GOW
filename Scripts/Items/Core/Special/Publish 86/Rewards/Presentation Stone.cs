using Server;
using System;

namespace Server.Items
{
    public class PresentationStone : Item, ITokunoDyable
    {
        public override int LabelNumber { get { return 1154745; } }

        [Constructable]
        public PresentationStone()
            : base(0x32F2)
        {
            Weight = 5;
        }
        public PresentationStone(Serial serial)
        {

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
    }
}