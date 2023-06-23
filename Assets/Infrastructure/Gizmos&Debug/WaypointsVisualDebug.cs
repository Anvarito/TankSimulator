using System.Linq;
using Infrastructure.Services.StaticData.Waypoints;
using UnityEngine;

namespace Infrastructure.Gizmos_Debug
{
    public class WaypointsVisualDebug : MonoBehaviour
    {
        private const float Radius = .5f;
        public bool _enabled = true;
        public WaypointsPackId PackId;

        public void OnDrawGizmos()
        {
            if (!_enabled) return;
            Transform[] children = GetComponentsInChildren<Transform>().Where(x => x != transform).ToArray();
            if (children.Length < 2) return;

        
            for (var index = 0; index < children.Length - 1; index++)
            {
                Vector3 tempPoint = children[index].position;
                Vector3 nextPoint = children[index + 1].position;
            
                Gizmos.color = Color.blue;
            
                Gizmos.DrawLine(tempPoint, nextPoint);
                Gizmos.DrawSphere(nextPoint, Radius);
            }
        
            Gizmos.DrawSphere(children.First().position, Radius);
            Gizmos.DrawLine(children.First().position, children.Last().position);
        }

        private void OnValidate()
        {
            gameObject.name = "WayPoint_" + PackId.ToString();
        }
    }
}