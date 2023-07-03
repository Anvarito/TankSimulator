using ChobiAssets.PTM;
using UnityEngine;

public class InputInitializerAI : InputInitializerBase
{
    [Space(30)]
    [SerializeField] private AI_CS ai_core_script;

    private void Start()
    {
        Initialize();
    }
    protected override void TankDestroyed(ID_Settings_CS tankId, ID_Settings_CS bulletInitiatorID)
    {
        base.TankDestroyed(tankId, bulletInitiatorID);
        ai_core_script.TankDestroyed();
    }

    protected override void SetypControlsByType()
    {
        base.SetypControlsByType();
        _driveControlType = new Drive_Control_Input_99_AI_CS();
        _fireControlType = new Cannon_Fire_Input_99_AI_CS(ai_core_script);
    }
}
