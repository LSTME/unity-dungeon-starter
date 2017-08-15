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

        public HashSet<Vector2> Cells = new HashSet<Vector2>();
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
            _cellSize = Mathf.Min(_frameRect.xMax / (Columns - 1), _frameRect.yMax / (Rows - 1));

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
            DrawQuad(new Rect(0, _frameRect.xMax, _frameRect.xMax + 6, 4), _border);
            DrawQuad(new Rect(_frameRect.xMax, 0, 4, _frameRect.xMax), _border);

            if (_playerQuad == null)
            {
                _playerQuad = new Texture2D(1, 1);
                _playerQuad.SetPixel(0, 0, Color.red);
                _playerQuad.Apply();
            }

            foreach (var cell in Cells)
            {
                if (!visited.Contains(cell)) continue;
                var quad = _quads[cell];
                quad.SetPixel(0, 0, GetCellColor(cell));
                quad.Apply();

                DrawCell(cell, quad);
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
                if (!Cells.Contains(locationToAdd)) continue;
                if (visited.Contains(locationToAdd)) continue;
                visited.Add(locationToAdd);
            }
        }

        private Color GetCellColor(Vector2 location)
        {
            var mapGenerator = MapGenerator.getInstance();

            Color result = new Color(0, 0, 0, 0);
            int highestPriority = int.MinValue;

            var mapBlock = mapGenerator.GetBlockAtLocation(location);
            if (mapBlock == null) return result;
            
            foreach (var gameObject in mapBlock.GameObjects)
            {
                var component = gameObject.GetComponent<Map.AbstractGameObjectController>();
                if (component != null && component.MinimapColorPriority > highestPriority)
                {
                    result = component.MinimapColor;
                    result.a = 1.0f;
                    highestPriority = component.MinimapColorPriority;
                }
            }

            return result;
        }
    }
}

