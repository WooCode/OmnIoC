using System;
using System.Collections.Concurrent;
using System.Threading;

namespace WooCode
{
    public class ResolveFailedException : Exception
    {
        public ResolveFailedException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Simple object factory / DiContainer
    /// Implement custom methods in a partial file since any changes will be lost on update.
    /// https://gist.github.com/WooCode/81eaa994852679f90bdb for usage example
    /// </summary>
    public partial class DiDay
    {
        #region Privates

        /// <summary>
        ///     Container that holds the "factory" for a specific type/value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class FactoryContainer<T> : FuncContainer<T>
        {
            private readonly Func<DiDay, T> _factory;

            public FactoryContainer(Func<DiDay, T> factory)
            {
                _factory = factory;
            }

            public override T Get(DiDay container)
            {
                return _factory(container);
            }
        }


        private abstract class FuncContainer<T> : IFuncContainer
        {
            public abstract T Get(DiDay container);
        }


        private interface IFuncContainer
        {
        }

        /// <summary>
        ///     Container that holds the "factory" for a specific type/value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class InstanceContainer<T> : FuncContainer<T>
        {
            private readonly T _instance;

            public InstanceContainer(T instance)
            {
                _instance = instance;
            }

            public override T Get(DiDay container)
            {
                return _instance;
            }
        }

        #endregion

        private static readonly Lazy<DiDay> DefaultContainer =
            new Lazy<DiDay>(LazyThreadSafetyMode.ExecutionAndPublication);

        private readonly ConcurrentDictionary<Type, IFuncContainer> _resolvers =
            new ConcurrentDictionary<Type, IFuncContainer>();

        public DiDay()
        {
            ThrowOnMissingType = true;
        }

        public static DiDay Default
        {
            get { return DefaultContainer.Value; }
        }

        public Func<Type, object> ManualResolver { get; set; }
        public bool ThrowOnMissingType { get; set; }

        /// <summary>
        ///     Register factory for <see cref="T" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        public void Register<T>(Func<DiDay, T> factory)
        {
            _resolvers[typeof (T)] = new FactoryContainer<T>(factory);
        }

        /// <summary>
        ///     Register instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public void Register<T>(T instance)
        {
            _resolvers[typeof (T)] = new InstanceContainer<T>(instance);
        }

        /// <summary>
        ///     Resolve implementation of <see cref="T" />
        /// </summary>
        /// <typeparam name="T">Type to be resolved</typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            Type type = typeof (T);

            if (_resolvers.ContainsKey(type))
            {
                var resolver = (FuncContainer<T>) _resolvers[type];
                return resolver.Get(this);
            }

            if (ManualResolver != null)
            {
                object returnType = ManualResolver(type);
                if (returnType is T)
                    return (T) returnType;
            }

            if (ThrowOnMissingType)
                throw new ResolveFailedException(string.Format("There is no type registered for {0}", type.FullName));

            return default(T);
        }
    }
}