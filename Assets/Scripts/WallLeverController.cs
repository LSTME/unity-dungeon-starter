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

            foreach (GameObject doors in GameObject.FindGameObjectsWithTag("Doors"))
            {
                var cont = (DoorsController)doors.GetComponent(typeof(DoorsController));
                if (IsActive)
                {
                    cont.Open();
                }
                else
                {
                    cont.Close();
                }
            } 

            UpdateVisuals();
        }

        void UpdateVisuals()
        {
            transform.Find("wall_lever-on").gameObject.SetActive(IsActive);
            transform.Find("wall_lever-off").gameObject.SetActive(!IsActive);
        }
    }
}
