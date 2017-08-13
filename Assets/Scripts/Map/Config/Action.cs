﻿using System;
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
        public const int ACTION_ON_MIN = 5;
        public const int ACTION_ON_MAX = 6;
        public const int ACTION_ON_ELSE = 7;

        public List<ActionCommand> OnActivate { get; set; }
        public List<ActionCommand> OnDeactivate { get; set; }
        public List<ActionCommand> OnTrue { get; set; }
        public List<ActionCommand> OnFalse { get; set; }
        public List<ActionCommand> OnMin { get; set; }
        public List<ActionCommand> OnMax { get; set; }
        public List<ActionCommand> OnElse { get; set; }

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
                case ACTION_ON_MIN:
                    return OnMin;
                case ACTION_ON_MAX:
                    return OnMax;
                case ACTION_ON_ELSE:
                    return OnElse;
                default:
                    return null;
            }
        }
    }
}
