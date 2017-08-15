using System;
using Scripts.Map;

namespace Scripts.Controllers
{

    public class FloorController : AbstractGameObjectController, Interfaces.IWalkable, Interfaces.IDropable
    {
        public bool IsWalkable()
        {
            return true;
        }

        public void SignalRemove()
        {
            
        }
    }

}
