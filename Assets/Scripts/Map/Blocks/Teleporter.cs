using System.Collections.Generic;
using UnityEngine;
using Scripts.Controllers;

namespace Scripts.Map.Blocks
{
    class Teleporter : EmptyCorridor
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            mapBlock.Initialize();

            base.createGameObject(mapBlock, prefabList, ref MapObject);

            GameObject template = prefabList["teleport"];

            GameObject teleport = AddObject(mapBlock.Location, template, ref MapObject);

            var teleportConfig = mapBlock.getObjectConfigForType("teleport");

            if (teleportConfig != null)
            {
                if (teleportConfig.Teleport != null)
                {
                    if (teleportConfig.Teleport.Target != null && teleportConfig.Teleport.Target.Length == 2)
                    {
                        var teleportController = teleport.GetComponent<TeleportController>();
                        teleportController.TargetColumn = teleportConfig.Teleport.Target[0] - 1;
                        teleportController.TargetRow = teleportConfig.Teleport.Target[1] - 1;
                    }

                    if (teleportConfig.Teleport.Rotation != null && teleportConfig.Teleport.Rotation.Length == 1)
                    {
                        var teleportController = teleport.GetComponent<TeleportController>();
                        teleportController.RotationDirection = teleportConfig.Teleport.Rotation[0];
                    }
                }
            }

            mapBlock.addGameObject(teleport);

            mapBlock.Type = "teleport";
        }

        public override char forMapChar()
        {
            return 'T';
        }
    }
}
