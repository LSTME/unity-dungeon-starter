using System.Collections.Generic;

namespace Scripts.Map.Config
{
    public class MapBlock
    {
        public int[] Position { get; set; }
        public List<ObjectConfig> Objects { get; set; }
    }
}
