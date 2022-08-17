using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Comn
{
	// Token: 0x0200002B RID: 43
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("F31DFDE2-07B6-11d2-B2D8-0060083BA1FB")]
	[ComImport]
	public interface IOPCCommon
	{
		// Token: 0x06000039 RID: 57
		void SetLocaleID([MarshalAs(UnmanagedType.I4)] int dwLcid);

		// Token: 0x0600003A RID: 58
		void GetLocaleID([MarshalAs(UnmanagedType.I4)] out int pdwLcid);

		// Token: 0x0600003B RID: 59
		void QueryAvailableLocaleIDs([MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr pdwLcid);

		// Token: 0x0600003C RID: 60
		void GetErrorString([MarshalAs(UnmanagedType.I4)] int dwError, [MarshalAs(UnmanagedType.LPWStr)] out string ppString);

		// Token: 0x0600003D RID: 61
		void SetClientName([MarshalAs(UnmanagedType.LPWStr)] string szName);
	}
}
