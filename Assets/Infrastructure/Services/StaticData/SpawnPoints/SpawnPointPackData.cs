using UnityEngine;

namespace Infrastructure.Services.StaticData.SpawnPoints
{
    [CreateAssetMenu(menuName = "Static Data/🏁SpawnPoints Static Data",fileName = "SpawnPointPackData")]
    public class SpawnPointPackData : ScriptableObject
    {
        public SpawnPointsPackConfig PackConfig;
    }
}