using System;
using System.Runtime.InteropServices;
using OpcRcw.Comn;

namespace OpcRcw.Da
{
	// Token: 0x0200009A RID: 154
	[Guid("39c13a4f-011e-11d0-9675-0020afd8adb3")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCBrowseServerAddressSpace
	{
		// Token: 0x06000118 RID: 280
		void QueryOrganization(out OPCNAMESPACETYPE pNameSpaceType);

		// Token: 0x06000119 RID: 281
		void ChangeBrowsePosition(OPCBROWSEDIRECTION dwBrowseDirection, [MarshalAs(UnmanagedType.LPWStr)] string szString);

		// Token: 0x0600011A RID: 282
		void BrowseOPCItemIDs(OPCBROWSETYPE dwBrowseFilterType, [MarshalAs(UnmanagedType.LPWStr)] string szFilterCriteria, [MarshalAs(UnmanagedType.I2)] short vtDataTypeFilter, [MarshalAs(UnmanagedType.I4)] int dwAccessRightsFilter, out IEnumString ppIEnumString);

		// Token: 0x0600011B RID: 283
		void GetItemID([MarshalAs(UnmanagedType.LPWStr)] string szItemDataID, [MarshalAs(UnmanagedType.LPWStr)] out string szItemID);

		// Token: 0x0600011C RID: 284
		void BrowseAccessPaths([MarshalAs(UnmanagedType.LPWStr)] string szItemID, out IEnumString pIEnumString);
	}
}
