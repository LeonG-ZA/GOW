using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
    public class GreenMarbleFireplaceSouthAddon : BaseAddon
    {
        public override BaseAddonDeed Deed
        {
            get
            {
                return new GreenMarbleFireplaceSouthAddonDeed();
            }
        }

        [Constructable]
        public GreenMarbleFireplaceSouthAddon()
        {
            AddonComponent ac = null;
            ac = new

            AddonComponent(39512);
            ac.Name = "Green Marble Fireplace";
            AddComponent(ac, 0, 0, 0);

            ac = new AddonComponent(39513);
            ac.Name = "Green Marble Fireplace";
            AddComponent(ac, 0, -1, 0);

            ac = new AddonComponent(39511);
            ac.Name = "Green Marble Fireplace";
            AddComponent(ac, 0, 1, 0);
             
        }

        public GreenMarbleFireplaceSouthAddon(Serial serial)
            : base(serial)
        {
        }

        public override void OnComponentUsed(AddonComponent ac, Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 1))
                from.SendMessage("You are too far away to use that!");
            else
            {
                if (ac.ItemID == 39541)
                {
                    ac.ItemID = 39512;
                    from.SendMessage("You put out the fireplace!");
                }
                else if (ac.ItemID == 39512)
                {
                    ac.ItemID = 39541;
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

    public class GreenMarbleFireplaceSouthAddonDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get
            {
                return new GreenMarbleFireplaceSouthAddon();
            }
        }

        [Constructable]
        public GreenMarbleFireplaceSouthAddonDeed()
        {
            Name = "Green Marble Fireplace South";
        }

        public GreenMarbleFireplaceSouthAddonDeed(Serial serial)
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