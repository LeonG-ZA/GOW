using System;
using Server.Mobiles;

namespace Server.Items
{
    public class CreatureBackpack : Backpack	//Used on BaseCreature
    {
        [Constructable]
        public CreatureBackpack(string name)
        {
            this.Name = name;
            this.Layer = Layer.Backpack;
            this.Hue = 5;
            this.Weight = 3.0;
        }

        public CreatureBackpack(Serial serial)
            : base(serial)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (this.Name != null)
                list.Add(1075257, this.Name); // Contents of ~1_PETNAME~'s pack.
            else
                base.AddNameProperty(list);
        }

        public override void OnItemRemoved(Item item)
        {
            if (this.Items.Count == 0)
                this.Delete();

            base.OnItemRemoved(item);
        }

        public override bool OnDragLift(Mobile from)
        {
            if (from.IsPlayer())
                return true;

            from.SendLocalizedMessage(500169); // You cannot pick that up.
            return false;
        }

        public override bool OnDragDropInto(Mobile from, Item item, Point3D p, byte gridloc)
        {
            return false;
        }

        public override bool TryDropItem(Mobile from, Item dropped, bool sendFullMessage)
        {
            return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                this.Weight = 13.0;
        }
    }

    public class StrongBackpack : Backpack	//Used on Pack animals
    {
        [Constructable]
        public StrongBackpack()
        {
            this.Layer = Layer.Backpack;
            this.Weight = 13.0;
        }

        public StrongBackpack(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultMaxWeight
        {
            get
            {
                return 1600;
            }
        }
        public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
        {
            return base.CheckHold(m, item, false, checkItems, plusItems, plusWeight);
        }

        public override bool CheckContentDisplay(Mobile from)
        {
            object root = this.RootParent;

            if (root is BaseCreature && ((BaseCreature)root).Controlled && ((BaseCreature)root).ControlMaster == from)
                return true;

            return base.CheckContentDisplay(from);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                this.Weight = 13.0;
        }
    }
}
