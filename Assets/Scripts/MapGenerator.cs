using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Map;

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
    	private Dictionary<Vector2, Scripts.Map.MapBlock> Blocks;

        private bool Initialized = false;

        private Vector2 startLocation;

        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            
            if (Initialized) return;

            Prefabs = new Dictionary<string, GameObject>();
            foreach (NamedPrefab pref in prefabs)
            {
                Prefabs.Add(pref.name, pref.prefab);
            }

            this.LoadMap(MapFile.text);
            Initialized = true;
        }

        void LoadMap(string mapString)
        {
            ClearMap();

            var mapLoader = new MapLoader();
            mapLoader.loadMap(mapString);
            Blocks = mapLoader.MapBlocks;
            var mapBuilder = new MapBlockBuilder(Blocks, Prefabs, ref MapObject);
            mapBuilder.Build();

            var miniMapController = MiniMapController.getInstance();
            miniMapController.Rows = mapLoader.Rows;
            miniMapController.Columns = mapLoader.Columns;

            foreach (var mapBlock in mapLoader.MapBlocks.Values)
            {
                miniMapController.Cells[mapBlock.Location] = mapBlock.MinimapColor;
            }

            if (startLocation != null) miniMapController.visit(startLocation);
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

        GameObject AddObject(Vector2 location, GameObject prefab)
        {
            var position = PositionForLocation(location);
			var instance = Instantiate(prefab, position, Quaternion.identity);
			instance.transform.parent = MapObject.transform;
            instance.name = prefab.name + "_" + location.x + "x" + location.y;

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

        public void MovePlayer(int x, int y)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            PlayerController controller = player.GetComponent<PlayerController>();

            controller.RotateTo(Direction.East, false);
            controller.MoveTo(new Vector2(x, y));
            startLocation = new Vector2(x, y);
        }

        public void OnEnable()
        {
            ClearMap();
            Initialize();
        }

        public void OnDisable()
        {
            ClearMap();
        }
    }
}
