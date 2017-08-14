using System;
using UnityEngine;
using Scripts.Map;

namespace Scripts.Controllers
{
    public class DoorsController : AbstractGameObjectController, Interfaces.IWalkable, Interfaces.IOpenable, Interfaces.IInteractive
    {
        public bool IsOpen = true;

        private bool DoorWalkable = false;

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
            return DoorWalkable;
        }

        public void ActionOpen(string target)
        {
            if (ObjectConfig == null) return;
            if (ObjectConfig.Name == null) return;
            if (!ObjectConfig.Name.Equals(target)) return;

            Open();
        }

        public void ActionClose(string target)
        {
            if (ObjectConfig == null) return;
            if (ObjectConfig.Name == null) return;
            if (!ObjectConfig.Name.Equals(target)) return;

            Close();
        }

        public void SetDoorWalkable()
        {
            DoorWalkable = true;
        }

        public void SetDoorNonWalkable()
        {
            DoorWalkable = false;
        }

        public bool Activate()
        {
            if (!IsReachableToActivate(true)) return false;

            if (ObjectConfig == null) return false;
            if (ObjectConfig.Door == null) return false;
            if (!ObjectConfig.Door.Manual) return false;

            Toggle(!IsOpen);

            return true;
        }
    }
}
