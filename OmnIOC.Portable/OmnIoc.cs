using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

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

        internal static readonly Dictionary<Type, IOmnIoc> Instances = new Dictionary<Type, IOmnIoc>();

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

        public static object Get(Type type)
        {
            var container = GetContainer(type);
            return container.GetTypeInstance();
        }

        private static IOmnIoc GetContainer(Type type)
        {
            lock (Instances)
            {
                IOmnIoc container;
                if (Instances.TryGetValue(type, out container))
                    return container;


                var containerType = typeof(OmnIoc<>).MakeGenericType(type);
                container = Activator.CreateInstance(containerType) as IOmnIoc;
                if (container == null)
                    throw new Exception("Failed to create instance");

                Instances[type] = container;
                return container;
            }
        }
    }

    /// <summary>
    ///     Generic container that holds registrations for type(s)
    /// </summary>
    public class OmnIoc<RegistrationType> : IOmnIoc
    {
        private static Dictionary<string, Func<RegistrationType>> _namedCollection;
        private static readonly object _syncLock = new object();
        
        /// <summary>
        ///     The main registration (unamed registration) or default of (<see cref="RegistrationType" />)
        /// </summary>
        public static Func<RegistrationType> Get = () => default(RegistrationType);

        private static Dictionary<string, Func<RegistrationType>>.ValueCollection _valueCollection;

        static OmnIoc()
        {
            OmnIoc.Instances[typeof(RegistrationType)] = new OmnIoc<RegistrationType>();
            OmnIoc.ClearAll += (sender, args) =>
            {
                lock (_syncLock)
                {
                    _namedCollection = new Dictionary<string, Func<RegistrationType>>(StringComparer.OrdinalIgnoreCase);
                    _valueCollection = _namedCollection.Values;
                    Get = () => default(RegistrationType);
                }
            };

            _namedCollection = new Dictionary<string, Func<RegistrationType>>(StringComparer.OrdinalIgnoreCase);
            _valueCollection = _namedCollection.Values;
        }

        public object GetTypeInstance()
        {
            return Get();
        }

        public static RegistrationType GetNamed(string name)
        {
            return _namedCollection[name]();
        }

        public static IEnumerable<RegistrationType> All()
        {
            return _valueCollection.Select(f => f());
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
        public static void Set<ImplementationType>(OmnIoc.Reuse reuse = OmnIoc.Reuse.Multiple, string name = null) where ImplementationType : RegistrationType, new()
        {
            if (reuse == OmnIoc.Reuse.Singleton)
                Set(new ImplementationType());
            else
                Set(() => new ImplementationType(), name);
        }
    }

    public interface IOmnIoc
    {
        object GetTypeInstance();
    }
}

// ReSharper restore StaticFieldInGenericType
// ReSharper restore InconsistentNaming