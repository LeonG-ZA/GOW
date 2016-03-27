using System;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Items
{
    public class AthenaeumTeleporter : Teleporter
    { 
        [Constructable]
        public AthenaeumTeleporter()
            : base(new Point3D(594, 3846, -31), Map.TerMur)
        {
        }

        public AthenaeumTeleporter(Serial serial)
            : base(serial)
        {
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m.NetState == null || !m.NetState.SupportsExpansion(Expansion.SA))
            {
                m.SendLocalizedMessage(1112943); // You must upgrade to Stygian Abyss in order to use this.				
                return true;
            }
			
            if (m is PlayerMobile)
            {
                PlayerMobile player = (PlayerMobile)m;

                if (QuestHelper.GetQuest(player, typeof(JourneyToTheAthenaeumIsleQuest)) != null)
                    return base.OnMoveOver(m);
				
                //player.SendMessage(1074274);
            }
			
            return true;
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