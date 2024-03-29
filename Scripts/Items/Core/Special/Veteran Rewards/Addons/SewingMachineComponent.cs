﻿using System;
namespace Server.Items
{
    class SewingMachineComponent : SpecialVeteranAddonComponent
    {        
        [Constructable]
        public SewingMachineComponent(int itemID)
            : base(itemID)
        {
        }

        public SewingMachineComponent(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1123504;
            }
        }// sewing machine

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

    public class SewingMachineBoxComponent : SpecialVeteranAddonComponentBox
    {
        [Constructable]
        public SewingMachineBoxComponent(int itemID)
            : base(itemID)
        {
        }

        public SewingMachineBoxComponent(Serial serial)
            : base(serial)
        {
        }

        public override Type[] AllowedTools {
            get
            {
                return new Type[]{ typeof(SewingKit) };
            }
        }

        public override int LabelNumber
        {
            get
            {
                return 1123522;
            }
        }// sewing machine

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
