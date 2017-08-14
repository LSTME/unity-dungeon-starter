using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Map.Blocks
{
    class Ducat : EmptyCorridor
    {
        public override void createGameObject(MapBlock mapBlock, Dictionary<string, GameObject> prefabList, ref GameObject MapObject)
        {
            mapBlock.Initialize();

            base.createGameObject(mapBlock, prefabList, ref MapObject);

            GameObject template = prefabList["ducat"];

            GameObject ducat = AddObject(mapBlock.Location, template, ref MapObject);
            ducat.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            mapBlock.addGameObject(ducat);

            mapBlock.Type = "ducat";
            mapBlock.MinimapColor = new Color(0.94f, 1f, 0.46f);
        }

        public override char forMapChar()
        {
            return 'D';
        }
    }
}
