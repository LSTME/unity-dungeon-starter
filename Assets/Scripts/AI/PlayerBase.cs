using System;
using System.IO;
using System.Linq;
using System.Threading;
using Scripts.Map;
using UnityEditor;
using UnityEngine;

namespace Scripts.AI
{
    public abstract class PlayerBase {
	    
	    #region Map

	    protected void StartMap(string mapName)
	    {
		    SequentialAction(() => MapGenerator.getInstance().LoadMapFile(mapName));
	    }
	    
	    #endregion

        #region Navigation
        
        protected Vector2 PlayerLocation()
        {
            return SequentialSensor(() => PlayerController.getInstance().CurrentLocation);
        }

	    protected Vector2 PlayerFrontLocation()
	    {
		    return SequentialSensor(() => MapUtils.GetFrontLocation(PlayerController.getInstance().CurrentLocation,
			    PlayerController.getInstance().CurrentDirection));
	    }

	    protected Vector2 PlayerBackLocation()
	    {
		    return SequentialSensor(() => MapUtils.GetBackLocation(PlayerController.getInstance().CurrentLocation,
			    PlayerController.getInstance().CurrentDirection));
	    }

	    protected Vector2 PlayerLeftLocation()
	    {
		    return SequentialSensor(() => MapUtils.GetLeftLocation(PlayerController.getInstance().CurrentLocation, PlayerController.getInstance().CurrentDirection));
	    }

	    protected Vector2 PlayerRightLocation()
	    {
		    return SequentialSensor(() => MapUtils.GetRightLocation(PlayerController.getInstance().CurrentLocation, PlayerController.getInstance().CurrentDirection));
	    }
	    
        protected Direction PlayerDirection()
        {
            return SequentialSensor(() => PlayerController.getInstance().CurrentDirection);
        }
        
        #endregion
        
        #region Movement

        protected void MoveForward()
        {
			SequentialAction(() => PlayerController.getInstance().IssueAction("Vertical", 1.0f));
        }
        
        protected void MoveBackward()
        {
	        SequentialAction(() => PlayerController.getInstance().IssueAction("Vertical", -1.0f));
        }
        
        protected void TurnLeft()
        {
	        SequentialAction(() => PlayerController.getInstance().IssueAction("Horizontal", -1.0f));
        }
        
        protected void TurnRight()
        {
	        SequentialAction(() => PlayerController.getInstance().IssueAction("Horizontal", 1.0f));
        }
        
        protected void StrafeLeft()
        {
	        SequentialAction(() => PlayerController.getInstance().IssueAction("Strafe", -1.0f));
        }
        
        protected void StrafeRight()
        {
	        SequentialAction(() => PlayerController.getInstance().IssueAction("Strafe", 1.0f));
        }
        
        #endregion
        
        #region Sensing
        
        #region Relative
        
        protected SafeBlockWrapper FrontBlock()
        {
	        return SequentialSensor(() =>
	        {
		        var location = MapUtils.GetFrontLocation(PlayerController.getInstance().CurrentLocation, PlayerController.getInstance().CurrentDirection);
		        var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

		        return SafeBlockWrapper.GetData(mapBlock);
	        });
        }

		protected SafeBlockWrapper BackBlock()
		{
			return SequentialSensor(() =>
			{
				var location = MapUtils.GetBackLocation(PlayerController.getInstance().CurrentLocation, PlayerController.getInstance().CurrentDirection);
				var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

				return SafeBlockWrapper.GetData(mapBlock);
			});
		}
        
        protected SafeBlockWrapper LeftBlock()
        {
	        return SequentialSensor(() =>
	        {
		        var location = MapUtils.GetLeftLocation(PlayerController.getInstance().CurrentLocation, PlayerController.getInstance().CurrentDirection);
		        var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

		        return SafeBlockWrapper.GetData(mapBlock);
	        });
        }
        
        protected SafeBlockWrapper RightBlock()
        {
	        return SequentialSensor(() =>
	        {
		        var location = MapUtils.GetRightLocation(PlayerController.getInstance().CurrentLocation, PlayerController.getInstance().CurrentDirection);
		        var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

		        return SafeBlockWrapper.GetData(mapBlock);
	        });
        }

