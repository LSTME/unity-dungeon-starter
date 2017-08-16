using UnityEngine;

namespace Scripts.Map.Config
{
    public class ActionCommand
    {
        public string Open { get; set; }
        public string Close { get; set; }
        public string SwitchOn { get; set; }
        public string SwitchOff { get; set; }
        public string SetTrue { get; set; }
        public string SetFalse { get; set; }
        public string Increment { get; set; }
        public string Decrement { get; set; }
		public string Message { get; set; }

        public void PerformAction()
        {
            PerformOpenAction();
            PerformCloseAction();
            PerformSwitchOnAction();
            PerformSwitchOffAction();
            PerformSetTrueAction();
            PerformSetFalseAction();
            PerformIncrementAction();
            PerformDecrementAction();
			PerformMessageAction();
        }

        private void PerformOpenAction()
        {
            if (Open == null || Open.Trim().Equals("")) return;

            foreach (var obj in getAllControlledGameObjects())
            {
                var component = obj.GetComponent<Interfaces.IOpenable>();
                if (component != null) component.ActionOpen(Open);
            }
        }

        private void PerformCloseAction()
        {
            if (Close == null || Close.Trim().Equals("")) return;

            foreach (var obj in getAllControlledGameObjects())
            {
                var component = obj.GetComponent<Interfaces.IOpenable>();
                if (component != null) component.ActionClose(Close);
            }
        }

        private void PerformSwitchOnAction()
        {
            if (SwitchOn == null || SwitchOn.Trim().Equals("")) return;

            foreach (var obj in getAllControlledGameObjects())
            {
                var component = obj.GetComponent<Interfaces.ISwitchable>();
                if (component != null) component.ActionSwitchOn(SwitchOn);
            }
        }

        private void PerformSwitchOffAction()
        {
            if (SwitchOff == null || SwitchOff.Trim().Equals("")) return;

            foreach (var obj in getAllControlledGameObjects())
            {
                var component = obj.GetComponent<Interfaces.ISwitchable>();
                if (component != null) component.ActionSwitchOff(SwitchOff);
            }
        }

        private void PerformSetTrueAction()
        {
            if (SetTrue == null || SetTrue.Trim().Equals("")) return;

            var GameLogic = getGameLogic();
            if (GameLogic == null) return;

            GameLogic.SetVariable(SetTrue, true);
        }

        private void PerformSetFalseAction()
        {
            if (SetFalse == null || SetFalse.Trim().Equals("")) return;

            var GameLogic = getGameLogic();
            if (GameLogic == null) return;

            GameLogic.SetVariable(SetFalse, false);
        }

        private void PerformIncrementAction()
        {
            if (Increment == null || Increment.Trim().Equals("")) return;

            var GameLogic = getGameLogic();
            if (GameLogic == null) return;

            GameLogic.IncrementCounter(Increment);
        }

        private void PerformDecrementAction()
        {
            if (Decrement == null || Decrement.Trim().Equals("")) return;

            var GameLogic = getGameLogic();
            if (GameLogic == null) return;

            GameLogic.DecrementCounter(Decrement);
        }

		private void PerformMessageAction()
		{
			if (Message == null || Message.Trim().Equals("")) return;

			Debug.Log("Vypisujem spravu " + Message);

			GUITexts.GetInstance().NewTextMessage(Message);
		}


		private GameObject[] getAllControlledGameObjects()
        {
            return Object.FindObjectsOfType<GameObject>();
        }

        private GameLogic getGameLogic()
        {
            var Maps = GameObject.FindGameObjectsWithTag("Map");

            foreach (var Map in Maps)
            {
                var mapGenerator = Map.GetComponent<MapGenerator>();
                if (mapGenerator != null) return mapGenerator.GameLogic;
            }

            return null;
        }
    }
}
