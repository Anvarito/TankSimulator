using System;

namespace Infrastructure.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public LeadersHolder LeadersСoop;
        public LeadersHolder LeadersSurvival;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            LeadersСoop = new LeadersHolder();
            LeadersSurvival = new LeadersHolder();
        }
    }
}