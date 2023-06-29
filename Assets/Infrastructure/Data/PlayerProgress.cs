using System;

namespace Infrastructure.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public LeadersHolder Leaders;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            Leaders = new LeadersHolder();
        }
    }
}