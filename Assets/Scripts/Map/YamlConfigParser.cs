﻿using System.Collections.Generic;
using System.IO;
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
            get { return Configuration.GameLogic; }
        }

        public static Config.Player Player
        {
            get { return Configuration.Player; }
        }

        public static void Parse(string config, Dictionary<Vector2, MapBlock> mapBlocks)
        {
            var stream = new StringReader(config);

            var DeserializeBuilderObject = new DeserializerBuilder();
            DeserializeBuilderObject.WithNamingConvention(new CamelCaseNamingConvention());
            var Deserializer = DeserializeBuilderObject.Build();

            Configuration = Deserializer.Deserialize<Config.Config>(stream);

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
