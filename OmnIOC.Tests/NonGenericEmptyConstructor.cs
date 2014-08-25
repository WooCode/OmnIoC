using System;
using System.Linq;
using OmnIoc.Portable;
using Xunit;

// ReSharper disable InconsistentNaming
namespace OmnIoc.Tests
{
    public class NonGenericEmptyConstructor
    {
        private readonly Type _implementationType = typeof (TestClass1);
        private readonly Type _registrationType = typeof(ITestClass);

        public NonGenericEmptyConstructor()
        {
            OmnIoc.Portable.OmnIoc.Clear();
        }

        [Fact]
        public void LoadAttributes()
        {
            // Register
            OmnIoc.Portable.OmnIoc.LoadAll(AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()));

            // Resolve
            var test1 = OmnIoc.Portable.OmnIoc.Get(typeof (AttributeTestClass), "testOne");
            var test2 = OmnIoc.Portable.OmnIoc.Get(typeof(AttributeTestClass), "testTwo");
            var test3 = OmnIoc.Portable.OmnIoc.Get(typeof(AttributeTestClass), "testTwo");

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
            OmnIoc.Portable.OmnIoc.Set(_registrationType, _implementationType);

            // Resolve
            var test1 = (TestClass1)OmnIoc.Portable.OmnIoc.Get(_registrationType);
            var test2 = (TestClass1)OmnIoc.Portable.OmnIoc.Get(_registrationType);

            // Assert
            Assert.NotNull(test1);
            Assert.NotNull(test2);
            Assert.NotEqual(test1,test2);
        }

        [Fact]
        public void SetSingleton()
        {
            // Register
            OmnIoc.Portable.OmnIoc.Set(_registrationType, _implementationType, IocReuse.Singleton);

            // Resolve
            var test1 = (ITestClass)OmnIoc.Portable.OmnIoc.Get(_registrationType);
            var test2 = (ITestClass)OmnIoc.Portable.OmnIoc.Get(_registrationType);

            // Assert
            Assert.NotNull(test1);
            Assert.NotNull(test2);
            Assert.Equal(test1, test2);
        }

        [Fact]
        public void SetManyTransient()
        {
            // Register
            OmnIoc.Portable.OmnIoc.Set(_registrationType, _implementationType,name:"test1");
            OmnIoc.Portable.OmnIoc.Set(_registrationType, _implementationType, name: "test2");
            OmnIoc.Portable.OmnIoc.Set(_registrationType, _implementationType, name: "test3");

            // Resolve
            var test1 = (ITestClass)OmnIoc.Portable.OmnIoc.Get(_registrationType, "test1");
            var test2 = (ITestClass)OmnIoc.Portable.OmnIoc.Get(_registrationType, "test2");
            var test3 = (ITestClass)OmnIoc.Portable.OmnIoc.Get(_registrationType, "test3");

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
            OmnIoc.Portable.OmnIoc.Set(_registrationType, _implementationType, name: "test1", iocReuse: IocReuse.Singleton);
            OmnIoc.Portable.OmnIoc.Set(_registrationType, _implementationType, name: "test2", iocReuse: IocReuse.Singleton);
            OmnIoc.Portable.OmnIoc.Set(_registrationType, _implementationType, name: "test3", iocReuse: IocReuse.Singleton);

            // Resolve
            var test1 = (ITestClass)OmnIoc.Portable.OmnIoc.Get(_registrationType, "test1");
            var test1_2 = (ITestClass)OmnIoc.Portable.OmnIoc.Get(_registrationType, "test1");

            var test2 = (ITestClass)OmnIoc.Portable.OmnIoc.Get(_registrationType, "test2");
            var test2_2 = (ITestClass)OmnIoc.Portable.OmnIoc.Get(_registrationType, "test2");

            var test3 = (ITestClass)OmnIoc.Portable.OmnIoc.Get(_registrationType, "test3");
            var test3_2 = (ITestClass)OmnIoc.Portable.OmnIoc.Get(_registrationType, "test3");

            var all = OmnIoc<ITestClass>.All();

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
