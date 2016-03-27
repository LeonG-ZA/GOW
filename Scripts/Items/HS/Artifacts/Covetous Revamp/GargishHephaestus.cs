using System;

namespace Server.Items
{
    public class GargishHephaestus : LargePlateShield
    {
		public override int LabelNumber { get { return 1152722; } } // GargishHephaestus
		
        [Constructable]
        public GargishHephaestus() : base()
        {	
			Hue = 1175;
			Attributes.SpellChanneling = 1; 
			Attributes.ReflectPhysical = 15;
			Attributes.DefendChance = 15;
			Attributes.CastSpeed = 1;
			Attributes.LowerManaCost = 8; 			
			ArmorAttributes.SelfRepair = 5;
			SkillBonuses.SetValues( 0, SkillName.Parry, 10 );
        }

        public GargishHephaestus(Serial serial) : base(serial)
        {
        }
		
        public override int InitMinHits { get { return 255; } }
		
        public override int InitMaxHits { get { return 255; } }
		
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);//version
        }
    }
}