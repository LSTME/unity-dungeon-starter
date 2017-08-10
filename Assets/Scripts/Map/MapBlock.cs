using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Map
{
    public class MapBlock
    {
        private char mapSymbol;
        private string[] inputAttributes;
        private List<GameObject> gameObjects;
        private Vector2 location;

        private Color minimapColor = Color.black;

        public string[] Attributes
        {
            get
            {
                return inputAttributes;
            }
        }

        public List<GameObject> GameObjects
        {
            get
            {
                return gameObjects;
            }
        }

        public Vector2 Location
        {
            get
            {
                return location;
            }
        }

        public string Type { get; set; }

        public bool Interactive { get; set; }

        public Color MinimapColor
        {
            get
            {
                return minimapColor;
            }

            set
            {
                minimapColor = value;
            }
        }

        public char MapSymbol
        {
            get
            {
                return mapSymbol;
            }
        }

        public bool IsWalkable
        {
            get
            {
                bool walkable = true;

                foreach (var gameObject in GameObjects)
                {
                    var component = gameObject.GetComponent<Interfaces.IWalkable>();
                    if (component != null) walkable &= component.IsWalkable();
                }

                return walkable;
            }
        }

        public MapBlock(char mapSymbol, int x, int y)
        {
            this.mapSymbol = mapSymbol;
            this.location = new Vector2(x, y);
            gameObjects = new List<GameObject>();
            inputAttributes = new string[0];
        }

        public void updateInputAttributes(string[] inputAttributes)
        {
            this.inputAttributes = inputAttributes;
        }

        public void addGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }

        public MapBlock getUnbuildedCopy()
        {
            var result = new MapBlock(mapSymbol, (int)location.x, (int)location.y);

            var inputAttributes = new string[Attributes.Length];
            Array.Copy(Attributes, 0, inputAttributes, 0, Attributes.Length);

            result.updateInputAttributes(inputAttributes);

            return result;
        }
    }
}
