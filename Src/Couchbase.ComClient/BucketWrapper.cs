using System;
using System.Runtime.InteropServices;
using Couchbase.Core;

namespace Couchbase.ComClient
{
	[Guid("C12A287D-5726-4E06-AAAF-F20F3E03C6F6")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IBucketWrapper
	{
		[DispId(1)]
		bool Exists(string key);
		[DispId(2)]
		OperationResultWrapper Get(string key);
		[DispId(3)]
		OperationResultWrapper GetAndTouch(string key, uint expiration);
		[DispId(4)]
		OperationResultWrapper Touch(string key, uint expiration);
		[DispId(5)]
		OperationResultWrapper Insert(string key, object value, uint expiration = 0);
		[DispId(6)]
		OperationResultWrapper Remove(string key);
		[DispId(7)]
		OperationResultWrapper RemoveCas(string key, ulong cas);
		[DispId(8)]
		OperationResultWrapper Replace(string key, object value, uint expiration = 0);
		[DispId(9)]
		OperationResultWrapper ReplaceCas(string key, object value, ulong cas, uint expiration = 0);
		[DispId(10)]
		OperationResultWrapper Upsert(string key, object value, uint expiration = 0);
		[DispId(11)]
		OperationResultWrapper UpsertCas(string key, object value, ulong cas, uint expiration = 0);
		[DispId(12)]
		OperationResultWrapper Append(string key, string value);
		[DispId(13)]
		OperationResultWrapper Prepend(string key, string value);
		[DispId(14)]
		OperationResultWrapper Increment(string key, ulong delta = 1, ulong initial = 1, uint expiration = 0);
		[DispId(15)]
		OperationResultWrapper Decrement(string key, ulong delta = 1, ulong initial = 1, uint expiration = 0);
	}

	[Guid("249D4D5A-218D-421A-823F-99DB30AF97CC")]
	[ClassInterface(ClassInterfaceType.None)]
	public class BucketWrapper : IBucketWrapper
	{
		private IBucket m_bucket;

		internal BucketWrapper(IBucket bucket)
		{
			m_bucket = bucket;
		}

		public bool Exists(string key)
		{
			return m_bucket.Exists(key);
		}

		public OperationResultWrapper Get(string key)
		{
			return OperationResultWrapper.Create(m_bucket.Get<object>(key));
		}

		public OperationResultWrapper GetAndTouch(string key, uint expiration)
		{
			return OperationResultWrapper.Create(m_bucket.GetAndTouch<object>(key, TimeSpan.FromSeconds(expiration)));
		}

		public OperationResultWrapper Touch(string key, uint expiration)
		{
			return OperationResultWrapper.Create(m_bucket.Touch(key, TimeSpan.FromSeconds(expiration)));
		}

		public OperationResultWrapper Insert(string key, object value, uint expiration = 0)
		{
			return OperationResultWrapper.Create(m_bucket.Insert<object>(key, value, expiration));
		}

		public OperationResultWrapper Remove(string key)
		{
			return OperationResultWrapper.Create(m_bucket.Remove(key));
		}

		public OperationResultWrapper RemoveCas(string key, ulong cas)
		{
			return OperationResultWrapper.Create(m_bucket.Remove(key, cas));
		}

		public OperationResultWrapper Replace(string key, object value, uint expiration = 0)
		{
			return OperationResultWrapper.Create(m_bucket.Replace<object>(key, value, expiration));
		}

		public OperationResultWrapper ReplaceCas(string key, object value, ulong cas, uint expiration = 0)
		{
			return OperationResultWrapper.Create(m_bucket.Replace<object>(key, value, cas, expiration));
		}

		public OperationResultWrapper Upsert(string key, object value, uint expiration = 0)
		{
			return OperationResultWrapper.Create(m_bucket.Upsert<object>(key, value, expiration));
		}

		public OperationResultWrapper UpsertCas(string key, object value, ulong cas, uint expiration = 0)
		{
			return OperationResultWrapper.Create(m_bucket.Upsert<object>(key, value, cas, expiration));
		}

		public OperationResultWrapper Append(string key, string value)
		{
			return OperationResultWrapper.Create(m_bucket.Append(key, value));
		}

		public OperationResultWrapper Prepend(string key, string value)
		{
			return OperationResultWrapper.Create(m_bucket.Prepend(key, value));
		}

		public OperationResultWrapper Increment(string key, ulong delta = 1, ulong initial = 1, uint expiration = 0)
		{
			return OperationResultWrapper.Create(m_bucket.Increment(key, delta, initial, expiration));
		}

		public OperationResultWrapper Decrement(string key, ulong delta = 1, ulong initial = 1, uint expiration = 0)
		{
			return OperationResultWrapper.Create(m_bucket.Decrement(key, delta, initial, expiration));
		}
	}
}
