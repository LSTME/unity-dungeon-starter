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
			PlayerController.getInstance().IssueAction("Vertical", 1.0f);
        }
        
        protected void MoveBackward()
        {
			PlayerController.getInstance().IssueAction("Vertical", -1.0f);
        }
        
        protected void TurnLeft()
        {
			PlayerController.getInstance().IssueAction("Horizontal", -1.0f);
        }
        
        protected void TurnRight()
        {
			PlayerController.getInstance().IssueAction("Horizontal", 1.0f);
        }
        
        protected void StrafeLeft()
        {
			PlayerController.getInstance().IssueAction("Strafe", -1.0f);
        }
        
        protected void StrafeRight()
        {
			PlayerController.getInstance().IssueAction("Strafe", 1.0f);
        }
        
        #endregion
        
        #region Sensing
        
        #region Relative
        
        protected SafeBlockWrapper FrontBlock()
		{
			var location = MapUtils.GetFrontLocation(PlayerLocation(), PlayerDirection());
			var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

			return WrapMapBlockData(mapBlock);
		}

		protected SafeBlockWrapper BackBlock()
        {
            var location = MapUtils.GetBackLocation(PlayerLocation(), PlayerDirection());
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

			return WrapMapBlockData(mapBlock);
        }
        
        protected SafeBlockWrapper LeftBlock()
        {
            var location = MapUtils.GetLeftLocation(PlayerLocation(), PlayerDirection());
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

			return WrapMapBlockData(mapBlock);
		}
        
        protected SafeBlockWrapper RightBlock()
        {
            var location = MapUtils.GetRightLocation(PlayerLocation(), PlayerDirection());
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

			return WrapMapBlockData(mapBlock);
		}

		protected SafeBlockWrapper CurrentBlock()
		{
			return BlockAt(PlayerLocation());
		}
        
        #endregion
        
        #region Absolute
        
        protected SafeBlockWrapper NorthBlock()
        {
            var location = MapUtils.GetNorthLocation(PlayerLocation());
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

			return WrapMapBlockData(mapBlock);
		}
        
        protected SafeBlockWrapper SouthBlock()
        {
            var location = MapUtils.GetSouthLocation(PlayerLocation());
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

			return WrapMapBlockData(mapBlock);
		}
        
        protected SafeBlockWrapper WestBlock()
        {
            var location = MapUtils.GetWestLocation(PlayerLocation());
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

			return WrapMapBlockData(mapBlock);
		}
        
        protected SafeBlockWrapper EastBlock()
        {
            var location = MapUtils.GetEastLocation(PlayerLocation());
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

			return WrapMapBlockData(mapBlock);
		}
        
        protected SafeBlockWrapper BlockAt(Vector2 location)
        {
            var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

			return WrapMapBlockData(mapBlock);
		}
        
        #endregion
        
        #endregion
        
        #region Interaction

        protected void UseBlock()
        {
			PlayerController.getInstance().IssueAction("Action");
        }

		protected bool IsObjectCarried()
		{
			return PlayerController.getInstance().IsObjectPickedUp();
		}

		#endregion

		#region Auxilliary

		private static SafeBlockWrapper WrapMapBlockData(MapBlock mapBlock)
		{
			var result = new SafeBlockWrapper();
			result.IsWalkable = mapBlock.IsWalkable;
			result.IsInteractive = mapBlock.IsInteractive;
			result.IsReachable = mapBlock.IsReachable;
			result.IsDropable = mapBlock.IsDropable;
			result.IsPickable = mapBlock.IsPickable;
			result.IsPressable = mapBlock.IsPressable;
			result.IsOpenable = mapBlock.IsOpenable;
			result.InteractiveObjectsDirections = mapBlock.InteractiveObjectsDirection;

			return result;
		}

		#endregion
	}
    
}


