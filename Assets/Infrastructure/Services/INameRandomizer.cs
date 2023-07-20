using System.Collections;
using System.Collections.Generic;
using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.Services
{
    public interface INameRandomizer : IService
    {
        public string GetRandomedName();

    }
}
