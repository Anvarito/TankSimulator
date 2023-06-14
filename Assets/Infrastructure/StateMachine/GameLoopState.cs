using System.Collections.Generic;
using ChobiAssets.PTM;
using Infrastructure.Factory;
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
        private int _countEnemys = 0;

        private int _allEnemyDestroyed;
        private int _playerEnemyDestroyed;
        private int _playerDestroyed;
        private Coroutine _gameTimeCoroutine;

        public GameLoopState(GameStateMachine gameStateMachine, ICoroutineRunner coroutineRunner, IFactories factories)
        {
            _gameStateMachine = gameStateMachine;
            _coroutineRunner = coroutineRunner;

            _playerFactory = factories.Single<IPlayerFactory>();
            _enemyFactory = factories.Single<IEnemyFactory>();

            //TEMP!!!!
            foreach(var i in _enemyFactory.EnemyDamageManagers)
            {
                if (i.GetComponentInParent<ID_Settings_CS>().Relationship != _playerFactory.PlayerParts[0].IdSettings.Relationship)
                    _countEnemys++;
            }
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

        private void PlayerDestroyed(ID_Settings_CS _killerID)
        {
            if (++_playerDestroyed >= _playerFactory.PlayerParts.Count)
                GameOver();
        }

        private void GameOver() =>
            _gameStateMachine.Enter<GameOverState, float>(_allEnemyDestroyed * _pointForEnemy);


        private void EnemyDestroyed(ID_Settings_CS _killerID)
        {
            //TEMP
            //if (_killerID.Relationship == _playerFactory.PlayerParts[0].IdSettings.Relationship)
            //    _playerEnemyDestroyed++;

            if (IsEnemiesDestroyed()) _gameStateMachine.Enter<VictoryState, float>(_playerEnemyDestroyed * _pointForEnemy);
        }

        private bool IsEnemiesDestroyed() =>
            ++_allEnemyDestroyed == _countEnemys;

        private void RegisterDamageManagers()
        {
            foreach (PlayerUiParts part in _playerFactory.PlayerParts)
                part.DamageReceiver.OnTankDestroyed.AddListener(PlayerDestroyed);

            foreach (DamageReciviersManager enemyDamageManager in _enemyFactory.EnemyDamageManagers)
            {
                //TEMP
                if (enemyDamageManager.GetComponentInParent<ID_Settings_CS>().Relationship != _playerFactory.PlayerParts[0].IdSettings.Relationship)
                {
                    _countEnemys++;
                    enemyDamageManager.OnTankDestroyed.AddListener(EnemyDestroyed);
                }
            }
        }

        private void UnregisterDamageManagers()
        {
            foreach (PlayerUiParts part in _playerFactory.PlayerParts)
                part.DamageReceiver.OnTankDestroyed.RemoveListener(PlayerDestroyed);

            foreach (DamageReciviersManager enemyDamageManager in _enemyFactory.EnemyDamageManagers)
            {
                enemyDamageManager.OnTankDestroyed.RemoveListener(EnemyDestroyed);
            }
        }
    }
}