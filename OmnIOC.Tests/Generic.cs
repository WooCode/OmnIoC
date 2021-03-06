﻿using System;
using System.Collections.Generic;
using System.Linq;
using OmnIoC.Portable;
using Xunit;

namespace OmnIoC.Tests
{
    public class Generic
    {
        public Generic()
        {
            OmnIoCContainer.Clear();
        }

        [Fact]
        public void RemoveShouldRemoveRegistrated()
        {
            OmnIoCContainer<int>.DefaultValueFactory = () => 33;
            // Register
            OmnIoCContainer<int>.Set(10, "Test");

            // Act
            OmnIoCContainer<int>.Remove("Test");

            // Assert
            Assert.Equal(33,OmnIoCContainer<int>.GetNamed("Test"));
        }

        [Fact]
        public void ResolveWithoutRegistration()
        {
            var testClass = OmnIoCContainer<TestClass>.Get();
            var testValueType = OmnIoCContainer<int>.Get();
            // Assert
            Assert.Null(testClass);
            Assert.Equal(0,testValueType);
        }

        [Fact]
        public void LoadByAttributes()
        {
            // Register all types decorated with OmnIoCAttribute
            OmnIoCContainer.Load(AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()));

            // Resolve
            var unnamedTest = OmnIoCContainer<IAttributeTestClass>.Get();
            var test1 = OmnIoCContainer<IAttributeTestClass>.GetNamed("multiple");
            var test2 = OmnIoCContainer<IAttributeTestClass>.GetNamed("multiple");
            var test3 = OmnIoCContainer<AttributeTestClass>.GetNamed("singleton");
            var test4 = OmnIoCContainer<AttributeTestClass>.GetNamed("singleton");

            // Assert
            Assert.NotNull(unnamedTest);
            Assert.NotNull(test1);
            Assert.NotNull(test2);
            Assert.NotNull(test3);

            Assert.NotEqual(unnamedTest,test1);
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
            OmnIoCContainer<TestClass1>.Set(() => new TestClass1());
            OmnIoCContainer<TestClass2>.Set(() => new TestClass2(OmnIoCContainer<TestClass1>.Get()));

            // Resolve
            var testClass = OmnIoCContainer<TestClass2>.Get();

            // Assert
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void SetManyTransient()
        {
            // Register
            OmnIoCContainer<TestClass1>.Set(() => new TestClass1(), "1a");
            OmnIoCContainer<TestClass2>.Set(() => new TestClass2(OmnIoCContainer<TestClass1>.GetNamed("1A")), "2X");

            // Resolve
            var testClass = OmnIoCContainer<TestClass2>.GetNamed("2x");

            // Assert
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void SetSingleton()
        {
            // Register
            OmnIoCContainer<TestClass1>.Set(new TestClass1());
            OmnIoCContainer<TestClass2>.Set(new TestClass2(OmnIoCContainer<TestClass1>.Get()));

            // Resolve
            var testClass = OmnIoCContainer<TestClass2>.Get();

            // Assert
            Assert.NotNull(testClass);
            Assert.NotNull(testClass.Inner);
        }

        [Fact]
        public void SetManySingleton()
        {
            // Register
            OmnIoCContainer<TestClass1>.Set(new TestClass1(), "1a");
            OmnIoCContainer<TestClass1>.Set(new TestClass1(), "2a");
            OmnIoCContainer<TestClass2>.Set(new TestClass2(OmnIoCContainer<TestClass1>.GetNamed("1a")), "1b");
            OmnIoCContainer<TestClass2>.Set(new TestClass2(OmnIoCContainer<TestClass1>.GetNamed("2a")), "2b");

            // Resolve
            var testClass1 = OmnIoCContainer<TestClass2>.GetNamed("1b");
            var testClass2 = OmnIoCContainer<TestClass2>.GetNamed("2b");

            var testClass1_2 = OmnIoCContainer<TestClass2>.GetNamed("1b");
            var testClass2_2 = OmnIoCContainer<TestClass2>.GetNamed("2b");

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
            var all = OmnIoCContainer<TestClass2>.All.ToList();

            // Assert
            Assert.Equal(1, all.Count());
            Assert.True(all.All(t => t.Inner != null));
        }

        [Fact]
        public void ResolveAllNames()
        {
            // Register
            OmnIoCContainer<TestClass1>.Set(() => new TestClass1()); // this one should not be returned
            OmnIoCContainer<TestClass1>.Set(() => new TestClass1(), "first");
            OmnIoCContainer<TestClass1>.Set(() => new TestClass1(), "second");

            // Resolve
            var all = OmnIoCContainer<TestClass1>.Names.ToList();

            // Assert
            Assert.Equal(2, all.Count());
            Assert.Contains("first",all);
            Assert.Contains("second", all);

        }
    }
}