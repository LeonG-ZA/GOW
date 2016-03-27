using System;
using Server;

namespace Server.Items
{
    public abstract class CleanupRobe : Robe
    {
        public CleanupRobe():base(Utility.RandomMinMax(2501,2644))
        {
            LootType = LootType.Blessed;
            StrRequirement = 10;
            Weight = 3;
        }
        public CleanupRobe(Serial serial)
        {

        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
    }
}