using System;
using System.Collections.Generic;
using System.Threading;

// ReSharper disable InconsistentNaming

namespace OmnIOC.Portable
{
    public static partial class OmnIoc
    {
        public enum Reuse
        {
            Multiple,
            Singleton
        }

        internal static readonly Dictionary<Type, IOmnIoc> Instances = new Dictionary<Type, IOmnIoc>();
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        internal static event EventHandler ClearAll = (sender, args) => { };

        /// <summary>
        ///     Clear all registrations
        /// </summary>
        public static void Clear()
        {
            var handler = ClearAll;
            if (handler != null) handler(null, EventArgs.Empty);
        }

        /// <summary>
        ///     Register factory for <see cref="RegistrationType" />
        /// </summary>
        /// <typeparam name="RegistrationType"></typeparam>
        /// <param name="factory"></param>
        /// <param name="name"></param>
        public static void Set<RegistrationType>(Func<RegistrationType> factory, string name = null)
        {
            OmnIoc<RegistrationType>.Set(factory, name);
        }

        /// <summary>
        ///     Register instance/singleton
        /// </summary>
        public static void Set<RegistrationType>(RegistrationType instance, string name = null)
        {
            Set(() => instance, name);
        }
    }

    
}

// ReSharper enable InconsistentNaming