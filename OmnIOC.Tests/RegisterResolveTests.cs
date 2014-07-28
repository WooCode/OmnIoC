using OmnIOC.Portable;
using Xunit;

namespace OmnIOC.Tests
{
    public class RegisterResolveTests
    {
        [Fact]
        public void Register()
        {
            OmniContainer.Default.Clear();
            OmniContainer.Default.Register(o => new TestClass1());
            OmniContainer.Default.Register(o => new TestClass2(o.Resolve<TestClass1>()));
        }

        [Fact]
        public void RegisterNamed()
        {
            OmniContainer.Default.Clear();
            OmniContainer.Default.RegisterNamed(o => new TestClass1(), "1");
            OmniContainer.Default.RegisterNamed(o => new TestClass2(o.Resolve<TestClass1>("1")), "2");
        }

        [Fact]
        public void RegisterInstance()
        {
            OmniContainer.Default.Clear();
            OmniContainer.Default.Register(new TestClass1());
            OmniContainer.Default.Register(new TestClass2(OmniContainer.Default.Resolve<TestClass1>()));
        }

        [Fact]
        public void RegisterNamedInstance()
        {
            OmniContainer.Default.Clear();
            OmniContainer.Default.RegisterNamed(new TestClass1(), "1");
            OmniContainer.Default.RegisterNamed(new TestClass2(OmniContainer.Default.Resolve<TestClass1>("1")), "2");
        }

        [Fact]
        public void Resolve()
        {
            Register();
            var testClass = OmniContainer.Default.Resolve<TestClass2>();
            Assert.NotNull(testClass);
        }

        [Fact]
        public void ResolveNamed()
        {
            RegisterNamed();
            var testClass = OmniContainer.Default.Resolve<TestClass2>("2");
            Assert.NotNull(testClass);
        }

        [Fact]
        public void ResolveInstance()
        {
            RegisterInstance();
            var testClass = OmniContainer.Default.Resolve<TestClass2>();
            Assert.NotNull(testClass);
        }

        [Fact]
        public void ResolveNamedInstance()
        {
            RegisterNamedInstance();
            var testClass = OmniContainer.Default.Resolve<TestClass2>("2");
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