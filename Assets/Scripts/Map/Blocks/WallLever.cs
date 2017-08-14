using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Map.Blocks
{
    class WallLever: EmptyCorridor
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            mapBlock.Initialize();

            base.createGameObject(mapBlock, prefabList, ref MapObject);

            GameObject template = prefabList["wall_lever"];

            GameObject lever = AddObject(mapBlock.Location, template, ref MapObject);

            var leverConfig = mapBlock.getObjectConfigForType("lever");
            if (leverConfig != null && leverConfig.Rotation != null && leverConfig.Rotation.Length == 1)
            {
                var direction = (Direction)"NESW".IndexOf(leverConfig.Rotation[0]);
                lever.transform.rotation = direction.GetRotation();
            }
            else
            {
                AttachToWall(ref lever);
            }

            AssignObjectConfigByType(lever, "lever", mapBlock);

            mapBlock.addGameObject(lever);
            
            mapBlock.Type = "wall_lever";
            mapBlock.MinimapColor = Color.magenta;
        }

        public override char forMapChar()
        {
            return 'l';
        }
    }
}
