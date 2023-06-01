using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;
using System;



public abstract class UIRecivierBase : MonoBehaviour
{
    [SerializeField] protected UIPresenterBase _presenterPrefab;

    protected ColorsHolder _colorsHolder;
    protected DamageReciviersManager _damageRecivierManager;
    protected Gun_Camera_CS _gunCamera;
    protected CameraViewSetup _cameraSetup;
    protected Aiming_Control_CS _aimingControl;
    protected ID_Settings_CS _IDSettings;

    private void Start()
    {
        
    }
    public void InitialUIRecivier(DamageReciviersManager damageRecivierManager, Gun_Camera_CS gunCamera, CameraViewSetup cameraSetup, Aiming_Control_CS aimingControl, ColorsHolder colorsHolder, ID_Settings_CS IDSettings)
    {
        _damageRecivierManager = damageRecivierManager;
        _gunCamera = gunCamera;
        _cameraSetup = cameraSetup;
        _aimingControl = aimingControl;
        _colorsHolder = colorsHolder;
        _IDSettings = IDSettings;

        Subscribes();

        InstantiateCanvas();
    }

    protected virtual void InstantiateCanvas()
    {

    }
    protected virtual void Subscribes()
    {
        _damageRecivierManager.OnTankDestroyed.AddListener(TankDestroyed);
        _gunCamera.OnSwitchCamera.AddListener(SwitchCamera);
    }


    protected virtual void SwitchCamera(EActiveCameraType activeCamera)
    {
    }
    private void TankDestroyed(ID_Settings_CS bulletInitiatorID)
    {
        DestroyUI();
    }
    protected abstract void DestroyUI();
}
