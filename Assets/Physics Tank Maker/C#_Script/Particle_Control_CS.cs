using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

    public class Particle_Control_CS : MonoBehaviour
    {
        /*
		 * This script is attached to the explosion effects prefabs.
		 * This script controls the sound and the light in the effects.
		*/


        // User options >>
        public ParticleSystem This_ParticleSystem;
        public bool Use_Random_Pitch;
        public float Random_Pitch_Min = 0.5f;
        public float Random_Pitch_Max = 1.0f;
        // << User options


        Light thisLight;
        AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            // Get the ParticleSystem.
            if (This_ParticleSystem == null)
            {
                This_ParticleSystem = GetComponent<ParticleSystem>();
            }

            // Get the Light.
            thisLight = GetComponent<Light>();
            if (thisLight)
            {
                StartCoroutine(Flash());
            }
        }

        void Update()
        {
            // Check the particles and the audio have finished.
            if (This_ParticleSystem.isStopped)
            { // The particle has finished.
                if (audioSource && audioSource.isPlaying)
                { // The audio is still playing now.
                    return;
                }

                // The audio has finished.
                Destroy(this.gameObject);
            }
        }


        IEnumerator Flash()
        {
            thisLight.enabled = true;
            yield return new WaitForSeconds(0.08f);
            thisLight.enabled = false;
        }

    }

}