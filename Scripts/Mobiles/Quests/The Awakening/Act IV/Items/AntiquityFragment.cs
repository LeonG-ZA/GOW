using System;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class AntiquityFragment : Item
	{
		[Constructable]
        public AntiquityFragment()
            : base(0x19b7)
        {
            Hue = 2724;
            Stackable = true;
        }

        public override int LabelNumber { get { return 1153870; } }// Antiquity Fragment

        public override void OnDoubleClick(Mobile from)
        {
            if (!this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            Container pack = from.Backpack;

            if (pack == null)
                return;

            int res = pack.ConsumeTotal( new Type[]
                {
                    typeof(AntiquityFragment)
                },
                new int[]
                {
                    10
                });

            switch (res)
            {
                case 0:
                    {
                        from.SendMessage("You must have 10 Antiquity Fragments.");
                        break;
                    }
                default:
                    {
                        switch (Utility.Random(20))
                        {
                            case 0: from.AddToBackpack(new AncientJukanArmor()); break;
                            case 1: from.AddToBackpack(new AncientJukanWaymasterBo()); break;
                            case 2: from.AddToBackpack(new CharredJukanHistoricalScroll()); break;
                            case 3: from.AddToBackpack(new JukanBones()); break;
                            case 4: from.AddToBackpack(new JukanHeirloomVase()); break;
                            case 5: from.AddToBackpack(new JukanTapestry()); break;
                            case 6: from.AddToBackpack(new JukanUrn()); break;
                            case 7: from.AddToBackpack(new KegofJukanMead()); break;
                            case 8: from.AddToBackpack(new KegofJukanWine()); break;
                            case 9: from.AddToBackpack(new RuinedJukanTome()); break;
                            case 10: from.AddToBackpack(new AncientMeerArmor()); break;
                            case 11: from.AddToBackpack(new AncientMeerEternalStaff()); break;
                            case 12: from.AddToBackpack(new CharredMeerHistoricalScroll()); break;
                            case 13: from.AddToBackpack(new KegofMeerAle()); break;
                            case 14: from.AddToBackpack(new KegofMeerWine()); break;
                            case 15: from.AddToBackpack(new MeerBones()); break;
                            case 16: from.AddToBackpack(new MeerHeirloomVase()); break;
                            case 17: from.AddToBackpack(new MeerTapestry()); break;
                            case 18: from.AddToBackpack(new MeerUrn()); break;
                            case 19: from.AddToBackpack(new RuinedMeerTomes()); break;
                        }
                        from.PlaySound(0x241);
                        break;
                    }
            }
        }
        public AntiquityFragment(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}