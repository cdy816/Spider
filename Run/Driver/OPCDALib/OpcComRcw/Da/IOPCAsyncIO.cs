using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x0200009E RID: 158
	[Guid("39c13a53-011e-11d0-9675-0020afd8adb3")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCAsyncIO
	{
		// Token: 0x06000125 RID: 293
		void Read([MarshalAs(UnmanagedType.I4)] int dwConnection, OPCDATASOURCE dwSource, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] phServer, [MarshalAs(UnmanagedType.I4)] out int pTransactionID, out IntPtr ppErrors);

		// Token: 0x06000126 RID: 294
		void Write([MarshalAs(UnmanagedType.I4)] int dwConnection, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 1)] object[] pItemValues, [MarshalAs(UnmanagedType.I4)] out int pTransactionID, out IntPtr ppErrors);

		// Token: 0x06000127 RID: 295
		void Refresh([MarshalAs(UnmanagedType.I4)] int dwConnection, OPCDATASOURCE dwSource, [MarshalAs(UnmanagedType.I4)] out int pTransactionID);

		// Token: 0x06000128 RID: 296
		void Cancel([MarshalAs(UnmanagedType.I4)] int dwTransactionID);
	}
}
