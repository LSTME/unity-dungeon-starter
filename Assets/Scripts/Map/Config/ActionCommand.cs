using UnityEngine;

namespace Scripts.Map.Config
{
    public class ActionCommand
    {
        public string Open { get; set; }
        public string Close { get; set; }
        public string SwitchOn { get; set; }
        public string SwitchOff { get; set; }

        public void PerformAction()
        {
            PerformOpenAction();
            PerformCloseAction();
            PerformSwitchOnAction();
            PerformSwitchOffAction();
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

        private GameObject[] getAllControlledGameObjects()
        {
            return Object.FindObjectsOfType<GameObject>();
        }
    }
}
