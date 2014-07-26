namespace Omniscience.Portable
{
    /// <summary>
    ///     Container that holds the "factory" for a specific type/value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InstanceContainer<T> : FuncContainer<T>
    {
        private readonly T _instance;

        public InstanceContainer(T instance)
        {
            _instance = instance;
        }

        public override T Get(OmnIOC container)
        {
            return _instance;
        }
    }
}