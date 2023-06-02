using System.Linq;
using UnityEngine;

namespace Infrastructure.Gizmos_Debug
{
    public class WaypointsVisualDebug : MonoBehaviour
    {
        private const float Radius = .5f;

        public void OnDrawGizmos()
        {
            Transform[] children = GetComponentsInChildren<Transform>().Where(x => x != transform).ToArray();
            if (children.Length < 2) return;

        
            for (var index = 0; index < children.Length - 1; index++)
            {
                Vector3 tempPoint = children[index].position;
                Vector3 nextPoint = children[index + 1].position;
            
                UnityEngine.Gizmos.color = Color.red;
            
                UnityEngine.Gizmos.DrawLine(tempPoint, nextPoint);
                UnityEngine.Gizmos.DrawSphere(nextPoint, Radius);
            }
        
            UnityEngine.Gizmos.DrawSphere(children.First().position, Radius);
            UnityEngine.Gizmos.DrawLine(children.First().position, children.Last().position);
        }
    }
}