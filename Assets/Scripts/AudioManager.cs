using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Life
{
    /// <summary>
    /// This class manages the game's audio effects.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager audioManager;
        
        [SerializeField] private AudioClip blockHover;
        [SerializeField] private AudioClip blockReplace;
        [SerializeField] private AudioClip blockDescend;
        [SerializeField] private new AudioSource audio;
        
        /// <summary>
        /// This method is used to prevent duplicate AudioManager objects.
        /// </summary>
        private void Awake()
        {
            if (audioManager == null)
            {
                audioManager = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// This method grabs audio source at start of program.
        /// </summary>
        public void Start()
        {
            audio = GetComponent<AudioSource>();
        }

        /// <summary>
        /// This method plays block hovering sound.
        /// </summary>
        public void BlockHover()
        {
            audio.PlayOneShot(blockHover);
        }
        
        /// <summary>
        /// This method plays block replacing sound.
        /// </summary>
        public void BlockReplace()
        {
            audio.PlayOneShot(blockReplace);
        }
        
        /// <summary>
        /// This method plays block descending sound.
        /// </summary>
        public void BlockDescend()
        {
            audio.PlayOneShot(blockDescend);
        }
    }
}
