using System;
using System.Linq;
using OmnIoC.Portable;
using Xunit;

// ReSharper disable InconsistentNaming
namespace OmnIoC.Tests
{
    public class NonGenericEmptyConstructor
    {
        private readonly Type _implementationType = typeof (TestClass1);
        private readonly Type _registrationType = typeof(ITestClass);

        public NonGenericEmptyConstructor()
        {
            OmnIoC.Portable.OmnIoC.Clear();
        }

        [Fact]
        public void LoadAttributes()
        {
            // Register
            OmnIoC.Portable.OmnIoC.LoadAll(AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()));

            // Resolve
            var test1 = OmnIoC.Portable.OmnIoC.GetNamed(typeof (AttributeTestClass), "testOne");
            var test2 = OmnIoC.Portable.OmnIoC.GetNamed(typeof(AttributeTestClass), "testTwo");
            var test3 = OmnIoC.Portable.OmnIoC.GetNamed(typeof(AttributeTestClass), "testTwo");

            // Assert
            Assert.NotNull(test1);
            Assert.NotNull(test2);
            Assert.NotNull(test3);

            Assert.NotEqual(test1, test2);
            Assert.NotEqual(test1, test3);
            Assert.Equal(test2, test3);
        }

        [Fact]
        public void SetTransient()
        {
            // Register
            OmnIoC.Portable.OmnIoC.Set(_registrationType, _implementationType);

            // Resolve
            var test1 = (TestClass1)OmnIoC.Portable.OmnIoC.Get(_registrationType);
            var test2 = (TestClass1)OmnIoC.Portable.OmnIoC.Get(_registrationType);

            // Assert
            Assert.NotNull(test1);
            Assert.NotNull(test2);
            Assert.NotEqual(test1,test2);
        }

        [Fact]
        public void SetSingleton()
        {
            // Register
            OmnIoC.Portable.OmnIoC.Set(_registrationType, _implementationType, Reuse.Singleton);

            // Resolve
            var test1 = (ITestClass)OmnIoC.Portable.OmnIoC.Get(_registrationType);
            var test2 = (ITestClass)OmnIoC.Portable.OmnIoC.Get(_registrationType);

            // Assert
            Assert.NotNull(test1);
            Assert.NotNull(test2);
            Assert.Equal(test1, test2);
        }

        [Fact]
        public void SetManyTransient()
        {
            // Register
            OmnIoC.Portable.OmnIoC.Set(_registrationType, _implementationType,name:"test1");
            OmnIoC.Portable.OmnIoC.Set(_registrationType, _implementationType, name: "test2");
            OmnIoC.Portable.OmnIoC.Set(_registrationType, _implementationType, name: "test3");

            // Resolve
            var test1 = (ITestClass)OmnIoC.Portable.OmnIoC.GetNamed(_registrationType, "test1");
            var test2 = (ITestClass)OmnIoC.Portable.OmnIoC.GetNamed(_registrationType, "test2");
            var test3 = (ITestClass)OmnIoC.Portable.OmnIoC.GetNamed(_registrationType, "test3");

            // Assert
            Assert.NotNull(test1);
            Assert.NotNull(test2);
            Assert.NotNull(test3);
            Assert.NotEqual(test1, test2);
            Assert.NotEqual(test2, test3);
            Assert.NotEqual(test1, test3);
        }

        [Fact]
        public void SetManySingleton()
        {
            // Register
            OmnIoC.Portable.OmnIoC.Set(_registrationType, _implementationType, name: "test1", reuse: Reuse.Singleton);
            OmnIoC.Portable.OmnIoC.Set(_registrationType, _implementationType, name: "test2", reuse: Reuse.Singleton);
            OmnIoC.Portable.OmnIoC.Set(_registrationType, _implementationType, name: "test3", reuse: Reuse.Singleton);

            // Resolve
            var test1 = (ITestClass)OmnIoC.Portable.OmnIoC.GetNamed(_registrationType, "test1");
            var test1_2 = (ITestClass)OmnIoC.Portable.OmnIoC.GetNamed(_registrationType, "test1");

            var test2 = (ITestClass)OmnIoC.Portable.OmnIoC.GetNamed(_registrationType, "test2");
            var test2_2 = (ITestClass)OmnIoC.Portable.OmnIoC.GetNamed(_registrationType, "test2");

            var test3 = (ITestClass)OmnIoC.Portable.OmnIoC.GetNamed(_registrationType, "test3");
            var test3_2 = (ITestClass)OmnIoC.Portable.OmnIoC.GetNamed(_registrationType, "test3");

            var all = OmnIoC<ITestClass>.All();

            // Assert
            Assert.NotNull(test1);
            Assert.Equal(test1, test1_2);

            Assert.NotNull(test2);
            Assert.Equal(test2, test2_2);

            Assert.NotNull(test3);
            Assert.Equal(test3, test3_2);

            Assert.Equal(all.Count(),3);
        }
    }
}
// ReSharper restore InconsistentNaming
