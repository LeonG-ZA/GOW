using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
    public class GoldMarbleFireplaceEastAddon : BaseAddon
    {
        public override BaseAddonDeed Deed
        {
            get
            {
                return new GoldMarbleFireplaceEastAddonDeed();
            }
        }

        [Constructable]
        public GoldMarbleFireplaceEastAddon()
        {
            AddonComponent ac = null;
            ac = new

            AddonComponent(39503);
            ac.Name = "Gold Marble Fireplace";
            AddComponent(ac, 0, 0, 0);

            ac = new AddonComponent(39502);
            ac.Name = "Gold Marble Fireplace";
            AddComponent(ac, 1, 0, 0);

            ac = new AddonComponent(39504);
            ac.Name = "Gold Marble Fireplace";
            AddComponent(ac, -1, 0, 0);
             
        }

        public GoldMarbleFireplaceEastAddon(Serial serial)
            : base(serial)
        {
        }

        public override void OnComponentUsed(AddonComponent ac, Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 1))
                from.SendMessage("You are too far away to use that!");
            else
            {
                if (ac.ItemID == 39523)
                {
                    ac.ItemID = 39503;
                    from.SendMessage("You put out the fireplace!");
                }
                else if (ac.ItemID == 39503)
                {
                    ac.ItemID = 39523;
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

    public class GoldMarbleFireplaceEastAddonDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get
            {
                return new GoldMarbleFireplaceEastAddon();
            }
        }

        [Constructable]
        public GoldMarbleFireplaceEastAddonDeed()
        {
            Name = "Gold Marble Fireplace East";
        }

        public GoldMarbleFireplaceEastAddonDeed(Serial serial)
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