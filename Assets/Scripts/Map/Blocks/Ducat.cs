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

            string prefab = getDucatType(mapBlock);

            GameObject template = prefabList[prefab];

            GameObject ducat = AddObject(mapBlock.Location, template, ref MapObject);

            if (prefab == "ducat")
            {
                ducat.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);                
            }

            AssignObjectConfigByType(ducat, "ducat", mapBlock);

            mapBlock.addGameObject(ducat);
        }

        public override char forMapChar()
        {
            return 'D';
        }        

        protected string getDucatType(MapBlock mapBlock)
        {
            var ducatConfig = mapBlock.getObjectConfigForType("ducat");

            var type = "";

            if (ducatConfig != null && ducatConfig.Model != null)
            {
                type = ducatConfig.Model;
            }

            return type;
        }
    }
}
