using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scripts.Map;
using UnityEngine;

namespace Scripts.Map.Blocks
{
    class FloorButton : AbstractBlockBuilder
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            GameObject templateCeiling = prefabList["ceiling"];

            GameObject ceiling = AddObject(mapBlock.Location, templateCeiling, ref MapObject);

            mapBlock.addGameObject(ceiling);

            GameObject templateFloorButton = prefabList["floor_button"];

            GameObject floorButton = AddObject(mapBlock.Location, templateFloorButton, ref MapObject);

            if (mapBlock.Attributes.Length >= 1)
            {
                var floorButtonController = floorButton.GetComponent<FloorButtonController>();
                floorButtonController.DoorTag = mapBlock.Attributes[0];
            }

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
