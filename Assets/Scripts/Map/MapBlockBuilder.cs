using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void build()
        {
            foreach (var mapBlock in mapBlocks.Values)
            {
                var Builder = BlockBuilderFactory.getBlockBuilder(mapBlock.MapSymbol);
                if (Builder == null) continue;
                Builder.assignSurroundingBlocks(getNorthBlock(mapBlock), getSouthBlock(mapBlock), getWestBlock(mapBlock), getEastBlock(mapBlock));
                Builder.createGameObject(mapBlock, prefabList, ref MapObject);
            }
        }

        private MapBlock getNorthBlock(MapBlock mapBlock)
        {
            var location = mapBlock.Location;
            var diff = new Vector2(0, -1);
            diff = diff + location;
            return getBlockAtLocation(diff);
        }

        private MapBlock getSouthBlock(MapBlock mapBlock)
        {
            var location = mapBlock.Location;
            var diff = new Vector2(0, 1);
            diff = diff + location;
            return getBlockAtLocation(diff);
        }

        private MapBlock getWestBlock(MapBlock mapBlock)
        {
            var location = mapBlock.Location;
            var diff = new Vector2(-1, 0);
            diff = diff + location;
            return getBlockAtLocation(diff);
        }

        private MapBlock getEastBlock(MapBlock mapBlock)
        {
            var location = mapBlock.Location;
            var diff = new Vector2(1, 0);
            diff = diff + location;
            return getBlockAtLocation(diff);
        }

        private MapBlock getBlockAtLocation(Vector2 location)
        {
            if (mapBlocks.ContainsKey(location)) return mapBlocks[location];
            return null;
        }
    }
}
