using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts.Map.Config
{
    public class GameLogic
    {
        public List<Logic> Logic { get; set; }

        public void SetVariable(string Variable, bool Value)
        {
            SetVariableInLogic(Variable, Value);

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

        private void SetVariableInLogic(string Variable, bool Value)
        {
            foreach (AbstractLogic Logic in Logic)
            {
                Logic.SetVariable(Variable, Value);
            }
        }
    }
}
