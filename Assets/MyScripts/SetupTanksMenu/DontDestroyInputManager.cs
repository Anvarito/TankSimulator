using UnityEngine;

public class DontDestroyInputManager : MonoBehaviour
{
    public static DontDestroyInputManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("[Singleton] Trying to instantiate a seccond instance of a singleton class.");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
}
