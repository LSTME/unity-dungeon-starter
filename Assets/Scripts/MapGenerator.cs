using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class MapGenerator : MonoBehaviour {

	[Serializable]
	public struct NamedPrefab {
		public string name;
		public GameObject prefab;
	}
	public NamedPrefab[] prefabs;
	private Dictionary<string, GameObject> Prefabs;

	public GameObject MapObject;
	public TextAsset MapFile;
	private List<Vector2> Walkable;

	// Use this for initialization
	void Start () {
		Prefabs = new Dictionary<string, GameObject>();
		foreach (NamedPrefab pref in prefabs)
		{
			Prefabs.Add(pref.name, pref.prefab);
		}

		this.LoadMap(MapFile.text);
	}

	void LoadMap(string mapString) {
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

	void ClearMap() {
		Debug.Log("Clearing map");

		for (int i = 0; i < MapObject.transform.childCount; i++)
		{
			var child = MapObject.transform.GetChild(0);
			child.parent = null;
			
			var obj = child.gameObject;
			GameObject.Destroy(obj);
		}
	}

	void AddBlock(char c, int x, int y) {
		Vector3 location = new Vector3(x, 0, y);

		if (c != '#') // put floor/ceiling under everything but wall
		{
			var floor = Instantiate(Prefabs["floor"], location, Quaternion.identity);
			floor.transform.parent = MapObject.transform;
			var ceiling = Instantiate(Prefabs["ceiling"], location, Quaternion.identity);
			ceiling.transform.parent = MapObject.transform;

			Walkable.Add(new Vector2(x, y));
		}

		GameObject GO = null; 
		var ortientation = Quaternion.identity;
		
		switch (c)
		{
			case '#': 
				GO = Prefabs["wall"]; 
				break;
			case '-':
				GO = Prefabs["gate"];
				ortientation = Quaternion.AngleAxis(90, Vector3.up);
				break;
			case '|':
				GO = Prefabs["gate"];
				break;
			case 'i':
			case 'j':
			case 'k':
			case 'l':
				GO = Prefabs["wall_lever"];
				ortientation = Quaternion.AngleAxis(90*"ijkl".IndexOf(c), Vector3.up);
				break;
		}
		
		if (GO != null)
		{
			var instance = Instantiate(GO, location, ortientation);
			instance.transform.parent = MapObject.transform;
		}
	}

	public bool IsWalkable(Vector2 loc)
	{
		return Walkable.Contains(loc);
	}

	public Vector3 PositionForLocation(Vector2 loc)
	{
		return new Vector3(loc.x, 0, loc.y);
	}

	void MovePlayer(int x, int y) {
		var player = GameObject.FindGameObjectWithTag("Player");
		PlayerController controller = (PlayerController)player.GetComponent(typeof(PlayerController));

		controller.RotateTo(Direction.East, false);
		controller.MoveTo(new Vector2(x, y), false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
