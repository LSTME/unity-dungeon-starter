using UnityEngine;
using Scripts.Map;
using System;

namespace Scripts.Controllers
{
    public class DucatController : AbstractGameObjectController, Interfaces.IUnplacableCorridor, Interfaces.IInteractive
    {
        public float RotationSpeed = 350.0f;
        public float BouncingSpeed = 150.0f;
        public float BouncingAmplitude = 0.05f;

        private float _angle = 0.0f;

		void Start() {
			gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		}

		public bool Activate()
		{
			PlayerController.InterpreterLock.Set();
			
			if (!IsReachable()) return false;

			GUITexts guiTexts = GUITexts.GetInstance();

			guiTexts.CollectCoin();

			var mapGenerator = MapGenerator.getInstance();

			var mapBlock = mapGenerator.GetBlockAtLocation(GetBlockPosition());

			mapBlock.DetachGameObject(gameObject);

			GameObject.Destroy(gameObject);

			PerformActions(Map.Config.Action.ACTION_ACTIVATE);

			return true;
		}

		public bool IsReachable()
		{
			return IsReachableToActivate(true);
		}

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


