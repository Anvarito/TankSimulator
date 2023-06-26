using System;
using ChobiAssets.PTM;

namespace Infrastructure.Services.KillCounter
{
    public interface IKillCounter : IService
    {
        public Action<ID_Settings_CS> OnEnemiesDestroyed { get; set; }
        public Action OnPlayersDestroyed { get; set; }
        public int PlayersDestroyed { get; }
        public int EnemiesDestroyed { get; }
    }
}