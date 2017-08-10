using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Map.Blocks
{
    class Wall : Scripts.Map.AbstractBlockBuilder
    {

        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            GameObject template = prefabList["wall"];

            GameObject wall = AddObject(mapBlock.Location, template, ref MapObject);

            mapBlock.addGameObject(wall);

            mapBlock.Type = "wall";
            mapBlock.Interactive = false;
            mapBlock.MinimapColor = new Color(0.545f, 0.271f, 0.075f);
        }

        public override char forMapChar()
        {
            return '#';
        }
    }
}
