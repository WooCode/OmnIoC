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

        private readonly Dictionary<string, IFuncContainer> _typedCollection =
            new Dictionary<string, IFuncContainer>();

        private OmnIOC()
        {
            ThrowOnMissingType = true;
        }

        public static OmnIOC Default
        {
            get { return DefaultContainer.Value; }
        }

        /// <summary>
        ///     Should DiDay return default or throw when registration is missing for a type.
        /// </summary>
        public bool ThrowOnMissingType { get; set; }

        /// <summary>
        ///     Exposed to test project
        /// </summary>
        internal void Clear()
        {
            _typedCollection.Clear();
        }

        /// <summary>
        ///     Register factory for <see cref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        public void Register<T>(Func<OmnIOC, T> factory)
        {
            try
            {
                _lock.EnterWriteLock();
                var factoryContainer = new FactoryContainer<T>(factory);
                _typedCollection[typeof (T).FullName] = factoryContainer;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        ///     Register named factory for <see cref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <param name="name"></param>
        public void RegisterNamed<T>(Func<OmnIOC, T> factory, string name)
        {
            try
            {
                _lock.EnterWriteLock();
                var factoryContainer = new FactoryContainer<T>(factory);
                string key = FormatKey<T>(name);
                _typedCollection[key] = factoryContainer;
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
        public void Register<T>(T instance)
        {
            try
            {
                _lock.EnterWriteLock();
                var instanceContainer = new InstanceContainer<T>(instance);
                _typedCollection[typeof (T).FullName] = instanceContainer;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        ///     Register named instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        public void RegisterNamed<T>(T instance, string name)
        {
            try
            {
                _lock.EnterWriteLock();
                var instanceContainer = new InstanceContainer<T>(instance);
                string key = FormatKey<T>(name);
                _typedCollection[key] = instanceContainer;
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
                string key = name == null ? type.FullName : FormatKey<T>(name);

                if (_typedCollection.TryGetValue(key, out container))
                {
                    var factory = (FuncContainer<T>) container;
                    return factory.Get(this);
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

        private static string FormatKey<T>(string name)
        {
            return string.Format("{0}_{1}", typeof (T).FullName, name);
        }
    }
}