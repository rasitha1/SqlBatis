# IBatisNet.Standard
IBatisNet rev513437 refactored and migrated to .NET Standard

## Changes

1. Decoupled Dynamic Proxy by dynamically loading `ILazyFactory` implementation via a new `settings` 
attribute called `lazyFactoryType` and splitting `ProxyGeneratorFactory` and `CachedProxyGenerator` to
a separate project called IBatisNet.DynamicProxy. You only need this if you have any lazy-loaded results.
	1.1. Refactored to use latest version of `Castle.Core` package


## Building



## Test Setup
