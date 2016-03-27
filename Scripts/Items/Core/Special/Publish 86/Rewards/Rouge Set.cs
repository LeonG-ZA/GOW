using System;
using Server;

namespace Server.Items
{
    public class SoleilRouge : GoldBracelet
    {

        public override int LabelNumber { get { return 1154371; } }
        public override SetItem SetID { get { return SetItem.Rouge; } }
        public override int Pieces { get { return 2; } }
        [Constructable]
        public SoleilRouge()
            : base()
        {
            Weight = 1.0;

            SetHue = 0x048E;
            Hue = 0x048E;

            Attributes.Luck = 150;
            Attributes.AttackChance = 10;
            Attributes.WeaponDamage = 20;

            SetAttributes.Luck = 100;
            SetAttributes.AttackChance = 10;
            SetAttributes.WeaponDamage = 20;
            SetAttributes.WeaponSpeed = 10;
            SetAttributes.RegenHits = 2;
            SetAttributes.RegenStam = 3;
        }

        public SoleilRouge(Serial serial)
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

    public class LuneRouge : GoldRing
    {
        public override int LabelNumber { get { return 1154372; } }
        public override SetItem SetID { get { return SetItem.Rouge; } }
        public override int Pieces { get { return 2; } }
        [Constructable]
        public LuneRouge()
            : base()
        {
            Weight = 1.0;

            SetHue = 0x048E;
            Hue = 0x048E;

            Attributes.Luck = 150;
            Attributes.AttackChance = 10;
            Attributes.WeaponDamage = 20;

            SetAttributes.Luck = 100;
            SetAttributes.AttackChance = 10;
            SetAttributes.WeaponDamage = 20;
            SetAttributes.WeaponSpeed = 10;
            SetAttributes.RegenHits = 2;
            SetAttributes.RegenStam = 3;
        }

        public LuneRouge(Serial serial)
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