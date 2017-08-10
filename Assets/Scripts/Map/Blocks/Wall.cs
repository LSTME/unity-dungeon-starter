﻿using System;
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
            if (north != null && north.MapSymbol != '#')
            {
                GameObject template = prefabList["wall_one_side"];

                GameObject wall = AddObject(mapBlock.Location, template, ref MapObject, "_N");

                wall.transform.rotation = Direction.North.GetRotation();

                mapBlock.addGameObject(wall);
            }

            if (south != null && south.MapSymbol != '#')
            {
                GameObject template = prefabList["wall_one_side"];

                GameObject wall = AddObject(mapBlock.Location, template, ref MapObject, "_S");

                wall.transform.rotation = Direction.South.GetRotation();

                mapBlock.addGameObject(wall);
            }

            if (west != null && west.MapSymbol != '#')
            {
                GameObject template = prefabList["wall_one_side"];

                GameObject wall = AddObject(mapBlock.Location, template, ref MapObject, "_W");

                wall.transform.rotation = Direction.West.GetRotation();

                mapBlock.addGameObject(wall);
            }

            if (east != null && east.MapSymbol != '#')
            {
                GameObject template = prefabList["wall_one_side"];

                GameObject wall = AddObject(mapBlock.Location, template, ref MapObject, "_E");

                wall.transform.rotation = Direction.East.GetRotation();

                mapBlock.addGameObject(wall);
            }

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
