using System.Collections.Generic;
using ChobiAssets.PTM;
using Infrastructure.Factory.Base;
using Infrastructure.Factory.Compose;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class GameLoopState : IState
    {
        private const int TimeInMinutes = 5;
        private const int SecondInMinute = 60;
        private const int GameTime = SecondInMinute * TimeInMinutes;
        private const float _pointForEnemy = 100f;

        private readonly GameStateMachine _gameStateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IPlayerFactory _playerFactory;
        private readonly IEnemyFactory _enemyFactory;
        
        private int _enemyDestroyed;
        private Coroutine _gameTimeCoroutine;

        public GameLoopState(GameStateMachine gameStateMachine, ICoroutineRunner coroutineRunner, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _coroutineRunner = coroutineRunner;
            
            _playerFactory = factories.Single<IPlayerFactory>();
            _enemyFactory = factories.Single<IEnemyFactory>();
        }

        public void Enter()
        {
            Debug.Log($"Entered {this.GetType().Name}");

            RegisterDamageManagers();

            _gameTimeCoroutine = _coroutineRunner.StartCoroutine(GameTimer(GameTime));
        }

        public void Exit()
        {
            UnregisterDamageManagers();
            
            _coroutineRunner.StopCoroutine(_gameTimeCoroutine);
        }

        private IEnumerator<WaitForSeconds> GameTimer(float time)
        {
            yield return new WaitForSeconds(time);
            GameOver();
        }

        private void PlayerDestroyed()
        {
            GameOver();
        }

        private void GameOver() => 
            _gameStateMachine.Enter<GameOverState>();

        private void EnemyDestroyed()
        {
            if (IsEnemiesDestroyed()) _gameStateMachine.Enter<VictoryState,float>(_enemyDestroyed * _pointForEnemy);
        }

        private bool IsEnemiesDestroyed() => 
            ++_enemyDestroyed == _enemyFactory.EnemyDamageManagers.Count;

        private void RegisterDamageManagers()
        {
            _playerFactory.PlayerParts.DamageReceiver.OnTankDestroyed.AddListener(PlayerDestroyed);

            foreach (DamageReciviersManager enemyDamageManager in _enemyFactory.EnemyDamageManagers)
            {
                enemyDamageManager.OnTankDestroyed.AddListener(EnemyDestroyed);
            }
        }

        private void UnregisterDamageManagers()
        {
            _playerFactory.PlayerParts.DamageReceiver.OnTankDestroyed.RemoveListener(PlayerDestroyed);

            foreach (DamageReciviersManager enemyDamageManager in _enemyFactory.EnemyDamageManagers)
            {
                enemyDamageManager.OnTankDestroyed.RemoveListener(EnemyDestroyed);
            }
        }
    }
}