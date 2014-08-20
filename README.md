[![Build status](https://ci.appveyor.com/api/projects/status/99akspvgoqjt1n43/branch/develop)](https://ci.appveyor.com/project/WooCode/omniscience/branch/develop)
## About
---
IOC'ish IOC.. 

Simple example
```
OmnIOC.Default.Register(o => new TestClass1());
OmnIOC.Default.Register(o => new TestClass2(o.Resolve<TestClass1>()));
var testClass = OmnIOC.Default.Resolve<TestClass2>();
```

Named registrations
```
OmnIOC.Default.RegisterNamed(o => new TestClass1(), "1");
OmnIOC.Default.RegisterNamed(o => new TestClass2(o.Resolve<TestClass1>("1")), "2");
var testClass = OmnIOC.Default.Resolve<TestClass2>("2");
```

Readme coming soon ™
