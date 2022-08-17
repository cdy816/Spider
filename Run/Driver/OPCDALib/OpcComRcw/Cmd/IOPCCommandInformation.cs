using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Cmd
{
	// Token: 0x02000068 RID: 104
	[Guid("3104B525-2016-442d-9696-1275DE978778")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IOPCCommandInformation
	{
		// Token: 0x060000C9 RID: 201
		void QueryCapabilities([MarshalAs(UnmanagedType.R8)] out double pdblMaxStorageTime, [MarshalAs(UnmanagedType.I4)] out int pbSupportsEventFilter);

		// Token: 0x060000CA RID: 202
		void QueryComands([MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppNamespaces);

		// Token: 0x060000CB RID: 203
		void BrowseCommandTargets([MarshalAs(UnmanagedType.LPWStr)] string szTargetID, [MarshalAs(UnmanagedType.LPWStr)] string szNamespaceUri, OpcCmdBrowseFilter eBrowseFilter, [MarshalAs(UnmanagedType.I4)] out int pdwCount, out IntPtr ppTargets);

		// Token: 0x060000CC RID: 204
		void GetCommandDescription([MarshalAs(UnmanagedType.LPWStr)] string szCommandName, [MarshalAs(UnmanagedType.LPWStr)] string szNamespaceUri, out OpcCmdCommandDescription pDescription);
	}
}
