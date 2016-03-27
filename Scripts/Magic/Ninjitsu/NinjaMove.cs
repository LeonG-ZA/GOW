using System;

namespace Server.Spells
{
    public class NinjaMove : SpecialMove
    {
        public override SkillName MoveSkill
        {
            get
            {
                return SkillName.Ninjitsu;
            }
        }
    }
}