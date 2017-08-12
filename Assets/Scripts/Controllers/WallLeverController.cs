﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Interfaces;
using Scripts.Map;

namespace Scripts.Controllers
{
    public class WallLeverController : AbstractGameObjectController, IInteractive, ISwitchable
    {
        public bool IsActive
        {
            get { return _isActive;  }
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

        public void Activate()
        {
            IsActive = !IsActive;

            PlaySound();

            if (OnToggle != null) OnToggle(IsActive);

            if (IsActive)
            {
                PerformActions(Map.Config.Action.ACTION_ACTIVATE);
            } else
            {
                PerformActions(Map.Config.Action.ACTION_DEACTIVATE);
            }
        }

        void UpdateVisuals()
        {
            transform.Find("wall_lever-on").gameObject.SetActive(IsActive);
            transform.Find("wall_lever-off").gameObject.SetActive(!IsActive);
        }

        void PlaySound()
        {
            var audioSource = transform.Find("wall_lever_audio_source").GetComponent<AudioSource>();
            audioSource.Stop();
            if (Clips.Count > 0)
            {
                audioSource.clip = Clips[UnityEngine.Random.Range(0, Clips.Count)];
            }
            audioSource.Play();
        }

        public void ActionSwitchOn(string target)
        {
            if (ObjectConfig == null) return;
            if (!ObjectConfig.Name.Equals(target)) return;

            IsActive = true;
        }

        public void ActionSwitchOff(string target)
        {
            if (ObjectConfig == null) return;
            if (!ObjectConfig.Name.Equals(target)) return;

            IsActive = false;
        }
    }
}
