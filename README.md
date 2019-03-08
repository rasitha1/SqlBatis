# IBatisNet.Standard
A fork of the Apache IBatisNet distribution which has been refactored and migrated to .NET Standard, as shown in the change log.

## Releases

### 2.1.0
* Merged changes from rev 709676 which picked up a number fixes 
* Including `IBatisNet.Common.Logging.Log4Net` .Net Framework assembly as well. 
* Getting rid of rest of the .net framework version.

### 2.0.0
* Migrated rev 513437 if IBatisNet to .Net Standard
* Decoupled Dynamic Proxy by dynamically loading `ILazyFactory` implementation via a new `settings` 
attribute called `lazyFactoryType` and splitting `ProxyGeneratorFactory` and `CachedProxyGenerator` to
a separate project called IBatisNet.DynamicProxy. You only need this if you have any lazy-loaded results. Refactored to use latest version of `Castle.Core` package
* Removed Transactions logic from `System.EnterpriseServices`
* Introduced `AsyncLocalSessionStore` and removed all other `ISessionStore` implementations (DataMapper & DataAccess)
* Removed auto instantiating of `ILoggerFactoryAdapter` using `ConfigurationManager` and defauled to `NoOpLoggerFA`. You must set `LogManager.Adapter` in your startup
* Updating assembly versions and package versions to 2.0 (original .net framework version was 1.6.2)

## Building



## Test Setup

1. Requires a SqlServer instance (Express works) 
2. Run DBCreation.sql and DataBase.sql to setup the database
3. SqlServer tests run fine. (Oracle, MySql, PostgreSQL ignored)
