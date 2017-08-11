using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Map;
using UnityEngine;

namespace Scripts.AI
{
    public abstract class PlayerBase : MonoBehaviour {

        #region Navigation
        
        protected Vector2 PlayerLocation()
        {
            return PlayerController.getInstance().CurrentLocation;
        }
        
        protected Direction PlayerDirection()
        {
            return PlayerController.getInstance().CurrentDirection;
        }
        
        #endregion
        
        #region Movement

        protected void MoveForward()
        {
            PlayerController.getInstance().MoveForward();
        }
        
        protected void MoveBackward()
        {
            PlayerController.getInstance().MoveBackward();
        }
        
        protected void TurnLeft()
        {
            PlayerController.getInstance().RotateLeft();
        }
        
        protected void TurnRight()
        {
            PlayerController.getInstance().RotateRight();
        }
        
        protected void StrafeLeft()
        {
            PlayerController.getInstance().StrafeLeft();
        }
        
        protected void StrafeRight()
        {
            PlayerController.getInstance().StrafeRight();
        }

        protected void LookAt(Direction direction)
        {
            PlayerController.getInstance().RotateTo(direction);
        }
        
        #endregion
        
        #region Sensing
        
        #region Relative
        
        protected string FrontBlock()
        {
            var location = MapUtils.GetFrontLocation(PlayerLocation(), PlayerDirection());
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);
            return mapBlock.Type;
        }
        
        protected string BackBlock()
        {
            var location = MapUtils.GetBackLocation(PlayerLocation(), PlayerDirection());
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);
            return mapBlock.Type;
        }
        
        protected string LeftBlock()
        {
            var location = MapUtils.GetLeftLocation(PlayerLocation(), PlayerDirection());
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);
            return mapBlock.Type;
        }
        
        protected string RightBlock()
        {
            var location = MapUtils.GetRightLocation(PlayerLocation(), PlayerDirection());
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);
            return mapBlock.Type;
        }
        
        #endregion
        
        #region Absolute
        
        protected string NorthBlock()
        {
            var location = MapUtils.GetNorthLocation(PlayerLocation());
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);
            return mapBlock.Type;
        }
        
        protected string SouthBlock()
        {
            var location = MapUtils.GetSouthLocation(PlayerLocation());
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);
            return mapBlock.Type;
        }
        
        protected string WestBlock()
        {
            var location = MapUtils.GetWestLocation(PlayerLocation());
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);
            return mapBlock.Type;
        }
        
        protected string EastBlock()
        {
            var location = MapUtils.GetEastLocation(PlayerLocation());
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);
            return mapBlock.Type;
        }
        
        protected string BlockAt(Vector2 location)
        {
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);
            return mapBlock.Type;
        }
        
        #endregion
        
        #endregion
        
        #region Interaction

        protected void UseBlock()
        {
            PlayerController.getInstance().PerformAction();
        }
        
        #endregion
    }
    
}


