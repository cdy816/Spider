using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x0200009D RID: 157
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("39c13a52-011e-11d0-9675-0020afd8adb3")]
	[ComImport]
	public interface IOPCSyncIO
	{
		// Token: 0x06000123 RID: 291
		void Read(OPCDATASOURCE dwSource, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] phServer, out IntPtr ppItemValues, out IntPtr ppErrors);

		// Token: 0x06000124 RID: 292
		void Write([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 0)] object[] pItemValues, out IntPtr ppErrors);
	}
}
