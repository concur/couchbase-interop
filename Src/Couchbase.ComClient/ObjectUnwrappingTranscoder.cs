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
using Couchbase.Core.Transcoders;
using Couchbase.IO.Operations;

namespace Couchbase.ComClient
{
	//use the same implementation as DefaultTranscoder, but base the data type on actual value type rather than T
	public class ObjectUnwrappingTranscoder : DefaultTranscoder
	{
		public override T Decode<T>(byte[] buffer, int offset, int length, Flags flags, OperationCode opcode)
		{
			object value;
			switch (flags.DataFormat)
			{
				case DataFormat.Reserved:
				case DataFormat.Private:
					if (typeof(T) == typeof(byte[]))
					{
						value = DecodeBinary(buffer, offset, length);
					}
					else
					{
						value = Decode<T>(buffer, offset, length, opcode);
					}
					break;

				case DataFormat.Json:
					if (typeof(T) == typeof(string))
					{
						value = DecodeString(buffer, offset, length);
					}
					else
					{
						value = DeserializeAsJson<T>(buffer, offset, length);
					}
					break;

				case DataFormat.Binary:
					if (typeof(T) == typeof(byte[]) || typeof(T) == typeof(object))
					{
						value = DecodeBinary(buffer, offset, length);
					}
					else
					{
						string msg = $"The value of T does not match the DataFormat provided: {flags.DataFormat}";
						throw new ArgumentException(msg);
					}
					break;

				case DataFormat.String:
					if (typeof(T) == typeof(char))
					{
						value = DecodeChar(buffer, offset, length);
					}
					else
					{
						value = DecodeString(buffer, offset, length);
					}
					break;

				default:
					value = DecodeString(buffer, offset, length);
					break;
			}

			return (T)value;
		}

		public override byte[] Encode<T>(T value, Flags flags, OperationCode opcode)
		{
			byte[] bytes;
			switch (flags.DataFormat)
			{
				case DataFormat.Reserved:
				case DataFormat.Private:
				case DataFormat.String:
					bytes = Encode(value, flags.TypeCode, opcode);
					break;

				case DataFormat.Json:
					bytes = SerializeAsJson(value);
					break;

				case DataFormat.Binary:
					if (value.GetType() == typeof(byte[]))
					{
						bytes = value as byte[];
					}
					else
					{
						string msg = $"The value of T does not match the DataFormat provided: {flags.DataFormat}";
						throw new ArgumentException(msg);
					}
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(flags), "Flag DataFormat is not supported!");
			}
			return bytes;
		}

		public override Flags GetFormat<T>(T value)
		{
			var dataFormat = DataFormat.Json;
			var typeCode = TypeCode.Object;
			if (value != null)
			{
				var type = value.GetType();
				typeCode = Type.GetTypeCode(type);
				switch (typeCode)
				{
					case TypeCode.Object:
						if (type == typeof(Byte[]))
						{
							dataFormat = DataFormat.Binary;
						}
						break;
					case TypeCode.Boolean:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
					case TypeCode.DateTime:
						dataFormat = DataFormat.Json;
						break;
					case TypeCode.Char:
					case TypeCode.String:
						dataFormat = DataFormat.String;
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(value), "TypeCode is not supported!");
				}
			}
			return new Flags() { Compression = Compression.None, DataFormat = dataFormat, TypeCode = typeCode };
		}
	}
}
