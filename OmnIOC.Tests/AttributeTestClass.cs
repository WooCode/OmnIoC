using OmnIoC.Portable;

namespace OmnIoC.Tests
{
    [OmnIoC(Name = "testOne", Reuse = Reuse.Multiple)]
    [OmnIoC(Name = "testTwo", Reuse = Reuse.Singleton)]
    public class AttributeTestClass
    {
        public string Name { get; set; }

        [OmnIoCConstructor(Name = "test")]
        public AttributeTestClass(string name)
        {
            Name = name;
        }

        [OmnIoCConstructor(Name = "test2")]
        public AttributeTestClass()
        {
            Name = "XXX";
        }
    }
}