Omniscience [![Build status](https://ci.appveyor.com/api/projects/status/5xeeg6rp6sgas90o)](https://ci.appveyor.com/project/CodeDux/omniscience)
===========

IOC'ish IOC.. 

Simple example
```
OmnIOC.Default.Register(o => new TestClass1());
OmnIOC.Default.Register(o => new TestClass2(o.Resolve<TestClass1>()));
var testClass = OmnIOC.Default.Resolve<TestClass2>();
```

Named registrations
```
OmnIOC.Default.RegisterNamed(new TestClass1(), "1");
OmnIOC.Default.RegisterNamed(new TestClass2(OmnIOC.Default.Resolve<TestClass1>("1")), "2");
var testClass = OmnIOC.Default.Resolve<TestClass2>("2");
```

Readme coming soon ™