using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x020000A9 RID: 169
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0967B97B-36EF-423e-B6F8-6BFF1E40D39D")]
	[ComImport]
	public interface IOPCAsyncIO3
	{
		// Token: 0x06000151 RID: 337
		void Read([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x06000152 RID: 338
		void Write([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 0)] object[] pItemValues, [MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x06000153 RID: 339
		void Refresh2(OPCDATASOURCE dwSource, [MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] out int pdwCancelID);

		// Token: 0x06000154 RID: 340
		void Cancel2([MarshalAs(UnmanagedType.I4)] int dwCancelID);

		// Token: 0x06000155 RID: 341
		void SetEnable([MarshalAs(UnmanagedType.I4)] int bEnable);

		// Token: 0x06000156 RID: 342
		void GetEnable([MarshalAs(UnmanagedType.I4)] out int pbEnable);

		// Token: 0x06000157 RID: 343
		void ReadMaxAge([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] pdwMaxAge, [MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x06000158 RID: 344
		void WriteVQT([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] OPCITEMVQT[] pItemVQT, [MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] out int pdwCancelID, out IntPtr ppErrors);

		// Token: 0x06000159 RID: 345
		void RefreshMaxAge([MarshalAs(UnmanagedType.I4)] int dwMaxAge, [MarshalAs(UnmanagedType.I4)] int dwTransactionID, [MarshalAs(UnmanagedType.I4)] out int pdwCancelID);
	}
}
