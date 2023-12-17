using ExtremelySimpleLogger;
using HarmonyLib;
using MLEM.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TinyLife.Objects;
using TinyLife.Utilities;
using static MLEM.Graphics.StaticSpriteBatch;

namespace SimpleObjectLoader.Utils
{
    internal class EnvironmentUtils
    {

        static public readonly Dictionary<string, Type> Types;

        static private readonly Dictionary<string, ColorScheme> ColorSchemes;

        static public readonly Dictionary<string, Direction2[]> Directions;

        public static Logger Logger = SimpleObjectLoader.Logger;

        static EnvironmentUtils()
        {

            // To force the loading of the DynamicEnums
            var forceLoadCategories = ObjectCategory.Table;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var forceLoadColorSchemes = ColorScheme.Bricks;
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            Types = GetFurnitureTypes();
            ColorSchemes = CreateColorSchemeMap();
            Directions = CreateDirectionsMap();

        }

        public static ColorScheme[] ParseColorSchemes(string[] schemes)
        {
            SimpleObjectLoader.Logger.Info(ColorSchemes.Count);
            ColorScheme[] found = [];
            foreach (var scheme in schemes)
            {
                ColorSchemes.TryGetValue(scheme, out var result);
                if(result != null)
                {
                    found = found.AddToArray(result);
                } else
                {
                    Logger.Warn($"Unknown color scheme requested: {scheme}");
                }
            }
            return found;
        }

        public static ObjectCategory ParseCategories(string[] categories)
        {
            var stringCategories = categories.Aggregate((x, y) => $"{x}|{y}");
            return ObjectCategory.Parse<ObjectCategory>(stringCategories);
        }

        private static Dictionary<string, Type> GetFurnitureTypes()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(TryExtractTypesFromAssembly)
                .ToDictionary(
                    type => type.Name,
                    type => type
                );
        }

        
        private static List<Type> TryExtractTypesFromAssembly(Assembly assembly)
        {
            List<Type> types = [];

            try
            {
                foreach (var type in assembly
                    .GetExportedTypes()
                    .Where(type => !type.IsAbstract)
                    .Where(type => type.IsSubclassOf(typeof(Furniture)))
                    )
                {
                    types.Add(type);
                }
            }
            catch
            {
                // Do nothing
            }

            return types;

        }

        private static Dictionary<string, ColorScheme> CreateColorSchemeMap()
        {
            return typeof(ColorScheme).GetFields()
            .ToDictionary(
                scheme => scheme.Name,
                scheme => (ColorScheme)scheme.GetValue(null)
            );
        }

        // Sadly some duplication, as I can't pass static types as type parameters
        private static Dictionary<string, Direction2[]> CreateDirectionsMap()
        {
            return typeof(Direction2Helper).GetFields()
            .ToDictionary(
                scheme => scheme.Name,
                scheme => (Direction2[])scheme.GetValue(null)
            );
        }

    }
}
