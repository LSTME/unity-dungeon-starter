using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GUIPainter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Initialize();
	}

	public int Columns = 1920;
	public int Rows = 1080;

	private Dictionary<Vector2, Color> _colorMap;
	private bool _Initialized = false;

	private void OnGUI()
	{
		PaintPoints();
	}

	private void Initialize()
	{
		if (_Initialized) return;
		
		_colorMap = new Dictionary<Vector2, Color>();

		_Initialized = true;
	}
	
	public static GUIPainter getInstance()
	{
		return GameObject.FindWithTag("GUI").GetComponent<GUIPainter>();
	}

	private void PaintPoints()
	{
		foreach (var Position in _colorMap.Keys)
		{
			var PointColor = _colorMap[Position];
			
			if (PointColor == null) continue;

			var PointTexture = new Texture2D(1, 1);
			PointTexture.SetPixel(0, 0, PointColor);
			PointTexture.Apply();
			
			var Area = new Rect(Position.x / Columns * Screen.width, Position.y / Rows * Screen.height, Screen.width / Columns, Screen.height / Rows);
			
			GUI.skin.box.normal.background = PointTexture;
			GUI.Box(Area, GUIContent.none);
		}
	}

	public void PaintPointAt(Vector2 PointPosition, Color PointColor)
	{
		var NewPointPosition = new Vector2((int)PointPosition.x, (int)PointPosition.y);
		
		if (NewPointPosition.x < 0 || NewPointPosition.x >= Columns) return;
		if (NewPointPosition.y < 0 || NewPointPosition.y >= Rows) return;

		if (_colorMap.ContainsKey(NewPointPosition))
		{
			_colorMap.Remove(NewPointPosition);
		}
		_colorMap.Add(NewPointPosition, PointColor);
	}

	public void RemovePointAt(Vector2 PointPosition)
	{
		var NewPointPosition = new Vector2((int)PointPosition.x, (int)PointPosition.y);
		
		if (_colorMap.ContainsKey(NewPointPosition))
		{
			_colorMap.Remove(NewPointPosition);
		}
	}
}
