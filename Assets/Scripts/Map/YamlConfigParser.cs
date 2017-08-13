using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using UnityEngine;

namespace Scripts.Map
{
    class YamlConfigParser
    {
        private static Config.Config Configuration;
        public static Config.GameLogic GameLogic
        {
            get
            {
                return Configuration.GameLogic;
            }
        }

        public static void Parse(string config, Dictionary<Vector2, MapBlock> mapBlocks)
        {
            var stream = new StringReader(config);

            var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

            Configuration = deserializer.Deserialize<Config.Config>(stream);

            assignConfiguration(Configuration, mapBlocks);
        }

        private static void assignConfiguration(Config.Config Configuration, Dictionary<Vector2, MapBlock> mapBlocks)
        {
            assignMapBlocks(Configuration.MapBlocks, mapBlocks);
        }

        private static void assignMapBlocks(List<Config.MapBlock> mapBlocksConfig, Dictionary<Vector2, MapBlock> mapBlocks)
        {
            if (mapBlocksConfig == null) { return; }

            foreach (var mapBlockConfig in mapBlocksConfig)
            {
                if (mapBlockConfig.Position.Length != 2) continue;

                int x = mapBlockConfig.Position[0] - 1;
                int y = mapBlockConfig.Position[1] - 1;

                var location = new Vector2(x, y);

                if (!mapBlocks.ContainsKey(location)) continue;

                mapBlocks[location].ObjectsConfig = mapBlockConfig.Objects;
            }
        }
    }
}
