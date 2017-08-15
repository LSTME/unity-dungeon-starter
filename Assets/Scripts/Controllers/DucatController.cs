using UnityEngine;
using Scripts.Map;
using System;

namespace Scripts.Controllers
{
    public class DucatController : AbstractGameObjectController, Interfaces.IUnplacableCorridor
    {
        public float RotationSpeed = 350.0f;
        public float BouncingSpeed = 150.0f;
        public float BouncingAmplitude = 0.05f;

        private float _angle = 0.0f;

        public bool IsUnplacable()
        {
            return true;
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, -RotationSpeed * Time.deltaTime);

            _angle += BouncingSpeed * Time.deltaTime;
            _angle = _angle > 360.0f ? _angle - 360.0f : _angle;
            var bounceHeight = BouncingAmplitude * Mathf.Sin(_angle * Mathf.PI / 180.0f);
            transform.position = new Vector3(transform.position.x, bounceHeight, transform.position.z);
        }
    }
}


