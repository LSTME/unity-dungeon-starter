using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts.Map.Config
{
    public class GameLogic : Interfaces.Logic.ILogical
    {
        private bool initialized = false;

        private Dictionary<string, Variable> NamedVariables;

        public List<Logic> Logic { get; set; }

        public List<Variable> Variables { get; set; }

        public void SetVariable(string Variable, bool Value)
        {
            Initialize();

            if (!SetVariableInLogic(Variable, Value)) return;

            FireLogic();
        }

        private void FireLogic()
        {
            var Fired = true;

            while (Fired)
            {
                Fired = false;

                foreach (AbstractLogic Logic in Logic)
                {
                    Fired |= Logic.Fire();
                }
            }
        }

        private bool SetVariableInLogic(string Variable, bool Value)
        {
            if (!NamedVariables.ContainsKey(Variable)) return false;

            if (NamedVariables[Variable].State == Value) return false;

            NamedVariables[Variable].State = Value;

            foreach (AbstractLogic Logic in Logic)
            {
                Logic.SignalVariableUpdate(Variable);
            }

            return true;
        }

        private void Initialize()
        {
            if (initialized) return;

            NamedVariables = new Dictionary<string, Variable>();

            foreach (var Variable in Variables)
            {
                if (!NamedVariables.ContainsKey(Variable.Name))
                {
                    NamedVariables.Add(Variable.Name, Variable);
                }
            }

            initialized = true;
        }

        public bool GetVariable(string Variable)
        {
            Initialize();

            if (!NamedVariables.ContainsKey(Variable)) return false;

            return NamedVariables[Variable].State;
        }
    }
}
