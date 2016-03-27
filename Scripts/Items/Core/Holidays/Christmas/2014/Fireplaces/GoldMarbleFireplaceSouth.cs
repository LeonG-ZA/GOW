using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
    public class GoldMarbleFireplaceSouthAddon : BaseAddon
    {
        public override BaseAddonDeed Deed
        {
            get
            {
                return new GoldMarbleFireplaceSouthAddonDeed();
            }
        }

        [Constructable]
        public GoldMarbleFireplaceSouthAddon()
        {
            AddonComponent ac = null;
            ac = new

            AddonComponent(39500);
            ac.Name = "Gold Marble Fireplace";
            AddComponent(ac, 0, 0, 0);

            ac = new AddonComponent(39501);
            ac.Name = "Gold Marble Fireplace";
            AddComponent(ac, 0, -1, 0);

            ac = new AddonComponent(39499);
            ac.Name = "Gold Marble Fireplace";
            AddComponent(ac, 0, 1, 0);
             
        }

        public GoldMarbleFireplaceSouthAddon(Serial serial)
            : base(serial)
        {
        }

        public override void OnComponentUsed(AddonComponent ac, Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 1))
                from.SendMessage("You are too far away to use that!");
            else
            {
                if (ac.ItemID == 39517)
                {
                    ac.ItemID = 39500;
                    from.SendMessage("You put out the fireplace!");
                }
                else if (ac.ItemID == 39500)
                {
                    ac.ItemID = 39517;
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

    public class GoldMarbleFireplaceSouthAddonDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get
            {
                return new GoldMarbleFireplaceSouthAddon();
            }
        }

        [Constructable]
        public GoldMarbleFireplaceSouthAddonDeed()
        {
            Name = "Gold Marble Fireplace South";
        }

        public GoldMarbleFireplaceSouthAddonDeed(Serial serial)
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