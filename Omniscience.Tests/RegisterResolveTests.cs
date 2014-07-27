using Omniscience.Portable;
using Xunit;

namespace Omniscience.Tests
{
    public class RegisterResolveTests
    {
        [Fact]
        public void Register()
        {
            OmnIOC.Default.Clear();
            OmnIOC.Default.Register(o => new TestClass1());
            OmnIOC.Default.Register(o => new TestClass2(o.Resolve<TestClass1>()));
        }

        [Fact]
        public void RegisterNamed()
        {
            OmnIOC.Default.Clear();
            OmnIOC.Default.RegisterNamed(o => new TestClass1(), "1");
            OmnIOC.Default.RegisterNamed(o => new TestClass2(o.Resolve<TestClass1>("1")), "2");
        }

        [Fact]
        public void RegisterInstance()
        {
            OmnIOC.Default.Clear();
            OmnIOC.Default.Register(new TestClass1());
            OmnIOC.Default.Register(new TestClass2(OmnIOC.Default.Resolve<TestClass1>()));
        }

        [Fact]
        public void RegisterNamedInstance()
        {
            OmnIOC.Default.Clear();
            OmnIOC.Default.RegisterNamed(new TestClass1(), "1");
            OmnIOC.Default.RegisterNamed(new TestClass2(OmnIOC.Default.Resolve<TestClass1>("1")), "2");
        }

        [Fact]
        public void Resolve()
        {
            Register();
            var testClass = OmnIOC.Default.Resolve<TestClass2>();
            Assert.NotNull(testClass);
        }

        [Fact]
        public void ResolveNamed()
        {
            RegisterNamed();
            var testClass = OmnIOC.Default.Resolve<TestClass2>("2");
            Assert.NotNull(testClass);
        }

        [Fact]
        public void ResolveInstance()
        {
            RegisterInstance();
            var testClass = OmnIOC.Default.Resolve<TestClass2>();
            Assert.NotNull(testClass);
        }

        [Fact]
        public void ResolveNamedInstance()
        {
            RegisterNamedInstance();
            var testClass = OmnIOC.Default.Resolve<TestClass2>("2");
            Assert.NotNull(testClass);
        }

        public class TestClass1
        {
        }

        public class TestClass2
        {
// ReSharper disable NotAccessedField.Local
            private readonly TestClass1 _inner;
// ReSharper restore NotAccessedField.Local

            public TestClass2(TestClass1 inner)
            {
                _inner = inner;
            }
        }
    }
}