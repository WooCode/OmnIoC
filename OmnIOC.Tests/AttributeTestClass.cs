using OmnIoc.Portable;

namespace OmnIoc.Tests
{
    [OmnIoc(Name = "testOne", Reuse = IocReuse.Multiple)]
    [OmnIoc(Name = "testTwo", Reuse = IocReuse.Singleton)]
    public class AttributeTestClass
    {
        public string Name { get; set; }

        [OmnIocConstructor(Name = "test")]
        public AttributeTestClass(string name)
        {
            Name = name;
        }

        [OmnIocConstructor(Name = "test2")]
        public AttributeTestClass()
        {
            Name = "XXX";
        }
    }
}