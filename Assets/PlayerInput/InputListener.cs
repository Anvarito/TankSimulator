using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    // Start is called before the first frame update
    private NewControl _newControl;
    private void Awake()
    {
        _newControl = GetControl();
    }

    public NewControl GetControl()
    {
        if (_newControl == null)
            _newControl = new NewControl();

        return _newControl;
    }

    private void OnEnable()
    {
        _newControl.Tank.Enable();
    }

    private void OnDisable()
    {
        _newControl.Tank.Disable();
    }

}
