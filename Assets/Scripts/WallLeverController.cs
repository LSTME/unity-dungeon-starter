using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class WallLeverController : MonoBehaviour, IInteractive
    {
        public bool IsActive = false;

        public Action<bool> OnToggle;

        // Use this for initialization
        void Start()
        {
            UpdateVisuals();
        }

        public void Activate()
        {
            IsActive = !IsActive;

            if (OnToggle != null) OnToggle(IsActive);

            UpdateVisuals();
        }

        void UpdateVisuals()
        {
            transform.Find("wall_lever-on").gameObject.SetActive(IsActive);
            transform.Find("wall_lever-off").gameObject.SetActive(!IsActive);
        }
    }
}
