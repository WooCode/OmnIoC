using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable InconsistentNaming
// ReSharper disable StaticFieldInGenericType

namespace OmnIoC.Portable
{
    /// <summary>
    ///     Generic container that holds registrations for type(s)
    /// </summary>
    public sealed class OmnIoCContainer<RegistrationType> : IOmnIoCContainer
    {
        #region IOmnIoC members

        object IOmnIoCContainer.Get()
        {
            return Get();
        }

        object IOmnIoCContainer.GetNamed(string name)
        {
            return GetNamed(name);
        }

        /// <summary>
        ///     Set implementation type for <see cref="RegistrationType" /> with name
        /// </summary>
        /// <param name="implementationType"></param>
        /// <param name="reuse"></param>
        /// <param name="name"></param>
        void IOmnIoCContainer.Set(Type implementationType, Reuse reuse, string name)
        {
            switch (reuse)
            {
                case Reuse.Multiple:
                    Set(() => (RegistrationType) Activator.CreateInstance(implementationType), name);
                    break;
                case Reuse.Singleton:
                    var instance = (RegistrationType) Activator.CreateInstance(implementationType);
                    Set(() => instance, name);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("reuse");
            }
        }

        IEnumerable<object> IOmnIoCContainer.All()
        {
            return _namedCollection.Values.Select(f => (object) f());
        }

        #endregion

        private static Dictionary<string, Func<RegistrationType>> _namedCollection = new Dictionary<string, Func<RegistrationType>>(StringComparer.OrdinalIgnoreCase);
        private static readonly object _syncLock = new object();

        /// <summary>
        ///     The main registration (unamed registration) or default of (<see cref="RegistrationType" />)
        /// </summary>
        public static Func<RegistrationType> Get = () => default(RegistrationType);

        static OmnIoCContainer()
        {
            OmnIoCContainer.ClearAll += Clear;
        }

        public static void Clear()
        {
            lock (_syncLock)
            {
                _namedCollection = new Dictionary<string, Func<RegistrationType>>(StringComparer.OrdinalIgnoreCase);
                Get = () => default(RegistrationType);
            }
        }

        /// <summary>
        ///     Get named instance for <see cref="RegistrationType" />
        /// </summary>
        public static RegistrationType GetNamed(string name)
        {
            return _namedCollection[name]();
        }

        /// <summary>
        ///     Get all registered instances for <see cref="RegistrationType" />
        /// </summary>
        public static IEnumerable<RegistrationType> All()
        {
            return _namedCollection.Values.Select(f => f());
        }

        /// <summary>
        ///     Register factory/constructor for <see cref="RegistrationType" />
        /// </summary>
        public static void Set(Func<RegistrationType> factory, string name = null)
        {
            lock (_syncLock)
            {
                if (name == null)
                {
                    Get = factory;
                    name = string.Empty;
                }
                // Safely copy the old dictionary to new one and add the factory
                var newCollection = new Dictionary<string, Func<RegistrationType>>(_namedCollection, StringComparer.OrdinalIgnoreCase);
                newCollection[name] = factory;
                _namedCollection = newCollection;
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
        public static void Set<ImplementationType>(Reuse reuse = Reuse.Multiple, string name = null) where ImplementationType : RegistrationType, new()
        {
            if (reuse == Reuse.Singleton)
                Set(new ImplementationType());
            else
                Set(() => new ImplementationType(), name);
        }
    }
}

// ReSharper restore StaticFieldInGenericType
// ReSharper restore InconsistentNaming