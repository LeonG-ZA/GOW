using System;

namespace Server.Spells
{
    public class SamuraiMove : SpecialMove
    {
        public override SkillName MoveSkill
        {
            get
            {
                return SkillName.Bushido;
            }
        }
    }
}