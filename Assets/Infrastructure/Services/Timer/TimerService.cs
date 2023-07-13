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
        public float CurrentSeconds { get; private set; }

        public TimerService(ICoroutineRunner coroutineRunner) =>
            _coroutineRunner = coroutineRunner;

        public void StartTimer(float seconds, Action onTimerEnd)
        {
            CurrentSeconds = seconds;
            _currentOnTimerEnd = onTimerEnd;
            _timerCoroutine = _coroutineRunner.StartCoroutine(Timer());
        }

        public void StopTimer()
        {
            if (_timerCoroutine != null)
                _coroutineRunner.StopCoroutine(_timerCoroutine);
            IsPaused = false;
            CurrentSeconds = 0;
            _timerCoroutine = null;
            _currentOnTimerEnd = null;
        }

        public void PauseTimer()
        {
            if (IsPaused)
            {
                if (CurrentSeconds > 0)
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
                if (CurrentSeconds > 0)
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
            while (CurrentSeconds > 0)
            {
                CurrentSeconds -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
                if (IsPaused) yield break;
            }

            _currentOnTimerEnd?.Invoke();
        }
    }
}