using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Interfaces;

namespace Scripts
{
    public class TeleportController : MonoBehaviour, ITeleport
    {
        public Light TeleportLight;

        public float BaseIntensity = 2f;
        public float MaxReduction = 0.5f;
        public float MaxIncrease = 0.3f;
        public float RateDamping = 0.05f;
        public float Strength = 20.0f;

        public int TargetColumn;
        public int TargetRow;

        private Animator animator
        {
            get { return GetComponent<Animator>(); }
        }

        public void Teleport()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var playerController = player.GetComponent<PlayerController>();

            playerController.MovePlayer(new Vector2(TargetColumn, TargetRow));
        }

        // Use this for initialization
        void Start()
        {
            StartCoroutine(FlickerIntensity());
        }

        IEnumerator FlickerIntensity()
        {
            while (true)
            {
                var intensity = Random.Range(BaseIntensity - MaxReduction, BaseIntensity + MaxIncrease);

                TeleportLight.intensity = Mathf.Lerp(TeleportLight.intensity, intensity, Strength * Time.deltaTime);

                yield return new WaitForSeconds(RateDamping);
            }
        }
    }
}
