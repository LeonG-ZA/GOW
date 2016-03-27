//This device is used to purify water. For it to work, three key ingredients are required: 
//a plague beast core, some moonfire brew, and a fragment of obsidian.
using System;
using Server;
using Server.Network;
using Server.Items;
using System.Collections;

namespace Server.Items
{
    public class ContraptionAddon : BaseAddon
    {
        [Constructable]
        public ContraptionAddon()
        {
            this.AddComponent(new AddonComponent(6432), 1, 0, 1);
            this.AddComponent(new ContraptionPiece(), 2, 0, 1);
            this.AddComponent(new AddonComponent(2816), 2, 0, 16);
            this.AddComponent(new AddonComponent(2818), 1, 0, 16);
            this.AddComponent(new AddonComponent(2815), 1, 0, 19);
            this.AddComponent(new AddonComponent(2813), 2, 0, 19);
            this.AddComponent(new AddonComponent(2923), 1, 2, 0);
            this.AddComponent(new AddonComponent(2924), 1, 1, 0);
            this.AddComponent(new AddonComponent(9), 0, 0, 2);
            this.AddComponent(new AddonComponent(2818), 1, 0, 16);
            this.AddComponent(new AddonComponent(4272), 1, 2, 23);
            this.AddComponent(new AddonComponent(4272), -2, 0, 1);
            this.AddComponent(new AddonComponent(13456), -2, 0, 0);
            this.AddComponent(new AddonComponent(13456), -1, 1, 1);
            this.AddComponent(new AddonComponent(13456), -1, 0, 0);
            this.AddComponent(new AddonComponent(13456), -2, 1, 0);

            this.AddComponent(new AddonComponent(8657), 1, 0, 3);
            this.AddComponent(new AddonComponent(8657), 1, -1, 3);
            this.AddComponent(new AddonComponent(8657), 1, 0, 5);
            this.AddComponent(new AddonComponent(441), -2, 0, 0);
            
        }

        public ContraptionAddon(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddonDeed Deed
        {
            get
            {
                return new ContraptionAddonDeed();
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

    public class ContraptionAddonDeed : BaseAddonDeed
    {
        [Constructable]
        public ContraptionAddonDeed()
        {
        }

        public ContraptionAddonDeed(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon
        {
            get
            {
                return new ContraptionAddon();
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