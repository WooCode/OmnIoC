using System;
using System.Linq;
using OmnIoC.Portable;
using Xunit;

// ReSharper disable InconsistentNaming

namespace OmnIoC.Tests
{
    public class NonGeneric
    {
        private readonly Type _implementationType = typeof (TestClass1);
        private readonly Type _registrationType = typeof (ITestClass);

        public NonGeneric()
        {
            OmnIoCContainer.Clear();
        }

        [Fact]
        public void LoadByAttributes()
        {
            // Register all types decorated with OmnIoCAttribute
            OmnIoCContainer.Load(AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()));

            // Resolve
            var unnamedTest = (IAttributeTestClass)OmnIoCContainer.Get(typeof(IAttributeTestClass));
            var test1 = (IAttributeTestClass) OmnIoCContainer.GetNamed(typeof (IAttributeTestClass), "multiple");
            var test2 = (IAttributeTestClass) OmnIoCContainer.GetNamed(typeof (IAttributeTestClass), "multiple");
            var test3 = (AttributeTestClass) OmnIoCContainer.GetNamed(typeof (AttributeTestClass), "singleton");
            var test4 = (AttributeTestClass) OmnIoCContainer.GetNamed(typeof (AttributeTestClass), "singleton");

            // Assert
            Assert.NotNull(unnamedTest);
            Assert.NotNull(test1);
            Assert.NotNull(test2);
            Assert.NotNull(test3);

            Assert.NotEqual(unnamedTest, test1);
            Assert.NotEqual(unnamedTest, test3);
            Assert.NotEqual(test1, test3);
            Assert.NotEqual(test2, test3);
            Assert.NotEqual(test1, test4);
            Assert.NotEqual(test2, test4);
            Assert.NotEqual(test1, test2);

            Assert.Equal(test3, test4);
            Assert.Equal(test1.Name, "XXX");
            Assert.Equal(test2.Name, "XXX");
            Assert.Equal(test1.Name, "XXX");
        }

        [Fact]
        public void SetTransient()
        {
            // Register
            OmnIoCContainer.Set(_registrationType, _implementationType);

            // Resolve
            var test1 = (TestClass1) OmnIoCContainer.Get(_registrationType);
            var test2 = (TestClass1) OmnIoCContainer.Get(_registrationType);

            // Assert
            Assert.NotNull(test1);
            Assert.NotNull(test2);
            Assert.NotEqual(test1, test2);
        }

        [Fact]
        public void SetSingleton()
        {
            // Register
            OmnIoCContainer.Set(_registrationType, _implementationType, Reuse.Singleton);

            // Resolve
            var test1 = (ITestClass) OmnIoCContainer.Get(_registrationType);
            var test2 = (ITestClass) OmnIoCContainer.Get(_registrationType);

            // Assert
            Assert.NotNull(test1);
            Assert.NotNull(test2);
            Assert.Equal(test1, test2);
        }

        [Fact]
        public void SetManyTransient()
        {
            // Register
            OmnIoCContainer.Set(_registrationType, _implementationType, name: "test1");
            OmnIoCContainer.Set(_registrationType, _implementationType, name: "test2");
            OmnIoCContainer.Set(_registrationType, _implementationType, name: "test3");

            // Resolve
            var test1 = (ITestClass) OmnIoCContainer.GetNamed(_registrationType, "test1");
            var test2 = (ITestClass) OmnIoCContainer.GetNamed(_registrationType, "test2");
            var test3 = (ITestClass) OmnIoCContainer.GetNamed(_registrationType, "test3");

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
            OmnIoCContainer.Set(_registrationType, _implementationType, name: "test1", reuse: Reuse.Singleton);
            OmnIoCContainer.Set(_registrationType, _implementationType, name: "test2", reuse: Reuse.Singleton);
            OmnIoCContainer.Set(_registrationType, _implementationType, name: "test3", reuse: Reuse.Singleton);

            // Resolve
            var test1 = (ITestClass) OmnIoCContainer.GetNamed(_registrationType, "test1");
            var test1_2 = (ITestClass) OmnIoCContainer.GetNamed(_registrationType, "test1");

            var test2 = (ITestClass) OmnIoCContainer.GetNamed(_registrationType, "test2");
            var test2_2 = (ITestClass) OmnIoCContainer.GetNamed(_registrationType, "test2");

            var test3 = (ITestClass) OmnIoCContainer.GetNamed(_registrationType, "test3");
            var test3_2 = (ITestClass) OmnIoCContainer.GetNamed(_registrationType, "test3");

            var all = OmnIoCContainer<ITestClass>.All();

            // Assert
            Assert.NotNull(test1);
            Assert.Equal(test1, test1_2);

            Assert.NotNull(test2);
            Assert.Equal(test2, test2_2);

            Assert.NotNull(test3);
            Assert.Equal(test3, test3_2);

            Assert.Equal(all.Count(), 3);
        }

        [Fact]
        public void ResolveAll()
        {
            // Register
            SetTransient();
            SetManyTransient();

            // Resolve
            var all = OmnIoCContainer.All(_registrationType).Select(o => (ITestClass)o).ToList();

            // Assert
            Assert.Equal(all.Count(), 4);
            Assert.True(all.All(o => o != null));
        }
    }
}

// ReSharper restore InconsistentNaming