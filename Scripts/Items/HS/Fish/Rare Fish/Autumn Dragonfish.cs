using System;

namespace Server.Items
{
    public class AutumnDragonfish : BaseHighSeasFish
    {
        private Mobile m_Fisher;
        [Constructable]
        public AutumnDragonfish()
            : base(0x44E6)
        {
        	Hue = 2112;

            this.Weight = Utility.RandomMinMax(3, 200);
        }

        public AutumnDragonfish(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Fisher
        {
            get
            {
                return this.m_Fisher;
            }
            set
            {
                this.m_Fisher = value;
                this.InvalidateProperties();
            }
        }
        public override int LabelNumber
        {
            get
            {
                return 1116090;
            }
        }// autumn dragonfish

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (this.Weight >= 20)
            {
                if (this.m_Fisher != null)
                    list.Add(1070857, this.m_Fisher.Name); // Caught by ~1_fisherman~

                list.Add(1070858, ((int)this.Weight).ToString()); // ~1_weight~ stones
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version

            writer.Write((Mobile)this.m_Fisher);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        this.m_Fisher = reader.ReadMobile();
                        break;
                    }
                case 0:
                    {
                        this.Weight = Utility.RandomMinMax(3, 200);
                        break;
                    }
            }
        }
    }
}