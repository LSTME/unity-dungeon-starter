using UnityEngine;
using Scripts.Map.Config;

namespace Scripts.Map
{
    abstract public class AbstractGameObjectController : MonoBehaviour
    {
        public ObjectConfig ObjectConfig { get; set; }

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
    }
}
