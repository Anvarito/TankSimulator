using System;
using System.Collections;
using System.Collections.Generic;
using ChobiAssets.PTM;
using UnityEngine;

[System.Serializable]
public struct ColorsHolder
{
    public Color mainColor;
    public Color secondaryColor;
}
public class RecivierUIManager : MonoBehaviour
{
    private List<UIRecivierBase> _uiReciviers = new List<UIRecivierBase>();

    [SerializeField] private DamageReciviersManager _damageRecivierManager;
    [SerializeField] private Gun_Camera_CS _gunCamera;
    [SerializeField] private CameraViewSetup _cameraSetup;
    [SerializeField] private Aiming_Control_CS _aiming_Control;
    [SerializeField] private PositionActorsMarkerRecivier _UI_Position_Marker_Control_CS;
    [SerializeField] private ID_Settings_CS _selfID;
    [SerializeField] private ColorsHolder _colorsHolder;
    internal void Initialize()
    {
        _uiReciviers.AddRange(GetComponents<UIRecivierBase>());

        foreach (var recivier in _uiReciviers)
        {
            if (recivier.enabled == true)
                recivier.InitialUIRecivier(_damageRecivierManager, _gunCamera, _cameraSetup, _aiming_Control, _colorsHolder, _selfID);
        }
    }
}
