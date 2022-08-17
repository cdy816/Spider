using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x020000A6 RID: 166
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("39227004-A18F-4b57-8B0A-5235670F4468")]
	[ComImport]
	public interface IOPCBrowse
	{
		// Token: 0x06000149 RID: 329
		void GetProperties([MarshalAs(UnmanagedType.I4)] int dwItemCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszItemIDs, [MarshalAs(UnmanagedType.I4)] int bReturnPropertyValues, [MarshalAs(UnmanagedType.I4)] int dwPropertyCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 3)] int[] dwPropertyIDs, out IntPtr ppItemProperties);

		// Token: 0x0600014A RID: 330
		void Browse([MarshalAs(UnmanagedType.LPWStr)] string szItemID, ref IntPtr pszContinuationPoint, [MarshalAs(UnmanagedType.I4)] int dwMaxElementsReturned, OPCBROWSEFILTER dwBrowseFilter, [MarshalAs(UnmanagedType.LPWStr)] string szElementNameFilter, [MarshalAs(UnmanagedType.LPWStr)] string szVendorFilter, [MarshalAs(UnmanagedType.I4)] int bReturnAllProperties, [MarshalAs(UnmanagedType.I4)] int bReturnPropertyValues, [MarshalAs(UnmanagedType.I4)] int dwPropertyCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 8)] int[] pdwPropertyIDs, [MarshalAs(UnmanagedType.I4)] out int pbMoreElements, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppBrowseElements);
	}
}
