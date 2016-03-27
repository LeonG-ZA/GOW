using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
    public class BlueMarbleFireplaceSouthAddon : BaseAddon
    {
        public override BaseAddonDeed Deed
        {
            get
            {
                return new BlueMarbleFireplaceSouthAddonDeed();
            }
        }

        [Constructable]
        public BlueMarbleFireplaceSouthAddon()
        {
            AddonComponent ac = null;
            ac = new

            AddonComponent(39506);
            ac.Name = "Blue Marble Fireplace";
            AddComponent(ac, 0, 0, 0);

            ac = new AddonComponent(39507);
            ac.Name = "Blue Marble Fireplace";
            AddComponent(ac, 0, -1, 0);

            ac = new AddonComponent(39505);
            ac.Name = "Blue Marble Fireplace";
            AddComponent(ac, 0, 1, 0);
             
        }

        public BlueMarbleFireplaceSouthAddon(Serial serial)
            : base(serial)
        {
        }

        public override void OnComponentUsed(AddonComponent ac, Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 1))
                from.SendMessage("You are too far away to use that!");
            else
            {
                if (ac.ItemID == 39529)
                {
                    ac.ItemID = 39506;
                    from.SendMessage("You put out the fireplace!");
                }
                else if (ac.ItemID == 39506)
                {
                    ac.ItemID = 39529;
                    ac.Light = LightType.Circle225;
                    from.SendMessage("You light the fireplace!");
                }
                else
                    return;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

    }

    public class BlueMarbleFireplaceSouthAddonDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get
            {
                return new BlueMarbleFireplaceSouthAddon();
            }
        }

        [Constructable]
        public BlueMarbleFireplaceSouthAddonDeed()
        {
            Name = "Blue Marble Fireplace South";
        }

        public BlueMarbleFireplaceSouthAddonDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}