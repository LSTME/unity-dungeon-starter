using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Map.Blocks
{
    class Torch : EmptyCorridor
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            mapBlock.Initialize();

            base.createGameObject(mapBlock, prefabList, ref MapObject);

            GameObject template = prefabList["torch"];

            GameObject torch = AddObject(mapBlock.Location, template, ref MapObject);

            var torchConfig = mapBlock.getObjectConfigForType("torch");
            if (torchConfig != null && torchConfig.Rotation != null && torchConfig.Rotation.Length == 1)
            {
                var direction = (Direction)"NESW".IndexOf(torchConfig.Rotation[0]);
                torch.transform.rotation = direction.GetRotation();
            }
            else
            {
                AttachToWall(ref torch);
            }

            AssignObjectConfigByType(torch, "lever", mapBlock);

            mapBlock.addGameObject(torch);

            mapBlock.Type = "torch";
            mapBlock.Interactive = false;
            mapBlock.MinimapColor = Color.yellow;
        }

        public override char forMapChar()
        {
            return 't';
        }
    }
}
