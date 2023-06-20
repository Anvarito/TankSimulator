using System.Collections.Generic;
using ChobiAssets.PTM;
using Infrastructure.Services.StaticData.Waypoints;
using UnityEngine;

namespace Infrastructure.TestMono
{
    public class SpawnMarker : MonoBehaviour
    {
        public bool EnabledGizmo = true;
        public bool IsOccupied { get; set; }

        public ERelationship Relationship;
        public EPlayerType ActorType;
        public WaypointsPackId WaypointsPackId;

        public GameObject WayPointsPack;

        private static Dictionary<ERelationship, Color> _colors = new Dictionary<ERelationship, Color>()
        {
            [ERelationship.TeamA] = Color.green,
            [ERelationship.TeamB] = Color.red,
        };

        public void OnDrawGizmos()
        {
            if (!EnabledGizmo) return;

            Gizmos.color = ForTeam(Relationship);

            switch (ActorType)
            {
                case (EPlayerType.AI):
                    Gizmos.DrawCube(transform.position, transform.localScale);
                    break;
                case (EPlayerType.Player):
                    Gizmos.DrawSphere(transform.position, transform.localScale.magnitude);
                    break;
            }
        }

        private Color ForTeam(ERelationship team) =>
            _colors.TryGetValue(team, out Color color)
                ? color
                : Color.white;
    }
}