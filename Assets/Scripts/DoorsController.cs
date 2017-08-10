using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class DoorsController : MonoBehaviour, Interfaces.IWalkable
    {
        public bool IsOpen = true;
        public string Tag;

        public AudioClip SoundOpen;
        public AudioClip SoundClose;

        public Action<GameObject, bool> OnToggle;

        private Animator animator
        {
            get { return GetComponent<Animator>(); }
        }
        private AudioSource audioSource
        {
            get { return GetComponent<AudioSource>(); }
        }

        void Start()
        {
            Close();
        }

        public void Open()
        {
            Toggle(true);
        }

        public void Close()
        {
            Toggle(false);
        }

        void Toggle(bool open)
        {
            if (IsOpen == open) return;
            audioSource.Stop();

            IsOpen = open;
            animator.Play(open ? "open" : "close");
            
            if (OnToggle != null)
            {
                OnToggle(gameObject, open);
            }
        }

        // This is called from animation
        public void PlayCloseSound()
        {
            audioSource.clip = SoundClose;
            audioSource.Play();
        }

        // This is called from animation
        public void PlayOpenSound()
        {
            audioSource.clip = SoundOpen;
            audioSource.Play();
        }

        public bool IsWalkable()
        {
            return IsOpen;
        }
    }
}
