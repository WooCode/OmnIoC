namespace OmnIOC.Portable
{
    public abstract class FuncContainer<T> : IFuncContainer
    {
        public abstract T Get(OmniContainer container);
    }
}