using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scripts.Map.Config
{
    abstract public class AbstractLogic : AbstractActionPerformer
    {
        protected bool Fireable = false;

        abstract public bool Fire();

        abstract public void SignalVariableUpdate(string Variable);
    }
}
