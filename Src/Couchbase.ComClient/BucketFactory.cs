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
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Couchbase.Configuration.Client;
using Couchbase.Configuration.Client.Providers;
using Couchbase.Core;

namespace Couchbase.ComClient
{
	[Guid("267DA2E9-727D-4D63-A9D6-D2391331A738")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IBucketFactory
	{
		[DispId(1)]
		void ConfigureCluster(string configPath, string sectionName, string clusterName = null);
		[DispId(2)]
		IBucketWrapper GetBucket(string bucketName, string clusterName = null);
		[DispId(3)]
		bool IsBucketOpen(string bucketName);
		[DispId(4)]
		bool IsClusterOpen(string clusterName);
		[DispId(5)]
		void CloseBucket(string bucketName);
		[DispId(6)]
		void CloseCluster(string clusterName);
		[DispId(7)]
		string Version { get; }
	}

	[Guid("FD25BAA3-3F34-4C97-9621-3C384E1F4591")]
	[ClassInterface(ClassInterfaceType.None)]
	[ProgId("Couchbase.ComClient.BucketFactory")]
	public class BucketFactory : IBucketFactory
	{
		private class ClusterInfo
		{
			public Cluster Cluster;
			public List<IBucket> Buckets;

			public ClusterInfo(Cluster cluster)
			{
				Cluster = cluster;
				Buckets = new List<IBucket>();
			}
		}

		private static readonly string s_version = typeof(BucketFactory).Assembly.GetName().Version.ToString();

		private static ReaderWriterLockSlim s_lock = new ReaderWriterLockSlim();
		private static Dictionary<string, ClusterInfo> s_clusters = new Dictionary<string, ClusterInfo>();
		private static Dictionary<string, IBucket> s_buckets = new Dictionary<string, IBucket>();

		//System.Configuration keeps refusing to use the Couchbase.NetClient assembly already loaded in memory
		//Fix this by manually resolving the assembly
		static BucketFactory()
		{
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
		}
		static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			if (args.Name.StartsWith("Couchbase.NetClient"))
			{
				return typeof(CouchbaseClientSection).Assembly;
			}
			return null;
		}

		public void ConfigureCluster(string configPath, string sectionName, string clusterName = null)
		{
			if (string.IsNullOrEmpty(clusterName)) clusterName = sectionName;

			//read new cluster configuration and create new instance
			var configFile = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(configPath));
			var clientConfig = new ClientConfiguration((CouchbaseClientSection)configFile.GetSection(sectionName));
			//override the transcoder setting to unwrap object on COM API
			clientConfig.Transcoder = () => new ObjectUnwrappingTranscoder();
			
			var cluster = new Cluster(clientConfig);

			ClusterInfo oldCluster = null;

			//make cluster swap fast without any unneccessary code inside the lock
			s_lock.EnterWriteLock();
			try
			{
				//if cluster with same name already exists, save the old instance for later disposal and remove its buckets
				if (s_clusters.TryGetValue(clusterName, out oldCluster))
				{
					foreach (IBucket bucket in oldCluster.Buckets)
					{
						s_buckets.Remove(bucket.Name);
					}
				}

				s_clusters[clusterName] = new ClusterInfo(cluster);
			}
			finally
			{
				s_lock.ExitWriteLock();
			}

			//dispose all objects tied to the old cluster
			if (oldCluster != null)
			{
				try
				{
					foreach (IBucket bucket in oldCluster.Buckets)
					{
						bucket.Dispose();
					}
					oldCluster.Cluster.Dispose();
				}
				catch {}
			}
		}

		public IBucketWrapper GetBucket(string bucketName, string clusterName = null)
		{
			IBucket bucket;
			
			//keep separate block with read lock first - do not mix with write or upgradable lock access
			s_lock.EnterReadLock();
			try
			{
				if (s_buckets.TryGetValue(bucketName, out bucket))
				{
					return new BucketWrapper(bucket);
				}
			}
			finally
			{
				s_lock.ExitReadLock();
			}

			//bucket was not initialized before, enter write lock
			s_lock.EnterWriteLock();
			try
			{
				//double check if bucket was open on another thread
				if (!s_buckets.TryGetValue(bucketName, out bucket))
				{
					ClusterInfo ci;
					//simplify work with single cluster (most common use case) by skipping clusterName
					if (string.IsNullOrEmpty(clusterName))
					{
						if (s_clusters.Count == 1)
						{
							ci = s_clusters.Values.First();
						}
						else
						{
							throw new InvalidOperationException("There are no known clusters. Use the ConfigureCluster method first.");
						}
					}
					else if (!s_clusters.TryGetValue(clusterName, out ci))
					{
						throw new InvalidOperationException("Cluster '" + clusterName + "' was not configured. Use the ConfigureCluster method first.");
					}

					bucket = ci.Cluster.OpenBucket(bucketName);
					ci.Buckets.Add(bucket);
					s_buckets.Add(bucketName, bucket);
				}
				return new BucketWrapper(bucket);
			}
			finally
			{
				s_lock.ExitWriteLock();
			}
		}

		public bool IsBucketOpen(string bucketName)
		{
			s_lock.EnterReadLock();
			try
			{
				return s_buckets.ContainsKey(bucketName);
			}
			finally
			{
				s_lock.ExitReadLock();
			}
		}

		public bool IsClusterOpen(string clusterName)
		{
			s_lock.EnterReadLock();
			try
			{
				return s_clusters.ContainsKey(clusterName);
			}
			finally
			{
				s_lock.ExitReadLock();
			}
		}

		public void CloseBucket(string bucketName)
		{
			IBucket bucket;

			s_lock.EnterWriteLock();
			try
			{
				if (s_buckets.TryGetValue(bucketName, out bucket))
				{
					s_buckets.Remove(bucketName);
				}
			}
			finally
			{
				s_lock.ExitWriteLock();
			}

			if (bucket != null)
			{
				try
				{
					bucket.Dispose();
				}
				catch { }
			}
		}

		public void CloseCluster(string clusterName)
		{
			ClusterInfo cluster;

			s_lock.EnterWriteLock();
			try
			{
				if (s_clusters.TryGetValue(clusterName, out cluster))
				{
					foreach (IBucket bucket in cluster.Buckets)
					{
						s_buckets.Remove(bucket.Name);
					}
					s_clusters.Remove(clusterName);
				}
			}
			finally
			{
				s_lock.ExitWriteLock();
			}

			if (cluster != null)
			{
				try
				{
					foreach (IBucket bucket in cluster.Buckets)
					{
						bucket.Dispose();
					}
					cluster.Cluster.Dispose();
				}
				catch { }
			}
		}

		public string Version
		{
			get { return s_version; }
		}
	}
}
