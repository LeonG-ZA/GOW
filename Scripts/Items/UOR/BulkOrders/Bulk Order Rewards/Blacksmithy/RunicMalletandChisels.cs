using System;
using Server.Engines.Craft;

namespace Server.Items
{
    [FlipableAttribute(0x13E4, 0x13E3)]
    public class RunicMalletandChisels : BaseRunicTool
    {
        [Constructable]
        public RunicMalletandChisels(CraftResource resource)
            : base(resource, 0x13E3)
        {
            this.Weight = 8.0;
            this.Layer = Layer.OneHanded;
            this.Hue = CraftResources.GetHue(resource);
        }

        [Constructable]
        public RunicMalletandChisels(CraftResource resource, int uses)
            : base(resource, uses, 0x13E3)
        {
            this.Weight = 8.0;
            this.Layer = Layer.OneHanded;
            this.Hue = CraftResources.GetHue(resource);
        }

        public RunicMalletandChisels(Serial serial)
            : base(serial)
        {
        }

        public override CraftSystem CraftSystem
        {
            get
            {
                return DefBlacksmithy.CraftSystem;
            }
        }
        public override int LabelNumber
        {
            get
            {
                int index = CraftResources.GetIndex(this.Resource);

                if (index >= 1 && index <= 8)
                    return 1111796 + index;

                return 1111803; // Valorite Runic Mallet and Chisels
            }
        }
        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            int index = CraftResources.GetIndex(this.Resource);

            if (index >= 1 && index <= 8)
                return;

            if (!CraftResources.IsStandard(this.Resource))
            {
                int num = CraftResources.GetLocalizationNumber(this.Resource);

                if (num > 0)
                    list.Add(num);
                else
                    list.Add(CraftResources.GetName(this.Resource));
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