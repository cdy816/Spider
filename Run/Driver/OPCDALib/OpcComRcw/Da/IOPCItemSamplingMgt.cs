using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x020000A5 RID: 165
	[Guid("3E22D313-F08B-41a5-86C8-95E95CB49FFC")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCItemSamplingMgt
	{
		// Token: 0x06000144 RID: 324
		void SetItemSamplingRate([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] pdwRequestedSamplingRate, out IntPtr ppdwRevisedSamplingRate, out IntPtr ppErrors);

		// Token: 0x06000145 RID: 325
		void GetItemSamplingRate([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, out IntPtr ppdwSamplingRate, out IntPtr ppErrors);

		// Token: 0x06000146 RID: 326
		void ClearItemSamplingRate([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, out IntPtr ppErrors);

		// Token: 0x06000147 RID: 327
		void SetItemBufferEnable([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] pbEnable, out IntPtr ppErrors);

		// Token: 0x06000148 RID: 328
		void GetItemBufferEnable([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, out IntPtr ppbEnable, out IntPtr ppErrors);
	}
}
