using ChobiAssets.PTM;
using UnityEngine;

public class TreeFaller : MonoBehaviour
{
    public Transform root;
    public Collider _collider;
    public GameObject DebugCallider;
    public float _speed = 60;

    private bool _isCollide = false;
    private Vector3 _colisionPoint;
    private float _accumAngle = 0;
    private Transform rotateObj;
    private Vector3 axisRotate;

    void Start()
    {
        if (root == null)
            rotateObj = transform.parent;
        else
            rotateObj = root;

        if (_collider == null)
            _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isCollide)
        {
            _accumAngle += _speed * Time.deltaTime;

            Quaternion rotation = Quaternion.AngleAxis(-_speed * Time.deltaTime, axisRotate);
            rotateObj.transform.rotation *= rotation;
            //print(_accumAngle);
            if (_accumAngle > 100)
                Destroy(rotateObj.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollide)
            return;

        if (collision.gameObject.layer == Layer_Settings_CS.Body_Layer
            || collision.gameObject.layer == Layer_Settings_CS.Bullet_Layer)
        {
            _colisionPoint = collision.contacts[0].point;
            Lauch();
        }
    }

    [ContextMenu("Launch")]
    public void DebugLaunch()
    {
        _colisionPoint = DebugCallider.transform.position;
        Lauch();
    }
    public void Lauch()
    {
        Vector3 RotatedObject = new Vector3(rotateObj.position.x, _colisionPoint.y, rotateObj.position.z);
        Vector3 toCenter = RotatedObject - _colisionPoint;
        axisRotate = Vector3.Cross(toCenter, Vector3.up).normalized;

        //Debug.DrawRay(_colisionPoint, toCenter, Color.red, 100);
        //Debug.DrawLine(toCenter + _colisionPoint, toCenter + _colisionPoint + Vector3.up, Color.blue, 100);
        //Debug.DrawLine(toCenter + _colisionPoint, _colisionPoint + toCenter + axisRotate, Color.yellow, 100);

        _isCollide = true;
        _collider.enabled = false;
    }

    [ContextMenu("Reset")]
    public void ResetAll()
    {
        _isCollide = false;
        _accumAngle = 0;
        rotateObj.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

}
