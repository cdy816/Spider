using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Batch
{
	// Token: 0x0200000E RID: 14
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a8080da3-e23e-11d2-afa7-00c04f539421")]
	[ComImport]
	public interface IOPCEnumerationSets
	{
		// Token: 0x0600001A RID: 26
		void QueryEnumerationSets([MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppdwEnumSetId, out IntPtr ppszEnumSetName);

		// Token: 0x0600001B RID: 27
		void QueryEnumeration([MarshalAs(UnmanagedType.I4)] int dwEnumSetId, [MarshalAs(UnmanagedType.I4)] int dwEnumValue, [MarshalAs(UnmanagedType.LPWStr)] out string pszEnumName);

		// Token: 0x0600001C RID: 28
		void QueryEnumerationList([MarshalAs(UnmanagedType.I4)] int dwEnumSetId, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppdwEnumValue, out IntPtr ppszEnumName);
	}
}
