using System;

namespace Server.Items
{
    [FlipableAttribute(0x49C8, 0x49C9)]
    public class TallGiftBox : Container
    {
        public override int LabelNumber { get { return 1097761; } } // tall gift box

        [Constructable]
        public TallGiftBox()
            : base(0x49C8)
        {
            AddToBox(new HeartShapedBox(), new Point3D(95, 80, 0));
            AddToBox(new ValentineCard2010(), new Point3D(90, 85, 0));
            AddToBox(new FreshlyPickedRose(), new Point3D(130, 55, 0));
            Weight = 1.0;
        }

        public TallGiftBox(Serial serial)
            : base(serial)
        {
        }

        public virtual void AddToBox(Item item, Point3D loc)
        {
            DropItem(item);
            item.Location = loc;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            /*int version = */
            reader.ReadInt();
        }
    }
}
