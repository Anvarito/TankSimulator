using UnityEngine;

public class MenuTankBehaviour : MonoBehaviour
{
    public Transform _turret;
    float _sinTurret;
    public float _speed;
    public float _amplitude;

    private Quaternion _startRot;
    private void Start()
    {
        _startRot = _turret.rotation;
    }
    private void Update()
    {
        _sinTurret = Mathf.Sin(Time.time * _speed) * _amplitude;
        Quaternion result = _startRot * Quaternion.Euler(0, _sinTurret, 0);
        _turret.rotation = result;
    }
}
