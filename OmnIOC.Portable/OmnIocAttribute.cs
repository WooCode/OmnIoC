using System;

namespace OmnIoc.Portable
{
    /// <summary>
    /// <remarks>Internal for now since it's not complete</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal class OmnIocAttribute : Attribute
    {
        public string Name { get; set; }
        public IocReuse Reuse { get; set; }

        public OmnIocAttribute()
        {
            Name = null;
        }
    }

    /// <summary>
    /// <remarks>Internal for now since it's not complete</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false)]
    internal class OmnIocConstructorAttribute : Attribute
    {
        /// <summary>
        /// Name that matches the name of one <see cref="OmnIocAttribute"/> attached to the class
        /// </summary>
        public string Name { get; set; }
    }
}