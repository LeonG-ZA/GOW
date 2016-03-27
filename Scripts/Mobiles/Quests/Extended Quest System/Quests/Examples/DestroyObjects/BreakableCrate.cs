using System;
using Server;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Items
{
    //This item is used in the DestroyObjectObjective Example Quest
    //QuestNPC GuardJedrek
    //Quest: Endthesupplyline
    public class BreakableCrate : BreakableObject
    {
        [Constructable]
        public BreakableCrate() : base(0x0E3C)
        {
            Name = "a Orichs Supply Crate";
            Hue = 647;
        }

        public BreakableCrate(Serial serial) : base(serial)
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
