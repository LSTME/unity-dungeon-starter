using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;

namespace Scripts.Map.Config
{
    public class Config
    {
        public List<MapBlock> MapBlocks { get; set; }
        public GameLogic GameLogic { get; set; }
    }
}
