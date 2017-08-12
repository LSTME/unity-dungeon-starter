using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scripts.Map;
using UnityEngine;
using Scripts.Controllers;

namespace Scripts.Map.Blocks
{
    class FloorButton : AbstractBlockBuilder
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            mapBlock.Initialize();

            GameObject templateCeiling = prefabList["ceiling"];

            GameObject ceiling = AddObject(mapBlock.Location, templateCeiling, ref MapObject);

            mapBlock.addGameObject(ceiling);

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
