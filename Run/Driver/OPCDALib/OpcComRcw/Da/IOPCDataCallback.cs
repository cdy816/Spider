using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x020000A1 RID: 161
	[Guid("39c13a70-011e-11d0-9675-0020afd8adb3")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCDataCallback
	{
		// Token: 0x06000134 RID: 308
		void OnDataChange([MarshalAs(UnmanagedType.I4)] int dwTransid, [MarshalAs(UnmanagedType.I4)] int hGroup, [MarshalAs(UnmanagedType.I4)] int hrMasterquality, [MarshalAs(UnmanagedType.I4)] int hrMastererror, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 4)] int[] phClientItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 4)] object[] pvValues, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I2, SizeParamIndex = 4)] short[] pwQualities, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 4)] FILETIME[] pftTimeStamps, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 4)] int[] pErrors);

		// Token: 0x06000135 RID: 309
		void OnReadComplete([MarshalAs(UnmanagedType.I4)] int dwTransid, [MarshalAs(UnmanagedType.I4)] int hGroup, [MarshalAs(UnmanagedType.I4)] int hrMasterquality, [MarshalAs(UnmanagedType.I4)] int hrMastererror, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 4)] int[] phClientItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 4)] object[] pvValues, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I2, SizeParamIndex = 4)] short[] pwQualities, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 4)] FILETIME[] pftTimeStamps, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 4)] int[] pErrors);

		// Token: 0x06000136 RID: 310
		void OnWriteComplete([MarshalAs(UnmanagedType.I4)] int dwTransid, [MarshalAs(UnmanagedType.I4)] int hGroup, [MarshalAs(UnmanagedType.I4)] int hrMastererr, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 3)] int[] pClienthandles, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 3)] int[] pErrors);

		// Token: 0x06000137 RID: 311
		void OnCancelComplete([MarshalAs(UnmanagedType.I4)] int dwTransid, [MarshalAs(UnmanagedType.I4)] int hGroup);
	}
}
