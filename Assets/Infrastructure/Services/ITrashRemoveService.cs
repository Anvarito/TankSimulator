using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services
{
    public interface ITrashRemoveService : IService
    {
        List<GameObject> AllTurrets { get; }
        void LaunchRemove();
    }
}