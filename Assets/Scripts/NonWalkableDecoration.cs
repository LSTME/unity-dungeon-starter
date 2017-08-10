using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class NonWalkableDecoration : MonoBehaviour, Interfaces.IWalkable
    {
        public bool IsWalkable()
        {
            return false;
        }
    }

}