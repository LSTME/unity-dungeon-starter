using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Map.Blocks
{
    class GateHorizontal : EmptyCorridor
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            mapBlock.Initialize();

            base.createGameObject(mapBlock, prefabList, ref MapObject);

            GameObject template = prefabList["gate"];

            GameObject gate = AddObject(mapBlock.Location, template, ref MapObject);

            AssignObjectConfigByType(gate, "door", mapBlock);

            mapBlock.addGameObject(gate);

            mapBlock.Type = "vgate";
            mapBlock.MinimapColor = Color.cyan;
        }

        public override char forMapChar()
        {
            return '-';
        }
    }
}
