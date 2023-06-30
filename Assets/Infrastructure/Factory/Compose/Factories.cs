using System;
using System.Collections.Generic;
using Infrastructure.Factory.Base;
using Infrastructure.Services;
using Infrastructure.Services.KillCounter;
using Infrastructure.Services.Score;
using Infrastructure.Services.Timer;

namespace Infrastructure.Factory.Compose
{
    public class Factories : IFactories
    {
        public Dictionary<Type, IGameFactory> All { get; } = new Dictionary<Type, IGameFactory>();

        public void Add<TFactory>(TFactory factory) where TFactory : IGameFactory =>
            All.Add(typeof(TFactory), factory);

        public TFactory Single<TFactory>() where TFactory : class, IGameFactory =>
            All[typeof(TFactory)] as TFactory;

        public void CleanUp()
        {
        }
    }

    public interface IBattleServicesFacade : IService
    {
        public IKillCounter KillCounter { get; }
        public IScoreCounter ScoreCounter { get; }

        public ITimerService TimerService { get; }

    }
    
    public class BattleServicesFacade : IBattleServicesFacade
    {
        public IKillCounter KillCounter { get; }

        public IScoreCounter ScoreCounter { get; }

        public ITimerService TimerService { get; }

        public BattleServicesFacade(IKillCounter killCounter, IScoreCounter scoreCounter, ITimerService timerService)
        {
            KillCounter = killCounter;
            ScoreCounter = scoreCounter;
            TimerService = timerService;
        }

        public void CleanUp()
        {
        }
    }
}