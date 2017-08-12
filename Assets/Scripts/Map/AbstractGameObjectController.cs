using UnityEngine;
using Scripts.Map.Config;

namespace Scripts.Map
{
    abstract public class AbstractGameObjectController : MonoBehaviour
    {
        public ObjectConfig ObjectConfig { get; set; }

        public void PerformActions(int actionType)
        {
            if (ObjectConfig == null) return;

            var actions = ObjectConfig.Actions.getActions(actionType);

            if (actions == null) return;

            foreach (var action in actions)
            {
                action.PerformAction();
            }
        }
    }
}
