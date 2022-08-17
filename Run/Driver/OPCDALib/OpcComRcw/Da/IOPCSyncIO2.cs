using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x020000A8 RID: 168
	[Guid("730F5F0F-55B1-4c81-9E18-FF8A0904E1FA")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCSyncIO2
	{
		// Token: 0x0600014D RID: 333
		void Read(OPCDATASOURCE dwSource, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] phServer, out IntPtr ppItemValues, out IntPtr ppErrors);

		// Token: 0x0600014E RID: 334
		void Write([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 0)] object[] pItemValues, out IntPtr ppErrors);

		// Token: 0x0600014F RID: 335
		void ReadMaxAge([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] pdwMaxAge, out IntPtr ppvValues, out IntPtr ppwQualities, out IntPtr ppftTimeStamps, out IntPtr ppErrors);

		// Token: 0x06000150 RID: 336
		void WriteVQT([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] OPCITEMVQT[] pItemVQT, out IntPtr ppErrors);
	}
}
