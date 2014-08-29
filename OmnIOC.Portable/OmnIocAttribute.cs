using System;

namespace OmnIoC.Portable
{
    /// <summary>
    ///     <see cref="OmnIoCContainer.Load"/> looks for this attribute when loading types
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class OmnIoCExportAttribute : Attribute
    {
        public string Name { get; set; }
        public Reuse Reuse { get; set; }

        /// <summary>
        /// Type to register the targeted class under
        /// </summary>
        public Type RegistrationType { get; set; }

        public OmnIoCExportAttribute()
        {
            Name = null;
            Reuse = Reuse.Multiple;
        }
    }
}