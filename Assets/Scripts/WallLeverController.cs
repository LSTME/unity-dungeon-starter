using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class WallLeverController : MonoBehaviour
    {
        public bool IsActive;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Toggle()
        {
            IsActive = !IsActive;

            UpdateVisuals();
        }

        void UpdateVisuals()
        {
            transform.Find("wall_lever-on").gameObject.SetActive(IsActive);
            transform.Find("wall_lever-off").gameObject.SetActive(!IsActive);
        }
    }
}
