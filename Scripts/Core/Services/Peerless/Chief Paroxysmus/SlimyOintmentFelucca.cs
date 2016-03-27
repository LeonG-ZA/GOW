using System;
using Server;
using System.Collections;
using Server.Mobiles;

namespace Server.Items
{
	public class SlimyOintmentFelucca : BaseActivation
	{
		public override int LabelNumber { get { return 1074330; } } // slimy ointment

		[Constructable]
		public SlimyOintmentFelucca()
			: base( 0x318E )
		{
			Mapa = Map.Felucca;
			Hue = 1109;
			//EnterPoint = new Point3D( 6519, 382, 0 );
		}

		public SlimyOintmentFelucca( Serial serial )
			: base( serial )
		{
		}

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.Player)
                return;

            if (from is PlayerMobile)
            {
                PlayerMobile player = (PlayerMobile)from;

                if (player.Parox)
                {
                    player.SendLocalizedMessage(1074715);  // You are already protected with this slimy ointment.
                }
                else
                {
                    player.Parox = true;

                    this.Delete();

                    player.SendLocalizedMessage(1074603);   // You rub the slimy ointment on your body, temporarily protecting yourself from the corrosive river.

                    Timer.DelayCall(TimeSpan.FromSeconds(60.0), delegate()
                    {
                        player.Parox = false;

                        player.SendLocalizedMessage(1074716);  // You are no longer under the protection of the slimy ointment.
                    });
                }
            }
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}