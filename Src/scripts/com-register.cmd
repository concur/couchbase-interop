@echo off
REM ---------------------------------------------------------------------------
REM
REM    Copyright 2015 Concur Technologies, Inc.
REM
REM    Licensed under the Apache License, Version 2.0 (the "License");
REM    you may not use this file except in compliance with the License.
REM    You may obtain a copy of the License at
REM
REM        http://www.apache.org/licenses/LICENSE-2.0
REM
REM    Unless required by applicable law or agreed to in writing, software
REM    distributed under the License is distributed on an "AS IS" BASIS,
REM    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
REM    See the License for the specific language governing permissions and
REM    limitations under the License.
REM
REM ---------------------------------------------------------------------------

REM Either run this from Vistual Studio command line, or set correct environment path for Gacutil and MSBuild tools first
REM If you do not have Gacutil tool, you can download it as part of Winsows SDK

IF NOT EXIST ..\Couchbase.ComClient\bin\Release\Couchbase.ComClient.dll (
    MSBuild.exe ..\Couchbase.ComClient\Couchbase.ComClient.csproj /t:Rebuild /p:Configuration=Release
)

echo on

Gacutil.exe /i ..\Couchbase.ComClient\bin\Release\Common.Logging.dll
Gacutil.exe /i ..\Couchbase.ComClient\bin\Release\Common.Logging.Core.dll
Gacutil.exe /i ..\Couchbase.ComClient\bin\Release\Common.Logging.NLog32.dll
Gacutil.exe /i ..\Couchbase.ComClient\bin\Release\NLog.dll
Gacutil.exe /i ..\Couchbase.ComClient\bin\Release\Newtonsoft.Json.dll
Gacutil.exe /i ..\Couchbase.ComClient\bin\Release\Couchbase.NetClient.dll
Gacutil.exe /i ..\Couchbase.ComClient\bin\Release\Couchbase.ComClient.dll

@REM Note that there are 2 versions of RegAsm tool. We need to run both to get the COM objects registered for 32 and 64 bit.

%windir%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /verbose ..\Couchbase.ComClient\bin\Release\Couchbase.ComClient.dll
%windir%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe /verbose ..\Couchbase.ComClient\bin\Release\Couchbase.ComClient.dll
