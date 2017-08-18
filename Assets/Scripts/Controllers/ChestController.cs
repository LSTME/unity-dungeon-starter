using Scripts.Map;

namespace Scripts.Controllers
{
	public class ChestController : AbstractGameObjectController, Interfaces.IWalkable, Interfaces.IUnplacableCorridor, Interfaces.IInteractive, Interfaces.IVault {

		private bool _isLooted = false;

		public UnityEngine.GameObject ChestClosed;
		public UnityEngine.GameObject ChestOpened;

		public bool Activate() {
			PlayerController.InterpreterLock.Set();

			if (_isLooted)
				return false;
			if (!IsReachable ())
				return false;

			GUITexts guiTexts = GUITexts.GetInstance();

			guiTexts.CollectCoin(GetValue());

			ChestClosed.SetActive (false);
			ChestOpened.SetActive (true);

			_isLooted = true;

			return true;
		}

		public bool IsReachable()
		{
			return IsReachableToActivate (true);
		}

		public bool IsWalkable()
		{
			return false;
		}

		public bool IsUnplacable()
		{
			return true;
		}

		public bool IsLooted()
		{
			return _isLooted;
		}

		private int GetValue()
		{
			if (ObjectConfig == null)
				return 1;
			if (ObjectConfig.Chest == null)
				return 1;

			return ObjectConfig.Chest.Value;
		}
	}	
}


