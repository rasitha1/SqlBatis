# Releases

## 4.0.0
* Removed **SqlBatis.DataAccess** project
* Removed **SqlBatis.Common.Logging.Log4Net** project
* Merged **SqlBatis.Common** info **SqlBatis.DataMapper** to be in a single assembly
* Azure DevOps pipelines



### 3.1.0
* Add support for registering named instances of ISqlMapper and getting them via ISqlMapperFactory

### 3.0.1
* Add support for registering and getting an ISqlMapper through DI


### 3.0.0
* Renamed from iBatisNet to SqlBatis
* Added NOTICE file
* BF: Correctly handle removal of sessions in `AsyncLocalSessionStore` 
* Updating NOTICE file to include original iBatisNet Notice.txt text
* Including LICENSE.txt and NOTICE.txt file in NuGet packages

___
Delist 2.x packages published to nuget.org under IBatisNet.*
Publishing new packages under SqlBatis name
___


### 2.1.0
* Merged changes from [rev 709676](http://archive.apache.org/dist/ibatis/source/ibatis.net/) which picked up a number fixes 
* Including `IBatisNet.Common.Logging.Log4Net` .Net Framework assembly as well. 
* Getting rid of rest of the .net framework version.

### 2.0.0
* Migrated [rev 513437](http://archive.apache.org/dist/ibatis/source/ibatis.net/) if IBatisNet to .Net Standard
* Decoupled Dynamic Proxy by dynamically loading `ILazyFactory` implementation via a new `settings` 
attribute called `lazyFactoryType` and splitting `ProxyGeneratorFactory` and `CachedProxyGenerator` to
a separate project called IBatisNet.DynamicProxy. You only need this if you have any lazy-loaded results. Refactored to use latest version of `Castle.Core` package
* Removed Transactions logic from `System.EnterpriseServices`
* Introduced `AsyncLocalSessionStore` and removed all other `ISessionStore` implementations (DataMapper & DataAccess)
* Removed auto instantiating of `ILoggerFactoryAdapter` using `ConfigurationManager` and defauled to `NoOpLoggerFA`. You must set `LogManager.Adapter` in your startup
* Updating assembly versions and package versions to 2.0 (original .net framework version was 1.6.2)
