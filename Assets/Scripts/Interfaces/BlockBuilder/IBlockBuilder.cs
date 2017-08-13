using System.Collections.Generic;
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
