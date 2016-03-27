using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
    public class BlueMarbleFireplaceEastAddon : BaseAddon
    {
        public override BaseAddonDeed Deed
        {
            get
            {
                return new BlueMarbleFireplaceEastAddonDeed();
            }
        }

        [Constructable]
        public BlueMarbleFireplaceEastAddon()
        {
            AddonComponent ac = null;
            ac = new

            AddonComponent(39509);
            ac.Name = "Blue Marble Fireplace";
            AddComponent(ac, 0, 0, 0);

            ac = new AddonComponent(39508);
            ac.Name = "Blue Marble Fireplace";
            AddComponent(ac, 1, 0, 0);

            ac = new AddonComponent(39510);
            ac.Name = "Blue Marble Fireplace";
            AddComponent(ac, -1, 0, 0);
             
        }

        public BlueMarbleFireplaceEastAddon(Serial serial)
            : base(serial)
        {
        }

        public override void OnComponentUsed(AddonComponent ac, Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 1))
                from.SendMessage("You are too far away to use that!");
            else
            {
                if (ac.ItemID == 39535)
                {
                    ac.ItemID = 39509;
                    Effects.PlaySound(from.Location, from.Map, 0x4B9);
                    from.SendMessage("You put out the fireplace!");
                }
                else if (ac.ItemID == 39509)
                {
                    ac.ItemID = 39535;
                    ac.Light = LightType.Circle225;
                    Effects.PlaySound(from.Location, from.Map, 0x4BA);
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

    public class BlueMarbleFireplaceEastAddonDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get
            {
                return new BlueMarbleFireplaceEastAddon();
            }
        }

        [Constructable]
        public BlueMarbleFireplaceEastAddonDeed()
        {
            Name = "Blue Marble Fireplace East";
        }

        public BlueMarbleFireplaceEastAddonDeed(Serial serial)
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