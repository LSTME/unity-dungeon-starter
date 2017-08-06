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

        // Update is called once per frame
        void Update()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            if (Vector3.Distance(transform.position, player.transform.position) < 1.1)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        void Open()
        {
            if (IsOpen) return;

            IsOpen = true;
            animator.Play("open");
        }

        void Close()
        {
            if (!IsOpen) return;

            IsOpen = false;
            animator.Play("close");
        }
    }
}
