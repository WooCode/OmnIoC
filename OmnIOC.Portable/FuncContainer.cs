namespace OmnIOC.Portable
{
    public abstract class FuncContainer<T>
    {
        public abstract T Get(OmnIOCContainer container);
    }
}