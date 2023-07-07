using UnityEngine;

public class AddCollider : MonoBehaviour
{
    [ContextMenu("Create")]
    public void CreateCollider()
    {
        foreach (Transform i in transform)
        {
            if (i.GetChild(0).gameObject.TryGetComponent(out TreeFaller boxCollider0))
            {
                boxCollider0._isFreeFall = false;
            }
        }
    }
}
