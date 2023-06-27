using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsAct : MonoBehaviour
{
    [SerializeField] private Button _button;

    public void Launch()
    {
        _button.Select();
    }
}
