using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Map.Blocks
{
    class Decoration : EmptyCorridor
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            base.createGameObject(mapBlock, prefabList, ref MapObject);

            string prefab = selectDecoration(mapBlock);

            GameObject template = prefabList[prefab];

            GameObject decoration = AddObject(mapBlock.Location, template, ref MapObject);

            if (mapBlock.Attributes.Length >= 2)
            {
                var direction = (Direction)"NESW".IndexOf(mapBlock.Attributes[1]);
                decoration.transform.rotation = direction.GetRotation();
            }
            else
            {
                AttachToWall(ref decoration);
            }

            mapBlock.addGameObject(decoration);

            mapBlock.Type = "decoration";
            mapBlock.Interactive = false;
            mapBlock.MinimapColor = selectDecorationColor(mapBlock);
        }

        public override char forMapChar()
        {
            return 'd';
        }

        protected string selectDecoration(MapBlock mapBlock)
        {
            var type = getDecorationType(mapBlock);

            switch (type)
            {
                case "pillar":
                    return "decoration_pillar";
                case "chair":
                    return "decoration_chair";
                case "table":
                    return "decoration_table";
                case "library":
                default:
                    return "decoration_library";
            }
        }

        protected Color selectDecorationColor(MapBlock mapBlock)
        {
            var type = getDecorationType(mapBlock);

            switch (type)
            {
                case "table":
                case "pillar":
                    return new Color(0.595f, 0.331f, 0.135f);
                default:
                    return new Color(0.645f, 0.371f, 0.175f);
            }
        }

        protected string getDecorationType(MapBlock mapBlock)
        {
            var type = "";

            if (mapBlock.Attributes.Length >= 1)
            {
                type = mapBlock.Attributes[0];
            }

            return type;
        }
    }
}
