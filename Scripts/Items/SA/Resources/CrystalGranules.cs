using System;

namespace Server.Items
{
    public class CrystalGranules : Item
    {
        [Constructable]
        public CrystalGranules()
            : base(0x4008)
        {
            this.Weight = 1;
            this.Hue = 0x47E;
        }

        public CrystalGranules(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1112329;
            }
        }// Crystal Granules
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
			
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
			
            int version = reader.ReadInt();
        }
    }
}