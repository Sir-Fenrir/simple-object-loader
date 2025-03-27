using ExtremelySimpleLogger;
using HarmonyLib;
using MLEM.Maths;
using MLEM.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TinyLife.Objects;
using TinyLife.Utilities;

namespace SimpleObjectLoader.Utils
{
    /// <summary>
    /// Helper class to index things like existing categories and furniture types,
    /// to make creating new objects easier.
    /// </summary>
    internal class ObjectUtils
    {

        /// <summary>
        /// A dictionary of all possible furniture types.
        /// </summary>
        static public readonly Dictionary<string, Type> Types;

        static private readonly Dictionary<string, ColorScheme> ColorSchemes;

        /// <summary>
        /// A dictionary of all possible predefined directions.
        /// </summary>
        static public readonly Dictionary<string, Direction2[]> Directions;

        private static Logger Logger = SimpleObjectLoader.Logger;

        static ObjectUtils()
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

        /// <summary>
        /// As the ColorSchemes given in <see cref="ObjectConfig"/> are all strings, 
        /// we need to parse them into usable versions.
        /// 
        /// Invalid schemes are logged.
        /// </summary>
        /// <param name="schemes">The schemes to parse</param>
        /// <returns>All succesfully parsed schemes.</returns>
        public static ColorScheme[] ParseColorSchemes(string[] schemes)
        {
            ColorScheme[] found = [];
            foreach (var scheme in schemes)
            {
                ColorSchemes.TryGetValue(scheme.ToLower(), out var result);
                if (result != null)
                {
                    found = found.AddToArray(result);
                }
                else
                {
                    Logger.Warn($"Unknown color scheme requested: {scheme}");
                }
            }
            return found;
        }

        /// <summary>
        /// As the Categories given in <see cref="ObjectConfig"/> are all strings, 
        /// we need to parse them into usable versions.
        /// </summary>
        /// <param name="categories">The categories in string format</param>
        /// <returns>One category, made out of the string categories</returns>
        public static ObjectCategory ParseCategories(string[] categories)
        {
            var stringCategories = categories.Aggregate((x, y) => $"{x},{y}");
            return ObjectCategory.Parse<ObjectCategory>(stringCategories);
        }

        public static ClothesIntention ParseClothingIntentions(string[] intentions)
        {
            return intentions
                .Select(i => Enum.Parse<ClothesIntention>(i, true))
                .Aggregate((x, y) => x | y);
        }

        /// <summary>
        /// Here we search the loaded assemblies for all possible furniture types.
        /// </summary>
        private static Dictionary<string, Type> GetFurnitureTypes()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(TryExtractTypesFromAssembly)
                .ToDictionary(
                    type => type.Name.ToLower(),
                    type => type
                );
        }

        /// <summary>
        /// Method to try and read all types in an assembly.
        /// In case of failure, the assembly is ignored.
        /// </summary>
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

        /// <summary>
        /// Helper method to create a dictionary of <see cref="ColorScheme"/>s.
        /// </summary>
        private static Dictionary<string, ColorScheme> CreateColorSchemeMap()
        {
            return typeof(ColorScheme).GetFields()
            .ToDictionary(
                scheme => scheme.Name.ToLower(),
                scheme => (ColorScheme)scheme.GetValue(null)
            );
        }

        /// <summary>
        /// Helper method to create a dictionary of <see cref="Direction2"/> arrays.
        /// 
        /// Seems like a duplication, but I can't use static types as typ arguments.
        /// </summary>
        private static Dictionary<string, Direction2[]> CreateDirectionsMap()
        {
            return typeof(Direction2Helper).GetFields()
            .ToDictionary(
                scheme => scheme.Name.ToLower(),
                scheme => (Direction2[])scheme.GetValue(null)
            );
        }

    }
}
