using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Omniscience.Portable;
using Xunit;

namespace Omniscience.Tests
{
    public class RegisterResolveTests
    {
        public class TestClass1
        {
             
        }

        public class TestClass2
        {
            public TestClass2(TestClass1 inner)
            {
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(x => x.IsAssignableFrom(x) && x.IsClass).ToList();
                //.ForEach(t =>
                //{
                //    var name = t.Name.ToLower();
                //    if (name.EndsWith("handler"))
                //        name = name.Substring(0, name.Length - 7);

                //    TinyIoCContainer.Current.Register(type, t, name);
                //});
            }
        }

        [Fact]
        public void Register()
        {
            OmnIOC.Default.Register(o => new TestClass1());
            OmnIOC.Default.Register(o => new TestClass2(o.Resolve<TestClass1>()));
        }

        [Fact]
        public void RegisterNamed()
        {
            OmnIOC.Default.Register(o => new TestClass1(), "1");
            OmnIOC.Default.Register(o => new TestClass2(o.Resolve<TestClass1>()), "2");
        }

        [Fact]
        public void RegisterInstance()
        {
            OmnIOC.Default.Register(new TestClass1());
            OmnIOC.Default.Register(new TestClass2(OmnIOC.Default.Resolve<TestClass1>()));
        }

        [Fact]
        public void RegisterNamedInstance()
        {
            OmnIOC.Default.Register(new TestClass1(), "1");
            OmnIOC.Default.Register(new TestClass2(OmnIOC.Default.Resolve<TestClass1>()), "2");
        }

        [Fact]
        public void Resolve()
        {
            Register();
            var testClass = OmnIOC.Default.Resolve<TestClass2>();
        }

        [Fact]
        public void ResolveNamed()
        {
            Register();
            RegisterNamed();
            var testClass = OmnIOC.Default.Resolve<TestClass2>("2");
        }
    }
}
