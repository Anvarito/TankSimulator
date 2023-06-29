using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services
{
    public class TrashRemoveService : ITrashRemoveService
    {
        public List<GameObject> AllTurrets { get; } = new List<GameObject>();

        public void LaunchRemove()
        {
            AllTurrets.AddRange(GameObject.FindGameObjectsWithTag(ChobiAssets.PTM.Layer_Settings_CS.FinishTag));
            foreach(var i in AllTurrets)
            {
                GameObject.Destroy(i.gameObject);
            }
        }

        public void CleanUp()
        {
        }
    }
}
