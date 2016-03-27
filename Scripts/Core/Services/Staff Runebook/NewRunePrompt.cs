using System;
using Server;
using Server.Prompts;

namespace Server.SR
{
    public class SR_NewRunePrompt : Prompt
    {
        public SR_RuneAccount RuneAcc;
        public bool IsRunebook;
        public Point3D TargetLoc;
        public Map TargetMap;
        public SR_NewRunePrompt(SR_RuneAccount runeAcc)
        {
            this.RuneAcc = runeAcc;
            this.IsRunebook = true;
        }

        public SR_NewRunePrompt(SR_RuneAccount runeAcc, Point3D targetLoc, Map targetMap)
        {
            this.RuneAcc = runeAcc;
            this.TargetLoc = targetLoc;
            this.TargetMap = targetMap;
        }

        public override void OnResponse(Mobile mob, string text)
        {
            text = text.Trim();

            if (text.Length > 40)
                text = text.Substring(0, 40);

            if (text.Length > 0)
            {
                SR_Rune rune = null;
                if (this.IsRunebook)
                    rune = new SR_Rune(text, true);
                else
                    rune = new SR_Rune(text, this.TargetMap, this.TargetLoc);

                if (this.RuneAcc.ChildRune == null)
                    this.RuneAcc.AddRune(rune);
                else
                    this.RuneAcc.ChildRune.AddRune(rune);
            }

            SR_Gump.Send(mob, this.RuneAcc);
        }
    }
}