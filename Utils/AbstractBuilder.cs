using HarmonyLib;
using SimpleObjectLoader.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleObjectLoader.Utils
{
    /// <summary>
    /// Base class to quickly create new types of object builders.
    /// </summary>
    /// <param name="config">The object to create.</param>
    /// <param name="SubClassType">The subclass to find the relevant handler methods in.</param>
    internal abstract class AbstractBuilder(ObjectConfig config, Type SubClassType)
    {
        /// <summary>
        /// The type of the subclass 'currently' using this abstract class,
        /// for finding the relevant handler methods.
        /// </summary>
        private readonly Type SubClassType = SubClassType;

        /// <summary>
        /// Cache for all the methodhandlers for properties per subclass of this one.
        /// </summary>
        protected static readonly Dictionary<Type, Dictionary<string, List<MethodInfo>>> methods = [];

        protected readonly ObjectConfig _config = config;

        /// <summary>
        /// Read the methodhandlers from the cache, or find them if the cache is empty.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected static Dictionary<string, List<MethodInfo>> GetHandlerMethodsFor(Type type)
        {
            if (methods.TryGetValue(type, out Dictionary<string, List<MethodInfo>> value))
            {
                return value;
            }
            else
            {
                methods.Add(type, []);
                type.GetMethods()
                    .Where(method => method.GetCustomAttribute<HandlerForAttribute>() != null)
                    .Do(method => PopulateMethodDictionary(methods[type], method));
                return methods[type];
            }
        }

        /// <summary>
        /// Find all methods marked with <see cref="HandlerForAttribute"/> 
        /// and group them based on the property they're supposed to handle.
        /// </summary>
        private static void PopulateMethodDictionary(Dictionary<string, List<MethodInfo>> methods, MethodInfo method)
        {
            var property = method.GetCustomAttribute<HandlerForAttribute>().Property;
            if (methods.TryGetValue(property, out List<MethodInfo> value))
            {
                value.Add(method);
            }
            else
            {
                methods[property] = [method];
            }
        }

        /// <summary>
        /// Build the object.
        /// It will dynamically find the relevant methods according to the used properties in <see cref="ObjectConfig"/>.
        /// </summary>
        public void Build()
        {
            Initialize();

            InvokeHandlerMethods();

            Register();
        }

        /// <summary>
        /// Dynamically execute all the handler methods.
        /// </summary>
        private void InvokeHandlerMethods()
        {
            var methods = GetHandlerMethodsFor(SubClassType);
            typeof(ObjectConfig)
                .GetProperties()
                .Where(field => field.GetValue(_config) != null)
                .Select(field => field.Name)
                .Where(methods.ContainsKey)
                .SelectMany(field => methods[field])
                .Do(method => method.Invoke(this, null));
        }

        /// <summary>
        /// The required setup for this type of object.
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// Register the object with the appropiate Game API.
        /// </summary>
        protected abstract void Register();

    }
}
