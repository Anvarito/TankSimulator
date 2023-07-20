using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMapScaler : MonoBehaviour
{
    public float _scale;
#if UNITY_EDITOR
    [ContextMenu("Scale")]
    public void Scale()
    {
        List<MeshRenderer> mrList = new List<MeshRenderer>();
        mrList.AddRange(GetComponentsInChildren<MeshRenderer>());
        foreach (var i in mrList)
        {
            i.scaleInLightmap = _scale;
        }
    }
#endif
}
