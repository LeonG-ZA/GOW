using System;

namespace Server.Items
{
    public class GreaterRefreshPotion : BaseRefreshPotion
    {
        [Constructable]
        public GreaterRefreshPotion()
            : base(PotionEffect.GreaterRefresh)
        {
        }

        public GreaterRefreshPotion(Serial serial)
            : base(serial)
        {
        }

        public override double Refresh
        {
            get
            {
                return 5.0;
            }
        }

        public override int LabelNumber
        {
            get
            {
                return 1041327;//Greater Refreshment potion
            }
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