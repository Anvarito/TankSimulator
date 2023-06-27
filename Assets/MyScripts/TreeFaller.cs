using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFaller : MonoBehaviour
{
    public Collider _collider;
    public LODGroup _lODGroup;
    Vector3 axisRotate;
    private bool _isCollide = false;
    private GameObject _ref;
    public float _speed = 2;
    float _accumAngle = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isCollide)
        {
            _accumAngle += _speed * Time.deltaTime;
            foreach(LOD i in _lODGroup.GetLODs())
            {
                i.renderers[0].transform.RotateAround(transform.position, axisRotate, -_speed * Time.deltaTime);
            }
            print(_accumAngle);
            if (_accumAngle > 100)
                Destroy(transform.parent.gameObject);
                //ResetAll();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollide)
            return;

        _ref = collision.gameObject;
        Lauch();

    }

    [ContextMenu("Launch")]
    public void Lauch()
    {
        _isCollide = true;
        Vector3 toCenter = (transform.position - _ref.transform.position).normalized;
        axisRotate = Vector3.Cross(toCenter, transform.up);
        _collider.enabled = false;
    }

    [ContextMenu("Reset")]
    public void ResetAll()
    {
        _isCollide = false;
        _accumAngle = 0;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

}
