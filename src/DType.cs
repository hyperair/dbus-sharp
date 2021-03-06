// Copyright 2006 Alp Toker <alp@atoker.com>
// Copyright 2010 Alan McGovern <alan.mcgovern@gmail.com>s
// This software is made available under the MIT License
// See COPYING for details

using System;

namespace DBus
{
	enum DType : byte
	{
		Invalid = (byte)'\0',

		Byte = (byte)'y',
		Boolean = (byte)'b',
		Int16 = (byte)'n',
		UInt16 = (byte)'q',
		Int32 = (byte)'i',
		UInt32 = (byte)'u',
		Int64 = (byte)'x',
		UInt64 = (byte)'t',
		Single = (byte)'f', //This is not yet supported!
		Double = (byte)'d',
		String = (byte)'s',
		ObjectPath = (byte)'o',
		Signature = (byte)'g',

		Array = (byte)'a',
		[Obsolete ("Not in protocol")]
		Struct = (byte)'r',
		[Obsolete ("Not in protocol")]
		DictEntry = (byte)'e',
		Variant = (byte)'v',

		StructBegin = (byte)'(',
		StructEnd = (byte)')',
		DictEntryBegin = (byte)'{',
		DictEntryEnd = (byte)'}',
	}
}
