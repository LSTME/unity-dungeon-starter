using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class DoorsController : MonoBehaviour
    {

        public bool IsOpen = true;

        public Action<GameObject, bool> OnToggle;

        private Animator animator
        {
            get { return GetComponent<Animator>(); }
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

            IsOpen = open;
            animator.Play(open ? "open" : "close");

            if (OnToggle != null)
            {
                OnToggle(gameObject, open);
            }
        }
    }
}
