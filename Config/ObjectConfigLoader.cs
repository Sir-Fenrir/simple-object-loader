using ExtremelySimpleLogger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TinyLife.Mods;

namespace SimpleObjectLoader.Config
{
    internal class ObjectConfigLoader
    {

        private static readonly string CONFIG_EXTENSION = "*.simple.json";

        public static Logger Logger = SimpleObjectLoader.Logger;

        public ObjectConfigLoader() 
        {
        }

        public List<ObjectConfig> GetMods()
        {
            var objectConfigs = Directory
                .GetFiles(ModLoader.GetModsFolder().FullName, CONFIG_EXTENSION, SearchOption.AllDirectories)
                .SelectMany(file => MapFileToObject(file))
                .ToList();

            Logger.Info($"{objectConfigs.Count} mods discovered");

            return objectConfigs;
        }

        private ObjectConfig[] MapFileToObject(string file)
        {
            ObjectConfig[] objectConfigs = [];

            try
            {
                var json = File.ReadAllText(file);

                if (json.StartsWith("[")) // This means there are multiple configs in this file
                {
                    objectConfigs = JsonConvert.DeserializeObject<ObjectConfig[]>(json);
                }
                else
                {
                    objectConfigs = [JsonConvert.DeserializeObject<ObjectConfig>(json)];
                }

                foreach (var config in objectConfigs)
                {
                    config.FilePath = Path.GetDirectoryName(file);
                }
            } 
            catch (Exception e)
            {
                Logger.Info($"Failed to load the {file}");
                Logger.Info($"With the following error: {e.Message}");
            }

            return objectConfigs;
        }

    }
}
