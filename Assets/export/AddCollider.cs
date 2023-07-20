using UnityEngine;

public class AddCollider : MonoBehaviour
{
    [ContextMenu("Create")]
    public void CreateCollider()
    {
        foreach (Transform i in transform)
        {
            i.GetChild(0).GetComponent<MeshRenderer>().scaleInLightmap = 0.1f;
            try
            {
                i.GetChild(1).GetComponent<MeshRenderer>().scaleInLightmap = 0.05f;
            }
            catch
            {

            }
            try
            {

            }
            catch
            {

                i.GetChild(2).GetComponent<MeshRenderer>().scaleInLightmap = 0.025f;
            }

        }
    }
}
