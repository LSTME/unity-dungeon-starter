using System;
using Scripts.Map;

namespace Scripts.Controllers
{
    public class NonWalkableDecoration : AbstractGameObjectController, Interfaces.IWalkable, Interfaces.IUnplacableCorridor
    {
        public bool IsUnplacable()
        {
            return true;
        }

        public bool IsWalkable()
        {
            return false;
        }
    }

}