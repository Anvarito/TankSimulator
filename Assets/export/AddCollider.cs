using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCollider : MonoBehaviour
{
    [ContextMenu("Create")]
    public void CreateCollider()
    {
        foreach (Transform i in transform)
        {
            if (i.name.Contains("Road_2L"))
            {
                if (i.GetChild(0).gameObject.TryGetComponent(out BoxCollider boxCollider0))
                {
                    DestroyImmediate(boxCollider0);
                }
            }
        }
    }
}
