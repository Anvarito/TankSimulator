using System.Collections.Generic;
using ChobiAssets.PTM;
using UnityEngine;

public class TeamSeparator : MonoBehaviour
{
    // Start is called before the first frame update
    public List<SpawnPoint> _spawnPoints;
    public int EnemysCount()
    {
        int count = 0;
        foreach (var i in _spawnPoints)
        {
            if (i.ActorType == EPlayerType.AI)
                count++;
        }

        return count;
    }
    public SpawnPoint GetPoint(EPlayerType playerType, ERelationship relationship)
    {
        foreach (var i in _spawnPoints)
        {
            if (!i.IsOccupied && i.Relationship == relationship && i.ActorType == playerType)
            {
                i.IsOccupied = true;
                return i;
            }
            else
                continue;
        }

        return null;
    }


    [ContextMenu("SetRelationshipToSpawnPoints")]
    public void SetRelationshipToSpawnPoints()
    {
        foreach (var i in _spawnPoints)
        {
            i.Relationship = i.transform.position.x > 0 ? ERelationship.TeamA : ERelationship.TeamB;
            i.transform.name += i.Relationship.ToString();
        }
    }


}
