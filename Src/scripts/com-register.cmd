@REM Either run this from Vistual Studio command line, or set correct environment path for Gacutil first
@REM If you do not have Gacutil tool, you can download it as part of Winsows SDK

Gacutil.exe /i ..\Couchbase.ComClient\bin\Release\Common.Logging.dll
Gacutil.exe /i ..\Couchbase.ComClient\bin\Release\Common.Logging.Core.dll
Gacutil.exe /i ..\Couchbase.ComClient\bin\Release\Common.Logging.NLog31.dll
Gacutil.exe /i ..\Couchbase.ComClient\bin\Release\NLog.dll
Gacutil.exe /i ..\Couchbase.ComClient\bin\Release\Newtonsoft.Json.dll
Gacutil.exe /i ..\Couchbase.ComClient\bin\Release\Couchbase.NetClient.dll
Gacutil.exe /i ..\Couchbase.ComClient\bin\Release\Couchbase.ComClient.dll

@REM Note that there are 2 versions of RegAsm tool. We need to run both to get the COM objects registered for 32 and 64 bit.

%windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /verbose ..\Couchbase.ComClient\bin\Release\Couchbase.ComClient.dll
%windir%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe /verbose ..\Couchbase.ComClient\bin\Release\Couchbase.ComClient.dll
