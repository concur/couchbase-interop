/* ************************************************************
 *
 *    Copyright 2015 Concur Technologies, Inc.
 *
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *
 * ************************************************************/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Couchbase.ComClient.Test
{
	[TestClass]
	public class BucketWrapperTest
	{
		private static IBucketFactory _iBf;
		private static IBucketWrapper _iBw;
		private OperationResultWrapper _oRw;

		[ClassInitialize()]
		public static void TestInitialize(TestContext context)
		{
			//this ensures tests don't mess with each other
			_iBf = new BucketFactory();
			_iBf.ConfigureCluster("Couchbase.ComClient.Test.dll.config", "local");
			_iBw = _iBf.GetBucket("default");
		}

		[ClassCleanup()]
		public static void TestFinalize()
		{
			_iBf.CloseBucket("default");
			_iBf.CloseCluster("local");
		}

		[TestMethod]
		public void TestBucketInsert()
		{
			string key = "insert_1" + Environment.TickCount;
			string value = "test_value";
			_oRw = _iBw.Insert(key, value, 60);
			Assert.AreEqual("", _oRw.Value, "Unexpected value of insert operation!");
			Assert.IsTrue(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of insert operation!");
			Assert.AreEqual(value, _iBw.Get(key).Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestBucketInsert2()
		{
			_iBw.Insert(null, "test_value", 60);
		}

		[TestMethod]
		public void TestBucketInsert3()
		{
			string key = "insert_3" + Environment.TickCount;
			string value = null;
			_oRw = _iBw.Insert(key, value, 60);
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of insert operation!");
			Assert.IsTrue(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of insert operation!");
			Assert.AreEqual(value, _iBw.Get(key).Value, "Reading inserted value failed!");
		}

		[TestMethod]
		public void TestBucketInsert4()
		{
			string key = "insert_4" + Environment.TickCount;
			string value = "";
			_iBw.Insert(key, value, 60);
			_oRw = _iBw.Insert(key, value, 60);
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of insert operation!");
			Assert.IsFalse(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(2, _oRw.Status, "Unexpected status of insert operation!");
			Assert.AreEqual(value, _iBw.Get(key).Value, "Reading inserted value failed!");
		}

		[TestMethod]
		[TestCategory("Expiration")]
		public void TestBucketInsert5()
		{
			string key = "insert_5" + Environment.TickCount;
			string value = "value5";
			_iBw.Insert(key, value, 1);
			_oRw = _iBw.Get(key);
			Assert.AreEqual(value, _oRw.Value, "Unexpected value of insert operation!");
			Assert.IsTrue(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of insert operation!");
			System.Threading.Thread.Sleep(1000);
			_oRw = _iBw.Get(key);
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of insert operation!");
			Assert.IsFalse(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(1, _oRw.Status, "Unexpected status of insert operation!");
		}

		[TestMethod]
		public void TestBucketInsert6()
		{
			string key = "insert_6" + Environment.TickCount;
			string value1 = "value6_1";
			string value2 = "value6_2";
			_iBw.Insert(key, value1, 60);
			_oRw = _iBw.Insert(key, value2, 60);
			Assert.IsFalse(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(2, _oRw.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of insert operation!");
			Assert.AreEqual(value1, _iBw.Get(key).Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketInsert7()
		{
			string key = "insert_7" + Environment.TickCount;
			var value = false;
			_oRw = _iBw.Insert(key, value, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of insert operation!");
			Assert.AreEqual(value, _iBw.Get(key).Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketInsert8()
		{
			string key = "insert_8" + Environment.TickCount;
			var value = 36.5;
			_oRw = _iBw.Insert(key, value, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of insert operation!");
			Assert.AreEqual(value, _iBw.Get(key).Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketInsert9()
		{
			string key = "insert_9" + Environment.TickCount;
			var value = "Ad!@#$%^&*()[]}|;'/.,?+";
			_oRw = _iBw.Insert(key, value, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual("", _oRw.Value, "Unexpected value of insert operation!");
			Assert.AreEqual(value, _iBw.Get(key).Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketGet()
		{
			string key = "get_1" + Environment.TickCount;
			_oRw = _iBw.Get(key);
			Assert.IsFalse(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(1, _oRw.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketGet2()
		{
			string key = "get_2" + Environment.TickCount;
			string value = "asdafsfds";
			_iBw.Insert(key, value, 60);
			_oRw = _iBw.Get(key);
			Assert.IsTrue(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(value, _oRw.Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketGet3()
		{
			string key = "get_3" + Environment.TickCount;
			Int64 value = 461;
			_iBw.Insert(key, value, 60);
			_oRw = _iBw.Get(key);
			Assert.IsTrue(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(value, _oRw.Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketGetAndTouch()
		{
			string key = "getandtouch_1" + Environment.TickCount;
			_oRw = _iBw.GetAndTouch(key, 60);
			Assert.IsFalse(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(1, _oRw.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		[TestCategory("Expiration")]
		public void TestBucketGetAndTouch2()
		{
			string key = "getandtouch_2" + Environment.TickCount;
			string value = "value";
			_iBw.Insert(key, value, 60);
			_oRw = _iBw.GetAndTouch(key, 1);
			Assert.AreEqual(value, _oRw.Value, "Unexpected value of insert operation!");
			Assert.IsTrue(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of insert operation!");
			System.Threading.Thread.Sleep(1000);
			_oRw = _iBw.Get(key);
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of insert operation!");
			Assert.IsFalse(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(1, _oRw.Status, "Unexpected status of insert operation!");
		}

		[TestMethod]
		public void TestBucketRemove()
		{
			string key = "remove_1" + Environment.TickCount;
			_oRw = _iBw.Remove(key);
			Assert.IsFalse(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(1, _oRw.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketRemove2()
		{
			string key = "remove_2" + Environment.TickCount;
			string value = "asd";
			_iBw.Insert(key, value, 60);
			_oRw = _iBw.Remove(key);
			Assert.IsTrue(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of insert operation!");
			Assert.AreEqual(null, _iBw.Get(key).Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		[TestCategory("Expiration")]
		public void TestBucketTouch()
		{
			string key = "touch_1" + Environment.TickCount;
			string value = "value";
			_iBw.Insert(key, value, 60);
			_iBw.Touch(key, 1);
			_oRw = _iBw.Get(key);
			Assert.AreEqual(value, _oRw.Value, "Unexpected value of insert operation!");
			Assert.IsTrue(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of insert operation!");
			System.Threading.Thread.Sleep(1000);
			_oRw = _iBw.Get(key);
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of insert operation!");
			Assert.IsFalse(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(1, _oRw.Status, "Unexpected status of insert operation!");
		}

		[TestMethod]
		public void TestBucketRemove3()
		{
			string key = "remove_3" + Environment.TickCount;
			var value = 85;
			_iBw.Insert(key, value, 60);
			_oRw = _iBw.Remove(key);
			Assert.IsTrue(_oRw.Success, "Unexpected succes of insert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of insert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of insert operation!");
			Assert.AreEqual(null, _iBw.Get(key).Value, "Unexpected value of insert operation!");
		}

		[TestMethod]
		public void TestBucketReplace()
		{
			string key = "replace_1" + Environment.TickCount;
			string value = "value";
			_oRw = _iBw.Replace(key, value, 60);
			Assert.IsFalse(_oRw.Success, "Unexpected success of replace operation!");
			Assert.AreEqual(1, _oRw.Status, "Unexpected status of replace operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of replace operation!");
		}

		[TestMethod]
		public void TestBucketReplace2()
		{
			string key = "replace_2" + Environment.TickCount;
			string value1 = "value1";
			string value2 = "value2";
			_iBw.Insert(key, value1, 60);
			_oRw = _iBw.Replace(key, value2, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected success of replace operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of replace operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual("", _oRw.Value, "Unexpected value of replace operation!");
			Assert.AreEqual(value2, _iBw.Get(key).Value, "Unexpected value of replace operation!");
		}

		[TestMethod]
		public void TestBucketReplace3()
		{
			string key = "replace_3" + Environment.TickCount;
			string value1 = "value1";
			string value2 = null;
			_iBw.Insert(key, value1, 60);
			_oRw = _iBw.Replace(key, value2, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected success of replace operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of replace operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of replace operation!");
			Assert.AreEqual(value2, _iBw.Get(key).Value, "Unexpected value of replace operation!");
		}

		[TestMethod]
		public void TestBucketUpsert()
		{
			string key = "upsert_1" + Environment.TickCount;
			string value = "value";
			_oRw = _iBw.Upsert(key, value, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected success of upsert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of upsert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual("", _oRw.Value, "Unexpected value of upsert operation!");
			Assert.AreEqual(value, _iBw.Get(key).Value, "Unexpected value of upsert operation!");
		}



		[TestMethod]
		public void TestBucketUpsert2()
		{
			string key = "upsert_2" + Environment.TickCount;
			string value1 = "value1";
			string value2 = "value2";
			_iBw.Insert(key, value1, 60);
			_oRw = _iBw.Upsert(key, value2, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected success of upsert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of upsert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual("", _oRw.Value, "Unexpected value of upsert operation!");
			Assert.AreEqual(value2, _iBw.Get(key).Value, "Unexpected value of upsert operation!");
		}

		[TestMethod]
		public void TestBucketUpsert3()
		{
			string key = "upsert_3" + Environment.TickCount;
			string value1 = "value1";
			string value2 = null;
			_iBw.Insert(key, value1, 60);
			_oRw = _iBw.Upsert(key, value2, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected success of upsert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of upsert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of upsert operation!");
			Assert.AreEqual(value2, _iBw.Get(key).Value, "Unexpected value of upsert operation!");
		}

		[TestMethod]
		public void TestBucketUpsert4()
		{
			string key = "upsert_4" + Environment.TickCount;
			var value1 = true;
			Int64 value2 = 56;
			//			int value2 = 56;
			_iBw.Insert(key, value1, 60);
			_oRw = _iBw.Upsert(key, value2, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected success of upsert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of upsert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of upsert operation!");
			Assert.AreEqual(value2, _iBw.Get(key).Value, "Unexpected value of upsert operation!");
		}

		[TestMethod]
		public void TestBucketUpsertCas()
		{
			string key = "upsert_cas_1" + Environment.TickCount;
			string value1 = "value1";
			string value2 = "value2";
			_iBw.Insert(key, value1, 60);
			ulong cas1 = _iBw.Get(key).Cas;
			_oRw = _iBw.UpsertCas(key, value2, cas1, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected success of upsert operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of upsert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual("", _oRw.Value, "Unexpected value of upsert operation!");
			Assert.AreEqual(value2, _iBw.Get(key).Value, "Unexpected value of upsert operation!");
		}

		[TestMethod]
		public void TestBucketUpsertCas2()
		{
			string key = "upsert_cas_2" + Environment.TickCount;
			string value1 = "value1";
			string value2 = "value2";
			string value3 = "value3";
			_iBw.Insert(key, value1, 60);
			ulong cas1 = _iBw.Get(key).Cas;
			_iBw.Replace(key, value2, 60);
			_oRw = _iBw.UpsertCas(key, value3, cas1, 60);
			Assert.IsFalse(_oRw.Success, "Unexpected success of upsert operation!");
			Assert.AreEqual(2, _oRw.Status, "Unexpected status of upsert operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of upsert operation!");
			Assert.AreEqual(value2, _iBw.Get(key).Value, "Unexpected value of upsert operation!");
		}

		[TestCategory("Expiration")]
		[TestMethod]
		public void TestBucketUpsertCas3()
		{
			string key = "upsert_cas_3" + Environment.TickCount;
			string value1 = "value1";
			string value2 = "value2";
			_iBw.Insert(key, value1, 60);
			ulong cas1 = _iBw.Get(key).Cas;
			_oRw = _iBw.UpsertCas(key, value2, cas1, 1);
			Assert.IsTrue(_oRw.Success, "Unexpected success of upsert operation!");
			Assert.AreEqual(value2, _iBw.Get(key).Value, "Unexpected value of upsert operation!");
			System.Threading.Thread.Sleep(1000);
			Assert.AreEqual(null, _iBw.Get(key).Value, "Unexpected value of upsert operation!");
		}

		[TestMethod]
		public void TestBucketAppend()
		{
			string key = "append_1" + Environment.TickCount;
			string value = "Hello";
			_oRw = _iBw.Append(key, value);
			Assert.IsFalse(_oRw.Success, "Unexpected success of append operation!");
			Assert.AreEqual(5, _oRw.Status, "Unexpected status of append operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of append operation!");
			Assert.AreEqual(null, _iBw.Get(key).Value, "Unexpected value of append operation!");
		}

		[TestMethod]
		public void TestBucketAppend2()
		{
			string key = "append_2" + Environment.TickCount;
			string value1 = "Hello";
			string value2 = " World!";
			_iBw.Insert(key, value1, 60);
			Assert.AreEqual(value1, _iBw.Get(key).Value, "Unexpected value of append operation!");
			_oRw = _iBw.Append(key, value2);
			Assert.IsTrue(_oRw.Success, "Unexpected success of append operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of append operation!");
			//			Assert.AreEqual(null, oRW.Value, "Unexpected value of append operation!");
			Assert.AreEqual(value1 + value2, _iBw.Get(key).Value, "Unexpected value of append operation!");
		}

		[TestMethod]
		public void TestBucketAppend3()
		{
			string key = "append_3" + Environment.TickCount;
			string value1 = "value1";
			string value2 = null;
			_iBw.Insert(key, value1, 60);
			_oRw = _iBw.Append(key, value2);
			Assert.IsTrue(_oRw.Success, "Unexpected success of append operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of append operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			//			Assert.AreEqual(null, oRW.Value, "Unexpected value of append operation!");
			Assert.AreEqual(value1, _iBw.Get(key).Value, "Unexpected value of append operation!");
		}

		[TestMethod]
		public void TestBucketPrepend()
		{
			string key = "prepend_1" + Environment.TickCount;
			string value = "World!";
			_oRw = _iBw.Prepend(key, value);
			Assert.IsFalse(_oRw.Success, "Unexpected success of prepend operation!");
			Assert.AreEqual(5, _oRw.Status, "Unexpected status of prepend operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of prepend operation!");
			Assert.AreEqual(null, _iBw.Get(key).Value, "Unexpected value of prepend operation!");
		}

		[TestMethod]
		public void TestBucketPrepend2()
		{
			string key = "prepend_2" + Environment.TickCount;
			string value1 = "World!";
			string value2 = "Hello ";
			_iBw.Insert(key, value1, 60);
			Assert.AreEqual(value1, _iBw.Get(key).Value, "Unexpected value of prepend operation!");
			_oRw = _iBw.Prepend(key, value2);
			Assert.IsTrue(_oRw.Success, "Unexpected success of prepend operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of prepend operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			//			Assert.AreEqual(null, oRW.Value, "Unexpected value of append operation!");
			Assert.AreEqual(value2 + value1, _iBw.Get(key).Value, "Unexpected value of prepend operation!");
		}

		[TestMethod]
		public void TestBucketPrepend3()
		{
			string key = "prepend_3" + Environment.TickCount;
			string value1 = "value1";
			string value2 = null;
			_iBw.Insert(key, value1, 60);
			_oRw = _iBw.Prepend(key, value2);
			Assert.IsTrue(_oRw.Success, "Unexpected success of prepend operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of prepend operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			//			Assert.AreEqual(null, oRW.Value, "Unexpected value of prepend operation!");
			Assert.AreEqual(value1, _iBw.Get(key).Value, "Unexpected value of prepend operation!");
		}

		[TestMethod]
		public void TestBucketIncrement()
		{
			string key = "increment_1" + Environment.TickCount;
			_oRw = _iBw.Increment(key);
			Assert.IsTrue(_oRw.Success, "Unexpected success of increment operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of increment operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual((UInt64)1, _oRw.Value, "Unexpected value of increment operation!");
			Assert.AreEqual((Int64)1, _iBw.Get(key).Value, "Unexpected value of increment operation!");
			_iBw.Remove(key);
		}

		[TestMethod]
		public void TestBucketIncrement2()
		{
			string key = "increment_2" + Environment.TickCount;
			ulong initial = 37;
			ulong delta = 8;
			_iBw.Insert(key, initial, 60);
			_oRw = _iBw.Increment(key, delta, initial, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected success of increment operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of increment operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(initial + delta, _oRw.Value, "Unexpected value of increment operation!");
			Assert.AreEqual((Int64)(initial + delta), _iBw.Get(key).Value, "Unexpected value of increment operation!");
		}

		[TestMethod]
		public void TestBucketIncrement3()
		{
			string key = "increment_3" + Environment.TickCount;
			ulong value = 0;
			ulong delta = 0;
			_iBw.Insert(key, value, 60);
			_oRw = _iBw.Increment(key, delta, 55, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected success of increment operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of increment operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(value + delta, _oRw.Value, "Unexpected value of increment operation!");
			Assert.AreEqual((Int64)(value + delta), _iBw.Get(key).Value, "Unexpected value of increment operation!");
		}

		[TestMethod]
		public void TestBucketIncrement4()
		{
			string key = "increment_4" + Environment.TickCount;
			ulong initial = 10;
			ulong delta = 7;
			_oRw = _iBw.Increment(key, delta, initial, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected success of increment operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of increment operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(initial, _oRw.Value, "Unexpected value of increment operation!");
			Assert.AreEqual((Int64)(initial), _iBw.Get(key).Value, "Unexpected value of increment operation!");
		}

		[TestMethod]
		[TestCategory("Expiration")]
		public void TestBucketIncrement5()
		{
			string key = "increment_5" + Environment.TickCount;
			ulong initial = 55;
			ulong delta = 5;
			_oRw = _iBw.Decrement(key, delta, initial, 1);
			Assert.AreEqual(initial, _oRw.Value, "Unexpected value of increment operation!");
			System.Threading.Thread.Sleep(1000);
			_oRw = _iBw.Get(key);
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of increment operation!");
			Assert.IsFalse(_oRw.Success, "Unexpected succes of increment operation!");
			Assert.AreEqual(1, _oRw.Status, "Unexpected status of increment operation!");
		}

		[TestMethod]
		public void TestBucketDecrement()
		{
			string key = "decrement_1" + Environment.TickCount;
			_oRw = _iBw.Decrement(key);
			Assert.IsTrue(_oRw.Success, "Unexpected success of decrement operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of decrement operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual((UInt64)1, _oRw.Value, "Unexpected value of decrement operation!");
			Assert.AreEqual((Int64)1, _iBw.Get(key).Value, "Unexpected value of decrement operation!");
			_iBw.Remove(key);
		}

		[TestMethod]
		public void TestBucketDecrement2()
		{
			string key = "decrement_2" + Environment.TickCount;
			ulong initial = 37;
			ulong delta = 8;
			_iBw.Insert(key, initial, 60);
			_oRw = _iBw.Decrement(key, delta, initial, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected success of decrement operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of decrement operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(initial - delta, _oRw.Value, "Unexpected value of decrement operation!");
			Assert.AreEqual((Int64)(initial - delta), _iBw.Get(key).Value, "Unexpected value of decrement operation!");
		}

		[TestMethod]
		public void TestBucketDecrement3()
		{
			string key = "decrement_3" + Environment.TickCount;
			ulong initial = 0;
			ulong delta = 0;
			_iBw.Insert(key, initial, 60);
			_oRw = _iBw.Decrement(key, delta, initial, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected success of decrement operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of decrement operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(initial - delta, _oRw.Value, "Unexpected value of decrement operation!");
			Assert.AreEqual((Int64)(initial - delta), _iBw.Get(key).Value, "Unexpected value of decrement operation!");
		}

		[TestMethod]
		public void TestBucketDecrement4()
		{
			string key = "decrement_4" + Environment.TickCount;
			ulong initial = 55;
			ulong delta = 5;
			_oRw = _iBw.Decrement(key, delta, initial, 60);
			Assert.IsTrue(_oRw.Success, "Unexpected success of increment operation!");
			Assert.AreEqual(0, _oRw.Status, "Unexpected status of increment operation!");
			Assert.IsNotNull(_oRw.Message, "Message was null!");
			Assert.AreEqual(initial, _oRw.Value, "Unexpected value of increment operation!");
			Assert.AreEqual((Int64)(initial), _iBw.Get(key).Value, "Unexpected value of increment operation!");
		}

		[TestMethod]
		[TestCategory("Expiration")]
		public void TestBucketDecrement5()
		{
			string key = "decrement_5" + Environment.TickCount;
			ulong initial = 55;
			ulong delta = 5;
			_oRw = _iBw.Decrement(key, delta, initial, 1);
			Assert.AreEqual(initial, _oRw.Value, "Unexpected value of decrement operation!");
			System.Threading.Thread.Sleep(1000);
			_oRw = _iBw.Get(key);
			Assert.AreEqual(null, _oRw.Value, "Unexpected value of decrement operation!");
			Assert.IsFalse(_oRw.Success, "Unexpected succes of decrement operation!");
			Assert.AreEqual(1, _oRw.Status, "Unexpected status of decrement operation!");
		}
	}
}