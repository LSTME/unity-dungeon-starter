using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Map.Blocks
{
    class PlayerStartPosition: EmptyCorridor
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            mapBlock.Initialize();

            base.createGameObject(mapBlock, prefabList, ref MapObject);

            var Map = GameObject.FindGameObjectWithTag("Map");
            var mapGenerator = Map.GetComponent<MapGenerator>();
            if (mapGenerator != null) mapGenerator.MovePlayer((int)mapBlock.Location.x, (int)mapBlock.Location.y);
        }

        public override char forMapChar()
        {
            return 'P';
        }

    }
}
