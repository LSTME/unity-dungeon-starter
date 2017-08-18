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

			GameObject template = prefabList[GetModel(mapBlock)];

            GameObject ducat = AddObject(mapBlock.Location, template, ref MapObject);

			AssignObjectConfigByType(ducat, "ducat", mapBlock);

			mapBlock.addGameObject(ducat);
        }

        public override char forMapChar()
        {
            return 'D';
        }

		protected string GetModel(MapBlock mapBlock) 
		{
			switch (GetDucatType (mapBlock))
			{
				case "chest":
					return "chest";
				default:
					return "ducat";
			}
		}

		protected string GetDucatType(MapBlock mapBlock)
		{
			var decorationConfig = mapBlock.getObjectConfigForType("ducat");

			var type = "";

			if (decorationConfig != null && decorationConfig.Model != null)
			{
				type = decorationConfig.Model;
			}

			return type;
		}
    }
}
