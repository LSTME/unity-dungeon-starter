using UnityEngine;
using Scripts.Map.Config;

namespace Scripts.Map
{
    abstract public class AbstractGameObjectController : MonoBehaviour
    {
        public ObjectConfig ObjectConfig { get; set; }

        public int MinimapColorPriority = 0;

        public UnityEngine.Color MinimapColor;

        private static UnityEngine.Rendering.ShadowCastingMode _pickedObjectShadowMode;

        public void PerformActions(int actionType)
        {
            if (ObjectConfig == null) return;
            if (ObjectConfig.Actions == null) return;

            var actions = ObjectConfig.Actions.getActions(actionType);

            if (actions == null) return;

            foreach (var action in actions)
            {
                action.PerformAction();
            }
        }

        protected Direction GetBlockOrientation()
        {
            float Angle = 0.0f;
            Vector3 Axis = Vector3.zero;
            gameObject.transform.rotation.ToAngleAxis(out Angle, out Axis);

            int IntAngle = (int)Mathf.Round(Angle);
            int IntX = (int)Mathf.Round(Axis.x);
            int IntY = (int)Mathf.Round(Axis.y);

            if (IntAngle == 90)
            {
                if (IntX == 0 && IntY == -1) return Direction.West;
                if (IntX == 0 && IntY == 1) return Direction.East;
            }
            if (IntAngle == 0 && IntX == 1 && IntY == 0) return Direction.North;
            if (IntAngle == 180 && IntX == 0 && IntY == 1) return Direction.South;

            return Direction.Unknown;
        }

        protected Vector2 GetBlockPosition()
        {
            return new Vector2(gameObject.transform.position.x, -gameObject.transform.position.z);
        }

        protected bool IsReachableToActivate(bool FromDistance = false)
        {
            var PlayerPosition = PlayerController.getInstance().CurrentLocation;
            var PlayerDirection = PlayerController.getInstance().CurrentDirection;

            var BlockPosition = GetBlockPosition();
            var BlockOrientation = GetBlockOrientation();

            if (!FromDistance)
            {
                if (PlayerDirection != BlockOrientation) { return false; }
                if (Vector2.Distance(PlayerPosition, BlockPosition) > 0.1) { return false; }
                return true;
            }
            else
            {
                var PlayerPositionAfterMove = PlayerController.getInstance().LocationForDirection(PlayerDirection, 1);
                if (Vector2.Distance(PlayerPositionAfterMove, BlockPosition) > 0.1) { return false; }
                return true;
            }
        }

        public bool PickUpObject()
        {
            if (!IsReachableToActivate(true)) return false;

            var playerController = PlayerController.getInstance();

            if (playerController.IsObjectPickedUp()) return false;

            playerController.PickUpObject(gameObject);

            var mapGenerator = MapGenerator.getInstance();

            var mapBlock = mapGenerator.GetBlockAtLocation(GetBlockPosition());

            var Player = GameObject.FindGameObjectWithTag("Player");
            gameObject.transform.parent = Player.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.Translate(GetObjectTranslate());
            var meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
            {
                _pickedObjectShadowMode = meshRenderer.shadowCastingMode;
                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }

            mapBlock.DetachGameObject(gameObject);

            foreach (var gameObject in mapBlock.GameObjects)
            {
                var component = gameObject.GetComponent<Interfaces.IDropable>();
                if (component != null) component.SignalRemove();
            }

            return true;
        }

        public virtual bool DropObject()
        {
            if (!IsReachableToActivate(true)) return false;

            var playerController = PlayerController.getInstance();
            if (!playerController.IsObjectPickedUp()) return false;

            if (!IsDropPointFree()) return false;

            var mapGenerator = MapGenerator.getInstance();
            var mapBlock = mapGenerator.GetBlockAtLocation(GetBlockPosition());

            var gameObjectToDrop = playerController.PutDownObject();

            var position = mapGenerator.PositionForLocation(GetBlockPosition());

            gameObjectToDrop.transform.parent = mapGenerator.MapObject.transform;
            gameObjectToDrop.transform.position = position;
            
            var meshRenderer = gameObjectToDrop.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.shadowCastingMode = _pickedObjectShadowMode;
            }

            mapBlock.addGameObject(gameObjectToDrop);

            return true;
        }

        private bool IsDropPointFree()
        {
            var mapGenerator = MapGenerator.getInstance();
            var mapBlock = mapGenerator.GetBlockAtLocation(GetBlockPosition());

            if (mapBlock == null) return false;
            if (mapBlock.GameObjects == null) return false;

            foreach (var gameObject in mapBlock.GameObjects)
            {
                var component = gameObject.GetComponent<Interfaces.IUnplacableCorridor>();
                if (component != null) return false;
            }

            return true;
        }

        private Vector3 GetObjectTranslate()
        {
            var PlayerPosition = PlayerController.getInstance().CurrentDirection;

            switch (PlayerPosition)
            {
                case Direction.North:
                    return new Vector3(0, -0.1f, -1);
                case Direction.South:
                    return new Vector3(0, -0.1f, 1);
                case Direction.West:
                    return new Vector3(1, -0.1f, 0);
                case Direction.East:
                    return new Vector3(-1, -0.1f, 0);
                default:
                    return Vector3.zero;
            }
        }
    }
}
