using System;

namespace Scripts.Controllers
{
    class WalkableDecoration : Scripts.Map.AbstractGameObjectController, Interfaces.IWalkable
    {
        public bool IsWalkable()
        {
            return true;
        }
    }
}
