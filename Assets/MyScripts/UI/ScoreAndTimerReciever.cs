using System;
using ChobiAssets.PTM;
using Infrastructure.Services.KillCounter;
using Infrastructure.Services.Score;
using Infrastructure.Services.Timer;

internal class ScoreAndTimerReciever : UIRecivierBase
{
    private ITimerService _timerService;
    private IScoreCounter _scoreCounter;
    ScoreAndTimerPresenter _scoreAndTimerPresenter;
    internal void Init(ITimerService timerService, IScoreCounter scoreCounter, Gun_Camera_CS gunCamera, CameraViewSetup cameraView, ID_Settings_CS selfID)
    {
        _timerService = timerService;
        _scoreCounter = scoreCounter;
        _gunCamera = gunCamera;
        _cameraSetup = cameraView;
        InitialUIRecivier();

        _scoreAndTimerPresenter = _spawnedPresenter as ScoreAndTimerPresenter;
        _scoreAndTimerPresenter.SetCamera(_cameraSetup.GetCamera());

        _scoreAndTimerPresenter.ScoreUI.Init(selfID);
        _scoreCounter.OnEnemiesDestroyed += EnemiesDestroyed;
    }

    private void OnDestroy()
    {
        _scoreCounter.OnEnemiesDestroyed -= EnemiesDestroyed;
    }
    private void EnemiesDestroyed(ID_Settings_CS killer)
    {
        _scoreAndTimerPresenter.ScoreUI.UpdateScore(_scoreCounter.Score, killer);
    }

    private void Update()
    {
        if(_scoreAndTimerPresenter != null)
            _scoreAndTimerPresenter.TimerUI.UpdateTimer(_timerService.CurrentSeconds);
    }

    protected override void SwitchCamera(EActiveCameraType activeCamera)
    {
        base.SwitchCamera(activeCamera);
        _spawnedPresenter.SetCamera(activeCamera == EActiveCameraType.GunCamera ? _cameraSetup.GetGunCamera() : _cameraSetup.GetCamera());
    }
    protected override void DestroyUI() { }
}