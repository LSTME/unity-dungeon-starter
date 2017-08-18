using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Map
{
    public class MapBlock
    {
        private char mapSymbol;
        private string[] inputAttributes;
        private List<GameObject> gameObjects;
        private Vector2 location;
        private bool initialized = false;

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

        public List<Config.ObjectConfig> ObjectsConfig { get; set; }

        public string Type {
			get
			{
				string BestType = "";
				int BestTypePriority = int.MinValue;

				foreach (var gameObject in GameObjects)
				{
					var component = gameObject.GetComponent<AbstractGameObjectController>();
					if (component != null)
					{
						if (component.TypePriority > BestTypePriority)
						{
							BestTypePriority = component.TypePriority;
							BestType = component.Type;
						}
					}
				}

				return BestType;
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
					if (!walkable) break;
                }

                return walkable;
            }
        }

		public bool IsInteractive
		{
			get
			{
				foreach (var gameObject in gameObjects)
				{
					var component = gameObject.GetComponent<Interfaces.IInteractive>();
					if (component != null) return true;
				}

				return false;
			}
		}

		public bool IsReachable
		{
			get
			{
				foreach (var gameObject in gameObjects)
				{
					var component = gameObject.GetComponent<Interfaces.IInteractive>();
					if (component != null && component.IsReachable()) return true;
				}

				return false;
			}
		}

		public bool IsPickable
		{
			get
			{
				foreach (var gameObject in gameObjects)
				{
					var component = gameObject.GetComponent<Interfaces.IPickable>();
					if (component != null) return true;
				}

				return false;
			}
		}

		public bool IsDropable
		{
			get
			{
				bool dropableFound = false;
				bool undropableFound = false;
				foreach (var gameObject in gameObjects)
				{
					var componentDropable = gameObject.GetComponent<Interfaces.IDropable>();
					if (componentDropable != null) dropableFound = true;

					var componentUndropable = gameObject.GetComponent<Interfaces.IUnplacableCorridor>();
					if (componentUndropable != null && componentUndropable.IsUnplacable()) undropableFound = true;
				}

				return dropableFound && !undropableFound;
			}
		}

		public bool IsPressable
		{
			get
			{
				foreach (var gameObject in gameObjects)
				{
					var component = gameObject.GetComponent<Interfaces.IPressable>();
					if (component != null) return true;
				}

				return false;
			}
		}

		public bool IsOpenable
		{
			get
			{
				foreach (var gameObject in gameObjects)
				{
					var component = gameObject.GetComponent<Interfaces.IOpenable>();
					if (component != null) return true;
				}

				return false;
			}
		}

		public List<Direction> InteractiveObjectsDirection
		{
			get
			{
				var result = new List<Direction>();

				foreach (var gameObject in gameObjects)
				{
					var component = gameObject.GetComponent<AbstractGameObjectController>();
					if (component != null && component is Interfaces.IInteractive)
					{
						result.Add(component.GetBlockOrientation());
					} 
				}

				return result;
			}
		}

		public bool IsLooted
		{
			get
			{
				foreach (var gameObject in gameObjects)
				{
					var component = gameObject.GetComponent<Interfaces.IVault>();
					if (component != null && component.IsLooted()) return true;
				}

				return false;
			}
		}

	    public bool IsOpen
	    {
		    get
		    {
			    foreach (var gameObject in gameObjects)
			    {
				    var component = gameObject.GetComponent<Interfaces.IOpenable>();
				    if (component != null && component.GetOpenState()) return true;
			    }
			    return false;
		    }
	    }

	    public bool IsSwitched
	    {
		    get
		    {
			    foreach (var gameObject in gameObjects)
			    {
				    var component = gameObject.GetComponent<Interfaces.ISwitchable>();
				    if (component != null && component.GetSwitchState()) return true;
			    }
			    return false;
		    }
	    }

        protected Dictionary<string, Config.ObjectConfig> NamedObjectsConfig;

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
            result.ObjectsConfig = ObjectsConfig;

            return result;
        }

        public void Initialize()
        {
            if (initialized) return;

            NamedObjectsConfig = new Dictionary<string, Config.ObjectConfig>();

            if (ObjectsConfig != null)
            {
                foreach (var objectConfig in ObjectsConfig)
                {
                    NamedObjectsConfig.Add(objectConfig.Type, objectConfig);
                }
            }

            initialized = true;
        }

        public Config.ObjectConfig getObjectConfigForType(string type)
        {
            if (!NamedObjectsConfig.ContainsKey(type)) return null;

            return NamedObjectsConfig[type];
        }

        public void DetachGameObject(GameObject Block)
        {
            Initialize();

            List<GameObject> newGameObjects = new List<GameObject>();

            foreach (var gameObject in gameObjects)
            {
                if (gameObject == Block) continue;
                newGameObjects.Add(gameObject);
            }

            gameObjects = newGameObjects;
        }
    }
}
