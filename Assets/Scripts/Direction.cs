using UnityEngine;

namespace Scripts
{
    public enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    static class DirectionMethods
    {
        public static Quaternion GetRotation(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North: return Quaternion.LookRotation(new Vector3(0, 0, 1));
                case Direction.East:  return Quaternion.LookRotation(new Vector3(1, 0, 0));
                case Direction.South: return Quaternion.LookRotation(new Vector3(0, 0, -1));
                default: /* West */   return Quaternion.LookRotation(new Vector3(-1, 0, 0));
            }
        }
    }
}
