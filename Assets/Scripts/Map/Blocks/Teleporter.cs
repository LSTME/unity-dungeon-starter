using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Map.Blocks
{
    class Teleporter : EmptyCorridor
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            base.createGameObject(mapBlock, prefabList, ref MapObject);

            GameObject template = prefabList["teleport"];

            GameObject teleport = AddObject(mapBlock.Location, template, ref MapObject);

            if (mapBlock.Attributes.Length >= 2)
            {
                var teleportController = teleport.GetComponent<TeleportController>();
                teleportController.TargetColumn = int.Parse(mapBlock.Attributes[0]);
                teleportController.TargetRow = int.Parse(mapBlock.Attributes[1]);
            }

            mapBlock.addGameObject(teleport);

            mapBlock.Type = "teleport";
            mapBlock.Interactive = false;
            mapBlock.MinimapColor = new Color(0.5f, 0.5f, 1f);
        }

        public override char forMapChar()
        {
            return 'T';
        }
    }
}
