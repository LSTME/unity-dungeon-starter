using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Map.Blocks
{
    class GateHorizontal : EmptyCorridor
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            base.createGameObject(mapBlock, prefabList, ref MapObject);

            GameObject template = prefabList["gate"];

            GameObject gate = AddObject(mapBlock.Location, template, ref MapObject);

            if (mapBlock.Attributes.Length >= 1)
            {
                var doorsController = gate.GetComponent<DoorsController>();
                doorsController.Tag = mapBlock.Attributes[0];
            }

            mapBlock.addGameObject(gate);

            mapBlock.Type = "vgate";
            mapBlock.Interactive = false;
            mapBlock.MinimapColor = Color.cyan;
        }

        public override char forMapChar()
        {
            return '-';
        }
    }
}
