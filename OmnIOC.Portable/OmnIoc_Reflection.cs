using System;

namespace OmnIOC.Portable
{
    public static partial class OmnIoc
    {
        /// <summary>
        ///     Get implementation of <see cref="type" />
        /// </summary>
        /// <returns></returns>
        public static object Get(Type type, string name = null)
        {
            var container = GetContainer(type);
            return container.GetTypeInstance(name);
        }

        /// <summary>
        ///     Get the right generic implementation of OmnIoc based on type.
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
                var containerType = typeof(OmnIoc<>).MakeGenericType(type);
                container = Activator.CreateInstance(containerType) as IOmnIoc;
                if (container == null)
                    throw new Exception("Failed to create instance");

                Instances[type] = container;
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
        /// <param name="implementationType">Implementation type of <see cref="registrationType" /></param>
        /// <param name="reuse">Should the registration be singleton or multiple</param>
        /// <param name="name"></param>
        public static void Set(Type registrationType, Type implementationType, Reuse reuse = Reuse.Multiple, string name = null)
        {
            var container = GetContainer(registrationType);
            container.SetTypeInstance(implementationType ?? registrationType, reuse, name);
        }
    }
}
