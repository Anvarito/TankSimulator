using System.Collections;
using System.Collections.Generic;
using ChobiAssets.PTM;
using UnityEngine;

public class RecivierUIManager : MonoBehaviour
{
    private List<UIRecivierBase> _uiReciviers = new List<UIRecivierBase>();

    [SerializeField] private DamageReciviersManager _damageRecivierManager;
    [SerializeField] private Gun_Camera_CS _gunCamera;
    [SerializeField] private CameraViewSetup _cameraSetup;
    [SerializeField] private Aiming_Control_CS _aiming_Control;
    private void Awake()
    {
        _uiReciviers.AddRange(GetComponents<UIRecivierBase>());

        foreach (var recivier in _uiReciviers)
        {
            if(recivier.enabled == true)
            recivier.InitialUIRecivier(_damageRecivierManager, _gunCamera, _cameraSetup, _aiming_Control);
        }
    }
}
