using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x020000A2 RID: 162
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("39c13a71-011e-11d0-9675-0020afd8adb3")]
	[ComImport]
	public interface IOPCAsyncIO2
	{
		// Token: 0x06000138 RID: 312
		void Read([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x06000139 RID: 313
		void Write([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 0)] object[] pItemValues, [MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x0600013A RID: 314
		void Refresh2(OPCDATASOURCE dwSource, [MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] out int pdwCancelID);

		// Token: 0x0600013B RID: 315
		void Cancel2([MarshalAs(UnmanagedType.I4)] int dwCancelID);

		// Token: 0x0600013C RID: 316
		void SetEnable([MarshalAs(UnmanagedType.I4)] int bEnable);

		// Token: 0x0600013D RID: 317
		void GetEnable([MarshalAs(UnmanagedType.I4)] out int pbEnable);
	}
}
