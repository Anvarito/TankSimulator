using System;

namespace Infrastructure.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public LeadersHolder FactoryLeaders;
        public LeadersHolder PoligonLeaders;
        public LeadersHolder WinterLeaders;
        public LeadersHolder SummerLeaders;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            FactoryLeaders = new LeadersHolder();
            PoligonLeaders = new LeadersHolder();
            WinterLeaders = new LeadersHolder();
            SummerLeaders = new LeadersHolder();
        }

    }
}