using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Scripts.Map;

namespace Scripts.Interfaces.BlockBuilder
{
    interface IBlockBuilder
    {
        void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject);

        char forMapChar();
    }
}
