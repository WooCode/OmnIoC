using System;

namespace OmnIOC.Portable
{
    /// <summary>
    ///     Container that holds the "factory" for a specific type/value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FactoryContainer<T> : FuncContainer<T>
    {
        private readonly Func<OmnIOCContainer, T> _factory;

        public FactoryContainer(Func<OmnIOCContainer, T> factory)
        {
            _factory = factory;
        }

        public override T Get(OmnIOCContainer container)
        {
            return _factory.Invoke(container);
        }
    }
}