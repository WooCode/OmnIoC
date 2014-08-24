## OmnIoc [![Build status](https://ci.appveyor.com/api/projects/status/99akspvgoqjt1n43/branch/develop)](https://ci.appveyor.com/project/WooCode/omniscience/branch/develop)

Portable Class Library Ioc that is **fast** and works in

1. .Net Framework 4+
2. Windows Store Apps
3. Windows Phone 8.1

**Reflection or Expression Trees isn't used in OmnIoc**

![Imgur](http://i.imgur.com/86uFn9A.png)
[Here](https://gist.github.com/CodeDux/f728e20095a9b6e57aa0) you can find more statistics (inluding Iocs that didn't reach the stats needed for the graph)
###Examples

```csharp
// Singleton with empty constructor
OmnIoc<ISingleton>.Set<Singleton>(OmnIoc.Reuse.Singleton);

// Transient with empty constructor
OmnIoc<ISingleton>.Set<Singleton>(OmnIoc.Reuse.Multiple);

// Register multiple singletons with constructor under different names
OmnIoc<ISimpleAdapter>.Set(new SimpleAdapterOne(1), "1");
OmnIoc<ISimpleAdapter>.Set(new SimpleAdapterTwo(2), "2");

// Register multiple transient with constructor under different names
OmnIoc<ISimpleAdapter>.Set(() => new SimpleAdapterThree(3), "3");
OmnIoc<ISimpleAdapter>.Set(() => new SimpleAdapterFour(4), "4");

// Transient with constructor parameters
OmnIoc<ICombined>.Set(() => new Combined( 
                                          OmnIoc<ISingleton>.Get(),
                                          OmnIoc<ITransient>.Get()
                                          ));
                                          
// Transient with IEnumerable<T> constructor parameter
OmnIoc.Set(() => new ImportMultiple(OmnIoc<ISimpleAdapter>.All()));

// Transient with properties
OmnIoc.Set<ISubObject>(() => new SubObject { Service = OmnIoc<IService>.Get() });

// Resolve
var combined = OmnIoc<ICombined>.Get();

// Resolve named
var namedCombined = OmnIoc<ISimpleAdapter>.Named("1");

// Resolve all as IEnumerable<T>
var allCombined = OmnIoc<ISimpleAdapter>.All();
```

More readme coming soon ™