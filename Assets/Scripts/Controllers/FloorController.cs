using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Controllers
{

    public class FloorController : MonoBehaviour, Scripts.Interfaces.IWalkable
    {
        public bool IsWalkable()
        {
            return true;
        }
    }

}
