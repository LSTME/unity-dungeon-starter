using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;

namespace Scripts.Map.Config
{
    public class MapBlock
    {
        public int[] Position { get; set; }
        public List<ObjectConfig> Objects { get; set; }
    }
}
