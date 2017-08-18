using Scripts.Map;

namespace Scripts.AI
{
	public class SafeBlockWrapper
	{
		// If player can pass the block.
		public bool IsWalkable { get; set; }
		// If object is interactive, does not test if it can be activated.
		public bool IsInteractive { get; set; }
		// If Interactive object can be activated. In case of doors is true only if they are manual.
		public bool IsReachable { get; set; }
		// If the object is pressable (like floor button), no action can be performed just walk over or drop the box.
		public bool IsPressable { get; set; }
		// If there is space to drop the box.
		public bool IsDropable { get; set; }
		// If the interactive object at the position is pickable box (or anything else).
		public bool IsPickable { get; set; }
		// If the interactive object is door object. Does not test if it can be open manually.
		public bool IsOpenable { get; set; }
		// The list of dorections of interactive objects.
		public System.Collections.Generic.List<Direction> InteractiveObjectsDirections { get; set; }
		// The most priorized object type (there are always more than one object on the same location)
		public string Type { get; set; }
		// If the openable object is open.
		public bool IsOpen { get; set; }
		// If the switchable object is switched on.
		public bool IsSwitched { get; set; }
		// If the interactive block is looted
		public bool IsLooted { get; set; }

		public static SafeBlockWrapper GetData(MapBlock mapBlock)
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
			result.Type = mapBlock.Type;
			result.IsOpen = mapBlock.IsOpen;
			result.IsSwitched = mapBlock.IsSwitched;
			result.IsLooted = mapBlock.IsLooted;

			return result;
		}
	}
}
