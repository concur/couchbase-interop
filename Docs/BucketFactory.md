# BucketFactory #
* **CLSID**: FD25BAA3-3F34-4C97-9621-3C384E1F4591
* **ProgId**: Couchbase.ComClient.BucketFactory
* **Interface UUID**: 267DA2E9-727D-4D63-A9D6-D2391331A738
* **Interface Type**: Dual

COM factory class used to configure underlying .NET SDK, manage connections to Couchbase clusters and buckets. Multiple instances of this COM facotry class can be used to manage the same singleton cluster and bucket instances inside the COM SDK.

````
IBucketFactory {
        [id(0x00000001)]
        void ConfigureCluster(
                        [in] BSTR configPath, 
                        [in] BSTR sectionName, 
                        [in, optional, defaultvalue("")] BSTR clusterName);
        [id(0x00000002)]
        IBucketWrapper* GetBucket(
                        [in] BSTR bucketName, 
                        [in, optional, defaultvalue("")] BSTR clusterName);
        [id(0x00000003)]
        VARIANT_BOOL IsBucketOpen([in] BSTR bucketName);
        [id(0x00000004)]
        VARIANT_BOOL IsClusterOpen([in] BSTR clusterName);
        [id(0x00000005)]
        void CloseBucket([in] BSTR bucketName);
        [id(0x00000006)]
        void CloseCluster([in] BSTR clusterName);
}
````


### ConfigureCluster ###
Read config file section and configure the underlying .NET SDK. At the same time open singleton cluster instance inside the COM SDK with this configuration.

This is the first method you need to call before opening any bucket objects. The COM SDK is relying on the underlying .NET SDK capabilities to be configured by stand-alone .NET config files.

Lifetime of the singleton cluster instance inside the COM SDK is not bound to lifetime of this BucketFactory COM object. You can safely destroy the BucketFactory at any time.

You can safely work with multiple Couchbase clusters at the same time. Just make sure they use different clusterName. Configuring the same cluster again will result in disposal of the old singleton cluster object and creation of new one.

If you need to create the configuration programatically, you can programatically create a temporaty .config file through your application XML DOM model and then use it here.

For example config file please refer to [hello-world.config](../Src/scripts/hello-world.config)

For full configuration documentation please refer to [Couchbase .NET SDK documentation](http://docs.couchbase.com/developer/dotnet-2.1/configuring-the-client.html).

##### params: #####
* **configPath**: Path to the configuration file.
* **sectionName**: Name of the configuration section to use.
* **clusterName**: Internal name of the cluster in COM SDK. This parameter is optional. If empty, the sectionName parameter will be used as name of the cluster.


### GetBucket ###
Open a singleton bucket instance on configured cluster and return COM wrapper object. Lifetime of the singleton bucket instance inside COM SDK is not bound to lifetime of the returned wrapper object. You can safely destroy the returned wrapper at any time.

This method can be safely called multiple times to return multiple wrapper instances pointing to the same underlying singleton bucket instance. One or multiple bucket wrapper COM objects can be safely used in parallel on multiple threads. For the best performance, try to reuse the same wrapper object without calling GetBucket too often.

##### params: #####
* **bucketName**: Name of the Couchbase bucket to be open.
* **clusterName**: Name of the cluster hosting this bucket. If COM SDK is configured with a single cluster, this parameter can be omitted.

##### return: #####
A pointer to [bucket wrapper COM object](BucketWrapper.md) represented as [IBucketWrapper](BucketWrapper.md) dual COM interface.


### IsBucketOpen ###
Check whether a singleton bucket object with a specified name is already open inside the COM SDK.

##### params: #####
* **bucketName**: Name of the Couchbase bucket to check.

##### return: #####
*True* if underlying singleton bucket object is open; otherwise *False*.


### IsClusterOpen ###
Check whether a singleton cluster object with a specified name is already open inside the COM SDK.

##### params: #####
* **clusterName**: Name of the Couchbase cluster to check.

##### return: #####
*True* if underlying singleton cluster object is open; otherwise *False*.


### CloseBucket ###
Close the underlying singleton bucket object in COM SDK. All existing bucket wrapper COM objects based on this singleton instance will cease to work.

##### params: #####
* **bucketName**: Name of the Couchbase bucket to be closed.


### CloseCluster ###
Close the underlying singleton cluster object in COM SDK. This will also close all bucket objects associated with this cluster.

##### params: #####
* **clusterName**: Name of the Couchbase cluster to be closed.