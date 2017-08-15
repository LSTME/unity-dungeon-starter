using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Map.Blocks
{
    class EmptyCorridor : AbstractBlockBuilder
    {
        public bool GenerateFloor = true;
        public bool GenerateCeiling = true;

        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            mapBlock.Initialize();

            MapObject = GenerateFloorObject(mapBlock, prefabList, MapObject);

            MapObject = GenerateCeilingObject(mapBlock, prefabList, MapObject);
        }

        private GameObject GenerateCeilingObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, GameObject MapObject)
        {
            if (!GenerateCeiling) return MapObject;

            GameObject templateCeiling = prefabList["ceiling"];

            GameObject ceiling = AddObject(mapBlock.Location, templateCeiling, ref MapObject);

            mapBlock.addGameObject(ceiling);

            return MapObject;
        }

        private GameObject GenerateFloorObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, GameObject MapObject)
        {
            if (!GenerateFloor) return MapObject;

            GameObject templateFloor = prefabList["floor"];

            GameObject floor = AddObject(mapBlock.Location, templateFloor, ref MapObject);

            mapBlock.addGameObject(floor);

            return MapObject;
        }

        public override char forMapChar()
        {
            return ' ';
        }
    }
}
