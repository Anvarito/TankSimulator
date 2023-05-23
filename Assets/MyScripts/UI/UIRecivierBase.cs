using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;
using System;

public abstract class UIRecivierBase : MonoBehaviour
{
    [Header("Canvas prefab")]
    [SerializeField] protected UIPresenterBase _uiPrefab;
    [Space(10)]

    protected DamageReciviersManager _damageRecivierManager;
    protected Gun_Camera_CS _gunCamera;
    protected CameraViewSetup _cameraSetup;
    protected Aiming_Control_CS _aimingControl;

    private void Start()
    {
        
    }
    public void InitialUIRecivier(DamageReciviersManager damageRecivierManager, Gun_Camera_CS gunCamera, CameraViewSetup cameraSetup, Aiming_Control_CS aimingControl)
    {
        _damageRecivierManager = damageRecivierManager;
        _gunCamera = gunCamera;
        _cameraSetup = cameraSetup;
        _aimingControl = aimingControl;

        Subscribes();

        InstantiateCanvas();
    }

    protected virtual void InstantiateCanvas()
    {

    }
    protected virtual void Subscribes()
    {
        _damageRecivierManager.OnTankDestroyed.AddListener(DestroyUI);
        _gunCamera.OnSwitchCamera.AddListener(SwitchCamera);
    }


    protected virtual void SwitchCamera(EActiveCameraType activeCamera)
    {
    }

    protected abstract void DestroyUI();
}
