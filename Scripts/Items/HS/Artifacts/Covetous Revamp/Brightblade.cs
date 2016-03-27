using System;

namespace Server.Items
{
    public class Brightblade : Katana
    {
		public override int LabelNumber { get { return 1152732; } } // Brightblade
		
        [Constructable]
        public Brightblade()
        {		
			Hue = 982;
            AbsorptionAttributes.SplinteringWeapon = 20;
			WeaponAttributes.HitLeechStam = 100; 
			Attributes.SpellChanneling = 1; 
			Attributes.ReflectPhysical = 15;
			Attributes.DefendChance = 15;
			Attributes.CastSpeed = 1;
			Attributes.LowerManaCost = 8; 			
			WeaponAttributes.SelfRepair = 5;
			SkillBonuses.SetValues( 0, SkillName.Parry, 10 );
        }

        public Brightblade(Serial serial) : base(serial)
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