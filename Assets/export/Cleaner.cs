using System.Collections.Generic;
using UnityEngine;

public class Cleaner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        List<Collider> list = new List<Collider>();
        
        //list.Clear();

        foreach(Transform i in transform)
        {
            list.AddRange(i.GetComponentsInChildren<Collider>());
        }

        for (int i = 0; i < list.Count; i++)
        {
            DestroyImmediate(list[i].GetComponent<Collider>());
        }
    }
}
