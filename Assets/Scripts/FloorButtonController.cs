using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Interfaces;

namespace Scripts
{
    public class FloorButtonController : MonoBehaviour, IPressable
    {
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; UpdateVisuals(); }
        }

        public List<AudioClip> Clips = new List<AudioClip>();

        public string DoorTag;

        public Action<bool> OnToggle;
        
        private bool _isActive;

        // Use this for initialization
        void Start()
        {
            UpdateVisuals();
        }

        public void Press(bool state = true)
        {
            IsActive = state;

            if (OnToggle != null) OnToggle(IsActive);

            foreach (var doors in GameObject.FindGameObjectsWithTag("Doors"))
            {
                var component = doors.GetComponent<DoorsController>();
                if (!component.Tag.Equals(DoorTag)) continue;

                if (IsActive)
                    component.Open();
                else
                    component.Close();
            }
        }

        void UpdateVisuals()
        {
            transform.Find("floor_switch_on").gameObject.SetActive(IsActive);
            transform.Find("floor_switch_off").gameObject.SetActive(!IsActive);
        }

        void PlaySound()
        {
            /*var audioSource = transform.Find("wall_lever_audio_source").GetComponent<AudioSource>();
            audioSource.Stop();
            if (Clips.Count > 0)
            {
                audioSource.clip = Clips[UnityEngine.Random.Range(0, Clips.Count)];
            }
            audioSource.Play();*/
        }
    }

}