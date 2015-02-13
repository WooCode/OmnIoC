OmnIoC [![Build status](https://ci.appveyor.com/api/projects/status/bf0uai6rj4octbbn/branch/develop)](https://ci.appveyor.com/project/WooCode/OmnIoC/branch/develop) [![NuGet](https://img.shields.io/nuget/v/OmnIoC.Portable.svg?style=flat-square)]()
===
Portable Class Library Ioc that is **fast** and works in

1. .Net Framework 4+
2. Windows Store Apps
3. Windows Phone 8.1

**OmnIoC** is very much in development and should not be seen as production-ready

**Install**
```
PM> Install-Package OmnIoC.Portable
```

**Current features**

- Nongeneric register/resolve (set/get)
- Generic register/resolve (set/get) **fast**
- Named registrations/resolves
- Resolve multiple registrations of type as IEnumerable< T >

**Planned features**

- Attribute based registration/discovery
- Auto resolve constructor parameters

[Example code](https://github.com/WooCode/OmnIoC/wiki)

Performance comparison
=====
![Imgur](http://i.imgur.com/IcEmlU2.png)
The values are from using [this](https://github.com/danielpalme/IocPerformance) performance test with the latest versions of all Ioc's and added adapter for OmnIoC.
Also modified adapters to use generics where possible since OmnIoC is generic in it's core and not be unfair to other Iocs.

**Here are all comparisons / stats including Iocs that didn't reach low enough values to be included in the graph**

**First value**: Time of single-threaded execution in [ms]  
**Second value**: Time of multi-threaded execution in [ms] 

**_*_**: Benchmark was stopped after 3 minutes and result is extrapolated.

Lower is better and **bold values indicate best in test**

|**Container**|**Singleton**|**Transient**|**Combined**|**Complex**|
|:------------|------------:|------------:|-----------:|----------:|
|**OmnIoC 0.0.18**|**29**<br/>**37**|**40**<br/>**56**|**54**<br/>**80**|**102**<br/>**82**|
|**[Caliburn.Micro 1.5.2](https://github.com/Caliburn-Micro/Caliburn.Micro)**|453<br/>312|549<br/>330|1508<br/>881|6460<br/>3927|
|**[Catel 3.9.0](http://www.catelproject.com)**|375<br/>385|3620<br/>4131|11233<br/>10056|24933<br/>28175|
|**[DryIoc 1.3.1](https://bitbucket.org/dadhi/dryioc)**|61<br/>56|70<br/>70|84<br/>95|109<br/>92|
|**[Dynamo 3.0.2.0](http://www.dynamoioc.com)**|113<br/>91|116<br/>93|221<br/>153|586<br/>368|
|**[fFastInjector 0.8.1](https://ffastinjector.codeplex.com)**|96<br/>76|152<br/>99|191<br/>139|270<br/>173|
|**[Funq 1.0.0.0](https://funq.codeplex.com)**|132<br/>95|169<br/>135|398<br/>276|1046<br/>599|
|**[Grace 2.3.10](https://github.com/ipjohnson/Grace)**|201<br/>138|338<br/>246|779<br/>462|1830<br/>1368|
|**[Griffin 1.1.2](https://github.com/jgauffin/griffin.container)**|289<br/>192|299<br/>208|691<br/>433|1834<br/>1067|
|**[HaveBox 2.0.0](https://bitbucket.org/Have/havebox)**|69<br/>70|77<br/>73|105<br/>94|136<br/>110|
|**[Hiro 1.0.4.41795](https://github.com/philiplaureano/Hiro)**|164<br/>112|162<br/>113|194<br/>140|245<br/>166|
|**[IfInjector 0.8.1](https://github.com/iamahern/IfInjector)**|98<br/>91|146<br/>113|173<br/>140|222<br/>155|
|**[LightInject 3.0.1.7](https://github.com/seesharper/LightInject)**|60<br/>55|70<br/>63|83<br/>93|104<br/>92|
|**[LinFu 2.3.0.41559](https://github.com/philiplaureano/LinFu)**|3134<br/>1771|17181<br/>12464|44774<br/>31226|118910<br/>76547|
|**[Maestro 1.4.0](https://github.com/JonasSamuelsson/Maestro)**|282<br/>205|329<br/>241|961<br/>624|2806<br/>1591|
|**[Mef 4.0.0.0](https://mef.codeplex.com)**|22914<br/>12557|37098<br/>22722|56772<br/>39858|113138<br/>124159|
|**[Mef2 1.0.27.0](https://blogs.msdn.com/b/bclteam/p/composition.aspx)**|229<br/>150|240<br/>167|311<br/>214|526<br/>323|
|**[MicroSliver 2.1.6.0](https://microsliver.codeplex.com)**|191<br/>219|800<br/>537|2455<br/>1421|6945<br/>4539|
|**[Mugen 3.5.1](http://mugeninjection.codeplex.com)**|439<br/>429|705<br/>567|1974<br/>1531|7592<br/>5958|
|**[Munq 3.1.6](http://munq.codeplex.com)**|108<br/>89|158<br/>113|503<br/>299|1620<br/>875|
|**[Ninject 3.2.2.0](http://ninject.org)**|4545<br/>2952|14408<br/>11068|38996<br/>27247|128918<br/>87802|
|**[Petite 0.3.2](https://github.com/andlju/Petite)**|4770<br/>2864|3382<br/>2175|4299<br/>2871|5222<br/>3430|
|**[SimpleInjector 2.5.2](https://simpleinjector.org)**|85<br/>73|115<br/>86|130<br/>110|192<br/>147|
|**[Spring.NET 1.3.2](http://www.springframework.net/)**|934<br/>722|12087<br/>7253|34341<br/>20719|83878<br/>50552|
|**[StructureMap 3.1.0.133](http://structuremap.net/structuremap)**|2425<br/>2820|2102<br/>1992|5998<br/>5945|14671<br/>14558|
|**[StyleMVVM 3.1.5](https://stylemvvm.codeplex.com)**|563<br/>329|610<br/>369|860<br/>520|1497<br/>956|
|**[TinyIoC 1.2](https://github.com/grumpydev/TinyIoC)**|390<br/>332|1794<br/>1124|7584<br/>4914|30510<br/>18941|
|**[Unity 3.5.1404.0](http://msdn.microsoft.com/unity)**|2273<br/>1399|3475<br/>2160|9667<br/>5899|28905<br/>18180|
|**[Windsor 3.3.0](http://castleproject.org)**|530<br/>336|1765<br/>1052|5512<br/>3627|16869<br/>10077|

More readme coming soon ™
