using System;
namespace Server.Items
{
    class SpinningLatheComponent : SpecialVeteranAddonComponent
    {        
        [Constructable]
        public SpinningLatheComponent(int itemID)
            : base(itemID)
        {
        }

        public SpinningLatheComponent(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1123986;
            }
        }// spinning lathe

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

    public class SpinningLatheBoxComponent : SpecialVeteranAddonComponentBox
    {
        [Constructable]
        public SpinningLatheBoxComponent(int itemID)
            : base(itemID)
        {
        }

        public SpinningLatheBoxComponent(Serial serial)
            : base(serial)
        {
        }

        public override Type[] AllowedTools {
            get
            {
                return new Type[] { 
                    typeof(Saw), typeof(DovetailSaw), typeof(Scorp), typeof(DrawKnife), typeof(Froe), typeof(Inshave), 
                    typeof(JointingPlane), typeof(MouldingPlane), typeof(SmoothingPlane) 
                };
            }
        }

        public override int LabelNumber
        {
            get
            {
                return 1124030;
            }
        }// bucket

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
