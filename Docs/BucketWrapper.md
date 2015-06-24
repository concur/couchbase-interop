# BucketWrapper #
* **CLSID**: 249D4D5A-218D-421A-823F-99DB30AF97CC
* **Interface UUID**: C12A287D-5726-4E06-AAAF-F20F3E03C6F6
* **Interface Type**: Dual

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
