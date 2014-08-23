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
            OmnIoc.Set(() =>  new TestClass1());
            OmnIoc.Set(() =>  new TestClass2(OmnIoc.Get<TestClass1>()));
            var testClass = OmnIoc.Get<TestClass2>();
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void NamedFactory()
        {
            OmnIoc.Set(() =>  new TestClass1(), "1a");
            OmnIoc.Set(() =>  new TestClass2(OmnIoc.Get<TestClass1>("1A")), "2X");
            var testClass = OmnIoc.Get<TestClass2>("2x");
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void GetAllNamesForType()
        {
            OmnIoc.Set(() =>  new TestClass1(), "1a");
            OmnIoc.Set(new TestClass1(), "2a");
            var names = OmnIoc.GetAllTuples<TestClass1>();
            Assert.NotNull(names);
            Assert.True(names.Count == 2);
            Assert.Contains("1a", names.Keys);
            Assert.Contains("2a", names.Keys);
        }

        [Fact]
        public void Instance()
        {
            OmnIoc.Set(new TestClass1());
            OmnIoc.Set(new TestClass2(OmnIoc.Get<TestClass1>()));
            var testClass = OmnIoc.Get<TestClass2>();
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void NamedInstance()
        {
            OmnIoc.Set(new TestClass1(), "1a");
            OmnIoc.Set(new TestClass2(OmnIoc.Get<TestClass1>("1A")), "2X");
            var testClass = OmnIoc.Get<TestClass2>("2x");
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
            var all = OmnIoc.GetAll<TestClass2>().ToList();

            // Should
            Assert.Equal(all.Count(),2);
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