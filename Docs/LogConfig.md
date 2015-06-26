# LogConfig #
* **CLSID**: 3E14F6C5-3E39-4C54-9E1A-6F4C0BCCEF39
* **ProgId**: Couchbase.ComClient.LogConfig
* **Interface UUID**: 46F8E1EC-9AA0-491D-8126-01C7454C549B
* **Interface Type**: Dual

COM API to manage internal logs of the undelying Couchbase .NET SDK. To get full logs from the .NET SDK, this class should be used on application startup before [BucketFactory](BucketFactory.md) is used.

The logging is built on top of [NLog](http://nlog-project.org/) library and uses [File](https://github.com/nlog/nlog/wiki/File-target) logging target. Please refer to [NLog Wiki](http://github.com/nlog/nlog/wiki) for detailed explanation of the properties and their usage.

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


### ArchiveAboveSize ###
Size in bytes above which log files will be automatically archived.

##### put #####
A number of bytes after which current log file is archived and new log file is open

##### get #####
Default value is -1, which means file size limit is not enabled


### ArchiveEvery ###
Indicates whether to automatically archive log files every time the specified time passes.
Available values are:
* Day - Archive daily.
* Hour - Archive every hour.
* Minute - Archive every minute.
* Month - Archive every month.
* None - Don't archive based on time.
* Year - Archive every year.

##### put #####
Name of the log archive period

##### get #####
Default value is *None*, which means file archiving based on time is disabled


### ArchiveNumbering ###
Way file archives are numbered.
Available values are:
* Rolling - The most recent is always #0 then #1, ..., #N
* Sequence - The most recent archive has the highest number.
* Date - Name archive files based on date

##### put #####
Name of the archive numbering style

##### get #####
Default value is *Sequence*


### DeleteOldFileOnStartup ###
Indicates whether to delete old log file on startup.

##### put #####
*True* if you want old logs automatically deleted on startup; *False* otherwise

##### get #####
Default value is *False*


### EnableFileDelete ###
Indicates whether to enable log file(s) to be deleted.

##### put #####
*True* if you want to enable logging system to remove log files (old log files, archived logs); *False* otherwise

##### get #####
Default value is *True*


### Encoding ###
Log file encoding.

##### put #####
File encoding name like "utf-8", "ascii" or "utf-16"

##### get #####
Default encoding used is obtained from *Encoding.Default*. The actual value may vary based on your system.


### FileName ###
Name of the file to write to. This property supports [NLog Layout](https://github.com/nlog/nlog/wiki/Layout-renderers).

##### put #####
Path and name of the log file output

##### get #####
Default file name is "Couchbase.ComClient.txt", which mean the file is created in the current application working directory.


### Footer ###
Log file footer.

##### put #####
String representing [NLog Layout](https://github.com/nlog/nlog/wiki/Layout-renderers) with the log file footer

##### get #####
Default log file footer is empty


### Header ###
Log file header.

##### put #####
String representing [NLog Layout](https://github.com/nlog/nlog/wiki/Layout-renderers) with the log file header

##### get #####
Default file header is empty


### Layout ###
Log format to be used in the file.

##### put #####
String representing [NLog Layout](https://github.com/nlog/nlog/wiki/Layout-renderers) with the log format

##### get #####
Default value for the log format is "${longdate}|${level:uppercase=true}|${logger}|${message}"


### MaxArchiveFiles ###
Maximum number of archive files that should be kept

##### put #####
Requested number of archive files

##### get #####
Default number of archive files is 9


### MinLogLevel ###
Cut-off log level of messages to be written into the log file.
Available log levels:
* Off
* Fatal
* Error
* Warn
* Info
* Debug
* Trace

##### put #####
Name of the minimal log level to be written into log file. All logs with this or higher log level will be written. (I.E. If *MinLogLevel* is set to *Warn*, all log messages with levels *Fatal*, *Error* and *Warn* will be written into the log file)

##### get #####
Default value of minimal log severity is *Info*


### SetupLogging ###
Setup logging of the underlying .NET SDK based on current state of properties of this class. This should be done on application start before you start working with [BucketFactory](BucketFactory.md) object.