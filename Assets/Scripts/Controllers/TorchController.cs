using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers
{
    public class TorchController : MonoBehaviour
    {
        public Light LeftLight;
        public Light RightLight;

        public float BaseIntensity = 3f;
        public float MaxReduction = 0.75f;
        public float MaxIncrease = 0.3f;
        public float RateDamping = 0.05f;
        public float Strength = 20.0f;

        void Start()
        {
            StartCoroutine(FlickerIntensity());
        }

        IEnumerator FlickerIntensity()
        {
            while (true)
            {
                var intensityL = Random.Range(BaseIntensity - MaxReduction, BaseIntensity + MaxIncrease);
                var intensityR = Random.Range(BaseIntensity - MaxReduction, BaseIntensity + MaxIncrease);

                LeftLight.intensity = Mathf.Lerp(LeftLight.intensity, intensityL, Strength * Time.deltaTime);
                RightLight.intensity = Mathf.Lerp(RightLight.intensity, intensityR, Strength * Time.deltaTime);

                yield return new WaitForSeconds(RateDamping);
            }
        }
    }
}


