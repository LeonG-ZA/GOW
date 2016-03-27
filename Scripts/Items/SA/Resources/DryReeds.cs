#region References
using Server;
using Server.Engines.Craft;
using Server.Engines.Plants;
using Server.Mobiles;
using Server.Targeting;
using System;
using System.Collections.Generic;
#endregion

namespace Server.Items
{
	public class DryReeds : Item
	{
		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }
		public override int LabelNumber { get { return 1112248; } } // Dry Reeds

		private PlantHue m_PlantHue;

		[CommandProperty(AccessLevel.GameMaster)]
		public PlantHue PlantHue
		{
			get { return m_PlantHue; }
			set
			{
				m_PlantHue = value;
				Hue = PlantHueInfo.GetInfo(value).Hue;
				InvalidateProperties();
			}
		}

		public virtual bool RetainsColor { get { return true; } }

        [Constructable]
        public DryReeds()
            : this(PlantHue.Plain)
        {
        }

        [Constructable]
        public DryReeds(PlantHue plantHue)
            : this(1, plantHue)
        {
        }

        [Constructable]
        public DryReeds(int amount, PlantHue plantHue)
            : base(0x1BD5)
        {
            PlantHue = plantHue;
            Stackable = true;
            Weight = 1.0;
            Amount = amount;
        }

		public DryReeds(Serial serial)
			: base(serial)
		{ }

        public static int GetTotalReeds(Container cont, PlantHue hue)
        {
            return GetTotalReeds(GetReeds(cont, hue), hue);
        }

        public static int GetTotalReeds(List<DryReeds> reeds, PlantHue hue)
        {
            int total = 0;

            for (int i = 0; i < reeds.Count; i++)
                total += reeds[i].Amount;

            return total;
        }

        public static List<DryReeds> GetReeds(Container cont, PlantHue hue)
        {
            return GetReeds(cont.FindItemsByType<DryReeds>(), hue);
        }

        public static List<DryReeds> GetReeds(List<DryReeds> reeds, PlantHue hue)
        {
            List<DryReeds> validReeds = new List<DryReeds>();

            for (int i = 0; i < reeds.Count; i++)
            {
                DryReeds reed = reeds[i];

                if (reed.PlantHue == hue)
                    validReeds.Add(reed);
            }

            return validReeds;
        }

        public static bool ConsumeReeds(Container cont, PlantHue hue, int amount)
        {
            List<DryReeds> reeds = GetReeds(cont, hue);

            if (GetTotalReeds(reeds, hue) >= amount)
            {
                for (int i = 0; i < reeds.Count; i++)
                {
                    DryReeds reed = reeds[i];

                    if (amount >= reed.Amount)
                    {
                        amount -= reed.Amount;
                        reed.Delete();
                    }
                    else
                    {
                        reed.Amount -= amount;
                        break;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

		public override void AddNameProperty(ObjectPropertyList list)
		{
			list.Add(1112289, "#" + PlantHueInfo.GetInfo(m_PlantHue).Name); // ~1_COLOR~ dry reeds
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); // version

			writer.Write((int)m_PlantHue);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
					m_PlantHue = (PlantHue)reader.ReadInt();
					break;
			}
		}
	}
}