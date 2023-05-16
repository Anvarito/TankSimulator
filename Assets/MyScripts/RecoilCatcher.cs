using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;
using System;

public class RecoilCatcher : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Cannon_Fire_CS _cannon_Fire_CS;
    [SerializeField] private Rigidbody _targetRigidbody;
    void Start()
    {
        _cannon_Fire_CS.OnRecoil.AddListener(RecoilBodyDeal);
    }

    private void RecoilBodyDeal(Vector3 force, Vector3 position)
    {
        _targetRigidbody.AddForceAtPosition(force, position, ForceMode.Impulse);
    }
}
