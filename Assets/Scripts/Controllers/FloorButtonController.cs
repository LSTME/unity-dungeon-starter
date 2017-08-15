using System;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Interfaces;
using Scripts.Map;

namespace Scripts.Controllers
{
    public class FloorButtonController : AbstractGameObjectController, IPressable, IDropable
    {
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; UpdateVisuals(); }
        }

        public List<AudioClip> Clips = new List<AudioClip>();

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

            if (IsActive)
            {
                PerformActions(Map.Config.Action.ACTION_ACTIVATE);
            }
            else
            {
                PerformActions(Map.Config.Action.ACTION_DEACTIVATE);
            }
        }

        public override bool DropObject()
        {
            var result = base.DropObject();

            if (result)
            {
                Press(true);
            }

            return result;
        }

        public void SignalRemove()
        {
            Press(false);
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