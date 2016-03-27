using System;

namespace Server.Items
{
    public class ExodusSummoningAlter : TransientItem
    {
        [Constructable]
        public ExodusSummoningAlter(): base(5360, TimeSpan.FromSeconds(21600.0))
        {
            Name = "Exodus Summoning Altar";
            //Hue = 2062;
        }

        public ExodusSummoningAlter(Serial serial)
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

