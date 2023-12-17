using System;

namespace SimpleObjectLoader.Config
{
    /// <summary>
    /// Attribute to mark a method as capable of handling a property in <see cref="ObjectConfig"/>.
    /// </summary>
    ///
    /// <param name="property">The property to handle</param>
    [AttributeUsage(AttributeTargets.Method)]
    internal class HandlerForAttribute(string property) : Attribute
    {

        public string Property { get; } = property;
    }
}
