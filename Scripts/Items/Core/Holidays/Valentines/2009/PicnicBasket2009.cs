
namespace Server.Items
{
    public class PicnicBasket2009 : BaseContainer
    {
        [Constructable]
        public PicnicBasket2009()
            : this(false)
        {
        }

        [Constructable]
        public PicnicBasket2009(bool fill)
            : base(0xE7A)
        {
            Weight = 1.0;

            if (fill)
            {
                AddToBox(new DarkChocolate2009(), new Point3D(95, 80, 0));
                AddToBox(new SliceOfBacon2009(), new Point3D(90, 85, 0));
                AddToBox(new ValentinesCardSouth2009(), new Point3D(130, 55, 0));
            }
        }

        public PicnicBasket2009(Serial serial)
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

            int version = reader.ReadInt();
        }
    }
}