using System;
using System.Runtime.InteropServices;
using OpcRcw.Comn;

namespace OpcRcw.Hda
{
	// Token: 0x02000041 RID: 65
	[Guid("1F1217B1-DEE0-11d2-A5E5-000086339399")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCHDA_Browser
	{
		// Token: 0x06000054 RID: 84
		void GetEnum(OPCHDA_BROWSETYPE dwBrowseType, out IEnumString ppIEnumString);

		// Token: 0x06000055 RID: 85
		void ChangeBrowsePosition(OPCHDA_BROWSEDIRECTION dwBrowseDirection, [MarshalAs(UnmanagedType.LPWStr)] string szString);

		// Token: 0x06000056 RID: 86
		void GetItemID([MarshalAs(UnmanagedType.LPWStr)] string szNode, [MarshalAs(UnmanagedType.LPWStr)] out string pszItemID);

		// Token: 0x06000057 RID: 87
		void GetBranchPosition([MarshalAs(UnmanagedType.LPWStr)] out string pszBranchPos);
	}
}
