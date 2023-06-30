using UnityEngine;

public class TreeFaller : MonoBehaviour
{
    public Collider _collider;
    Vector3 axisRotate;
    private bool _isCollide = false;
    private GameObject _ref;
    public float _speed = 60;
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
            transform.parent.RotateAround(transform.position, axisRotate, -_speed * Time.deltaTime);
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
