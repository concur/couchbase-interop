# OperationResultWrapper #
* **CLSID**: 7128DCB8-FC58-4A47-AE4D-92D40EDB37D5
* **Interface UUID**: F4619C4B-3930-4DE7-92AD-5EC0BD1CC017
* **Interface Type**: Dual

````
IOperationResultWrapper {
        [id(00000000), propget]
        VARIANT value();
        [id(0x00000001), propget]
        VARIANT_BOOL Success();
        [id(0x00000002), propget]
        long Status();
        [id(0x00000003), propget]
        BSTR Message();
        [id(0x00000004), propget]
        BSTR ExceptionData();
        [id(0x00000005), propget]
        uint64 cas();
        [id(0x00000006)]
        VARIANT_BOOL ShouldRetry();
}
````
