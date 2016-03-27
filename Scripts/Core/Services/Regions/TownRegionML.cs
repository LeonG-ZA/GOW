using System;
using System.Xml;
using Server.Spells.Chivalry;
using Server.Spells.Fourth;
using Server.Spells.Seventh;
using Server.Spells.Sixth;

namespace Server.Regions
{
    public class TownRegionML : GuardedRegion
    {
        public TownRegionML(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        public override bool OnBeginSpellCast(Mobile m, ISpell s)
        {
            if ((s is GateTravelSpell || s is RecallSpell || s is MarkSpell || s is SacredJourneySpell) && m.IsPlayer())
            {
                m.SendLocalizedMessage(501802); // Thy spell doth not appear to work...

                return false;
            }

            return base.OnBeginSpellCast(m, s);
        }
    }
}