		protected SafeBlockWrapper CurrentBlock()
		{
			return SequentialSensor(() =>
			{
				var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(PlayerController.getInstance().CurrentLocation);

				return SafeBlockWrapper.GetData(mapBlock);
			});
		}
        
        #endregion
        
        #region Absolute
        
        protected SafeBlockWrapper NorthBlock()
        {
	        return SequentialSensor(() =>
	        {
		        var location = MapUtils.GetNorthLocation(PlayerController.getInstance().CurrentLocation);
		        var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

		        return SafeBlockWrapper.GetData(mapBlock);
	        });
        }
        
        protected SafeBlockWrapper SouthBlock()
        {
	        return SequentialSensor(() =>
	        {
		        var location = MapUtils.GetSouthLocation(PlayerController.getInstance().CurrentLocation);
		        var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

		        return SafeBlockWrapper.GetData(mapBlock);
	        });
        }
        
        protected SafeBlockWrapper WestBlock()
        {
	        return SequentialSensor(() =>
	        {
		        var location = MapUtils.GetWestLocation(PlayerController.getInstance().CurrentLocation);
		        var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

		        return SafeBlockWrapper.GetData(mapBlock);
	        });
        }
        
        protected SafeBlockWrapper EastBlock()
        {
	        return SequentialSensor(() =>
	        {
		        var location = MapUtils.GetEastLocation(PlayerController.getInstance().CurrentLocation);
		        var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

		        return SafeBlockWrapper.GetData(mapBlock);
	        });
        }
        
        protected SafeBlockWrapper BlockAt(Vector2 location)
        {
	        return SequentialSensor(() =>
	        {
		        var mapBlock = MapGenerator.getInstance().GetBlockAtLocation(location);

		        return SafeBlockWrapper.GetData(mapBlock);
	        });
        }
        
        #endregion
        
        #endregion
        
        #region Interaction

	    protected bool IsKeyDown(string keyName)
	    {
		    return SequentialSensor(() => Input.GetKey(keyName));
	    }
	    
	    protected bool WasKeyDown(string keyName)
	    {
		    return SequentialSensor(() => Input.GetKeyDown(keyName));
	    }

        protected void UseBlock()
        {
	        SequentialAction(() => PlayerController.getInstance().IssueAction("Action"));
	        
        }

		protected bool IsObjectCarried()
		{
			return SequentialSensor(() => PlayerController.getInstance().IsObjectPickedUp());
		}

		#endregion

		#region Coins

		protected int GetNumberOfCollectedCoins()
		{
			return SequentialSensor(() => GUITexts.GetInstance().GetCoinsCount());
		}

		#endregion

		#region Messages

	    protected void ShowMessage(string Message)
		{
			SequentialProcedure(() => GUITexts.GetInstance().NewTextMessage(Message));
		}

	    protected void ShowMessage(string Message, Color TColor)
		{
			SequentialProcedure(() => GUITexts.GetInstance().NewTextMessage(Message, TColor));
		}

		#endregion

	    #region Painting

	    protected void PaintColorAt(int X, int Y, UnityEngine.Color PointColor)
	    {
		    SequentialProcedure(() =>
		    {
			    Vector2 PointPosition = new Vector2(X, Y);
			    
			    GUIPainter.getInstance().PaintPointAt(PointPosition, PointColor);
		    });
	    }

	    protected void RemoveColorAt(int X, int Y)
	    {
		    SequentialProcedure(() =>
		    {
			    Vector2 PointPosition = new Vector2(X, Y);
			    
			    GUIPainter.getInstance().RemovePointAt(PointPosition);
		    });
	    }

	    #endregion
	    
        private T SequentialSensor<T>(Func<T> action)
        {
            var manualResetEvent = new ManualResetEvent(true);
            manualResetEvent.Reset();
            var result = default(T);
            PlayerController.ActionQueue.Enqueue(() =>
            {
                result = action();
                manualResetEvent.Set();
            });

            manualResetEvent.WaitOne();
            return result;
        }

        private void SequentialAction(Action action)
        {
            PlayerController.InterpreterLock.Reset();
            PlayerController.ActionQueue.Enqueue(action);
            PlayerController.InterpreterLock.WaitOne();
            Thread.Sleep(100);
        }
	    
        private void SequentialProcedure(Action action)
        {
            PlayerController.ActionQueue.Enqueue(action);
        }
	}
    
}


