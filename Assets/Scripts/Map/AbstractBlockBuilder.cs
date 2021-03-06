﻿using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Map
{
    abstract class AbstractBlockBuilder: Interfaces.BlockBuilder.IBlockBuilder
    {
        protected MapBlock north;
        protected MapBlock south;
        protected MapBlock west;
        protected MapBlock east;

        public void assignSurroundingBlocks(MapBlock north, MapBlock south, MapBlock west, MapBlock east)
        {
            this.north = copyIfNotNull(north);
            this.south = copyIfNotNull(south);
            this.west = copyIfNotNull(west);
            this.east = copyIfNotNull(east);
        }

        private MapBlock copyIfNotNull(MapBlock mapBlock)
        {
            if (mapBlock == null) return null;

            return mapBlock.getUnbuildedCopy();
        }

        abstract public void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject);

        abstract public char forMapChar();
        
        GameObject AddBlockByPrefab(GameObject prefab)
        {
            return prefab;
        }

        protected Vector3 PositionForLocation(Vector2 loc)
        {
            return new Vector3(loc.x, 0, -loc.y);
        }

        protected GameObject AddObject(Vector2 location, GameObject prefab, ref GameObject MapObject, string suffix = "")
        {
            var position = PositionForLocation(location);
            var instance = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
            instance.transform.localScale = new Vector3((1.0f / 0.042f) / 24.0f, (1.0f / 0.042f) / 24.0f, (1.0f / 0.042f) / 24.0f);
            instance.transform.parent = MapObject.transform;
            instance.name = prefab.name + "_" + location.x + "x" + location.y + suffix;

            return instance;
        }

        protected void AttachToWall(ref GameObject gameObject)
        {
            if (north != null && north.MapSymbol == '#')
            {
                gameObject.transform.rotation = Direction.North.GetRotation();
                return;
            }

            if (south != null && south.MapSymbol == '#')
            {
                gameObject.transform.rotation = Direction.South.GetRotation();
                return;
            }

            if (west != null && west.MapSymbol == '#')
            {
                gameObject.transform.rotation = Direction.West.GetRotation();
                return;
            }

            if (east != null && east.MapSymbol == '#')
            {
                gameObject.transform.rotation = Direction.East.GetRotation();
                return;
            }
        }

        protected void AssignObjectConfigByType(GameObject gameObject, string type, MapBlock mapBlock)
        {
            mapBlock.Initialize();

            var components = gameObject.GetComponents<AbstractGameObjectController>();
            foreach (var component in components)
            {
                if (component == null) continue;
                component.ObjectConfig = mapBlock.getObjectConfigForType(type);
            }
        }
    }
}
