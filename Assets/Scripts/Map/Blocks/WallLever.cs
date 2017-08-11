using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Scripts.Controllers;

namespace Scripts.Map.Blocks
{
    class WallLever: EmptyCorridor
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            base.createGameObject(mapBlock, prefabList, ref MapObject);

            GameObject template = prefabList["wall_lever"];

            GameObject lever = AddObject(mapBlock.Location, template, ref MapObject);

            if (mapBlock.Attributes.Length >= 1)
            {
                var direction = (Direction)"NESW".IndexOf(mapBlock.Attributes[0]);
                lever.transform.rotation = direction.GetRotation();
            }
            else
            {
                AttachToWall(ref lever);
            }

            if (mapBlock.Attributes.Length == 2)
            {
                var wallLeverController = lever.GetComponent<WallLeverController>();
                wallLeverController.DoorTag = mapBlock.Attributes[1];
            }

            mapBlock.addGameObject(lever);

            mapBlock.Interactive = true;
            mapBlock.Type = "wall_lever";
            mapBlock.MinimapColor = Color.magenta;
        }

        public override char forMapChar()
        {
            return 'l';
        }
    }
}
