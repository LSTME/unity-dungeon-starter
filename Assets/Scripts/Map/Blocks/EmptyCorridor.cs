using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Map.Blocks
{
    class EmptyCorridor : AbstractBlockBuilder
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            mapBlock.Initialize();

            GameObject templateFloor = prefabList["floor"];

            GameObject floor = AddObject(mapBlock.Location, templateFloor, ref MapObject);

            mapBlock.addGameObject(floor);

            GameObject templateCeiling = prefabList["ceiling"];

            GameObject ceiling = AddObject(mapBlock.Location, templateCeiling, ref MapObject);

            mapBlock.addGameObject(ceiling);

            mapBlock.Type = "floor";
            mapBlock.Interactive = false;
            mapBlock.MinimapColor = new Color(0.645f, 0.371f, 0.175f);
        }

        public override char forMapChar()
        {
            return ' ';
        }
    }
}
