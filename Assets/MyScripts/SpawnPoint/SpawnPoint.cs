using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;

public class SpawnPoint : MonoBehaviour
{
    public ERelationship Relationship;
    public EPlayerType ActorType;
    public bool IsOccupied { get; set; } = false;

    public GameObject WayPointsPack;
}
