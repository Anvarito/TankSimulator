using System.Collections.Generic;
using Infrastructure.Services;
using UnityEngine;

public interface ITrashRemoveService : IService
{
    List<GameObject> AllTurrets { get; }
    void LaunchRemove();
}