using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Couchbase.ComClient.Test
{
	[TestClass]
	public class BucketFactorTest
	{
		private IBucketFactory iBF;
		private IBucketWrapper iBW;
		private OperationResultWrapper oRW;

		[TestInitialize()]
		public void TestInitialize()
		{
			//this ensures tests don't mess with each other
			iBF = new BucketFactory();
			iBF.ConfigureCluster("Couchbase.ComClient.Test.dll.config", "local");
			iBW = iBF.GetBucket("default");
		}

		[ClassCleanup()]
		public static void TestFinalize()
		{
			//TODO
		}

		/*
		[TestMethod]
		[ExpectedException(typeof(System.ArgumentException))]
		public void TestException()
		{
			iBF.ConfigureCluster("non_existing.config", "XXX");
		}
		
		[TestMethod]
		public void TestConfigureCluster()
		{
			iBF.ConfigureCluster("Couchbase.ComClient.Test.dll.config", "local_2");
			Assert.IsTrue(iBF.IsClusterOpen("local_2"), "Cluster local was not opened");
			iBF.CloseCluster("local_2");
			Assert.IsFalse(iBF.IsClusterOpen("local_2"), "Cluster local was opened");
		}*/

		[TestMethod]
		public void TestBucketInsert()
		{
			string key = "insert_1";
			string value = "test_value";
			oRW = iBW.Insert(key, value, 60);
			Assert.AreEqual(null, oRW.Value, "Unexpected value of insert operation!");
			Assert.IsTrue(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of insert operation!");
			Assert.AreEqual(value, iBW.Get(key).Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestBucketInsert2()
		{
			iBW.Insert(null, "test_value", 60);
		}

		[TestMethod]
		public void TestBucketInsert3()
		{
			string key = "insert_3";
			string value = null;
			oRW = iBW.Insert(key, value, 60);
			Assert.AreEqual(null, oRW.Value, "Unexpected value of insert operation!");
			Assert.IsTrue(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of insert operation!");
			Assert.AreEqual(value, iBW.Get(key).Value, "Reading inserted value failed!");
		}

		[TestMethod]
		public void TestBucketInsert4()
		{
			string key = "insert_4";
			string value = "";
			iBW.Insert(key, value, 60);
			oRW = iBW.Insert(key, value, 60);
			Assert.AreEqual(null, oRW.Value, "Unexpected value of insert operation!");
			Assert.IsFalse(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(2, oRW.Status, "Unexpected status of insert operation!");
			Assert.AreEqual(value, iBW.Get(key).Value, "Reading inserted value failed!");
		}

		[TestMethod]
		[TestCategory("Expiration")]
		[TestCategory("Insert")]
		public void TestBucketInsert5()
		{
			string key = "insert_5";
			string value = "value5";
			iBW.Insert(key, value, 1);
			oRW = iBW.Get(key);
			Assert.AreEqual(value, oRW.Value, "Unexpected value of insert operation!");
			Assert.IsTrue(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of insert operation!");
			System.Threading.Thread.Sleep(1000);
			oRW = iBW.Get(key);
			Assert.AreEqual(null, oRW.Value, "Unexpected value of insert operation!");
			Assert.IsFalse(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(1, oRW.Status, "Unexpected status of insert operation!");
		}

		[TestMethod]
		public void TestBucketInsert6()
		{
			string key = "insert_6";
			string value1 = "value6_1";
			string value2 = "value6_2";
			iBW.Insert(key, value1, 60);
			oRW = iBW.Insert(key, value2, 60);
			Assert.IsFalse(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(2, oRW.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of insert operation!");
			Assert.AreEqual(value1, iBW.Get(key).Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketInsert7()
		{
			string key = "insert_7";
			var value = false;
			oRW = iBW.Insert(key, value, 60);
			Assert.IsTrue(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of insert operation!");
			Assert.AreEqual(value, iBW.Get(key).Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketInsert8()
		{
			string key = "insert_8";
			var value = 36.5;
			oRW = iBW.Insert(key, value, 60);
			Assert.IsTrue(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of insert operation!");
			Assert.AreEqual(value, iBW.Get(key).Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketInsert9()
		{
			string key = "insert_9";
			var value = "Ad!@#$%^&*()[]}|;'/.,?+";
			oRW = iBW.Insert(key, value, 60);
			Assert.IsTrue(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of insert operation!");
			Assert.AreEqual(value, iBW.Get(key).Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketGet()
		{
			string key = "get_1";
			oRW = iBW.Get(key);
			Assert.IsFalse(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(1, oRW.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketGet2()
		{
			string key = "get_2";
			string value = "asdafsfds";
			iBW.Insert(key, value, 60);
			oRW = iBW.Get(key);
			Assert.IsTrue(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(value, oRW.Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketGet3()
		{
			string key = "get_3";
			Int64 value = 461;
			iBW.Insert(key, value, 60);
			oRW = iBW.Get(key);
			Assert.IsTrue(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(value, oRW.Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketGetAndTouch()
		{
			string key = "getandtouch_1";
			oRW = iBW.GetAndTouch(key, 60);
			Assert.IsFalse(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(1, oRW.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		[TestCategory("Expiration")]
		public void TestBucketGetAndTouch2()
		{
			string key = "getandtouch_2";
			string value = "value";
			iBW.Insert(key, value, 60);
			oRW = iBW.GetAndTouch(key, 1);
			Assert.AreEqual(value, oRW.Value, "Unexpected value of insert operation!");
			Assert.IsTrue(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of insert operation!");
			System.Threading.Thread.Sleep(1000);
			oRW = iBW.Get(key);
			Assert.AreEqual(null, oRW.Value, "Unexpected value of insert operation!");
			Assert.IsFalse(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(1, oRW.Status, "Unexpected status of insert operation!");
		}

		[TestMethod]
		public void TestBucketRemove()
		{
			string key = "remove_1";
			oRW = iBW.Remove(key);
			Assert.IsFalse(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(1, oRW.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketRemove2()
		{
			string key = "remove_2";
			string value = "asd";
			iBW.Insert(key, value, 60);
			oRW = iBW.Remove(key);
			Assert.IsTrue(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of insert operation!");
			Assert.AreEqual(null, iBW.Get(key).Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		[TestCategory("Expiration")]
		public void TestBucketTouch()
		{
			string key = "touch_1";
			string value = "value";
			iBW.Insert(key, value, 60);
			iBW.Touch(key, 1);
			oRW = iBW.Get(key);
			Assert.AreEqual(value, oRW.Value, "Unexpected value of insert operation!");
			Assert.IsTrue(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of insert operation!");
			System.Threading.Thread.Sleep(1000);
			oRW = iBW.Get(key);
			Assert.AreEqual(null, oRW.Value, "Unexpected value of insert operation!");
			Assert.IsFalse(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(1, oRW.Status, "Unexpected status of insert operation!");
		}

		[TestMethod]
		public void TestBucketRemove3()
		{
			string key = "remove_3";
			var value = 85;
			iBW.Insert(key, value, 60);
			oRW = iBW.Remove(key);
			Assert.IsTrue(oRW.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of insert operation!");
			Assert.AreEqual(null, iBW.Get(key).Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketReplace()
		{
			string key = "replace_1";
			string value = "value";
			oRW = iBW.Replace(key, value, 60);
			Assert.IsFalse(oRW.Success, "Unexpected success of replace operation!");
			Assert.AreEqual(1, oRW.Status, "Unexpected status of replace operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of replace operation!");
		}

		[TestMethod]
		public void TestBucketReplace2()
		{
			string key = "replace_2";
			string value1 = "value1";
			string value2 = "value2";
			iBW.Insert(key, value1, 60);
			oRW = iBW.Replace(key, value2, 60);
			Assert.IsTrue(oRW.Success, "Unexpected success of replace operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of replace operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of replace operation!");
			Assert.AreEqual(value2, iBW.Get(key).Value, "Unexpected value of replace operation!");
		}

		[TestMethod]
		public void TestBucketReplace3()
		{
			string key = "replace_3";
			string value1 = "value1";
			string value2 = null;
			iBW.Insert(key, value1, 60);
			oRW = iBW.Replace(key, value2, 60);
			Assert.IsTrue(oRW.Success, "Unexpected success of replace operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of replace operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of replace operation!");
			Assert.AreEqual(value2, iBW.Get(key).Value, "Unexpected value of replace operation!");
		}

		[TestMethod]
		public void TestBucketUpsert()
		{
			string key = "upsert_1";
			string value = "value";
			oRW = iBW.Upsert(key, value, 60);
			Assert.IsTrue(oRW.Success, "Unexpected success of upsert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of upsert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of upsert operation!");
			Assert.AreEqual(value, iBW.Get(key).Value, "Unexpected value of upsert operation!");
		}



		[TestMethod]
		public void TestBucketUpsert2()
		{
			string key = "upsert_2";
			string value1 = "value1";
			string value2 = "value2";
			iBW.Insert(key, value1, 60);
			oRW = iBW.Upsert(key, value2, 60);
			Assert.IsTrue(oRW.Success, "Unexpected success of upsert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of upsert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of upsert operation!");
			Assert.AreEqual(value2, iBW.Get(key).Value, "Unexpected value of upsert operation!");
		}

		[TestMethod]
		public void TestBucketUpsert3()
		{
			string key = "upsert_3";
			string value1 = "value1";
			string value2 = null;
			iBW.Insert(key, value1, 60);
			oRW = iBW.Upsert(key, value2, 60);
			Assert.IsTrue(oRW.Success, "Unexpected success of upsert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of upsert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of upsert operation!");
			Assert.AreEqual(value2, iBW.Get(key).Value, "Unexpected value of upsert operation!");
		}

		[TestMethod]
		public void TestBucketUpsert4()
		{
			string key = "upsert_4";
			var value1 = true;
//TODO			Int64 value2 = 56;
			int value2 = 56;
			iBW.Insert(key, value1, 60);
			oRW = iBW.Upsert(key, value2, 60);
			Assert.IsTrue(oRW.Success, "Unexpected success of upsert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of upsert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of upsert operation!");
			Assert.AreEqual(value2, iBW.Get(key).Value, "Unexpected value of upsert operation!");
		}

		[TestMethod]
		public void TestBucketUpsertCas()
		{
			string key = "upsert_cas_1";
			string value1 = "value1";
			string value2 = "value2";
			iBW.Insert(key, value1, 60);
			ulong cas1 = iBW.Get(key).Cas;
			oRW = iBW.UpsertCas(key, value2, cas1, 60);
			Assert.IsTrue(oRW.Success, "Unexpected success of upsert operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of upsert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of upsert operation!");
			Assert.AreEqual(value2, iBW.Get(key).Value, "Unexpected value of upsert operation!");
		}

		[TestMethod]
		public void TestBucketUpsertCas2()
		{
			string key = "upsert_cas_2";
			string value1 = "value1";
			string value2 = "value2";
			string value3 = "value3";
			iBW.Insert(key, value1, 60);
			ulong cas1 = iBW.Get(key).Cas;
			iBW.Replace(key, value2, 60);
			oRW = iBW.UpsertCas(key, value3, cas1, 60);
			Assert.IsFalse(oRW.Success, "Unexpected success of upsert operation!");
			Assert.AreEqual(2, oRW.Status, "Unexpected status of upsert operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of upsert operation!");
			Assert.AreEqual(value2, iBW.Get(key).Value, "Unexpected value of upsert operation!");
		}

		[TestCategory("Expiration")]
		[TestMethod]
		public void TestBucketUpsertCas3()
		{
			string key = "upsert_cas_3";
			string value1 = "value1";
			string value2 = "value2";
			iBW.Insert(key, value1, 60);
			ulong cas1 = iBW.Get(key).Cas;
			oRW = iBW.UpsertCas(key, value2, cas1, 1);
			Assert.IsTrue(oRW.Success, "Unexpected success of upsert operation!");
			Assert.AreEqual(value2, iBW.Get(key).Value, "Unexpected value of upsert operation!");
			System.Threading.Thread.Sleep(1000);
			Assert.AreEqual(null, iBW.Get(key).Value, "Unexpected value of upsert operation!");
		}

		[TestMethod]
		public void TestBucketAppend()
		{
			string key = "append_1";
			string value = "Hello";
			oRW = iBW.Append(key, value);
			Assert.IsFalse(oRW.Success, "Unexpected success of append operation!");
			Assert.AreEqual(5, oRW.Status, "Unexpected status of append operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of append operation!");
			Assert.AreEqual(null, iBW.Get(key).Value, "Unexpected value of append operation!");
		}

		[TestMethod]
		public void TestBucketAppend2()
		{
			string key = "append_2";
			string value1 = "Hello";
			string value2 = " World!";
			iBW.Insert(key, value1, 60);
			Assert.AreEqual(value1, iBW.Get(key).Value, "Unexpected value of append operation!");
			oRW = iBW.Append(key, value2);
			Assert.IsTrue(oRW.Success, "Unexpected success of append operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of append operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
//			Assert.AreEqual(null, oRW.Value, "Unexpected value of append operation!");
			Assert.AreEqual(value1 + value2, iBW.Get(key).Value, "Unexpected value of append operation!");
		}

		[TestMethod]
		public void TestBucketAppend3()
		{
			string key = "append_3";
			string value1 = "value1";
			string value2 = null;
			iBW.Insert(key, value1, 60);
			oRW = iBW.Append(key, value2);
			Assert.IsTrue(oRW.Success, "Unexpected success of append operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of append operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			//			Assert.AreEqual(null, oRW.Value, "Unexpected value of append operation!");
			Assert.AreEqual(value1, iBW.Get(key).Value, "Unexpected value of append operation!");
		}

		[TestMethod]
		public void TestBucketPrepend()
		{
			string key = "prepend_1";
			string value = "World!";
			oRW = iBW.Prepend(key, value);
			Assert.IsFalse(oRW.Success, "Unexpected success of prepend operation!");
			Assert.AreEqual(5, oRW.Status, "Unexpected status of prepend operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(null, oRW.Value, "Unexpected value of prepend operation!");
			Assert.AreEqual(null, iBW.Get(key).Value, "Unexpected value of prepend operation!");
		}

		[TestMethod]
		public void TestBucketPrepend2()
		{
			string key = "prepend_2";
			string value1 = "World!";
			string value2 = "Hello ";
			iBW.Insert(key, value1, 60);
			Assert.AreEqual(value1, iBW.Get(key).Value, "Unexpected value of prepend operation!");
			oRW = iBW.Prepend(key, value2);
			Assert.IsTrue(oRW.Success, "Unexpected success of prepend operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of prepend operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
//			Assert.AreEqual(null, oRW.Value, "Unexpected value of append operation!");
			Assert.AreEqual(value2 + value1, iBW.Get(key).Value, "Unexpected value of prepend operation!");
		}

		[TestMethod]
		public void TestBucketPrepend3()
		{
			string key = "prepend_3";
			string value1 = "value1";
			string value2 = null;
			iBW.Insert(key, value1, 60);
			oRW = iBW.Prepend(key, value2);
			Assert.IsTrue(oRW.Success, "Unexpected success of prepend operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of prepend operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			//			Assert.AreEqual(null, oRW.Value, "Unexpected value of prepend operation!");
			Assert.AreEqual(value1, iBW.Get(key).Value, "Unexpected value of prepend operation!");
		}

		[TestMethod]
		public void TestBucketIncrement()
		{
			string key = "increment_1";
			oRW = iBW.Increment(key);
			Assert.IsTrue(oRW.Success, "Unexpected success of increment operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of increment operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual((UInt64) 1, oRW.Value, "Unexpected value of increment operation!");
			Assert.AreEqual((Int64) 1, iBW.Get(key).Value, "Unexpected value of increment operation!");
			iBW.Remove(key);
		}

		[TestMethod]
		public void TestBucketIncrement2()
		{
			string key = "increment_2";
			ulong initial = 37;
			ulong delta = 8;
			iBW.Insert(key, initial, 60);
			oRW = iBW.Increment(key, delta, initial, 60);
			Assert.IsTrue(oRW.Success, "Unexpected success of increment operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of increment operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(initial + delta, oRW.Value, "Unexpected value of increment operation!");
			Assert.AreEqual((Int64) (initial + delta), iBW.Get(key).Value, "Unexpected value of increment operation!");
		}

		[TestMethod]
		public void TestBucketIncrement3()
		{
			string key = "increment_3";
			ulong value = 0;
			ulong delta = 0;
			iBW.Insert(key, value, 60);
			oRW = iBW.Increment(key, delta, 55, 60);
			Assert.IsTrue(oRW.Success, "Unexpected success of increment operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of increment operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(value + delta, oRW.Value, "Unexpected value of increment operation!");
			Assert.AreEqual((Int64)(value + delta), iBW.Get(key).Value, "Unexpected value of increment operation!");
		}

		[TestMethod]
		public void TestBucketIncrement4()
		{
			string key = "increment_4";
			ulong initial = 10;
			ulong delta = 7;
			oRW = iBW.Increment(key, delta, initial, 60);
			Assert.IsTrue(oRW.Success, "Unexpected success of increment operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of increment operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(initial, oRW.Value, "Unexpected value of increment operation!");
			Assert.AreEqual((Int64)(initial), iBW.Get(key).Value, "Unexpected value of increment operation!");
		}

		[TestMethod]
		[TestCategory("Expiration")]
		public void TestBucketIncrement5()
		{
			string key = "increment_5";
			ulong initial = 55;
			ulong delta = 5;
			oRW = iBW.Decrement(key, delta, initial, 1);
			Assert.AreEqual(initial, oRW.Value, "Unexpected value of increment operation!");
			System.Threading.Thread.Sleep(1000);
			oRW = iBW.Get(key);
			Assert.AreEqual(null, oRW.Value, "Unexpected value of increment operation!");
			Assert.IsFalse(oRW.Success, "Unexpected succes of increment operation!");
			Assert.AreEqual(1, oRW.Status, "Unexpected status of increment operation!");
		}

		[TestMethod]
		public void TestBucketDecrement()
		{
			string key = "decrement_1";
			oRW = iBW.Decrement(key);
			Assert.IsTrue(oRW.Success, "Unexpected success of decrement operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of decrement operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual((UInt64)1, oRW.Value, "Unexpected value of decrement operation!");
			Assert.AreEqual((Int64)1, iBW.Get(key).Value, "Unexpected value of decrement operation!");
			iBW.Remove(key);
		}

		[TestMethod]
		public void TestBucketDecrement2()
		{
			string key = "decrement_2";
			ulong initial = 37;
			ulong delta = 8;
			iBW.Insert(key, initial, 60);
			oRW = iBW.Decrement(key, delta, initial, 60);
			Assert.IsTrue(oRW.Success, "Unexpected success of decrement operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of decrement operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(initial - delta, oRW.Value, "Unexpected value of decrement operation!");
			Assert.AreEqual((Int64)(initial - delta), iBW.Get(key).Value, "Unexpected value of decrement operation!");
		}

		[TestMethod]
		public void TestBucketDecrement3()
		{
			string key = "decrement_3";
			ulong initial = 0;
			ulong delta = 0;
			iBW.Insert(key, initial, 60);
			oRW = iBW.Decrement(key, delta, initial, 60);
			Assert.IsTrue(oRW.Success, "Unexpected success of decrement operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of decrement operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(initial - delta, oRW.Value, "Unexpected value of decrement operation!");
			Assert.AreEqual((Int64)(initial - delta), iBW.Get(key).Value, "Unexpected value of decrement operation!");
		}

		[TestMethod]
		public void TestBucketDecrement4()
		{
			string key = "decrement_4";
			ulong initial = 55;
			ulong delta = 5;
			oRW = iBW.Decrement(key, delta, initial, 60);
			Assert.IsTrue(oRW.Success, "Unexpected success of increment operation!");
			Assert.AreEqual(0, oRW.Status, "Unexpected status of increment operation!");
			Assert.IsNotNull(oRW.Message, "Message was null!");
			Assert.AreEqual(initial, oRW.Value, "Unexpected value of increment operation!");
			Assert.AreEqual((Int64)(initial), iBW.Get(key).Value, "Unexpected value of increment operation!");
		}

		[TestMethod]
		[TestCategory("Expiration")]
		public void TestBucketDecrement5()
		{
			string key = "decrement_5";
			ulong initial = 55;
			ulong delta = 5;
			oRW = iBW.Decrement(key, delta, initial, 1);
			Assert.AreEqual(initial, oRW.Value, "Unexpected value of decrement operation!");
			System.Threading.Thread.Sleep(1000);
			oRW = iBW.Get(key);
			Assert.AreEqual(null, oRW.Value, "Unexpected value of decrement operation!");
			Assert.IsFalse(oRW.Success, "Unexpected succes of decrement operation!");
			Assert.AreEqual(1, oRW.Status, "Unexpected status of decrement operation!");
		}
	}
}
