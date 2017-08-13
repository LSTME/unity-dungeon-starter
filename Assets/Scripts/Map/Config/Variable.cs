using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts.Map.Config
{
    public class Variable
    {
        public string Name { get; set; }
        public bool State { get; set; }
        public bool Negate { get; set; }

        public bool GetState()
        {
            if (Negate) return !State;
            return State;
        }
    }
}
