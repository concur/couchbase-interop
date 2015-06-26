# BucketWrapper #
* **CLSID**: 249D4D5A-218D-421A-823F-99DB30AF97CC
* **Interface UUID**: C12A287D-5726-4E06-AAAF-F20F3E03C6F6
* **Interface Type**: Dual

Wraps the bucket classes from .NET SDK and expose them with COM interface. Instances of this object can not be created directly by user. They are created by COM SDK as a result of *GetBucket* method on [BucketFactory](BucketFactory.md).

All methods using *expiration* parameter will interpret value 0 as no expiration. Expiration values between 1 and 2592000 are interpreted as number of seconds the value has to live. Expirations over 2592000 (the amount of seconds in 30 days) are interpreted as a UNIX timestamp of the date at which the value expires. See Couchbase documentation section [about expiration](http://docs.couchbase.com/couchbase-devguide-2.5/#about-document-expiration).

```chapel
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
```


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


### RemoveCas ###
Removes a stored value for a given key from Couchbase, if the *cas* value in Couchbase matches supplied value.

##### params: #####
* **key**: The key to remove from Couchbase
* **cas**: The CAS (Check and Set) value for optimistic concurrency.

##### return: #####
A pointer to [operation result wrapper COM object](OperationResultWrapper.md) represented as [IOperationResultWrapper](OperationResultWrapper.md) dual COM interface.


### Replace ###
Replaces a stored value for a given key if it exists, otherwise fails. 

##### params: #####
* **key**: The unique key for indexing.
* **value**: The value for the key.
* **expiration**: The time-to-live (ttl) for the key in seconds. This parameter is optional. Default value is 0 (no expiration).

##### return: #####
A pointer to [operation result wrapper COM object](OperationResultWrapper.md) represented as [IOperationResultWrapper](OperationResultWrapper.md) dual COM interface.


### ReplaceCas ###
Replaces a stored value for a given key if it exists and it's *cas* matches the suplied value, otherwise fails.

##### params: #####
* **key**: The unique key for indexing.
* **value**: The value for the key.
* **cas**: The CAS (Check and Set) value for optimistic concurrency.
* **expiration**: The time-to-live (ttl) for the key in seconds. This parameter is optional. Default value is 0 (no expiration).

##### return: #####
A pointer to [operation result wrapper COM object](OperationResultWrapper.md) represented as [IOperationResultWrapper](OperationResultWrapper.md) dual COM interface.


### Upsert ###
Inserts or replaces an existing stored value into Couchbase Server.

##### params: #####
* **key**: The unique key for indexing.
* **value**: The value for the key. 
* **expiration**: The time-to-live (ttl) for the key in seconds. This parameter is optional. Default value is 0 (no expiration). 

##### return: #####
A pointer to [operation result wrapper COM object](OperationResultWrapper.md) represented as [IOperationResultWrapper](OperationResultWrapper.md) dual COM interface.


### UpsertCas ###
Inserts or replaces an existing stored value into Couchbase Server, if the *cas* value in Couchbase matches supplied value.
##### params: #####
* **key**: The unique key for indexing.
* **value**: The value for the key.
* **cas**: The CAS (Check and Set) value for optimistic concurrency.
* **expiration**: The time-to-live (ttl) for the key in seconds. This parameter is optional. Default value is 0 (no expiration). 

##### return: #####
A pointer to [operation result wrapper COM object](OperationResultWrapper.md) represented as [IOperationResultWrapper](OperationResultWrapper.md) dual COM interface.


### Append ###
Appends a value to a give key. 

##### params: #####
* **key**: The key to append too.
* **value**: The value to append to the key.

##### return: #####
A pointer to [operation result wrapper COM object](OperationResultWrapper.md) represented as [IOperationResultWrapper](OperationResultWrapper.md) dual COM interface.


### Prepend ###
Prepends a value to a give key. 

##### params: #####
* **key**: The key to Prepend too.
* **value**: The value to prepend to the key.

##### return: #####
A pointer to [operation result wrapper COM object](OperationResultWrapper.md) represented as [IOperationResultWrapper](OperationResultWrapper.md) dual COM interface.


### Increment ###
Increments the value of a key by the *delta*. If the key doesn't exist, it will be created and seeded with the *initial* value.

##### params: #####
* **key**: The key to us for the counter.
* **delta**: The number to increment the key by. This parameter is optional. Default value is 1.
* **initial**: The initial value to use. If the key doesn't exist, this value will be returned. This parameter is optional. Default value is 1.
* **expiration**: The time-to-live (ttl) for the key in seconds. This parameter is optional. Default value is 0 (no expiration).

##### return: #####
A pointer to [operation result wrapper COM object](OperationResultWrapper.md) represented as [IOperationResultWrapper](OperationResultWrapper.md) dual COM interface.


### Decrement ###
Decrements the value of a key by the *delta*. If the key doesn't exist, it will be created and seeded with the *initial* value. 

##### params: #####
* **key**: The key to us for the counter.
* **delta**: The number to decrement the key by. This parameter is optional. Default value is 1.
* **initial**: The initial value to use. If the key doesn't exist, this value will be returned. This parameter is optional. Default value is 1.
* **expiration**: The time-to-live (ttl) for the key in seconds. This parameter is optional. Default value is 0 (no expiration).

##### return: #####
A pointer to [operation result wrapper COM object](OperationResultWrapper.md) represented as [IOperationResultWrapper](OperationResultWrapper.md) dual COM interface.
