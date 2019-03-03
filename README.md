# IBatisNet.Standard
IBatisNet rev513437 refactored and migrated to .NET Standard

## Changes

1. Decoupled Dynamic Proxy by dynamically loading `ILazyFactory` implementation via a new `settings` 
attribute called `lazyFactoryType` and splitting `ProxyGeneratorFactory` and `CachedProxyGenerator` to
a separate project called IBatisNet.DynamicProxy. You only need this if you have any lazy-loaded results. Refactored to use latest version of `Castle.Core` package

2. Removed Transactions logic from `System.EnterpriseServices`
3. Introduced `AsyncLocalSessionStore` and removed all other `ISessionStore` implementations (DataMapper & DataAccess)
4. Removed auto instantiating of `ILoggerFactoryAdapter` using `ConfigurationManager` and defauled to `NoOpLoggerFA`. You must set `LogManager.Adapter` in your startup

5. Updating assembly versions and package versions to 2.0 (original .net framework version was 1.6.2)

## Building



## Test Setup
