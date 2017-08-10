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

            foreach (var row in mapString.Split('\n'))
            {
                if (row.Length == 0) continue;
                if (row.StartsWith("//")) continue;

                loadMapChar(row, ref y, ref columns);
                loadBlockAttributes(row);
            }

            rows = y;
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
    }
}
