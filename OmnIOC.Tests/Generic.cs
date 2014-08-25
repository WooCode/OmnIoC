using System.Linq;
using OmnIoC.Portable;
using Xunit;

namespace OmnIoC.Tests
{
    public class Generic
    {
        public Generic()
        {
            OmnIoC.Portable.OmnIoC.Clear();
        }

        [Fact]
        public void SetTransient()
        {
            // Register
            OmnIoC<TestClass1>.Set(() => new TestClass1());
            OmnIoC<TestClass2>.Set(() => new TestClass2(OmnIoC<TestClass1>.Get()));

            // Resolve
            var testClass = OmnIoC<TestClass2>.Get();

            // Assert
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void SetManyTransient()
        {
            // Register
            OmnIoC<TestClass1>.Set(() => new TestClass1(), "1a");
            OmnIoC<TestClass2>.Set(() => new TestClass2(OmnIoC<TestClass1>.GetNamed("1A")), "2X");

            // Resolve
            var testClass = OmnIoC<TestClass2>.GetNamed("2x");

            // Assert
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void SetSingleton()
        {
            // Register
            OmnIoC<TestClass1>.Set(new TestClass1());
            OmnIoC<TestClass2>.Set(new TestClass2(OmnIoC<TestClass1>.Get()));

            // Resolve
            var testClass = OmnIoC<TestClass2>.Get();

            // Assert
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void SetManySingleton()
        {
            // Register
            OmnIoC<TestClass1>.Set(new TestClass1(), "1a");
            OmnIoC<TestClass1>.Set(new TestClass1(), "2a");
            OmnIoC<TestClass2>.Set(new TestClass2(OmnIoC<TestClass1>.GetNamed("1a")), "1b");
            OmnIoC<TestClass2>.Set(new TestClass2(OmnIoC<TestClass1>.GetNamed("2a")), "2b");

            // Resolve
            var testClass1 = OmnIoC<TestClass2>.GetNamed("1b");
            var testClass2 = OmnIoC<TestClass2>.GetNamed("2b");

            var testClass1_2 = OmnIoC<TestClass2>.GetNamed("1b");
            var testClass2_2 = OmnIoC<TestClass2>.GetNamed("2b");

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
            var all = OmnIoC<TestClass2>.All().ToList();

            // Assert
            Assert.Equal(all.Count(), 2);
            Assert.True(all.All(t => t.Inner != null));
            Assert.False(all.First().Inner == all.Last().Inner);
        }
    }
}