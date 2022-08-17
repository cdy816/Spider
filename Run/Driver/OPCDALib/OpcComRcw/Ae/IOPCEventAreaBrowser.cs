using System;
using System.Runtime.InteropServices;
using OpcRcw.Comn;

namespace OpcRcw.Ae
{
	// Token: 0x02000078 RID: 120
	[Guid("65168857-5783-11D1-84A0-00608CB8A7E9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCEventAreaBrowser
	{
		// Token: 0x060000EB RID: 235
		void ChangeBrowsePosition(OPCAEBROWSEDIRECTION dwBrowseDirection, [MarshalAs(UnmanagedType.LPWStr)] string szString);

		// Token: 0x060000EC RID: 236
		void BrowseOPCAreas(OPCAEBROWSETYPE dwBrowseFilterType, [MarshalAs(UnmanagedType.LPWStr)] string szFilterCriteria, out IEnumString ppIEnumString);

		// Token: 0x060000ED RID: 237
		void GetQualifiedAreaName([MarshalAs(UnmanagedType.LPWStr)] string szAreaName, [MarshalAs(UnmanagedType.LPWStr)] out string pszQualifiedAreaName);

		// Token: 0x060000EE RID: 238
		void GetQualifiedSourceName([MarshalAs(UnmanagedType.LPWStr)] string szSourceName, [MarshalAs(UnmanagedType.LPWStr)] out string pszQualifiedSourceName);
	}
}
