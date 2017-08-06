using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{

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
        private List<Vector2> Walkable;

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

            Walkable = new List<Vector2>();
            var x = 0;
            foreach (string row in mapString.Split('\n'))
            {
                if (row.Length == 0) continue;

                for (int y = 0; y < row.Length; y++)
                {
                    this.AddBlock(row[y], x, y);

                    if (row[y] == 'P') this.MovePlayer(x, y);
                }
                x++;
            };
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
        }

        void AddBlock(char c, int x, int y)
        {
            Vector2 location = new Vector3(x, y);

            if (c != '#') // put floor/ceiling under everything but wall
            {
				AddObject(location, Direction.North, Prefabs["floor"]);
				AddObject(location, Direction.North, Prefabs["ceiling"]);
                Walkable.Add(location);
            }

            GameObject GO = null;
            var ortientation = Direction.North;

            switch (c)
            {
                case '#':
                    GO = Prefabs["wall"];
                    break;
                case '-':
                    GO = Prefabs["gate"];
                    break;
                case '|':
                    GO = Prefabs["gate"];
                    ortientation = Direction.East;
                    break;
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                    GO = Prefabs["wall_lever"];
                    ortientation = (Direction)"ijkl".IndexOf(c);
                    break;
            }

            if (GO != null)
            {
				AddObject(location, ortientation, GO);
            }
        }

        void AddObject(Vector2 location, Direction direction, GameObject prefab)
        {
			var instance = Instantiate(prefab, new Vector3(location.x, 0, location.y), direction.GetRotation());
			instance.transform.parent = MapObject.transform;
        }

        public bool IsWalkable(Vector2 loc)
        {
            return Walkable.Contains(loc);
        }

        public Vector3 PositionForLocation(Vector2 loc)
        {
            return new Vector3(loc.x, 0, loc.y);
        }

        void MovePlayer(int x, int y)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            PlayerController controller = (PlayerController)player.GetComponent(typeof(PlayerController));

            controller.RotateTo(Direction.East, false);
            controller.MoveTo(new Vector2(x, y), false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
