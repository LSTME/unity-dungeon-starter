using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers
{
    public class WallController : MonoBehaviour, Scripts.Interfaces.IWalkable
    {

        public bool IsWalkable()
        {
            return false;
        }

    }

}
