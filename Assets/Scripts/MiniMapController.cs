using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class MiniMapController : MonoBehaviour
    {
        public float SmallSize = 20.0f;
        public float LargeSize = 80.0f;

        public int Columns;
        public int Rows;

        public Dictionary<Vector2, Color> Cells = new Dictionary<Vector2, Color>();
        public Vector2 PlayerLocation;

        private float _cellSize;
        private Rect _frameRect;

        private bool UseLarge = false;

        private Texture2D _backgroundQuad;
        private Texture2D _border;
        private Texture2D _playerQuad;
        private Dictionary<Vector2, Texture2D> _quads;
        private HashSet<Vector2> visited = new HashSet<Vector2>();

        public static MiniMapController getInstance()
        {
            return GameObject.FindWithTag("GUI").GetComponent<MiniMapController>();
        }

        public void SwitchLarge()
        {
            UseLarge = !UseLarge;
        }

        void OnGUI()
        {
            float Size = SmallSize;
            if (UseLarge) Size = LargeSize;

            _frameRect = new Rect(0, 0, Screen.height * Size / 100.0f, Screen.height * Size / 100.0f);
            _cellSize = Mathf.Min(_frameRect.xMax / Columns, _frameRect.yMax / Rows);

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
                _backgroundQuad.SetPixel(0, 0, new Color(0, 0, 0, 0.4f));
                _backgroundQuad.Apply();
            }
            DrawQuad(_frameRect, _backgroundQuad);

            if (_border == null)
            {
                _border = new Texture2D(1, 1);
                _border.SetPixel(0, 0, new Color(0.8f, 0.46f, 0.0f));
                _border.Apply();
            }
            DrawQuad(new Rect(0, _frameRect.xMax, _frameRect.xMax + _cellSize, _cellSize), _border);
            DrawQuad(new Rect(_frameRect.xMax, 0, _cellSize, _frameRect.xMax), _border);

            if (_playerQuad == null)
            {
                _playerQuad = new Texture2D(1, 1);
                _playerQuad.SetPixel(0, 0, Color.red);
                _playerQuad.Apply();
            }

            foreach (var cell in Cells)
            {
                if (!visited.Contains(cell.Key)) continue;
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

        public void visit(Vector2 location)
        {
            Vector2[] directions = { new Vector2(0,0), new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(-1, -1) };

            foreach (var direction in directions)
            {
                var locationToAdd = location + direction;
                if (!Cells.ContainsKey(locationToAdd)) continue;
                if (visited.Contains(locationToAdd)) continue;
                visited.Add(locationToAdd);
            }
        }
    }
}

