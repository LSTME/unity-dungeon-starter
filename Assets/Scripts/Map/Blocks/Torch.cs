using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Map.Blocks
{
    class Torch : EmptyCorridor
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            base.createGameObject(mapBlock, prefabList, ref MapObject);

            GameObject template = prefabList["torch"];

            GameObject torch = AddObject(mapBlock.Location, template, ref MapObject);

            if (mapBlock.Attributes.Length >= 1)
            {
                var direction = (Direction)"NESW".IndexOf(mapBlock.Attributes[0]);
                torch.transform.rotation = direction.GetRotation();
            } else
            {
                AttachToWall(ref torch);
            }

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
