namespace Omniscience.Portable
{
    public abstract class FuncContainer<T> : IFuncContainer
    {
        public abstract T Get(OmnIOC container);
    }
}