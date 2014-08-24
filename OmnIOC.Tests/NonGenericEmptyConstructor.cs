using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OmnIOC.Portable;
using Xunit;
// ReSharper disable InconsistentNaming
namespace OmnIOC.Tests
{
    public class NonGenericEmptyConstructor
    {
        private readonly Type _type = typeof (TestClass1);

        public NonGenericEmptyConstructor()
        {
            OmnIoc.Clear();
        }

        [Fact]
        public void SetTransient()
        {
            // Register
            OmnIoc.Set(_type, _type);

            // Resolve
            var test1 = OmnIoc<TestClass1>.Get();
            var test2 = OmnIoc<TestClass1>.Get();

            // Assert
            Assert.NotNull(test1);
            Assert.NotNull(test2);
            Assert.NotEqual(test1,test2);
        }

        [Fact]
        public void SetSingleton()
        {
            // Register
            OmnIoc.Set(_type, _type, OmnIoc.Reuse.Singleton);

            // Resolve
            var test1 = OmnIoc<TestClass1>.Get();
            var test2 = OmnIoc<TestClass1>.Get();

            // Assert
            Assert.NotNull(test1);
            Assert.NotNull(test2);
            Assert.Equal(test1, test2);
        }

        [Fact]
        public void SetManyTransient()
        {
            // Register
            OmnIoc.Set(_type, _type,name:"test1");
            OmnIoc.Set(_type, _type, name: "test2");
            OmnIoc.Set(_type, _type, name: "test3");

            // Resolve
            var test1 = OmnIoc<TestClass1>.GetNamed("test1");
            var test2 = OmnIoc<TestClass1>.GetNamed("test2");
            var test3 = OmnIoc<TestClass1>.GetNamed("test3");

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
            OmnIoc.Set(_type, _type, name: "test1", reuse: OmnIoc.Reuse.Singleton);
            OmnIoc.Set(_type, _type, name: "test2", reuse: OmnIoc.Reuse.Singleton);
            OmnIoc.Set(_type, _type, name: "test3", reuse: OmnIoc.Reuse.Singleton);

            // Resolve
            var test1 = OmnIoc<TestClass1>.GetNamed("test1");
            var test1_2 = OmnIoc<TestClass1>.GetNamed("test1");

            var test2 = OmnIoc<TestClass1>.GetNamed("test2");
            var test2_2 = OmnIoc<TestClass1>.GetNamed("test2");

            var test3 = OmnIoc<TestClass1>.GetNamed("test3");
            var test3_2 = OmnIoc<TestClass1>.GetNamed("test3");

            var all = OmnIoc<TestClass1>.All();

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
