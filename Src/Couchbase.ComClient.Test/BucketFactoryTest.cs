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
	public class BucketFactoryTest
	{
		private static IBucketFactory _iBf;

		[ClassInitialize()]
		public static void TestInitialize(TestContext context)
		{
			//this ensures tests don't mess with each other
			_iBf = new BucketFactory();
			_iBf.ConfigureCluster("Couchbase.ComClient.Test.dll.config", "local");
		}

		[ClassCleanup()]
		public static void TestFinalize()
		{
			_iBf.CloseBucket("default");
			_iBf.CloseCluster("local");
		}

		[TestMethod]
		public void TestCluster()
		{
			_iBf.ConfigureCluster("Couchbase.ComClient.Test.dll.config", "local");
			Assert.IsTrue(_iBf.IsClusterOpen("local"), "Cluster local was not opened");
			_iBf.CloseCluster("local");
			Assert.IsFalse(_iBf.IsClusterOpen("local"), "Cluster local was opened");
		}

		[TestMethod]
		public void TestCluster2()
		{
			Assert.IsFalse(_iBf.IsClusterOpen("nonexist"), "Cluster nonexist was opened");
			_iBf.CloseCluster("nonexist");
			Assert.IsFalse(_iBf.IsClusterOpen("nonexist"), "Cluster nonexist was opened");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void TestCluster3()
		{
			_iBf.ConfigureCluster("non_existing.config", "XXX");
		}

		[TestMethod]
		public void TestBucket()
		{
			IBucketWrapper bucket = _iBf.GetBucket("default", "local");
			Assert.IsNotNull(bucket);
			Assert.IsTrue(_iBf.IsBucketOpen("default"), "Bucket default was not opened");
			_iBf.CloseBucket("default");
			Assert.IsFalse(_iBf.IsBucketOpen("default"), "Bucket default was opened");
			_iBf.CloseBucket("default");
			Assert.IsFalse(_iBf.IsBucketOpen("default"), "Bucket default was opened");
		}


		[TestMethod]
		public void TestBucket2()
		{
			Assert.IsFalse(_iBf.IsBucketOpen("nonexist"), "Bucket nonexist was opened");
			_iBf.CloseBucket("nonexist");
			Assert.IsFalse(_iBf.IsBucketOpen("nonexist"), "Bucket nonexist was opened");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException), "Cluster 'nonexist' was not configured. Use the ConfigureCluster method first")]
		public void TestBucket3()
		{
			_iBf.GetBucket("default", "nonexist");
		}

		[TestMethod]
		[ExpectedException(typeof(AggregateException), "Could not bootstrap - check inner exceptions for details.")]
		public void TestBucket4()
		{
			_iBf.GetBucket("nonexist");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException), "There are no known clusters. Use the ConfigureCluster method first.")]
		public void TestBucket5()
		{
			_iBf.CloseCluster("local");
			_iBf.GetBucket("default");
		}
	}
}
