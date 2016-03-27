using System;
using Server;

namespace Server.VeteranRewardsConfiguration
{
    class VeteranRewardsConfig
    {
        /// <summary>
        /// Change to false to disable veterain rewards completely
        /// </summary>
        public static bool RewardEnabled = true;
        /// <summary>
        /// The base skill cap for all players
        /// </summary>
        public static int SkillBonusCap = 7200;
        /// <summary>
        /// If true character skill caps will be increased based on reward level. Note
        /// that this is no longer the case on OSI servers.
        /// </summary>
        public static bool SkillCapRewards = false;
        /// <summary>
        /// The skill cap bonus per level
        /// </summary>
        public static int SkillCapBonusIncrement = 5;
        /// <summary>
        /// The maximum number of levels to give skill cap bonuses
        /// </summary>
        public static int SkillCapBonusLevels = 4;
        /// <summary>
        /// Change to false to disable veterain rewards completely
        /// </summary>
        public static bool SkillCapVetRewardEnabled = true;
        /// <summary>
        /// The time duration per reward level. On OSI servers this would be one year,
        /// but G.O.W servers have always defaulted to 90 days.
        /// </summary>
        public static TimeSpan RewardInterval = TimeSpan.FromDays(90.0);
        /// <summary>
        /// The starting level for a new account. Normally this is 0. Use this to give
        /// new players vet rewards or for testing vet rewards. Not that this is *not*
        /// the number of rewards available, but the number of "years" old the account
        /// defaults to.
        /// </summary>
        public static int StartingLevel = 0;
        /// <summary>
        /// If true, a player's account must be old enough to have recieved the reward
        /// to be able to use it. This functionality was disabled in OSI publish 83.
        /// Broken will fix later.
        /// </summary>
        //public static bool AgeCheckOnUse = false;
    }
}
