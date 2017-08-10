using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{

    public class FloorController : MonoBehaviour, Scripts.Interfaces.IWalkable
    {
        public bool IsWalkable()
        {
            return true;
        }
    }

}
