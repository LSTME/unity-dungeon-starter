using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Map
{
    class MapBlockBuilder
    {
        private Dictionary<Vector2, MapBlock> mapBlocks;
        private Dictionary<string, GameObject> prefabList;
        private GameObject MapObject;

        public Dictionary<Vector2, MapBlock> MapBlocks
        {
            get
            {
                return mapBlocks;
            }
        }

        public MapBlockBuilder(Dictionary<Vector2, MapBlock> mapBlocks, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            this.mapBlocks = mapBlocks;
            this.prefabList = prefabList;
            this.MapObject = MapObject;
        }

        public void Build()
        {
            foreach (var mapBlock in mapBlocks.Values)
            {
                var builder = BlockBuilderFactory.getBlockBuilder(mapBlock.MapSymbol);
                if (builder == null) continue;
                builder.assignSurroundingBlocks(GetNorthBlock(mapBlock), GetSouthBlock(mapBlock), GetWestBlock(mapBlock), GetEastBlock(mapBlock));
                builder.createGameObject(mapBlock, prefabList, ref MapObject);
            }
        }

        private MapBlock GetNorthBlock(MapBlock mapBlock)
        {
            return GetBlockAtLocation(MapUtils.GetNorthLocation(mapBlock.Location));
        }

        private MapBlock GetSouthBlock(MapBlock mapBlock)
        {
            return GetBlockAtLocation(MapUtils.GetSouthLocation(mapBlock.Location));
        }

        private MapBlock GetWestBlock(MapBlock mapBlock)
        {
            return GetBlockAtLocation(MapUtils.GetWestLocation(mapBlock.Location));
        }

        private MapBlock GetEastBlock(MapBlock mapBlock)
        {
            return GetBlockAtLocation(MapUtils.GetEastLocation(mapBlock.Location));
        }

        private MapBlock GetBlockAtLocation(Vector2 location)
        {
            if (mapBlocks.ContainsKey(location)) return mapBlocks[location];
            return null;
        }
    }
}
