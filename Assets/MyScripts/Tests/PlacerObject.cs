using System.Collections.Generic;
using UnityEngine;

public class PlacerObject : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> Trees;
    public Transform Container;

    [ContextMenu("Clear")]
    public void DeletaAll()
    {
        List<Transform> spawned = new List<Transform>();
        foreach(Transform tr in Container.transform)
        {
            spawned.Add(tr);
        }

        foreach (Transform tr in spawned)
        {
            DestroyImmediate(tr.gameObject);

        }

        spawned.Clear();
    }

}
