using System;
using System.Linq;
using System.Threading;
using OmnIOC.Portable;
using Xunit;

namespace OmnIOC.Tests
{
    public class RegisterResolveTests
    {
        public RegisterResolveTests()
        {
            OmnIoc.Clear();
        }

        [Fact]
        public void Factory()
        {
            OmnIoc.Set(() => new TestClass1());
            OmnIoc.Set(() => new TestClass2(OmnIoc<TestClass1>.Get()));

            var test = OmnIoc.Get(typeof(TestClass1));
            Console.WriteLine(test == null);
            var testClass = OmnIoc<TestClass2>.Get();
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void NamedFactory()
        {
            OmnIoc.Set(() => new TestClass1(), "1a");
            OmnIoc.Set(() => new TestClass2(OmnIoc<TestClass1>.GetNamed("1A")), "2X");
            var testClass = OmnIoc<TestClass2>.GetNamed("2x");
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void Instance()
        {
            OmnIoc.Set(new TestClass1());
            OmnIoc.Set(new TestClass2(OmnIoc<TestClass1>.Get()));
            var testClass = OmnIoc<TestClass2>.Get();
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void NamedInstance()
        {
            OmnIoc.Set(new TestClass1(), "1a");
            OmnIoc.Set(new TestClass2(OmnIoc<TestClass1>.GetNamed("1A")), "2X");
            var testClass = OmnIoc<TestClass2>.GetNamed("2x");
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void ResolveAll()
        {
            // Given
            Factory();
            NamedFactory();

            // Then
            var all = OmnIoc<TestClass2>.All().ToList();

            // Should
            Assert.Equal(all.Count(), 2);
            Assert.True(all.All(t => t.Inner != null));
            Assert.False(all.First().Inner == all.Last().Inner);
        }

        public class TestClass1
        {
        }

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
}