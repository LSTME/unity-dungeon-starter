using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts.Map.Config
{
    public class Action
    {
        public const int ACTION_ACTIVATE = 1;
        public const int ACTION_DEACTIVATE = 2;
        public const int ACTION_ON_TRUE = 3;
        public const int ACTION_ON_FALSE = 4;

        public List<ActionCommand> OnActivate { get; set; }
        public List<ActionCommand> OnDeactivate { get; set; }
        public List<ActionCommand> OnTrue { get; set; }
        public List<ActionCommand> OnFalse { get; set; }

        public List<ActionCommand> getActions(int actionType)
        {
            switch (actionType)
            {
                case ACTION_ACTIVATE:
                    return OnActivate;
                case ACTION_DEACTIVATE:
                    return OnDeactivate;
                case ACTION_ON_TRUE:
                    return OnTrue;
                case ACTION_ON_FALSE:
                    return OnFalse;
                default:
                    return null;
            }
        }
    }
}
