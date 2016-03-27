using System;
using Server.Engines.Plants;

namespace Server.Items
{ 
    public class PlantClippings : Item
    {
        private PlantHue m_PlantHue;

        [CommandProperty(AccessLevel.GameMaster)]
        public PlantHue PlantHue
        {
            get
            {
                return m_PlantHue;
            }
            set
            {
                m_PlantHue = value;
                Hue = PlantHueInfo.GetInfo(value).Hue;
                InvalidateProperties();
            }
        }

        [Constructable]
        public PlantClippings()
            : this(PlantHue.Plain)
        {
        }

        [Constructable]
        public PlantClippings(PlantHue plantHue)
            : this(1, plantHue)
        {
        }

        [Constructable]
        public PlantClippings(int amount, PlantHue plantHue)
            : base(0x4022)
        {
            PlantHue = plantHue;
            Stackable = true;
            Weight = 1.0;
            Amount = amount;
        }

        public PlantClippings(Serial serial)
            : base(serial)
        {
        }

        public override bool ForceShowProperties
        {
            get
            {
                return ObjectPropertyList.Enabled;
            }
        }
        public override int LabelNumber
        {
            get
            {
                return 1112131;
            }
        }// Plant Clippings
        public override void AddNameProperty(ObjectPropertyList list)
        {
            PlantHueInfo hueInfo = PlantHueInfo.GetInfo(m_PlantHue);
            if (Amount != 1)
            {
                list.Add(hueInfo.IsBright()
                        ? 1113272 // ~1_AMOUNT~ bright ~2_COLOR~ plant clippings
                        : 1113274 // ~1_AMOUNT~ ~2_COLOR~ plant clippings
                    , String.Format("{0}\t#{1}", Amount, hueInfo.Name));
            }
            else
            {
                list.Add(hueInfo.IsBright()
                        ? 1112121 // bright ~1_COLOR~ plant clippings
                        : 1112122 // ~1_COLOR~ plant clippings
                    , String.Format("#{0}", hueInfo.Name));
            }
            /*
           
            if (Amount > 1)
                list.Add(1113274, "{0}\t{1}", Amount, "#" + hueInfo.Name);  // ~1_COLOR~ Softened Reeds
            else 
                list.Add(1112122, "#" + hueInfo.Name);  // ~1_COLOR~ plant clippings
             */
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)m_PlantHue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            /*int version = */
            reader.ReadInt();

            m_PlantHue = (PlantHue)reader.ReadInt();
        }
    }
}