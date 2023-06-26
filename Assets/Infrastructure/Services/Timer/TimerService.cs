using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.Timer
{
    public class TimerService : ITimerService
    {
        public bool IsPaused { get; private set; }
        
        private readonly ICoroutineRunner _coroutineRunner;
        private Coroutine _timerCoroutine;
        private Action _currentOnTimerEnd;
        private float _currentSeconds;

        public TimerService(ICoroutineRunner coroutineRunner) =>
            _coroutineRunner = coroutineRunner;

        public void StartTimer(float seconds, Action onTimerEnd)
        {
            _currentSeconds = seconds;
            _currentOnTimerEnd = onTimerEnd;
            _timerCoroutine = _coroutineRunner.StartCoroutine(Timer());
        }

        public void StopTimer()
        {
            _coroutineRunner.StopCoroutine(_timerCoroutine);
            IsPaused = false;
            _currentSeconds = 0;
            _currentOnTimerEnd = null;
        }

        public void PauseTimer()
        {
            if (IsPaused)
            {
                if (_currentSeconds > 0)
                {
                    _coroutineRunner.StartCoroutine(Timer());
                    IsPaused = !IsPaused;
                }
                else
                {
                    Debug.Log("You cannot unpause timer if it's already stopped");
                }
            }
            else
            {
                if (_currentSeconds > 0)
                {
                    _coroutineRunner.StopCoroutine(_timerCoroutine);
                    IsPaused = !IsPaused;
                }
            }
        }

        public void CleanUp() =>
            StopTimer();

        private IEnumerator<WaitForSeconds> Timer()
        {
            while (_currentSeconds > 0)
            {
                _currentSeconds -= Time.deltaTime;
                yield return null;
            }

            _currentOnTimerEnd?.Invoke();
        }
    }
}