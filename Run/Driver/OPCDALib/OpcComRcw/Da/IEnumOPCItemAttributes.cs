using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x020000A0 RID: 160
	[Guid("39c13a55-011e-11d0-9675-0020afd8adb3")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IEnumOPCItemAttributes
	{
		// Token: 0x06000130 RID: 304
		void Next([MarshalAs(UnmanagedType.I4)] int celt, out IntPtr ppItemArray, [MarshalAs(UnmanagedType.I4)] out int pceltFetched);

		// Token: 0x06000131 RID: 305
		void Skip([MarshalAs(UnmanagedType.I4)] int celt);

		// Token: 0x06000132 RID: 306
		void Reset();

		// Token: 0x06000133 RID: 307
		void Clone(out IEnumOPCItemAttributes ppEnumItemAttributes);
	}
}
