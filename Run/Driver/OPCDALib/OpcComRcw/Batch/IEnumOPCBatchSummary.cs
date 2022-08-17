using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Batch
{
	// Token: 0x0200000D RID: 13
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a8080da2-e23e-11d2-afa7-00c04f539421")]
	[ComImport]
	public interface IEnumOPCBatchSummary
	{
		// Token: 0x06000015 RID: 21
		void Next([MarshalAs(UnmanagedType.I4)] int celt, out IntPtr ppSummaryArray, [MarshalAs(UnmanagedType.I4)] out int celtFetched);

		// Token: 0x06000016 RID: 22
		void Skip([MarshalAs(UnmanagedType.I4)] int celt);

		// Token: 0x06000017 RID: 23
		void Reset();

		// Token: 0x06000018 RID: 24
		void Clone(out IEnumOPCBatchSummary ppEnumBatchSummary);

		// Token: 0x06000019 RID: 25
		void Count([MarshalAs(UnmanagedType.I4)] out int pcelt);
	}
}
