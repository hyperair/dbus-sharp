// Copyright 2006 Alp Toker <alp@atoker.com>
// This software is made available under the MIT License
// See COPYING for details

//defined by default, since this is not a controversial extension
#define PROTO_TYPE_SINGLE

using System;
using System.Collections.Generic;

namespace NDesk.DBus
{
	//yyyyuua{yv}
	public struct Header
	{
		public EndianFlag Endianness;
		public MessageType MessageType;
		public HeaderFlag Flags;
		public byte MajorVersion;
		public uint Length;
		public uint Serial;
		//public HeaderField[] Fields;
		public IDictionary<FieldCode,object> Fields;

		/*
		public static DType TypeForField (FieldCode f)
		{
			switch (f) {
				case FieldCode.Invalid:
					return DType.Invalid;
				case FieldCode.Path:
					return DType.ObjectPath;
				case FieldCode.Interface:
					return DType.String;
				case FieldCode.Member:
					return DType.String;
				case FieldCode.ErrorName:
					return DType.String;
				case FieldCode.ReplySerial:
					return DType.UInt32;
				case FieldCode.Destination:
					return DType.String;
				case FieldCode.Sender:
					return DType.String;
				case FieldCode.Signature:
					return DType.Signature;
#if PROTO_REPLY_SIGNATURE
				case FieldCode.ReplySignature: //note: not supported in dbus
					return DType.Signature;
#endif
				default:
					return DType.Invalid;
			}
		}
		*/
	}

	/*
	public struct HeaderField
	{
		//public HeaderField (FieldCode code, object value)
		//{
		//	this.Code = code;
		//	this.Value = value;
		//}

		public static HeaderField Create (FieldCode code, object value)
		{
			HeaderField hf;

			hf.Code = code;
			hf.Value = value;

			return hf;
		}

		public FieldCode Code;
		public object Value;
	}
	*/

	public enum MessageType : byte
	{
		//This is an invalid type.
		Invalid,
		//Method call.
		MethodCall,
		//Method reply with returned data.
		MethodReturn,
		//Error reply. If the first argument exists and is a string, it is an error message.
		Error,
		//Signal emission.
		Signal,
	}

	public enum FieldCode : byte
	{
		Invalid,
			Path,
			Interface,
			Member,
			ErrorName,
			ReplySerial,
			Destination,
			Sender,
			Signature,
#if PROTO_REPLY_SIGNATURE
			ReplySignature, //note: not supported in dbus
#endif
	}

	public enum EndianFlag : byte
	{
		Little = (byte)'l',
		Big = (byte)'B',
	}

	[Flags]
	public enum HeaderFlag : byte
	{
		None = 0,
		NoReplyExpected = 0x1,
		NoAutoStart = 0x2,
	}

	public struct ObjectPath
	{
		public static readonly ObjectPath Root = new ObjectPath ("/");

		public string Value;

		public ObjectPath (string value)
		{
			this.Value = value;
		}

		public override string ToString ()
		{
			return Value;
		}

		//this may or may not prove useful
		public string[] Decomposed
		{
			get {
				return Value.Split ('/');
			} set {
				Value = String.Join ("/", value);
			}
		}
	}

	public static class Protocol
	{
		//protocol versions that we support
		public const byte MinVersion = 0;
		public const byte Version = 1;
#if PROTO_TYPE_SINGLE
		public const byte MaxVersion = 2;
#else
		public const byte MaxVersion = 1;
#endif

		public static int PadNeeded (int pos, int alignment)
		{
			int pad = pos % alignment;
			pad = pad == 0 ? 0 : alignment - pad;

			return pad;
		}

		public static int Padded (int pos, int alignment)
		{
			int pad = pos % alignment;
			if (pad != 0)
				pos += alignment - pad;

			return pos;
		}

		public static int GetAlignment (DType dtype)
		{
			switch (dtype) {
				case DType.Byte:
					return 1;
				case DType.Boolean:
					return 4;
				case DType.Int16:
				case DType.UInt16:
					return 2;
				case DType.Int32:
				case DType.UInt32:
					return 4;
				case DType.Int64:
				case DType.UInt64:
					return 8;
#if PROTO_TYPE_SINGLE
				case DType.Single: //Not yet supported!
					return 4;
#endif
				case DType.Double:
					return 8;
				case DType.String:
					return 4;
				case DType.ObjectPath:
					return 4;
				case DType.Signature:
					return 1;
				case DType.Array:
					return 4;
				case DType.Struct:
					return 8;
				case DType.Variant:
					return 1;
				case DType.DictEntry:
					return 8;
				case DType.Invalid:
				default:
					throw new Exception ("Cannot determine alignment of " + dtype);
			}
		}

		//this class may not be the best place for Verbose
		public readonly static bool Verbose;

		static Protocol ()
		{
			Verbose = !String.IsNullOrEmpty (Environment.GetEnvironmentVariable ("DBUS_VERBOSE"));
		}
	}

#if UNDOCUMENTED_IN_SPEC
/*
"org.freedesktop.DBus.Error.Failed"
"org.freedesktop.DBus.Error.NoMemory"
"org.freedesktop.DBus.Error.ServiceUnknown"
"org.freedesktop.DBus.Error.NameHasNoOwner"
"org.freedesktop.DBus.Error.NoReply"
"org.freedesktop.DBus.Error.IOError"
"org.freedesktop.DBus.Error.BadAddress"
"org.freedesktop.DBus.Error.NotSupported"
"org.freedesktop.DBus.Error.LimitsExceeded"
"org.freedesktop.DBus.Error.AccessDenied"
"org.freedesktop.DBus.Error.AuthFailed"
"org.freedesktop.DBus.Error.NoServer"
"org.freedesktop.DBus.Error.Timeout"
"org.freedesktop.DBus.Error.NoNetwork"
"org.freedesktop.DBus.Error.AddressInUse"
"org.freedesktop.DBus.Error.Disconnected"
"org.freedesktop.DBus.Error.InvalidArgs"
"org.freedesktop.DBus.Error.FileNotFound"
"org.freedesktop.DBus.Error.UnknownMethod"
"org.freedesktop.DBus.Error.TimedOut"
"org.freedesktop.DBus.Error.MatchRuleNotFound"
"org.freedesktop.DBus.Error.MatchRuleInvalid"
"org.freedesktop.DBus.Error.Spawn.ExecFailed"
"org.freedesktop.DBus.Error.Spawn.ForkFailed"
"org.freedesktop.DBus.Error.Spawn.ChildExited"
"org.freedesktop.DBus.Error.Spawn.ChildSignaled"
"org.freedesktop.DBus.Error.Spawn.Failed"
"org.freedesktop.DBus.Error.UnixProcessIdUnknown"
"org.freedesktop.DBus.Error.InvalidSignature"
"org.freedesktop.DBus.Error.SELinuxSecurityContextUnknown"
*/
#endif
}
