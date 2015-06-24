# LogConfig #
* **CLSID**: 3E14F6C5-3E39-4C54-9E1A-6F4C0BCCEF39
* **ProgId**: Couchbase.ComClient.LogConfig
* **Interface UUID**: 46F8E1EC-9AA0-491D-8126-01C7454C549B
* **Interface Type**: Dual

````
ILogConfig {
        [id(0x00000001), propget]
        int64 ArchiveAboveSize();
        [id(0x00000001), propput]
        void ArchiveAboveSize([in] int64 rhs);
        [id(0x00000002), propget]
        BSTR ArchiveEvery();
        [id(0x00000002), propput]
        void ArchiveEvery([in] BSTR rhs);
        [id(0x00000003), propget]
        BSTR ArchiveNumbering();
        [id(0x00000003), propput]
        void ArchiveNumbering([in] BSTR rhs);
        [id(0x60020006), propget]
        VARIANT_BOOL DeleteOldFileOnStartup();
        [id(0x60020006), propput]
        void DeleteOldFileOnStartup([in] VARIANT_BOOL rhs);
        [id(0x60020008), propget]
        VARIANT_BOOL EnableFileDelete();
        [id(0x60020008), propput]
        void EnableFileDelete([in] VARIANT_BOOL rhs);
        [id(0x00000006), propget]
        BSTR Encoding();
        [id(0x00000006), propput]
        void Encoding([in] BSTR rhs);
        [id(0x00000007), propget]
        BSTR FileName();
        [id(0x00000007), propput]
        void FileName([in] BSTR rhs);
        [id(0x00000008), propget]
        BSTR Footer();
        [id(0x00000008), propput]
        void Footer([in] BSTR rhs);
        [id(0x00000009), propget]
        BSTR Header();
        [id(0x00000009), propput]
        void Header([in] BSTR rhs);
        [id(0x0000000a), propget]
        BSTR Layout();
        [id(0x0000000a), propput]
        void Layout([in] BSTR rhs);
        [id(0x0000000b), propget]
        long MaxArchiveFiles();
        [id(0x0000000b), propput]
        void MaxArchiveFiles([in] long rhs);
        [id(0x0000000c), propget]
        BSTR MinLogLevel();
        [id(0x0000000c), propput]
        void MinLogLevel([in] BSTR rhs);
        [id(0x0000000d)]
        void SetupLogging();
}
````
