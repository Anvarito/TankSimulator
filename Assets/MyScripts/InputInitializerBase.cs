using ChobiAssets.PTM;
using UnityEngine;

public class InputInitializerBase : MonoBehaviour
{
    [Header("Main links")]
    [SerializeField] protected DamageReciviersManager _damageManager;
    [SerializeField] protected Turret_Finishing_CS _turret_Finishing;

    [Space(30)]
    [SerializeField] protected Drive_Control_CS _driveControl;
    [SerializeField] protected Cannon_Fire_CS _fireControl;
    [SerializeField] protected Aiming_Control_CS _aimingControl;

    protected Drive_Control_Input_00_Base_CS _driveControlType;
    protected Cannon_Fire_Input_00_Base_CS _fireControlType;
    protected Aiming_Control_Input_00_Base_CS _aimingControlType;

    public virtual void Initialize()
    {
        _turret_Finishing.Assembling();

        SetypControlsByType();

        _damageManager.OnTankDestroyed.AddListener(TankDestroyed);
        InititalizeControlls();
    }

    protected virtual void TankDestroyed(ID_Settings_CS bulletInitiatorID)
    {
        transform.tag = Layer_Settings_CS.FinishTag;

        _driveControl.TankDestroyed();
        _fireControl.TankDestroyed();
        _aimingControl.TankDestroyed();
    }

    protected virtual void InititalizeControlls()
    {
        if (_driveControl != null)
            _driveControl.Initialize(_driveControlType);
        else
            Debug.LogError("Drive_Control_CS is not linked!!!");

        if (_fireControl != null)
            _fireControl.Initialize(_fireControlType);
        else
            Debug.LogError("Canon_Fire_CS is not linked!!!");

        if (_aimingControl != null)
            _aimingControl.Initialize(_aimingControlType);
        else
            Debug.LogError("Aiming_Control_CS is not linked!!!");
    }

    protected virtual void SetypControlsByType()
    {

    }
}
