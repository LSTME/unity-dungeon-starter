using UnityEngine;
using Scripts.Map;
using System;

namespace Scripts.Controllers
{
    public class ChestController : AbstractGameObjectController, Interfaces.IUnplacableCorridor, Interfaces.IInteractive, Interfaces.IVault
    {
        public byte Value = 1;

        public bool Activate()
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty()
        {
            throw new NotImplementedException();
        }

        public bool IsReachable()
        {
            throw new NotImplementedException();
        }

        public bool IsUnplacable()
        {
            throw new NotImplementedException();
        }
    }
}

