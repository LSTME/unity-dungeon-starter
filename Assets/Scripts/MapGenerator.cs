using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Scripts.Map;
using UnityEditor;

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
        private Dictionary<Vector2, MapBlock> Blocks;

        public Map.Config.GameLogic GameLogic { get; set; }

        private bool Initialized = false;
        private bool _isMapLoaded = false;

        public bool IsMapLoaded
        {
            get { return _isMapLoaded; }
        }

        private Vector2 startLocation = new Vector2(0, 0);

        public static MapGenerator getInstance()
        {
            return GameObject.FindGameObjectWithTag("Map").GetComponent<MapGenerator>();
        }

        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (Initialized) return;

            Prefabs = new Dictionary<string, GameObject>();
            foreach (NamedPrefab pref in prefabs)
            {
                Prefabs.Add(pref.name, pref.prefab);
            }

            Initialized = true;
        }

        public void LoadMapFile(string mapName)
        {
            var mapPath = Path.Combine("Maps", mapName);
            var textAsset = Resources.Load<TextAsset>(mapPath);
            if (textAsset != null)
            {
                LoadMap(textAsset.text);
                _isMapLoaded = true;
                PlayerController.InterpreterLock.Set();
            }
        }

        void LoadMap(string mapString)
        {
            ClearMap();

            var mapLoader = new MapLoader();
            mapLoader.loadMap(mapString);
            Blocks = mapLoader.MapBlocks;
            GameLogic = YamlConfigParser.GameLogic;
            var mapBuilder = new MapBlockBuilder(Blocks, Prefabs, ref MapObject);
            mapBuilder.Build();
            MovePlayer(YamlConfigParser.Player.Start[0] - 1, YamlConfigParser.Player.Start[1] - 1, YamlConfigParser.Player.Rotation);
            

            var miniMapController = MiniMapController.getInstance();
            miniMapController.Rows = mapLoader.Rows;
            miniMapController.Columns = mapLoader.Columns;

            foreach (var mapBlock in mapLoader.MapBlocks.Values)
            {
                miniMapController.Cells.Add(mapBlock.Location);
            }

            miniMapController.visit(startLocation);
        }

        public MapBlock GetMapBlockAtLocation(Vector2 location)
        {
            if (Blocks == null) return null;
            if (!Blocks.ContainsKey(location)) return null;

            return Blocks[location];
        }

        void ClearMap()
        {
            if (MapObject == null) return;

            while (MapObject.transform.childCount > 0)
            {
                var child = MapObject.transform.GetChild(0);
                GameObject.Destroy(child.gameObject);
                child.parent = null;
            }

            Initialized = false;
        }

        public MapBlock GetBlockAtLocation(Vector2 loc)
        {
            if (Blocks != null && Blocks.ContainsKey(loc))
                return Blocks[loc];

            return null;
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

        public void MovePlayer(int x, int y, string rotation = "East")
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            PlayerController controller = player.GetComponent<PlayerController>();

            controller.RotateTo(DirectionMethods.GetFromString(rotation), false);
            controller.MoveTo(new Vector2(x, y));
            startLocation = new Vector2(x, y);
        }

        public void OnEnable()
        {
            ClearMap();
            Initialize();
        }
    }
}