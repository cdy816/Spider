using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x020000A3 RID: 163
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("39c13a72-011e-11d0-9675-0020afd8adb3")]
	[ComImport]
	public interface IOPCItemProperties
	{
		// Token: 0x0600013E RID: 318
		void QueryAvailableProperties([MarshalAs(UnmanagedType.LPWStr)] string szItemID, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppPropertyIDs, out IntPtr ppDescriptions, out IntPtr ppvtDataTypes);

		// Token: 0x0600013F RID: 319
		void GetItemProperties([MarshalAs(UnmanagedType.LPWStr)] string szItemID, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] pdwPropertyIDs, out IntPtr ppvData, out IntPtr ppErrors);

		// Token: 0x06000140 RID: 320
		void LookupItemIDs([MarshalAs(UnmanagedType.LPWStr)] string szItemID, [MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 1)] int[] pdwPropertyIDs, out IntPtr ppszNewItemIDs, out IntPtr ppErrors);
	}
}
