using System;
using ChobiAssets.PTM;
using Infrastructure.Services.StaticData.WaypointsPack;
using UnityEngine;

namespace Infrastructure.Services.StaticData.SpawnPoints
{
    [Serializable]
    public class SpawnPointConfig
    {
        public EPlayerType ActorType;
        public ERelationship Team;
        public WaypointsPackId WaypointsPackId;
        public Vector3 Position;
    }
}