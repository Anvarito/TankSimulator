using ChobiAssets.PTM;
using UnityEngine;

public class TreeFaller : MonoBehaviour
{
    public Transform root;
    public Collider _collider;
    public float _speed = 60;

    public bool _isFreeFall = true;

    private bool _isCollide = false;
    private Vector3 _colisionDirection;
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
        if (_isCollide && !_isFreeFall)
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
            if (!_isFreeFall)
            {
                _colisionDirection = (collision.contacts[0].point - collision.transform.position).normalized;
                axisRotate = Vector3.Cross(_colisionDirection, rotateObj.up);
                _collider.enabled = false;

                Debug.DrawLine(collision.transform.position, collision.transform.position + _colisionDirection, Color.red, 100);
                Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + rotateObj.up, Color.blue, 100);
                Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + axisRotate, Color.green, 100);
            }
            else
            {
                Transform parent = rotateObj;
                transform.parent = null;
                Destroy(parent.gameObject);
                gameObject.layer = 15;
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                Destroy(gameObject, 5);
            }

            // print(Vector3.Dot(collision.transform.forward, rotateObj.forward));

            _isCollide = true;
        }
    }
}
