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
		private static IBucketWrapper _iBw;

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
		[ExpectedException(typeof(System.ArgumentException))]
		public void TestException()
		{
			_iBf.ConfigureCluster("non_existing.config", "XXX");
		}

		[TestMethod]
		public void TestConfigureCluster()
		{
			Assert.IsTrue(_iBf.IsClusterOpen("local"), "Cluster local was not opened");
			_iBf.CloseCluster("local");
			Assert.IsFalse(_iBf.IsClusterOpen("local"), "Cluster local was opened");
		}
	}
}
