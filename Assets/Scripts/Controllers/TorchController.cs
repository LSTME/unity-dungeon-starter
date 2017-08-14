using System.Collections;
using UnityEngine;
using Scripts.Map;
using System;

namespace Scripts.Controllers
{
    public class TorchController : AbstractGameObjectController, Interfaces.ISwitchable
    {
        public Light LeftLight;
        public Light RightLight;
        public GameObject TorchFlame;

        public float BaseIntensity = 3f;
        public float MaxReduction = 0.75f;
        public float MaxIncrease = 0.3f;
        public float RateDamping = 0.05f;
        public float Strength = 20.0f;

        private bool Enabled = true;

        private bool initialized = false;

        void Start()
        {
            UpdateLight();
            StartCoroutine(FlickerIntensity());
        }

        private void Update()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (initialized) return;

            if (ObjectConfig == null) return;
            if (ObjectConfig.Torch == null) return;

            Enabled = ObjectConfig.Torch.State;

            UpdateLight();

            initialized = true;
        }

        private void UpdateLight()
        {
            TorchFlame.SetActive(Enabled);
            LeftLight.enabled = Enabled;
            RightLight.enabled = Enabled;
        }

        IEnumerator FlickerIntensity()
        {
            while (true)
            {
                var intensityL = UnityEngine.Random.Range(BaseIntensity - MaxReduction, BaseIntensity + MaxIncrease);
                var intensityR = UnityEngine.Random.Range(BaseIntensity - MaxReduction, BaseIntensity + MaxIncrease);

                LeftLight.intensity = Mathf.Lerp(LeftLight.intensity, intensityL, Strength * Time.deltaTime);
                RightLight.intensity = Mathf.Lerp(RightLight.intensity, intensityR, Strength * Time.deltaTime);

                yield return new WaitForSeconds(RateDamping);
            }
        }

        public void ActionSwitchOn(string target)
        {
            if (ObjectConfig == null) return;
            if (ObjectConfig.Name == null || !ObjectConfig.Name.Equals(target)) return;

            Enabled = true;
            UpdateLight();
        }

        public void ActionSwitchOff(string target)
        {
            if (ObjectConfig == null) return;
            if (ObjectConfig.Name == null || !ObjectConfig.Name.Equals(target)) return;

            Enabled = false;
            UpdateLight();
        }
    }
}


