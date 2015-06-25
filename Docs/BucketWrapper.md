# BucketWrapper #
* **CLSID**: 249D4D5A-218D-421A-823F-99DB30AF97CC
* **Interface UUID**: C12A287D-5726-4E06-AAAF-F20F3E03C6F6
* **Interface Type**: Dual

Wraps the bucket classes from .NET SDK and expose them with COM interface. Instances of this object can not be created directly by user. They are created by COM SDK as a result of *GetBucket* method on [BucketFactory](BucketFactory.md).

All methods using *expiration* parameter will interpret value 0 as no expiration. Expiration values between 1 and 2592000 are interpreted as number of seconds the value has to live. Expirations over 2592000 (the amount of seconds in 30 days) are interpreted as a UNIX timestamp of the date at which the value expires. See Couchbase documentation section [about expiration](http://docs.couchbase.com/couchbase-devguide-2.5/#about-document-expiration).

````
IBucketWrapper {
        [id(0x00000001)]
        VARIANT_BOOL Exists([in] BSTR key);
        [id(0x00000002)]
        IOperationResultWrapper* Get([in] BSTR key);
        [id(0x00000003)]
        IOperationResultWrapper* GetAndTouch(
                        [in] BSTR key, 
                        [in] unsigned long expiration);
        [id(0x00000004)]
        IOperationResultWrapper* Touch(
                        [in] BSTR key, 
                        [in] unsigned long expiration);
        [id(0x00000005)]
        IOperationResultWrapper* Insert(
                        [in] BSTR key, 
                        [in] VARIANT value, 
                        [in, optional, defaultvalue(0)] unsigned long expiration);
        [id(0x00000006)]
        IOperationResultWrapper* Remove([in] BSTR key);
        [id(0x00000007)]
        IOperationResultWrapper* RemoveCas(
                        [in] BSTR key, 
                        [in] uint64 cas);
        [id(0x00000008)]
        IOperationResultWrapper* Replace(
                        [in] BSTR key, 
                        [in] VARIANT value, 
                        [in, optional, defaultvalue(0)] unsigned long expiration);
        [id(0x00000009)]
        IOperationResultWrapper* ReplaceCas(
                        [in] BSTR key, 
                        [in] VARIANT value, 
                        [in] uint64 cas, 
                        [in, optional, defaultvalue(0)] unsigned long expiration);
        [id(0x0000000a)]
        IOperationResultWrapper* Upsert(
                        [in] BSTR key, 
                        [in] VARIANT value, 
                        [in, optional, defaultvalue(0)] unsigned long expiration);
        [id(0x0000000b)]
        IOperationResultWrapper* UpsertCas(
                        [in] BSTR key, 
                        [in] VARIANT value, 
                        [in] uint64 cas, 
                        [in, optional, defaultvalue(0)] unsigned long expiration);
        [id(0x0000000c)]
        IOperationResultWrapper* Append(
                        [in] BSTR key, 
                        [in] BSTR value);
        [id(0x0000000d)]
        IOperationResultWrapper* Prepend(
                        [in] BSTR key, 
                        [in] BSTR value);
        [id(0x0000000e)]
        IOperationResultWrapper* Increment(
                        [in] BSTR key, 
                        [in, optional, defaultvalue(1)] uint64 delta, 
                        [in, optional, defaultvalue(1)] uint64 initial, 
                        [in, optional, defaultvalue(0)] unsigned long expiration);
        [id(0x0000000f)]
        IOperationResultWrapper* Decrement(
                        [in] BSTR key, 
                        [in, optional, defaultvalue(1)] uint64 delta, 
                        [in, optional, defaultvalue(1)] uint64 initial, 
                        [in, optional, defaultvalue(0)] unsigned long expiration);
}
````


### Exists ###
Checks for the existance of a given key.

##### params: #####
* **key**: Key to check.

##### return: #####
*True* if the key exists; *False* otherwise.


### Get ###
Gets value for a given key.

##### params: #####
* **key**: The key to use as a lookup.

##### return: #####
A pointer to [operation result wrapper COM object](OperationResultWrapper.md) represented as [IOperationResultWrapper](OperationResultWrapper.md) dual COM interface.


### GetAndTouch ###
Retrieves a value by key and additionally updates the expiry with a new value.

##### params: #####
* **key**: The key to use as a lookup.
* **expiration**: New time-to-live (ttl) for the key in seconds.

##### return: #####
A pointer to [operation result wrapper COM object](OperationResultWrapper.md) represented as [IOperationResultWrapper](OperationResultWrapper.md) dual COM interface.


### Touch ###
Updates the expiration of a key without modifying or returning it's value.

##### params: #####
* **key**: The key to lookup for expiration update.
* **expiration**: New time-to-live (ttl) for the key in seconds.

##### return: #####
A pointer to [operation result wrapper COM object](OperationResultWrapper.md) represented as [IOperationResultWrapper](OperationResultWrapper.md) dual COM interface.


### Insert ###
Inserts a value into Couchbase for a given key, failing if it exists.

##### params: #####
* **key**: The unique key for indexing.
* **value**: The value for the key.
* **expiration**: Time-to-live (ttl) for the key in seconds. This parameter is optional. Default value is 0 (no expiration).

##### return: #####
A pointer to [operation result wrapper COM object](OperationResultWrapper.md) represented as [IOperationResultWrapper](OperationResultWrapper.md) dual COM interface.


### Remove ###
Removes a stored value for a given key from Couchbase.

##### params: #####
* **key**: The key to remove from Couchbase

##### return: #####
A pointer to [operation result wrapper COM object](OperationResultWrapper.md) represented as [IOperationResultWrapper](OperationResultWrapper.md) dual COM interface.