using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Hda
{
	// Token: 0x02000044 RID: 68
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("1F1217B3-DEE0-11d2-A5E5-000086339399")]
	[ComImport]
	public interface IOPCHDA_SyncUpdate
	{
		// Token: 0x06000064 RID: 100
		void QueryCapabilities(out OPCHDA_UPDATECAPABILITIES pCapabilities);

		// Token: 0x06000065 RID: 101
		void Insert([MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] OPCHDA_FILETIME[] ftTimeStamps, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 0)] object[] vDataValues, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] pdwQualities, out IntPtr ppErrors);

		// Token: 0x06000066 RID: 102
		void Replace([MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] OPCHDA_FILETIME[] ftTimeStamps, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 0)] object[] vDataValues, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] pdwQualities, out IntPtr ppErrors);

		// Token: 0x06000067 RID: 103
		void InsertReplace([MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] OPCHDA_FILETIME[] ftTimeStamps, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 0)] object[] vDataValues, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] pdwQualities, out IntPtr ppErrors);

		// Token: 0x06000068 RID: 104
		void DeleteRaw(ref OPCHDA_TIME htStartTime, ref OPCHDA_TIME htEndTime, [MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 2)] int[] phServer, out IntPtr ppErrors);

		// Token: 0x06000069 RID: 105
		void DeleteAtTime([MarshalAs(UnmanagedType.I4)] int dwNumItems, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] OPCHDA_FILETIME[] ftTimeStamps, out IntPtr ppErrors);
	}
}
