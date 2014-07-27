using System;
using System.Collections.Generic;
using System.Threading;

namespace Omniscience.Portable
{
    /// <summary>
    ///     Simple object factory / DiContainer
    ///     Implement custom methods in a partial file since any changes will be lost on update.
    /// </summary>
    public class OmnIOC
    {
        private static readonly Lazy<OmnIOC> DefaultContainer =
            new Lazy<OmnIOC>(() => new OmnIOC(), LazyThreadSafetyMode.ExecutionAndPublication);

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        private readonly Dictionary<string, IFuncContainer> _namedCollection =
            new Dictionary<string, IFuncContainer>();

        private readonly Dictionary<string, IFuncContainer> _typedCollection =
            new Dictionary<string, IFuncContainer>();

        private OmnIOC()
        {
            ThrowOnMissingType = true;
        }

        public static OmnIOC Default { get { return DefaultContainer.Value; } }

        /// <summary>
        ///     Should DiDay return default or throw when registration is missing for a type.
        /// </summary>
        public bool ThrowOnMissingType { get; set; }

        /// <summary>
        ///     Register factory for <see cref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="name"></param>
        public void Register<T>(Func<OmnIOC, T> factory, string name = null)
        {
            try
            {
                _lock.EnterWriteLock();
                var factoryContainer = new FactoryContainer<T>(factory);
                _typedCollection[typeof (T).FullName] = factoryContainer;


                if (name != null)
                    _namedCollection[name] = factoryContainer;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        ///     Register instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        public void Register<T>(T instance, string name = null)
        {
            try
            {
                _lock.EnterWriteLock();
                var instanceContainer = new InstanceContainer<T>(instance);
                _typedCollection[typeof (T).FullName] = instanceContainer;

                if (name != null)
                    _namedCollection[name] = instanceContainer;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        ///     Resolve implementation of <see cref="T" />
        /// </summary>
        /// <typeparam name="T">Type to be resolved</typeparam>
        /// <param name="name"></param>
        /// <param name="throwOnMissingType">Overrides ThrowOnMissingType property</param>
        /// <returns></returns>
        public T Resolve<T>(string name = null, bool? throwOnMissingType = null)
        {
            if (!throwOnMissingType.HasValue)
                throwOnMissingType = ThrowOnMissingType;

            Type type = typeof (T);
            try
            {
                _lock.EnterReadLock();

                IFuncContainer container;
                if (_typedCollection.TryGetValue(type.FullName, out container) ||
                    _namedCollection.TryGetValue(type.FullName, out container))
                {
                    var resolver = (FuncContainer<T>) container;
                    return resolver.Get(this);
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }

            if (throwOnMissingType.Value)
                throw new Exception(string.Format("There is no type registered for {0}", type.FullName));

            return default(T);
        }
    }
}