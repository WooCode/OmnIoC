using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable InconsistentNaming
// ReSharper disable StaticFieldInGenericType

namespace OmnIoc.Portable
{
    /// <summary>
    ///     Generic container that holds registrations for type(s)
    /// </summary>
    public sealed class OmnIoc<RegistrationType> : IOmnIoc
    {
        private static Dictionary<string, Func<RegistrationType>> _namedCollection;
        private static readonly object _syncLock = new object();

        /// <summary>
        ///     The main registration (unamed registration) or default of (<see cref="RegistrationType" />)
        /// </summary>
        public static Func<RegistrationType> Get = () => default(RegistrationType);

        static OmnIoc()
        {
            OmnIoc.Instances[typeof (RegistrationType)] = new OmnIoc<RegistrationType>();
            OmnIoc.ClearAll += (sender, args) =>
            {
                lock (_syncLock)
                {
                    _namedCollection = new Dictionary<string, Func<RegistrationType>>(StringComparer.OrdinalIgnoreCase);
                    Get = () => default(RegistrationType);
                }
            };

            _namedCollection = new Dictionary<string, Func<RegistrationType>>(StringComparer.OrdinalIgnoreCase);
        }

        object IOmnIoc.Get(string name)
        {
            return GetNamed(name ?? string.Empty);
        }

        /// <summary>
        /// Set implementation type for <see cref="RegistrationType"/> with name 
        /// </summary>
        /// <param name="implementationType"></param>
        /// <param name="reuse"></param>
        /// <param name="name"></param>
        void IOmnIoc.Set(Type implementationType, IocReuse reuse, string name)
        {
            switch (reuse)
            {
                case IocReuse.Multiple:
                    Set(() => (RegistrationType) Activator.CreateInstance(implementationType), name);
                    break;
                case IocReuse.Singleton:
                    var instance = (RegistrationType) Activator.CreateInstance(implementationType);
                    Set(() => instance, name);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("reuse");
            }
        }

        IEnumerable<object> IOmnIoc.All()
        {
            lock (_syncLock)
                return _namedCollection.Values.Select(f => (object)f());
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
            lock (_syncLock)
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
        public static void Set<ImplementationType>(IocReuse reuse = IocReuse.Multiple, string name = null) where ImplementationType : RegistrationType, new()
        {
            if (reuse == IocReuse.Singleton)
                Set(new ImplementationType());
            else
                Set(() => new ImplementationType(), name);
        }
    }
}

// ReSharper restore StaticFieldInGenericType
// ReSharper restore InconsistentNaming