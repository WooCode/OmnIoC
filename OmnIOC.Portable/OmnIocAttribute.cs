using System;

namespace OmnIoC.Portable
{
    /// <summary>
    /// <remarks>Internal for now since it's not complete</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal class OmnIoCAttribute : Attribute
    {
        public string Name { get; set; }
        public Reuse Reuse { get; set; }

        public OmnIoCAttribute()
        {
            Name = null;
        }
    }

    /// <summary>
    /// <remarks>Internal for now since it's not complete</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false)]
    internal class OmnIoCConstructorAttribute : Attribute
    {
        /// <summary>
        /// Name that matches the name of one <see cref="OmnIoCAttribute"/> attached to the class
        /// </summary>
        public string Name { get; set; }
    }
}