using System;

namespace Server.Items
{
    public class MaleGargishClothLegs : BaseClothing
    {
        public override Race RequiredRace
        {
            get
            {
                return Race.Gargoyle;
            }
        }
        public override bool CanBeWornByGargoyles
        {
            get
            {
                return true;
            }
        }

        [Constructable]
        public MaleGargishClothLegs()
            : this(0)
        {
        }

        [Constructable]
        public MaleGargishClothLegs(int hue)
            : base(0x040A, Layer.Pants, hue)
        {
            this.Weight = 2.0;
        }

        public MaleGargishClothLegs(Serial serial)
            : base(serial)
        {
        }

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