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

	        if (EnableRotation(mapBlock))
	        {
		        var ducatConfig = mapBlock.getObjectConfigForType("ducat");
		        if (ducatConfig != null && ducatConfig.Rotation != null && ducatConfig.Rotation.Length == 1)
		        {
			        var direction = (Direction)"NESW".IndexOf(ducatConfig.Rotation[0]);
			        ducat.transform.rotation = direction.GetRotation();
		        }
		        else
		        {
			        AttachToWall(ref ducat);
		        }    
	        }

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

	    protected bool EnableRotation(MapBlock mapBlock)
	    {
		    switch (GetDucatType(mapBlock))
		    {
				case "chest":
					return true;
				default:
					return false;
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
