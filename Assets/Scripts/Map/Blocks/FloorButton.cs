using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Map.Blocks
{
    class FloorButton : EmptyCorridor
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            mapBlock.Initialize();

            GenerateFloor = false;

            base.createGameObject(mapBlock, prefabList, ref MapObject);

            GameObject templateFloorButton = prefabList["floor_button"];

            GameObject floorButton = AddObject(mapBlock.Location, templateFloorButton, ref MapObject);

            AssignObjectConfigByType(floorButton, "floor_button", mapBlock);

            mapBlock.addGameObject(floorButton);

            mapBlock.Interactive = true;
            mapBlock.Type = "floor_button";
            mapBlock.MinimapColor = Color.blue;
        }

        public override char forMapChar()
        {
            return 'b';
        }
    }
}
