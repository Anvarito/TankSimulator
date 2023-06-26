using System;
using ChobiAssets.PTM;

namespace Infrastructure.Services.Score
{
    public interface IScoreCounter : IService
    {
        public float Score { get; }
        public Action<ID_Settings_CS> OnEnemiesDestroyed { get; set; }
        public void LoadData();
    }
}