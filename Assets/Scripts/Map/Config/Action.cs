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

        public List<ActionCommand> OnActivate { get; set; }
        public List<ActionCommand> OnDeactivate { get; set; }

        public List<ActionCommand> getActions(int actionType)
        {
            switch (actionType)
            {
                case ACTION_ACTIVATE:
                    return OnActivate;
                case ACTION_DEACTIVATE:
                    return OnDeactivate;
                default:
                    return null;
            }
        }
    }
}
