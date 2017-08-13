using Scripts.Map;

namespace Scripts.Controllers
{
    public class NonWalkableDecoration : AbstractGameObjectController, Interfaces.IWalkable
    {
        public bool IsWalkable()
        {
            return false;
        }
    }

}