using System;

namespace Server.Items
{
    public class LeatherContainerEngraver : BaseEngravingTool
    {
        [Constructable]
        public LeatherContainerEngraver()
            : base(0xF9D, 30)
        { 
        }

        public LeatherContainerEngraver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1072152;
            }
        }// leather container engraving tool
        public override Type[] Engraves
        {
            get
            {
                return new Type[]
                {
                    typeof(Pouch), typeof(Backpack), typeof(Bag)
                };
            }
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

    public class WoodenContainerEngraver : BaseEngravingTool
    {
        [Constructable]
        public WoodenContainerEngraver()
            : base(0x1026, 30)
        { 
        }

        public WoodenContainerEngraver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1072153;
            }
        }// wooden container engraving tool
        public override Type[] Engraves
        {
            get
            {
                return new Type[]
                {
                    typeof(WoodenBox), typeof(LargeCrate), typeof(MediumCrate),
                    typeof(SmallCrate), typeof(WoodenChest), typeof(EmptyBookcase),
                    typeof(Armoire), typeof(FancyArmoire), typeof(PlainWoodenChest),
                    typeof(OrnateWoodenChest), typeof(GildedWoodenChest), typeof(WoodenFootLocker),
                    typeof(FinishedWoodenChest), typeof(TallCabinet), typeof(ShortCabinet),
                    typeof(RedArmoire), typeof(CherryArmoire), typeof(MapleArmoire),
                    typeof(ElegantArmoire), typeof(Keg), typeof(SimpleElvenArmoire),
                    typeof(DecorativeBox), typeof(FancyElvenArmoire), typeof(RarewoodChest),
                };
            }
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

    public class MetalContainerEngraver : BaseEngravingTool
    {
        [Constructable]
        public MetalContainerEngraver()
            : base(0x1EB8, 30)
        { 
        }

        public MetalContainerEngraver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1072154;
            }
        }// metal container engraving tool
        public override Type[] Engraves
        {
            get
            {
                return new Type[]
                {
                    typeof(ParagonChest), typeof(MetalChest)
                };
            }
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

    public class FoodEngraver : BaseEngravingTool
    {
        [Constructable]
        public FoodEngraver()
            : base(0x1BD1, 30)
        { 
        }

        public FoodEngraver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1072951;
            }
        }// food decoration tool
        public override Type[] Engraves
        {
            get
            {
                return new Type[]
                {
                    typeof(Cake), typeof(CheesePizza), typeof(SausagePizza),
                    typeof(Cookies)
                };
            }
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

    public class SpellbookEngraver : BaseEngravingTool
    {
        [Constructable]
        public SpellbookEngraver()
            : base(0xFBF, 30)
        { 
        }

        public SpellbookEngraver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1072151;
            }
        }// spellbook engraving tool
        public override Type[] Engraves
        {
            get
            {
                return new Type[]
                {
                    typeof(Spellbook)
                };
            }
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

    public class ArmorEngraver : BaseEngravingTool
    {
        [Constructable]
        public ArmorEngraver()
            : base(0x32F8, 30)
        {
        }

        public ArmorEngraver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041026;
            }
        }// an armor engraving tool
        public override Type[] Engraves
        {
            get
            {
                return new Type[]
                {
                    typeof(BaseArmor)
                };
            }
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

    public class JeweleryEngraver : BaseEngravingTool
    {
        [Constructable]
        public JeweleryEngraver()
            : base(0x32F8, 30)
        {
        }

        public JeweleryEngraver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041029;
            }
        }// a jewelry engraving tool
        public override Type[] Engraves
        {
            get
            {
                return new Type[]
                {
                    typeof(BaseJewel)
                };
            }
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

    public class StatuetteEngraver : BaseEngravingTool
    {
        [Constructable]
        public StatuetteEngraver()
            : base(0x12B3, 30)
        {
        }

        public StatuetteEngraver(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1080201;
            }
        }// Statuette Engraving Tool
        public override Type[] Engraves
        {
            get
            {
                return new Type[]
                {
                    typeof(BaseStatue)
                };
            }
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