using System;

namespace Server.Items
{
    public class BigFish : Item, ICarvable
    {
        private Mobile m_Fisher;
        [Constructable]
        public BigFish()
            : base(0x09CC)
        {
            Weight = Utility.RandomMinMax(3, 200);
            Hue = Utility.RandomBool() ? 0x847 : 0x58C;
        }

        public BigFish(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Fisher
        {
            get
            {
                return m_Fisher;
            }
            set
            {
                m_Fisher = value;
                InvalidateProperties();
            }
        }
        public override int LabelNumber
        {
            get
            {
                return 1041112;
            }
        }// a big fish
        public void Carve(Mobile from, Item item)
        {
            base.ScissorHelper(from, new RawFishSteak(), Math.Max(16, (int)Weight) / 4, false);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (Weight >= 20)
            {
                if (m_Fisher != null)
                {
                    list.Add(1070857, m_Fisher.Name); // Caught by ~1_fisherman~
                }

                list.Add(1070858, ((int)Weight).ToString()); // ~1_weight~ stones
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((Mobile)m_Fisher);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 1:
                    {
                        m_Fisher = reader.ReadMobile();
                        break;
                    }
                case 0:
                    {
                        Weight = Utility.RandomMinMax(3, 200);
                        break;
                    }
            }
        }
    }
}