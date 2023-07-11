using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIshaker : MonoBehaviour
{
    float _sin;
    public float _speed;
    public float _amplitude;

    private Vector3 _startPos;
    private void Start()
    {
        _startPos = transform.position;
    }
    private void Update()
    {
        _sin = Mathf.Sin(Time.time * _speed) * _amplitude;
        transform.position = new Vector3(transform.position.x, _startPos.y + _sin, transform.position.z);
    }
}
