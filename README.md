Couchbase COM SDK
=================
## Overview ##
The **couchbase-interop** is a project to wrap [Official Couchbase .NET SDK](https://github.com/couchbase/couchbase-net-client) with classes providing [COM](https://msdn.microsoft.com/en-us/library/ms680573.aspx) interface. The main goal is to make Couchbase available for [Classic ASP](https://msdn.microsoft.com/en-us/library/aa286483.aspx) applications. However any legacy application capable of consuming COM objects can benefit from this SDK.

Due to missing support for JSON format in legacy applications, the COM API does not offer full range of methods like typical Couchbase 2.0 SDK. Instead it's focusing on the basic Memcached operations which can be easily used by all legacy applications.

## Versioning ##
The first 3 digits (Major, Minior, Build) of Couchbase.ComClient.dll will always reflect the version of Couchbase.NetClient.dll it is based upon. The last digit (Revision) may be used if patch for COM SDK is required without update of the underlying .NET SDK.

## Dependencies ##
This project requires several [NuGet](https://www.nuget.org/) packages as dependencies. For a complete list please refer to [packages.config](Src/Couchbase.ComClient/packages.config). Because of the fundaments of COM - .NET Interoperability, there is no app.config file available to handle .NET assembly redirection. For this reason all shared dependencies must all sync on one exact version to be used through out the project.

For example if Couchbase.NetClient.dll references Common.Logging version 3.0.0, the Couchbase.ComClient can not reference Common.Logging version 3.1.0.

## Deployment ##
The easiest deployment model is to register Couchbase.ComClient and all its dependencies into [GAC](https://msdn.microsoft.com/en-us/library/yf1d93sz.aspx) through [Gacutil](https://msdn.microsoft.com/en-us/library/ex0ss12c.aspx) tool. After that you can register the COM interfaces with [Regasm](https://msdn.microsoft.com/en-us/library/tzat5yw6.aspx). This deployment model will ensure that the COM object assembly and its dependencies are always available and can be easily updated at any time.

Alternatively you can avoid usage of GAC if you use **/codebase** argument in Regasm. In this case make sure that the path you used to register COM components contains all required dependencies in their correct versions and that it's accessible by consuming applications.

Example script with GAC deployment: [com-register.cmd](Src/scripts/com-register.cmd)

## Usage Example ##
```vbs
dim logConfig, factory, bucket, result

set logConfig = CreateObject("Couchbase.ComClient.LogConfig")
logConfig.FileName = "example.log"
call logConfig.SetupLogging()
set logConfig = nothing

set factory = CreateObject("Couchbase.ComClient.BucketFactory")
call factory.ConfigureCluster("example.config", "local")
set bucket = factory.GetBucket("default")

set result = bucket.Upsert("testkey", "Hello World!", 60)
call WScript.StdOut.WriteLine("Upsert call success: " & result.Success)
set result = nothing

call factory.CloseBucket("default")
set bucket = nothing
factory.CloseCluster("local")
set factory = nothing
```
## COM API ##
* [LogConfig](Docs/LogConfig.md) - Configuration of underlying .NET SDK logging
* [BucketFactory](Docs/BucketFactory.md) - Management of Couchbase clusters and buckets
* [BucketWrapper](Docs/BucketWrapper.md) - Executing operations on Couchbase buckets
* [OperationResultWrapper](Docs/OperationResultWrapper.md) - Reading operation results
