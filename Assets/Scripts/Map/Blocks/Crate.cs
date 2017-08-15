using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Map.Blocks
{
    class Crate : EmptyCorridor
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            mapBlock.Initialize();

            base.createGameObject(mapBlock, prefabList, ref MapObject);

            var crateTemplate = prefabList["crate_01"];

            GameObject crate = AddObject(mapBlock.Location, crateTemplate, ref MapObject);

            mapBlock.addGameObject(crate);
        }

        public override char forMapChar()
        {
            return 'c';
        }
    }
}
