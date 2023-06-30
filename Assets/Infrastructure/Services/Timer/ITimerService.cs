using System;

namespace Infrastructure.Services.Timer
{
    public interface ITimerService : IService
    {
        bool IsPaused { get; }

        public float CurrentSeconds { get; }
        public void StartTimer(float seconds, Action onTimerEnd);
        public void PauseTimer();
        public void StopTimer();
    }
}