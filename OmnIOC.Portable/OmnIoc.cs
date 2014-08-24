using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable InconsistentNaming
// ReSharper disable StaticFieldInGenericType

namespace OmnIOC.Portable
{
    public static class OmnIoc
    {
        public enum Reuse
        {
            Multiple,
            Singleton
        }

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
        ///     Register factory for <see cref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="name"></param>
        public static void Set<T>(Func<T> factory, string name = null)
        {
            OmnIoc<T>.Set(factory, name);
        }
    }

    /// <summary>
    ///     Generic container that holds registrations for type(s)
    /// </summary>
    public static class OmnIoc<RegistrationType>
    {
        internal static Dictionary<string, Func<RegistrationType>> _namedCollection = new Dictionary<string, Func<RegistrationType>>(StringComparer.OrdinalIgnoreCase);
        internal static readonly object SyncLock = new object();

        /// <summary>
        ///     The main registration (unamed registration) or default of (<see cref="RegistrationType" />)
        /// </summary>
        public static Func<RegistrationType> Get = () => default(RegistrationType);

        static OmnIoc()
        {
            OmnIoc.ClearAll += (sender, args) =>
            {
                lock (SyncLock)
                {
                    _namedCollection = new Dictionary<string, Func<RegistrationType>>(StringComparer.OrdinalIgnoreCase);
                    Get = () => default(RegistrationType);
                }
            };
        }

        public static RegistrationType GetNamed(string name)
        {
            return _namedCollection[name]();
        }

        public static IEnumerable<RegistrationType> GetAll()
        {
            return _namedCollection.Values.Select(f => f.Invoke()).ToArray();
        }

        /// <summary>
        ///     Register factory/constructor for <see cref="RegistrationType" />
        /// </summary>
        public static void Set(Func<RegistrationType> factory, string name = null)
        {
            lock (SyncLock)
            {
                if (name == null || Get == null)
                {
                    Get = factory;
                    name = string.Empty;
                }

                _namedCollection[name] = factory;
            }
        }

        /// <summary>
        ///     Register instance/singleton
        /// </summary>
        public static void Set(RegistrationType instance, string name = null)
        {
            Set(() => instance, name);
        }

        /// <summary>
        ///     Register instance
        /// </summary>
        public static void Set<ImplementationType>(OmnIoc.Reuse reuse = OmnIoc.Reuse.Multiple, string name = null) where ImplementationType : RegistrationType, new()

        {
            if (reuse == OmnIoc.Reuse.Singleton)
                Set(new ImplementationType());
            else
                Set(() => new ImplementationType(), name);
        }
    }
}

// ReSharper restore StaticFieldInGenericType
// ReSharper restore InconsistentNaming