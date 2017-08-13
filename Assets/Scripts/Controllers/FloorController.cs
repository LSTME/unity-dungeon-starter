using Scripts.Map;

namespace Scripts.Controllers
{

    public class FloorController : AbstractGameObjectController, Scripts.Interfaces.IWalkable
    {
        public bool IsWalkable()
        {
            return true;
        }
    }

}
