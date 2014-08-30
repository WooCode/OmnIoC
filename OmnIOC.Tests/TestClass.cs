namespace OmnIoC.Tests
{
    public class TestClass : ITestClass
    {
        public TestClass1 Inner { get; set; }
        public TestClass2 Inner2 { get; set; }
        public TestClass3 Inner3 { get; set; }

        public TestClass(TestClass1 inner)
        {
            Inner = inner;
        }

        public TestClass(TestClass1 inner1, TestClass2 inner2, TestClass3 inner3)
        {
            Inner = inner1;
            Inner2 = inner2;
            Inner3 = inner3;
        }
    }

    public class TestClass1 : ITestClass
    {
    }

    public class TestClass2 : ITestClass
    {
        public TestClass1 Inner { get; set; }

        public TestClass2(TestClass1 inner)
        {
            Inner = inner;
        }
    }

    public class TestClass3 : ITestClass
    {
        public TestClass2 Inner { get; set; }

        public TestClass3(TestClass2 inner)
        {
            Inner = inner;
        }
    }
}