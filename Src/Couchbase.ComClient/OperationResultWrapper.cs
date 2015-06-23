using System;
using System.Runtime.InteropServices;

namespace Couchbase.ComClient
{
	[Guid("F4619C4B-3930-4DE7-92AD-5EC0BD1CC017")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IOperationResultWrapper
	{
		//DispId 0 is special value designating default property of the object
		//This will behave similar like ADODB Field.Value
		[DispId(0)]
		object Value { get; }
		[DispId(1)]
		bool Success { get; }
		[DispId(2)]
		int Status { get; }
		[DispId(3)]
		string Message { get; }
		[DispId(4)]
		string ExceptionData { get; }
		[DispId(5)]
		ulong Cas { get; }
		[DispId(6)]
		bool ShouldRetry();
	}

	[Guid("7128DCB8-FC58-4A47-AE4D-92D40EDB37D5")]
	[ClassInterface(ClassInterfaceType.None)]
	public class OperationResultWrapper : IOperationResultWrapper
	{
		//We don't want generics on public COM interface. Work around this by hiding the generics on internal factory method.
		internal static OperationResultWrapper Create<T>(IOperationResult<T> result)
		{
			return new OperationResultWrapper() { m_result = result, Value = result.Value };
		}
		internal static OperationResultWrapper Create(IOperationResult result)
		{
			return new OperationResultWrapper() { m_result = result, Value = null };
		}
		//Force use of the factory methods by making constructor private
		private OperationResultWrapper() { }

		private IOperationResult m_result;

		public object Value
		{
			get;
			private set;
		}

		public bool Success
		{
			get { return m_result.Success; }
		}

		public int Status
		{
			get { return (int)m_result.Status; }
		}

		public string Message
		{
			get { return m_result.Message; }
		}

		public string ExceptionData
		{
			get { return m_result.Exception != null ? m_result.Exception.ToString() : null; }
		}

		public ulong Cas
		{
			get { return m_result.Cas; }
		}

		public bool ShouldRetry()
		{
			return m_result.ShouldRetry();
		}
	}

}
