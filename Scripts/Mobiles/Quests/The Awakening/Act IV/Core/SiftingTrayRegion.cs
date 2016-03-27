using System;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Spells.Eighth;
using Server.Spells.Chivalry;
using Server.SkillHandlers;


namespace Server.Regions
{
    public class SiftingTrayRegion : BaseRegion
    {
        public SiftingTrayRegion(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            if (from.AccessLevel == AccessLevel.Player)
                return false;
            else
                return base.AllowHousing(from, p);
        }
    }
}
	
