using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class MapBlock
	{
		public string Type { get; set; }
		public Vector2 Location  { get; set; }
		public GameObject GameObject { get; set; }
		public string[] Attributes { get; set; }

		public bool IsWalkable
		{
			get
			{
				switch (Type)
				{
					case "wall":
						return false;
					case "torch":
						return false;
                    case "vgate":
                    case "hgate":
                        return ((DoorsController)GameObject.GetComponent(typeof(DoorsController))).IsOpen;
					default:
						return true;
				}
			}
		}
	}

    public class MapGenerator : MonoBehaviour
    {

        [Serializable]
        public struct NamedPrefab
        {
            public string name;
            public GameObject prefab;
        }
        public NamedPrefab[] prefabs;
        private Dictionary<string, GameObject> Prefabs;

        public GameObject MapObject;
        public TextAsset MapFile;
    	private Dictionary<Vector2, MapBlock> Blocks;
        private Dictionary<Vector2, GameObject> Interactives;

        // Use this for initialization
        void Start()
        {
            Prefabs = new Dictionary<string, GameObject>();
            foreach (NamedPrefab pref in prefabs)
            {
                Prefabs.Add(pref.name, pref.prefab);
            }

            this.LoadMap(MapFile.text);
        }

        void LoadMap(string mapString)
        {
            ClearMap();

            var y = 0;
            foreach (var row in mapString.Split('\n'))
            {
                if (row.Length == 0) continue;
                if (row.StartsWith("//")) continue;

                if (row[0] == '#')
                {
                    for (int x = 0; x < row.Length; x++) 
                    {
                        AddBlock(row[x], x, y);
                    }
                    y++;
                }
                else
                {
                    var attrs = row.Split(' ');
                    if (attrs.Length < 3) continue;

                    var attrX = int.Parse(attrs[0]);
                    var attrY = int.Parse(attrs[1]);
                    var mapBlock = GetBlockAtLocation(new Vector2(attrX, attrY));
                    if (mapBlock != null)
                    {
                        mapBlock.Attributes = new string[attrs.Length - 2];
                        Array.Copy(attrs, 2, mapBlock.Attributes, 0, attrs.Length - 2);
                    }
                }
            }

            foreach (var mapBlock in Blocks.Values)
            {
                switch (mapBlock.Type)
                {
                    case "wall_lever":
                    case "torch":
                        Debug.Log(mapBlock.Attributes);
                        if (mapBlock.Attributes.Length > 0)
                        {
                            var direction = (Direction)"NESW".IndexOf(mapBlock.Attributes[0]);
                            mapBlock.GameObject.transform.rotation = direction.GetRotation();
                        }
                        break;
                    case "vgate":
                        mapBlock.GameObject.transform.rotation = Direction.East.GetRotation();
                        break;
                }
            }
        }

        void ClearMap()
        {
            Debug.Log("Clearing map");

            for (int i = 0; i < MapObject.transform.childCount; i++)
            {
                var child = MapObject.transform.GetChild(0);
                child.parent = null;

                var obj = child.gameObject;
                GameObject.Destroy(obj);
            }

            Interactives = new Dictionary<Vector2, GameObject>();
    		Blocks = new Dictionary<Vector2, MapBlock>();
        }

        void AddBlock(char c, int x, int y)
        {
            var location = new Vector2(x, y);

            if (c != '#') // put floor/ceiling under everything but wall
            {
				AddObject(location, Prefabs["floor"]);
				// AddObject(location, Prefabs["ceiling"]);
            }

            GameObject gameObjectTemplate = null; 
            var orientation = Quaternion.identity;
            
            var mapBlock = new MapBlock();
            switch (c)
            {
                case '#': 
                    gameObjectTemplate = Prefabs["wall"];
                    mapBlock.Type = "wall";
                    break;
                case '-':
                    gameObjectTemplate = Prefabs["gate"];
                    mapBlock.Type = "hgate";
                    break;
                case '|':
                    gameObjectTemplate = Prefabs["gate"];
                    mapBlock.Type = "vgate";
                    break;
                case 'P':
                    MovePlayer(x, y);
                    mapBlock.Type = "spawn";
                    break;
                case 'l':
                    gameObjectTemplate = Prefabs["wall_lever"];
                    mapBlock.Type = "wall_lever";
                    break;
                case 't':
                    gameObjectTemplate = Prefabs["torch"];
                    mapBlock.Type = "torch";
                    break;
            }
            
            if (gameObjectTemplate != null)
            {
                mapBlock.GameObject = AddObject(location, gameObjectTemplate);
                Blocks.Add(new Vector2(x, y), mapBlock);
            }
        }

        public MapBlock GetBlockAtLocation(Vector2 loc)
        {
            if (Blocks.ContainsKey(loc))
                return Blocks[loc];
            
            return null;
        }

        GameObject AddObject(Vector2 location, GameObject prefab)
        {
            var position = PositionForLocation(location);
			var instance = Instantiate(prefab, position, Quaternion.identity);
			instance.transform.parent = MapObject.transform;

            if (instance.tag == "Interactive")
            {
                Interactives.Add(location, instance);
            }

            return instance;
        }

        public bool IsWalkable(Vector2 loc)
        {
            var mapBlock = GetBlockAtLocation(loc);
            if (mapBlock != null)
                return mapBlock.IsWalkable;
            
            return true;
        }

        public Vector3 PositionForLocation(Vector2 loc)
        {
            return new Vector3(loc.x, 0, -loc.y);
        }

        public GameObject GetInteractive(Vector2 loc)
        {
            if (Interactives.ContainsKey(loc))
            {
                return Interactives[loc];
            }
            else
            {
                return null;
            }
        }

        void MovePlayer(int x, int y)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            PlayerController controller = (PlayerController)player.GetComponent(typeof(PlayerController));

            controller.RotateTo(Direction.East, false);
            controller.MoveTo(new Vector2(x, y), false);
        }
    }
}
