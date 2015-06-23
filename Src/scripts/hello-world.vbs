dim logConfig, factory, bucket, result

set logConfig = CreateObject("Couchbase.ComClient.LogConfig")
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