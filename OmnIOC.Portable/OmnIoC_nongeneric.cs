using System;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace OmnIoC.Portable
{
    /// <summary>
    ///     Nongeneric part of the container
    ///     The generic version is faster and this version is supplied just for the sake of registering by <see cref="Type" />
    /// </summary>
    public static class OmnIoC
    {
        private static Dictionary<Type, IOmnIoC> Instances = new Dictionary<Type, IOmnIoC>();
        internal static event EventHandler ClearAll = (sender, args) => { };
        
        private static readonly object _syncLock = new object();
        private static readonly Type GenericBase = typeof (OmnIoC<>);
        

        /// <summary>
        ///     Clear all registrations
        /// </summary>
        public static void Clear()
        {
            var handler = ClearAll;
            if (handler != null) handler(null, EventArgs.Empty);
        }

        /// <summary>
        ///     Get implementation of <see cref="registrationType" />
        /// </summary>
        /// <returns></returns>
        public static object Get(Type registrationType)
        {
            var container = GetContainer(registrationType);
            return container.Get();
        }

        /// <summary>
        ///     Get named implementation of <see cref="registrationType" />
        /// </summary>
        /// <returns></returns>
        public static object GetNamed(Type registrationType, string name)
        {
            var container = GetContainer(registrationType);
            return container.GetNamed(name);
        }

        /// <summary>
        ///     Load all attributed types
        ///     <remarks>Internal for now since it's not complete</remarks>
        /// </summary>
        internal static void LoadAll(IEnumerable<Type> types)
        {
            var classAttribute = typeof (OmnIoCAttribute);
            var constructorAttribute = typeof (OmnIoCConstructorAttribute);

            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes(classAttribute, false);
                if (attributes.Length == 0)
                    continue;

                foreach (var attr in (OmnIoCAttribute[]) attributes)
                {
                    Set(type, null, attr.Reuse, attr.Name);
                }
            }
        }

        /// <summary>
        ///     Get the right generic type of OmnIoC based on type.
        /// </summary>
        /// <returns></returns>
        private static IOmnIoC GetContainer(Type type)
        {
            IOmnIoC container;
            if (Instances.TryGetValue(type, out container)) return container;
            // We could get multiple creations here since we enter and exit the lock but that's nothing to worry about.
            // The created instance is only a light instance to access the static generic registration without generics
            lock (_syncLock)
            {
                if (Instances.TryGetValue(type, out container)) return container;
                container = Activator.CreateInstance(GenericBase.MakeGenericType(type)) as IOmnIoC;

                // Safely copy the old dictionary to new one and add the new container
                var newCollection = new Dictionary<Type, IOmnIoC>(Instances);
                newCollection[type] = container;
                Instances = newCollection;
            }

            return container;
        }

        /// <summary>
        ///     Register implementation of type
        /// </summary>
        /// <param name="registrationType">Type to register</param>
        /// <param name="implementationType">Implementation type of <see cref="registrationType" /> null to use <see cref="registrationType" /></param>
        /// <param name="reuse">Should the registration be singleton or multiple</param>
        /// <param name="name"></param>
        public static void Set(Type registrationType, Type implementationType, Reuse reuse = Reuse.Multiple, string name = null)
        {
            var container = GetContainer(registrationType);
            container.Set(implementationType ?? registrationType, reuse, name);
        }

        public static IEnumerable<object> All(Type registrationType)
        {
            return GetContainer(registrationType).All();
        }
    }
}

// ReSharper enable InconsistentNaming