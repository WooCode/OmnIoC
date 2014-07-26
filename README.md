[![Build status](https://ci.appveyor.com/api/projects/status/5xeeg6rp6sgas90o)](https://ci.appveyor.com/project/CodeDux/omniscience)

Omniscience 
===========

IOC'ish IOC.. 

```
OmnIOC.Default.Register(o => new TestClass1());
OmnIOC.Default.Register(o => new TestClass2(o.Resolve<TestClass1>()));
var testClass = OmnIOC.Default.Resolve<TestClass2>();
```

Readme coming soon ™

