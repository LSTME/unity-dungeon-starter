using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
