using UnityEngine;

namespace Infrastructure.Components
{
    public class SoundSource : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        
        private void Awake() => 
            DontDestroyOnLoad(this);
    }
}