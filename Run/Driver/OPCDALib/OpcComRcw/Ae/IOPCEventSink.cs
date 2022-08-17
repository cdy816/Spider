using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Ae
{
	// Token: 0x02000079 RID: 121
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6516885F-5783-11D1-84A0-00608CB8A7E9")]
	[ComImport]
	public interface IOPCEventSink
	{
		// Token: 0x060000EF RID: 239
		void OnEvent([MarshalAs(UnmanagedType.I4)] int hClientSubscription, [MarshalAs(UnmanagedType.I4)] int bRefresh, [MarshalAs(UnmanagedType.I4)] int bLastRefresh, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 3)] ONEVENTSTRUCT[] pEvents);
	}
}
