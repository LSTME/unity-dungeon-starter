using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Map
{
    class MapLoader
    {
        private Dictionary<Vector2, MapBlock> mapBlocks = new Dictionary<Vector2, MapBlock>();

        private int columns = 0;
        private int rows = 0;

        public Dictionary<Vector2, MapBlock> MapBlocks
        {
            get
            {
                return mapBlocks;
            }
        }

        public int Rows
        {
            get
            {
                return rows;
            }
        }

        public int Columns
        {
            get
            {
                return columns;
            }
        }

        public void loadMap(string mapString)
        {
            mapBlocks.Clear();

            columns = 0;
            rows = 0;

            int y = 0;

            StringBuilder yamlConfig = new StringBuilder();

            foreach (var row in mapString.Split('\n'))
            {
                if (row.Length == 0) continue;
                if (row.StartsWith("//")) continue;

                loadMapChar(row, ref y, ref columns);
                loadYamlConfig(row, yamlConfig);
            }

            YamlConfigParser.Parse(yamlConfig.ToString(), mapBlocks);
            
            rows = y;

            deleteUnaccessibleBlocks();
        }

        private void loadMapChar(string row, ref int y, ref int columns)
        {
            if (row[0] != '#') return;

            for (int x = 0; x < row.Length; x++)
            {
                char mapChar = row[x];
                updateMapBlockAt(x, y, mapChar);
            }

            y++;
            columns = Math.Max(columns, row.Length);
        }

        private void loadBlockAttributes(string row)
        {
            if (row[0] == '#') return;

            var attrs = row.Trim().Split(' ');
            if (attrs.Length < 3) return;

            var attrX = int.Parse(attrs[0]);
            var attrY = int.Parse(attrs[1]);

            updateMapBlockAt(attrX, attrY, attrs);
        }

        private void loadYamlConfig(string row, StringBuilder yamlConfig)
        {
            if (row[0] == '#') return;

            string trimRow = row.Trim('\n').Trim('\r');

            if (trimRow.Length > 0)
            {
                yamlConfig.AppendLine(trimRow);
            }
        }

        private void updateMapBlockAt(int x, int y, char mapChar)
        {
            var key = new Vector2(x, y);

            if (mapBlocks.ContainsKey(key)) return;

            mapBlocks.Add(key, new MapBlock(mapChar, x, y));
        }

        private void updateMapBlockAt(int x, int y, string[] attributes)
        {
            var key = new Vector2(x, y);

            if (!mapBlocks.ContainsKey(key)) return;

            var mapBlock = mapBlocks[key];

            var inputAttributes = new string[attributes.Length - 2];
            Array.Copy(attributes, 2, inputAttributes, 0, attributes.Length - 2);

            mapBlock.updateInputAttributes(inputAttributes);
        }

        private void deleteUnaccessibleBlocks()
        {
            var start = findStartLocation();
            HashSet<Vector2> accessible = new HashSet<Vector2>();
            markMapCorridors(start, accessible);
            addWallsToAccessible(accessible);
            clearUnaccessibleBlocks(accessible);
        }

        private Vector2 findStartLocation()
        {
            foreach (var mapBlock in MapBlocks.Values)
            {
                if (mapBlock.MapSymbol == 'P')
                {
                    return mapBlock.Location;
                }
            }

            return new Vector2(-1,-1);
        }

        private void markMapCorridors(Vector2 start, HashSet<Vector2> accessible)
        {
            Queue<Vector2> searchQueue = new Queue<Vector2>();
            searchQueue.Enqueue(start);

            Vector2[] directions = { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };

            while (searchQueue.Count > 0)
            {
                Vector2 current = searchQueue.Dequeue();
                if (accessible.Contains(current)) continue;
                accessible.Add(current);
                
                foreach (var direction in directions)
                {
                    var nextDirection = direction + current;
                    if (MapBlocks.ContainsKey(nextDirection) && MapBlocks[nextDirection].MapSymbol != '#')
                    {
                        searchQueue.Enqueue(nextDirection);
                    }
                }

                MapBlocks[current].Initialize();

                var teleportConfig = MapBlocks[current].getObjectConfigForType("teleport");

                if (teleportConfig != null && teleportConfig.Teleport != null && teleportConfig.Teleport.Target != null && teleportConfig.Teleport.Target.Length == 2)
                {
                    Vector2 nextDirection = new Vector2(teleportConfig.Teleport.Target[0] - 1, teleportConfig.Teleport.Target[1] - 1);
                    if (mapBlocks.ContainsKey(nextDirection) && MapBlocks[nextDirection].MapSymbol != '#')
                    {
                        searchQueue.Enqueue(nextDirection);
                    }
                }
            }
        }

        private void addWallsToAccessible(HashSet<Vector2> accessible)
        {
            Vector2[] directions = { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(-1, -1) };

            HashSet<Vector2> toAdd = new HashSet<Vector2>();

            foreach (var current in accessible)
            {
                foreach (var direction in directions)
                {
                    var location = current + direction;
                    if (mapBlocks.ContainsKey(location) && mapBlocks[location].MapSymbol == '#' && !toAdd.Contains(location))
                    {
                        toAdd.Add(location);
                    }
                }
            }

            accessible.UnionWith(toAdd);
        }

        private void clearUnaccessibleBlocks(HashSet<Vector2> accessible)
        {
            List<Vector2> toRemove = new List<Vector2>();
            foreach (var location in mapBlocks.Keys)
            {
                if (!accessible.Contains(location))
                {
                    toRemove.Add(location);
                }
            }

            foreach (var location in toRemove)
            {
                if (mapBlocks.ContainsKey(location))
                {
                    mapBlocks.Remove(location);
                }
            }
        }
    }
}
