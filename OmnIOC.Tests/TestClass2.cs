namespace OmnIOC.Tests
{
    public class TestClass2
    {
        // ReSharper disable NotAccessedField.Local
        public readonly TestClass1 Inner;
        // ReSharper restore NotAccessedField.Local

        public TestClass2(TestClass1 inner)
        {
            Inner = inner;
        }
    }
}