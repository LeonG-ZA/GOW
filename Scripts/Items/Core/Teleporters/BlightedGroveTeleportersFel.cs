using Server.MLConfiguration;
using System;

namespace Server.Items
{ 
    public class BlightedGroveTeleFel : Teleporter
    { 
        [Constructable]
        public BlightedGroveTeleFel()
            : base(new Point3D(6478, 863, 11), Map.Felucca)
        {
        }

        public BlightedGroveTeleFel(Serial serial)
            : base(serial)
        {
        }

        public static BoneMachete GetBoneMachete(Mobile m)
        {
            for (int i = 0; i < m.Items.Count; i ++)
            {
                if (m.Items[i] is BoneMachete)
                    return (BoneMachete)m.Items[i];
            }
			
            if (m.Backpack != null)
                return m.Backpack.FindItemByType(typeof(BoneMachete), true) as BoneMachete;
				
            return null;
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m.NetState == null || !m.NetState.SupportsExpansion(Expansion.ML))
            {
                m.SendLocalizedMessage(1072608); // You must upgrade to the Mondain's Legacy expansion in order to enter here.				
                return true;
            }
            else if (!MLConfig.BlightedGrove && (int)m.AccessLevel < (int)AccessLevel.GameMaster)
            {
                m.SendLocalizedMessage(1042753, "Blighted Grove"); // ~1_SOMETHING~ has been temporarily disabled.
                return true;
            }
			
            BoneMachete machete = GetBoneMachete(m);
			
            if (machete != null)
            {
                if (Utility.RandomDouble() < 0.75 || machete.Insured || machete.LootType == LootType.Blessed)
                {
                    m.SendLocalizedMessage(1075008); // Your bone handled machete has grown dull but you still manage to force your way past the venomous branches.
                }
                else
                {
                    machete.Delete();
                    m.SendLocalizedMessage(1075007); // Your bone handled machete snaps in half as you force your way through the poisonous undergrowth.
                }
				
                return base.OnMoveOver(m);
            }
            else
                m.SendLocalizedMessage(1074275); // You are unable to push your way through the tangling roots of the mighty tree.
				
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

    public class BlightedGroveTreeInTeleFel : Teleporter
    { 
        [Constructable]
        public BlightedGroveTreeInTeleFel()
            : base(new Point3D(6574, 889, 0), Map.Felucca)
        {
        }

        public BlightedGroveTreeInTeleFel(Serial serial)
            : base(serial)
        {
        }

        public override bool OnMoveOver(Mobile m)
        {
            m.SendLocalizedMessage(1074162); // You notice a hole in the tree and climb down
            return base.OnMoveOver(m);
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

    public class BlightedGroveTreeOutTeleFel : Teleporter
    { 
        [Constructable]
        public BlightedGroveTreeOutTeleFel()
            : base(new Point3D(6488, 849, 40), Map.Felucca)
        {
        }

        public BlightedGroveTreeOutTeleFel(Serial serial)
            : base(serial)
        {
        }

        public override bool OnMoveOver(Mobile m)
        {
            m.SendLocalizedMessage(1074163); // You find a way to climb back outside the tree
            return base.OnMoveOver(m);
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