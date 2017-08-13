using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts.Map.Config
{
    abstract public class AbstractLogic
    {
        public Action Actions { get; set; }

        protected bool Fireable = false;

        public void PerformActions(int actionType)
        {
            if (Actions == null) return;

            var actions = Actions.getActions(actionType);

            if (actions == null) return;

            foreach (var action in actions)
            {
                action.PerformAction();
            }
        }

        abstract public bool Fire();

        abstract public void SignalVariableUpdate(string Variable);
    }
}
