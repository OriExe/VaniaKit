using UnityEngine;

namespace Vaniakit.Map
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField]private AudioSource audioSource;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
            audioSource.Play();
            audioSource.loop = true;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

