using UnityEngine;
using ChobiAssets.PTM;


public abstract class UIRecivierBase : MonoBehaviour
{
    [SerializeField] private UIPresenterBase _presenterPrefab;
     protected UIPresenterBase _spawnedPresenter;

    protected Gun_Camera_CS _gunCamera;
    protected CameraViewSetup _cameraSetup;

    protected UIPresenterBase GetPrefabPresenter => _presenterPrefab;

    protected void InitialUIRecivier()
    {
        Subscribes();
        InstantiateCanvas();
    }

    protected virtual void InstantiateCanvas()
    {
        _spawnedPresenter = Instantiate(_presenterPrefab);
        _spawnedPresenter.InitialCanvas();
        _spawnedPresenter.SetCamera(_cameraSetup.GetCamera());
    }
    protected virtual void Subscribes()
    {
        _gunCamera.OnSwitchCamera.AddListener(SwitchCamera);
    }


    protected virtual void SwitchCamera(EActiveCameraType activeCamera)
    {
    }
    public void PlayerDestoryed()
    {
        Destroy(_spawnedPresenter.gameObject);

        _gunCamera.OnSwitchCamera.RemoveListener(SwitchCamera);
        DestroyUI();
        Destroy(this);
    }
    protected abstract void DestroyUI();
}
