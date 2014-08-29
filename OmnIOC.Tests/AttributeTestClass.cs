using OmnIoC.Portable;

namespace OmnIoC.Tests
{
    // Export as unnamed IAttributeTestClass that is resolved with OmnIoCContainer<IAttributeTestClass>.Get()
    [OmnIoCExport(Reuse = Reuse.Multiple, RegistrationType = typeof(IAttributeTestClass))]
    // Export as named IAttributeTestClass that is resolved with OmnIoCContainer<IAttributeTestClass>.Get("multiple")
    [OmnIoCExport(Name = "multiple", Reuse = Reuse.Multiple, RegistrationType = typeof(IAttributeTestClass))]
    // Export as named singleton AttributeTestClass that is resolved with OmnIoCContainer<IAttributeTestClass>.Get("singleton")
    [OmnIoCExport(Name = "singleton", Reuse = Reuse.Singleton, RegistrationType = typeof(AttributeTestClass))]
    public class AttributeTestClass : IAttributeTestClass
    {
        public string Name { get; set; }

        public AttributeTestClass(string name)
        {
            Name = name;
        }

        public AttributeTestClass()
        {
            Name = "XXX";
        }
    }

    public interface IAttributeTestClass
    {
        string Name { get; set; }
    }
}