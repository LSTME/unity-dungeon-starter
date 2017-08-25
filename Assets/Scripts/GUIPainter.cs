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
	private Dictionary<Vector2, Texture2D> _textures;
	private bool _initialized = false;

	private void OnGUI()
	{
		PaintPoints();
	}

	private void Initialize()
	{
		if (_initialized) return;
		
		_colorMap = new Dictionary<Vector2, Color>();
		_textures = new Dictionary<Vector2, Texture2D>();

		_initialized = true;
	}
	
	public static GUIPainter GetInstance()
	{
		return GameObject.FindWithTag("GUI").GetComponent<GUIPainter>();
	}

	private void PaintPoints()
	{
		foreach (var position in _colorMap.Keys)
		{
			var pointColor = _colorMap[position];

			if (!_textures.ContainsKey(position))
			{
				var newTexture = new Texture2D(1, 1);
				_textures.Add(position, newTexture);
			}
			var pointTexture = _textures[position];
			pointTexture.SetPixel(0, 0, pointColor);
			pointTexture.Apply();
			
			var area = new Rect(position.x / Columns * Screen.width, position.y / Rows * Screen.height, Screen.width / Columns, Screen.height / Rows);
			
			GUI.skin.box.normal.background = pointTexture;
			GUI.Box(area, GUIContent.none);
		}
	}

	public void PaintPointAt(Vector2 pointPosition, Color pointColor)
	{
		var newPointPosition = new Vector2((int)pointPosition.x, (int)pointPosition.y);
		
		if (newPointPosition.x < 0 || newPointPosition.x >= Columns) return;
		if (newPointPosition.y < 0 || newPointPosition.y >= Rows) return;

		if (_colorMap.ContainsKey(newPointPosition))
		{
			_colorMap.Remove(newPointPosition);
		}
		_colorMap.Add(newPointPosition, pointColor);
	}

	public void RemovePointAt(Vector2 pointPosition)
	{
		var newPointPosition = new Vector2((int)pointPosition.x, (int)pointPosition.y);
		
		if (_colorMap.ContainsKey(newPointPosition))
		{
			_colorMap.Remove(newPointPosition);
		}
	}
}
