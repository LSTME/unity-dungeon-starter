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
            mapBlock.Initialize();

            base.createGameObject(mapBlock, prefabList, ref MapObject);

            string prefab = selectDecoration(mapBlock);

            GameObject template = prefabList[prefab];

            GameObject decoration = AddObject(mapBlock.Location, template, ref MapObject);

            var decorationConfig = mapBlock.getObjectConfigForType("decoration");
            if (decorationConfig != null && decorationConfig.Rotation != null && decorationConfig.Rotation.Length == 1)
            {
                var direction = (Direction)"NESW".IndexOf(decorationConfig.Rotation[0]);
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
            var decorationConfig = mapBlock.getObjectConfigForType("decoration");

            var type = "";

            if (decorationConfig != null && decorationConfig.Model != null)
            {
                type = decorationConfig.Model;
            }

            return type;
        }
    }
}
