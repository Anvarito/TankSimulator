using UnityEngine;

public class Replacer : MonoBehaviour
{
    // Start is called before the first frame update
    public bool _ToDynamic = true;
    [ContextMenu("Replace")]
    public void Replace()
    {
        Transform target = _ToDynamic ? GameObject.Find("_Props").transform : GameObject.Find("StaticProps").transform;
        transform.parent = target;
    }
       

}
