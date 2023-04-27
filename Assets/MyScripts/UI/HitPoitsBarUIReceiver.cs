using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;
public class HitPoitsBarUIReceiver : MonoBehaviour
{
    [SerializeField] private UI_HP_Bars_Self_CS _ui_HP_Bars_Self_CSprefab;
    [SerializeField] private DamageManager _damageControlManager;
    private UI_HP_Bars_Self_CS _ui_HP_Bars_Self_CS;
    void Start()
    {
        _ui_HP_Bars_Self_CS = Instantiate(_ui_HP_Bars_Self_CSprefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
