using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCollider : MonoBehaviour
{
    [ContextMenu("Create")]
    public void CreateCollider()
    {
        foreach(Transform i in transform)
        {
            if (!i.GetChild(0).TryGetComponent(out BoxCollider boxCollider))
            {
                i.GetChild(0).gameObject.AddComponent<BoxCollider>();
            }
        }
    }
}
