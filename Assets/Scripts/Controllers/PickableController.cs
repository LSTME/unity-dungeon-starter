using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Controllers
{
    class PickableController : Map.AbstractGameObjectController, Interfaces.IPickable, Interfaces.IWalkable, Interfaces.IInteractive, Interfaces.IUnplacableCorridor
    {
        public bool Activate()
        {
            return PickUpObject();
        }

        public bool IsWalkable()
        {
            return false;
        }

        public bool IsUnplacable()
        {
            return true;
        }
    }
}
