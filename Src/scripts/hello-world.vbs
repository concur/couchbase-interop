'-----------------------------------------------------------------------------
'
'    Copyright 2015 Concur Technologies, Inc.
'
'    Licensed under the Apache License, Version 2.0 (the "License");
'    you may not use this file except in compliance with the License.
'    You may obtain a copy of the License at
'
'        http://www.apache.org/licenses/LICENSE-2.0
'
'    Unless required by applicable law or agreed to in writing, software
'    distributed under the License is distributed on an "AS IS" BASIS,
'    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
'    See the License for the specific language governing permissions and
'    limitations under the License.
'
'-----------------------------------------------------------------------------

dim logConfig, factory, bucket, result

on error resume next
set logConfig = CreateObject("Couchbase.ComClient.LogConfig")
if err then
    call WScript.StdOut.WriteLine(err.Description)
    call WScript.StdOut.WriteLine("Make sure the COM components are registered correctly.")
    call WScript.Quit
end if
on error goto 0

logConfig.MinLogLevel = "Debug"
logConfig.FileName = "hello-world.log"
logConfig.Encoding = "utf-8"
call logConfig.SetupLogging()
set logConfig = nothing

set factory = CreateObject("Couchbase.ComClient.BucketFactory")
call factory.ConfigureCluster("hello-world.config", "local")
set bucket = factory.GetBucket("default")

call WScript.StdOut.Write("Upsert call success: ")
set result = bucket.Upsert("testkey", "Hello World!", 60)
call WScript.StdOut.WriteLine(result.Success)
set result = nothing

call WScript.StdOut.Write("Get value: ")
set result = bucket.Get("testkey")
call WScript.StdOut.WriteLine(result.Value)
set result = nothing

call factory.CloseBucket("default")
set bucket = nothing
factory.CloseCluster("local")
set factory = nothing