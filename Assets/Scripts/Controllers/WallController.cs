using Scripts.Map;

namespace Scripts.Controllers
{
    public class WallController : AbstractGameObjectController, Scripts.Interfaces.IWalkable
    {

        public bool IsWalkable()
        {
            return false;
        }

    }

}
