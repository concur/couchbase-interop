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
	public class ObjectUnwrappingTranscoder : DefaultTranscoder, ITypeTranscoder
	{
		//this will override the GetFormat<T> implementation from DefaultTranscoder
		Flags ITypeTranscoder.GetFormat<T>(T value)
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
						throw new ArgumentOutOfRangeException();
				}
			}
			return new Flags() { Compression = Compression.None, DataFormat = dataFormat, TypeCode = typeCode };
		}
	}
}
