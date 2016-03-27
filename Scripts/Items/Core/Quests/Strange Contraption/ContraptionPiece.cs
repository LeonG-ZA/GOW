using System;
using Server;
using Server.Network;
using Server.Items;
using System.Collections;
using System.Collections.Generic;
using Server.Engines.Quests.Collector;
using Server.Engines.Quests.Hag;

namespace Server.Items
{
    public class ContraptionPiece : AddonComponent
    {

        private static readonly TimeSpan UseDelay = TimeSpan.FromHours(1.0);

        private DateTime m_LastClicked;
        public DateTime LastClicked { get { return m_LastClicked; } set { m_LastClicked = value; } }

        [Constructable]
        public ContraptionPiece()
            : base(0x1927)
        {

        }

        public ContraptionPiece(Serial serial)
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

        public override void OnDoubleClick(Mobile from)
        {

            if (!from.InRange(GetWorldLocation(), 1))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return;
            }

            if (DateTime.UtcNow < m_LastClicked + TimeSpan.FromHours(1.0))
            {
                from.SendLocalizedMessage(1060001);//You throw the switch, but the mechanism cannot be engaged again so soon.
                return;
            }
           
                Container pack = from.Backpack as Container;
                if (pack == null)
                    return;
                {
                    Item PlagueBeastMutationCore = from.Backpack.FindItemByType(typeof(PlagueBeastMutationCore));
                    Item Obsidian = from.Backpack.FindItemByType(typeof(Obsidian));
                    Item MoonfireBrew = from.Backpack.FindItemByType(typeof(MoonfireBrew));

                    if (PlagueBeastMutationCore != null && Obsidian != null && MoonfireBrew != null)
                    {
                        from.PlaySound(0x21E);
                        from.SendLocalizedMessage(1055143);//You add the required ingredients and activate the contraption. It rumbles and smokes and then falls silent. The water shines for a brief moment, and you feel confident that it is now much less tainted then before. 
                        from.Karma += 500; //On OSI completion awards one dot in the Compassion Virtue

                        PlagueBeastMutationCore.Delete();
                        Obsidian.Delete();
                        MoonfireBrew.Delete();
                        LastClicked = DateTime.UtcNow;
                        return;

                    }
                    else
                    {
                        from.SendLocalizedMessage(1055142);//You do not have the necessary ingredients. The contraptions rumbles angrily but does nothing.
                    }
                }
            }
        }
    }

      

