using System;

namespace Server.Engines.ChampionSpawns
{
    public class RandomizeTimer : Timer
    {
        private ChampionSpawnController m_Controller;

        public RandomizeTimer(ChampionSpawnController controller, TimeSpan delay) : base(delay)
        {
            m_Controller = controller;
            Priority = TimerPriority.FiveSeconds;
        }

        protected override void OnTick()
        {
            m_Controller.Slice();
        }
    }
}