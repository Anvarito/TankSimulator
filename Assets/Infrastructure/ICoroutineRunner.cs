using System.Collections;
using UnityEngine;

namespace Infrastructure
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator loadScene);
        void StopCoroutine(Coroutine coroutine);
    }
}