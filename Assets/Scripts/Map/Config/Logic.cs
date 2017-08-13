using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts.Map.Config
{
    public class Logic : AbstractLogic
    {
        private bool Initialized = false;
        private bool LastState = false;

        public List<VariableMapping> Variables { get; set; }
        public bool FireAlways { get; set; }
        public bool AndLogic { get; set; }

        private Dictionary<string, VariableMapping> NamedVariables;

        private void Initialize()
        {
            if (Initialized) return;

            InitializeNamedVariables();

            InitializeLastState();

            Initialized = true;
        }

        private void InitializeLastState()
        {
            LastState = AndLogic;

            foreach (var Variable in NamedVariables.Values)
            {
                if (AndLogic) LastState &= Variable.GetState();
                else LastState |= Variable.GetState();
            }
        }

        private void InitializeNamedVariables()
        {
            NamedVariables = new Dictionary<string, VariableMapping>();

            foreach (var Variable in Variables)
            {
                if (!NamedVariables.ContainsKey(Variable.Name))
                {
                    NamedVariables.Add(Variable.Name, Variable);
                }
            }
        }

        private bool GetCurrentState()
        {
            var State = AndLogic;

            foreach (var Variable in NamedVariables.Values)
            {
                if (AndLogic) State &= Variable.GetState();
                else State |= Variable.GetState();
            }

            return State;
        }

        override public bool Fire()
        {
            if (!Fireable) return false;

            Fireable = false;

            var State = GetCurrentState();
            if (FireAlways == false && State == LastState) return false;

            if (State)
            {
                PerformActions(Action.ACTION_ON_TRUE);
            } else
            {
                PerformActions(Action.ACTION_ON_FALSE);
            }

            LastState = State;

            return true;
        }

        public override void SignalVariableUpdate(string Variable)
        {
            Initialize();

            if (!NamedVariables.ContainsKey(Variable)) return;

            Fireable = true;
        }
    }
}
