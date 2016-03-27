using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
    public class GreenMarbleFireplaceEastAddon : BaseAddon
    {
        public override BaseAddonDeed Deed
        {
            get
            {
                return new GreenMarbleFireplaceEastAddonDeed();
            }
        }

        [Constructable]
        public GreenMarbleFireplaceEastAddon()
        {
            AddonComponent ac = null;
            ac = new

            AddonComponent(39515);
            ac.Name = "Green Marble Fireplace";
            AddComponent(ac, 0, 0, 0);

            ac = new AddonComponent(39514);
            ac.Name = "Green Marble Fireplace";
            AddComponent(ac, 1, 0, 0);

            ac = new AddonComponent(39516);
            ac.Name = "Green Marble Fireplace";
            AddComponent(ac, -1, 0, 0);
             
        }

        public GreenMarbleFireplaceEastAddon(Serial serial)
            : base(serial)
        {
        }

        public override void OnComponentUsed(AddonComponent ac, Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 1))
                from.SendMessage("You are too far away to use that!");
            else
            {
                if (ac.ItemID == 39547)
                {
                    ac.ItemID = 39515;
                    from.SendMessage("You put out the fireplace!");
                }
                else if (ac.ItemID == 39515)
                {
                    ac.ItemID = 39547;
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

    public class GreenMarbleFireplaceEastAddonDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get
            {
                return new GreenMarbleFireplaceEastAddon();
            }
        }

        [Constructable]
        public GreenMarbleFireplaceEastAddonDeed()
        {
            Name = "Green Marble Fireplace East";
        }

        public GreenMarbleFireplaceEastAddonDeed(Serial serial)
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