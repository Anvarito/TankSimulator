using UnityEngine;

public class InputControlHolder : MonoBehaviour
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
        _newControl.TankMovement.Enable();
    }

    private void OnDisable()
    {
        _newControl.TankMovement.Disable();
    }

}
