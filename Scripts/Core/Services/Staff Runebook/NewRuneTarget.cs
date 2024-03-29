using System;
using Server;
using Server.Targeting;

namespace Server.SR
{
    public class SR_NewRuneTarget : Target
    {
        public SR_RuneAccount RuneAcc;
        public SR_NewRuneTarget(SR_RuneAccount runeAcc)
            : base(12, true, TargetFlags.None)
        {
            this.RuneAcc = runeAcc;
        }

        protected override void OnTarget(Mobile mob, object targ)
        {
            Point3D loc = new Point3D(0, 0, 0);
            if (targ is LandTarget)
                loc = (targ as LandTarget).Location;
            else if (targ is StaticTarget)
                loc = (targ as StaticTarget).Location;
            else if (targ is Mobile)
                loc = (targ as Mobile).Location;
            else if (targ is Item)
                loc = (targ as Item).Location;

            mob.SendMessage("Enter a description:");
            mob.Prompt = new SR_NewRunePrompt(this.RuneAcc, loc, mob.Map);
        }
    }
}