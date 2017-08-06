using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class DoorsController : MonoBehaviour
    {

        public bool IsOpen = true;

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
            if (IsOpen) return;

            IsOpen = true;
            animator.Play("open");
        }

        public void Close()
        {
            if (!IsOpen) return;

            IsOpen = false;
            animator.Play("close");
        }
    }
}
