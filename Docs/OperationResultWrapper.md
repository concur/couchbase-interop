# OperationResultWrapper #
* **CLSID**: 7128DCB8-FC58-4A47-AE4D-92D40EDB37D5
* **Interface UUID**: F4619C4B-3930-4DE7-92AD-5EC0BD1CC017
* **Interface Type**: Dual

Wraps the operation result classes from .NET SDK and expose them with COM interface. Instances of this object can not be created directly by user. They are created by COM SDK as a result of [BucketWrapper](BucketWrapper.md) object methods.

````
IOperationResultWrapper {
        [id(00000000), propget]
        VARIANT Value();
        [id(0x00000001), propget]
        VARIANT_BOOL Success();
        [id(0x00000002), propget]
        long Status();
        [id(0x00000003), propget]
        BSTR Message();
        [id(0x00000004), propget]
        BSTR ExceptionData();
        [id(0x00000005), propget]
        uint64 Cas();
        [id(0x00000006)]
        VARIANT_BOOL ShouldRetry();
}
````


### Value ###
Value associated with executed operation. In case that the executed operation did not have any value associated with is (for example Remove), the value of this property will be uninitialized.

##### return: #####
*Variant* containing the Couchbase operation value. I.E. value stored in couchbase when Get operation is executed.


### Success ###
Indicates whether the opration was successful.

##### return: #####
*True* if the opertation was successful; *False* otherwise.


### Status ###
Detailed operation status code. This may indicate what exactly failed if operation was not successful. For more information please refer to the [.NET SDK Response Status documentation](http://docs.couchbase.com/sdk-api/couchbase-net-client-2.1.0/?topic=html/6a25fcd7-8827-5150-a120-7219769e482b.htm).

##### return: #####
Numeric representation of Couchbase result code.


### Message ###
If the operation was not successful, the property getter contains a message with short explanation.

##### return: #####
Reason for operation failure.


### ExceptionData ###
If there was an exception associated with this operation result in .NET SDK, the full exception data will be available in this property getter.

##### return: #####
Text containing exception description and stack trace, including any inner exceptions.


### Cas ###
Get the "Check and Set" number for optimistic locking. This number can be used for *RemoveCas*, *ReplaceCas* and *UpsertCas* methods on [bucket wrapper](BucketWrapper.md).

##### return: #####
The numerical "Check and Set" value for Couchbase optimistic locking.


### ShouldRetry ###
Indicate whether the operation failed on an intermittent error and should be repeated immediattely. Intermittent errors like these may appear during Couchbase cluster rebalance or failover.

##### return: #####
*True* if the opertation should be retried immediatelly; *False* otherwise.
