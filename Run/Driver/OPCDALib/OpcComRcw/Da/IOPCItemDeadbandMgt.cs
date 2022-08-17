using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x020000A4 RID: 164
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("5946DA93-8B39-4ec8-AB3D-AA73DF5BC86F")]
	[ComImport]
	public interface IOPCItemDeadbandMgt
	{
		// Token: 0x06000141 RID: 321
		void SetItemDeadband([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4, SizeParamIndex = 0)] float[] pPercentDeadband, out IntPtr ppErrors);

		// Token: 0x06000142 RID: 322
		void GetItemDeadband([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, out IntPtr ppPercentDeadband, out IntPtr ppErrors);

		// Token: 0x06000143 RID: 323
		void ClearItemDeadband([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, out IntPtr ppErrors);
	}
}
