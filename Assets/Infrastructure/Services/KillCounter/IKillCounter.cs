using System;

namespace Infrastructure.Services.KillCounter
{
    public interface IKillCounter : IService
    {
        public Action OnEnemiesDestroyed { get; set; }
        public Action OnPlayersDestroyed { get; set; }
        public int PlayersDestroyed { get; }
        public int EnemiesDestroyed { get; }
    }
}