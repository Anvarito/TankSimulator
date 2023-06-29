using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamperRollover : MonoBehaviour
{
    Rigidbody rigidbody;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.localRotation.eulerAngles.x) >= 50)
        {
            print("Stop rollover x");
            rigidbody.AddRelativeTorque(1, 0, 0);
        }

        if (Mathf.Abs(transform.localRotation.eulerAngles.z) >= 50)
        {
            print("Stop rollover y");
            rigidbody.AddRelativeTorque(0, 0, 1);
        }
    }
}
