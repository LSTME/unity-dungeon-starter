using System.Collections.Generic;

namespace Scripts.Map.Config
{
    public class GameLogic : Interfaces.Logic.ILogical, Interfaces.Logic.ICounter
    {
        private bool initialized = false;

        private Dictionary<string, Variable> NamedVariables;

        private Dictionary<string, Counter> NamedCounters;

        public List<Logic> Logic { get; set; }

        public List<Variable> Variables { get; set; }

        public List<Counter> Counters { get; set; }

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

            InitializeNamedVariables();

            InitializeNamedCounters();

            initialized = true;
        }

        private void InitializeNamedCounters()
        {
            NamedCounters = new Dictionary<string, Counter>();

            if (Counters == null) return; 

            foreach (var Counter in Counters)
            {
                if (!NamedCounters.ContainsKey(Counter.Name))
                {
                    NamedCounters.Add(Counter.Name, Counter);
                }
            }
        }

        private void InitializeNamedVariables()
        {
            NamedVariables = new Dictionary<string, Variable>();

            if (Variables == null) return;

            foreach (var Variable in Variables)
            {
                if (!NamedVariables.ContainsKey(Variable.Name))
                {
                    NamedVariables.Add(Variable.Name, Variable);
                }
            }
        }

        public bool GetVariable(string Variable)
        {
            Initialize();

            if (!NamedVariables.ContainsKey(Variable)) return false;

            return NamedVariables[Variable].State;
        }

        public void IncrementCounter(string Name)
        {
            Initialize();

            if (!NamedCounters.ContainsKey(Name)) return;

            NamedCounters[Name].Increment();
        }

        public void DecrementCounter(string Name)
        {
            Initialize();

            if (!NamedCounters.ContainsKey(Name)) return;

            NamedCounters[Name].Decrement();
        }
    }
}
