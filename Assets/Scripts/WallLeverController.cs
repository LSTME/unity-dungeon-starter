using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class WallLeverController : MonoBehaviour, IInteractive
    {
        public bool IsActive
        {
            get { return _isActive;  }
            set { _isActive = value; UpdateVisuals(); }
        }

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

            if (OnToggle != null) OnToggle(IsActive);

            foreach (var doors in GameObject.FindGameObjectsWithTag("Doors"))
            {
                var component = doors.GetComponent<DoorsController>();
                if (!component.Tag.Equals(DoorTag)) continue;
                
                if (IsActive)
                    component.Open();
                else
                    component.Close();

                foreach (var interactiveGameObject in GameObject.FindGameObjectsWithTag("Interactive"))
                {
                    var wallLeverController = interactiveGameObject.GetComponent<WallLeverController>();
                    if (wallLeverController != null && wallLeverController.DoorTag.Equals(DoorTag))
                        wallLeverController.IsActive = IsActive;
                }
            } 
        }

        void UpdateVisuals()
        {
            transform.Find("wall_lever-on").gameObject.SetActive(IsActive);
            transform.Find("wall_lever-off").gameObject.SetActive(!IsActive);
        }
    }
}
