using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Interfaces;
using Scripts.Map;

namespace Scripts.Controllers
{
    public class TeleportController : AbstractGameObjectController, ITeleport
    {
        public Light TeleportLight;

        public float BaseIntensity = 2f;
        public float MaxReduction = 0.5f;
        public float MaxIncrease = 0.3f;
        public float RateDamping = 0.05f;
        public float Strength = 20.0f;

        public int TargetColumn;
        public int TargetRow;
        public char RotationDirection = ' ';

        private Animator animator
        {
            get { return GetComponent<Animator>(); }
        }

        public void Teleport()
        {
            if (TargetColumn <= 0 || TargetRow <= 0) return;

            var player = GameObject.FindGameObjectWithTag("Player");
            var playerController = player.GetComponent<PlayerController>();

            playerController.MovePlayer(new Vector2(TargetColumn, TargetRow));

            var directions = getDirections();

            if (!directions.ContainsKey(RotationDirection)) return;

            playerController.RotateTo(directions[RotationDirection], false);
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

        Dictionary<char, Direction> getDirections()
        {
            var result = new Dictionary<char, Direction>();

            result.Add('N', Direction.North);
            result.Add('S', Direction.South);
            result.Add('W', Direction.West);
            result.Add('E', Direction.East);

            return result;
        }
    }
}
