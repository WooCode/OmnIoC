using System;
using System.Collections.Generic;

namespace OmnIoc.Portable
{
    public static partial class OmnIoc
    {
        private static readonly Type GenericBase = typeof (OmnIoc<>);
        /// <summary>
        ///     Get implementation of <see cref="type" />
        /// </summary>
        /// <returns></returns>
        public static object Get(Type registrationType, string name = null)
        {
            var container = GetContainer(registrationType);
            return container.Get(name);
        }

        /// <summary>
        /// Load all attributed types
        /// <remarks>Internal for now since it's not complete</remarks>
        /// </summary>
        internal static void LoadAll(IEnumerable<Type> types)
        {
            var classAttribute = typeof (OmnIocAttribute);
            var constructorAttribute = typeof (OmnIocConstructorAttribute);

            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes(classAttribute, false);
                if(attributes.Length == 0)
                    continue;

                foreach (var attr in (OmnIocAttribute[])attributes)
                {
                    Set(type,null,attr.Reuse,attr.Name);
                }
            }
        }

        /// <summary>
        ///     Get the right generic type of OmnIoc based on type.
        /// </summary>
        /// <returns></returns>
        private static IOmnIoc GetContainer(Type type)
        {
            IOmnIoc container;
            try
            {
                _lock.EnterReadLock();
                if (Instances.TryGetValue(type, out container))
                    return container;
            }
            finally
            {
                _lock.ExitReadLock();
            }

            try
            {
                // We could get multiple creations here since we enter and exit the lock but that's nothing to worry about.
                // The created instance is only a light instance to access the static generic registration without generics
                _lock.EnterWriteLock();
                container = Activator.CreateInstance(GenericBase.MakeGenericType(type)) as IOmnIoc;
                return container;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        ///     Register implementation of type
        /// </summary>
        /// <param name="registrationType">Type to register</param>
        /// <param name="implementationType">Implementation type of <see cref="registrationType" /> null to use <see cref="registrationType"/></param>
        /// <param name="iocReuse">Should the registration be singleton or multiple</param>
        /// <param name="name"></param>
        public static void Set(Type registrationType, Type implementationType, IocReuse iocReuse = IocReuse.Multiple, string name = null)
        {
            var container = GetContainer(registrationType);
            container.Set(implementationType ?? registrationType, iocReuse, name);
        }

        public static IEnumerable<object> All(Type registrationType)
        {
            return GetContainer(registrationType).All();
        } 
    }
}
