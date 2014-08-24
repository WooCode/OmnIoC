using System;
using System.Linq;
using System.Threading;
using OmnIOC.Portable;
using Xunit;

namespace OmnIOC.Tests
{
    public class Generic
    {
        public Generic()
        {
            OmnIoc.Clear();
        }

        [Fact]
        public void SetTransient()
        {
            // Register
            OmnIoc<TestClass1>.Set(() => new TestClass1());
            OmnIoc<TestClass2>.Set(() => new TestClass2(OmnIoc<TestClass1>.Get()));

            // Resolve
            var testClass = OmnIoc<TestClass2>.Get();

            // Assert
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void SetManyTransient()
        {
            // Register
            OmnIoc<TestClass1>.Set(() => new TestClass1(), "1a");
            OmnIoc<TestClass2>.Set(() => new TestClass2(OmnIoc<TestClass1>.GetNamed("1A")), "2X");

            // Resolve
            var testClass = OmnIoc<TestClass2>.GetNamed("2x");

            // Assert
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void SetSingleton()
        {
            // Register
            OmnIoc<TestClass1>.Set(new TestClass1());
            OmnIoc<TestClass2>.Set(new TestClass2(OmnIoc<TestClass1>.Get()));

            // Resolve
            var testClass = OmnIoc<TestClass2>.Get();

            // Assert
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void SetManySingleton()
        {
            // Register
            OmnIoc<TestClass1>.Set(new TestClass1(), "1a");
            OmnIoc<TestClass1>.Set(new TestClass1(), "2a");
            OmnIoc<TestClass2>.Set(new TestClass2(OmnIoc<TestClass1>.GetNamed("1a")), "1b");
            OmnIoc<TestClass2>.Set(new TestClass2(OmnIoc<TestClass1>.GetNamed("2a")), "2b");

            // Resolve
            var testClass1 = OmnIoc<TestClass2>.GetNamed("1b");
            var testClass2 = OmnIoc<TestClass2>.GetNamed("2b");

            var testClass1_2 = OmnIoc<TestClass2>.GetNamed("1b");
            var testClass2_2 = OmnIoc<TestClass2>.GetNamed("2b");

            // Assert
            Assert.NotNull(testClass1);
            Assert.NotNull(testClass1.Inner);

            Assert.NotNull(testClass2);
            Assert.NotNull(testClass2.Inner);

            Assert.NotEqual(testClass1,testClass2);
            Assert.NotEqual(testClass1.Inner,testClass2.Inner);

            // Assert that the second get returned the same instance
            Assert.Equal(testClass1, testClass1_2);
            Assert.Equal(testClass1.Inner, testClass1_2.Inner);

            Assert.Equal(testClass2, testClass2_2);
            Assert.Equal(testClass2.Inner, testClass2_2.Inner);
        }

        [Fact]
        public void ResolveAll()
        {
            // Register
            SetTransient();
            SetManyTransient();

            // Resolve
            var all = OmnIoc<TestClass2>.All().ToList();

            // Assert
            Assert.Equal(all.Count(), 2);
            Assert.True(all.All(t => t.Inner != null));
            Assert.False(all.First().Inner == all.Last().Inner);
        }
    }
}