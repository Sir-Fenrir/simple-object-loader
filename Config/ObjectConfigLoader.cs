using ExtremelySimpleLogger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TinyLife.Mods;

namespace SimpleObjectLoader.Config
{
    /// <summary>
    /// Class for loading all simple mods in the Tiny Life mod directory.
    /// </summary>
    internal class ObjectConfigLoader
    {

        /// <summary>
        /// We're requiring the 'extension' .simple.json, 
        /// to recognize JSON files meant for this modloader.
        /// </summary>
        private static readonly string CONFIG_EXTENSION = "*.simple.json";

        public static Logger Logger = SimpleObjectLoader.Logger;

        /// <summary>
        /// Search for all simple mods.
        /// </summary>
        /// <returns>A list of simple mod configurations</returns>
        public List<ObjectConfig> GetMods()
        {
            var objectConfigs = Directory
                .GetFiles(ModLoader.GetModsFolder().FullName, CONFIG_EXTENSION, SearchOption.AllDirectories)
                .SelectMany(file => MapFileToObject(file))
                .ToList();

            Logger.Info($"{objectConfigs.Count} mods discovered");

            return objectConfigs;
        }

        /// <summary>
        /// Parse a file into a list of <see cref="ObjectConfig"/>s,
        /// regardless whether there is only one item or more in the file.
        /// </summary>
        /// <param name="file">The file to read</param>
        /// <returns>All succesfully loaded configurations</returns>
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
