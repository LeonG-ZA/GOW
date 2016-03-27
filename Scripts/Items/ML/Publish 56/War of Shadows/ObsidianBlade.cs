using System;
using Server;

namespace Server.Items
{
	public class ObsidianBlade : ThinLongsword
	{

		[Constructable]
		public ObsidianBlade()
		{
			Hue = 1175;
			WeaponAttributes.HitLeechHits = 10;
			WeaponAttributes.HitLeechStam = 20;
			Attributes.AttackChance = 20;
		}

		public ObsidianBlade( Serial serial ) : base( serial )
		{
		}

        public override int LabelNumber
        {
            get
            {
                return 1080144;
            }
        }// obsidian blade

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}