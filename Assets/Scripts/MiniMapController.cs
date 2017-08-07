using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    public float Width = 240.0f;
    public float Height = 140.0f;

    public int Columns;
    public int Rows;

    public Dictionary<Vector2, Color> Cells = new Dictionary<Vector2, Color>();
    public Vector2 PlayerLocation;

    private float _cellSize;
    private Rect _frameRect;


    private Texture2D _backgroundQuad;
    private Texture2D _playerQuad;
    private Dictionary<Vector2, Texture2D> _quads;

    public static MiniMapController getInstance()
    {
        return GameObject.FindWithTag("GUI").GetComponent<MiniMapController>();
    }

    void OnGUI()
    {
        _frameRect = new Rect(Screen.width - Width, Screen.height - Height, Width, Height);
        _cellSize = Mathf.Min(Width / Columns, Height / Rows);

        if (_quads == null)
        {
            _quads = new Dictionary<Vector2, Texture2D>();
            for (var j = 0; j < Rows; j++)
            for (var i = 0; i < Columns; i++)
                _quads.Add(new Vector2(i, j), new Texture2D(1, 1));
        }

        if (_backgroundQuad == null)
        {
            _backgroundQuad = new Texture2D(1, 1);
            _backgroundQuad.SetPixel(0, 0, Color.black);
            _backgroundQuad.Apply();
        }
        DrawQuad(_frameRect, _backgroundQuad);

        if (_playerQuad == null)
        {
            _playerQuad = new Texture2D(1, 1);
            _playerQuad.SetPixel(0, 0, Color.red);
            _playerQuad.Apply();
        }

        foreach (var cell in Cells)
        {
            var quad = _quads[cell.Key];
            quad.SetPixel(0, 0, cell.Value);
            quad.Apply();

            DrawCell(cell.Key, quad);
        }

        DrawCell(PlayerLocation, _playerQuad);
    }

    void DrawQuad(Rect position, Texture2D quad)
    {
        GUI.skin.box.normal.background = quad;
        GUI.Box(position, GUIContent.none);
    }

    void DrawCell(Vector2 position, Texture2D quad)
    {
        var rect = new Rect(_frameRect.xMin + position.x * _cellSize, _frameRect.yMin + position.y * _cellSize,
            _cellSize, _cellSize);
        DrawQuad(rect, quad);
    }
}