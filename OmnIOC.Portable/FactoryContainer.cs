using System;

namespace OmnIOC.Portable
{
    /// <summary>
    ///     Container that holds the "factory" for a specific type/value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FactoryContainer<T> : FuncContainer<T>
    {
        private readonly Func<OmniContainer, T> _factory;

        public FactoryContainer(Func<OmniContainer, T> factory)
        {
            _factory = factory;
        }

        public override T Get(OmniContainer container)
        {
            return _factory.Invoke(container);
        }
    }
}