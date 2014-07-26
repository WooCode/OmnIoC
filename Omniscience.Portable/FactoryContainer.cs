using System;

namespace Omniscience.Portable
{
    /// <summary>
    ///     Container that holds the "factory" for a specific type/value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FactoryContainer<T> : FuncContainer<T>
    {
        private readonly Func<OmnIOC, T> _factory;

        public FactoryContainer(Func<OmnIOC, T> factory)
        {
            _factory = factory;
        }

        public override T Get(OmnIOC container)
        {
            return _factory.Invoke(container);
        }
    }
}