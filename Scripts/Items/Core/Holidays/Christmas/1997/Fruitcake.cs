using System; 
using Server.Items; 

namespace Server.Items 
{ 
    public class Fruitcake : BaseFood
    {
        [Constructable]
        public Fruitcake()
            : base(0x1044)
        {
            Name = "Fruitcake";
            Stackable = false;
            LootType = LootType.Blessed;
        }

        public Fruitcake(Serial serial)
            : base(serial)
        {
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
