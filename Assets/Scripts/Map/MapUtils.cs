using System;
using UnityEngine;

namespace Scripts.Map
{
    public class MapUtils
    {
        public static Vector2 GetNorthLocation(Vector2 location)
        {
            var diff = new Vector2(0, -1);
            return diff + location;
        }
        
        public static Vector2 GetSouthLocation(Vector2 location)
        {
            var diff = new Vector2(0, 1);
            return diff + location;
        }
        
        public static Vector2 GetWestLocation(Vector2 location)
        {
            var diff = new Vector2(-1, 0);
            return diff + location;
        }
        
        public static Vector2 GetEastLocation(Vector2 location)
        {
            var diff = new Vector2(1, 0);
            return diff + location;
        }
        
        public static Vector2 GetFrontLocation(Vector2 location, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return GetNorthLocation(location);
                case Direction.East:
                    return GetEastLocation(location);
                case Direction.South:
                    return GetSouthLocation(location);
                case Direction.West:
                    return GetWestLocation(location);
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
        }
        
        public static Vector2 GetBackLocation(Vector2 location, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return GetSouthLocation(location);
                case Direction.East:
                    return GetWestLocation(location);
                case Direction.South:
                    return GetNorthLocation(location);
                case Direction.West:
                    return GetEastLocation(location);
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
        }

        public static Vector2 GetLeftLocation(Vector2 location, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return GetWestLocation(location);
                case Direction.East:
                    return GetNorthLocation(location);
                case Direction.South:
                    return GetEastLocation(location);
                case Direction.West:
                    return GetSouthLocation(location);
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
        }
        
        public static Vector2 GetRightLocation(Vector2 location, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return GetEastLocation(location);
                case Direction.East:
                    return GetSouthLocation(location);
                case Direction.South:
                    return GetWestLocation(location);
                case Direction.West:
                    return GetNorthLocation(location);
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
        }
        
    }
}