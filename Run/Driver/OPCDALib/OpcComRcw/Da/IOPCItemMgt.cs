using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x0200009F RID: 159
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("39c13a54-011e-11d0-9675-0020afd8adb3")]
	[ComImport]
	public interface IOPCItemMgt
	{
		// Token: 0x06000129 RID: 297
		void AddItems([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] OPCITEMDEF[] pItemArray, out IntPtr ppAddResults, out IntPtr ppErrors);

		// Token: 0x0600012A RID: 298
		void ValidateItems([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] OPCITEMDEF[] pItemArray, [MarshalAs(UnmanagedType.I4)] int bBlobUpdate, out IntPtr ppValidationResults, out IntPtr ppErrors);

		// Token: 0x0600012B RID: 299
		void RemoveItems([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, out IntPtr ppErrors);

		// Token: 0x0600012C RID: 300
		void SetActiveState([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.I4)] int bActive, out IntPtr ppErrors);

		// Token: 0x0600012D RID: 301
		void SetClientHandles([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phClient, out IntPtr ppErrors);

		// Token: 0x0600012E RID: 302
		void SetDatatypes([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] phServer, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I2, SizeParamIndex = 0)] short[] pRequestedDatatypes, out IntPtr ppErrors);

		// Token: 0x0600012F RID: 303
		void CreateEnumerator(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown, IidParameterIndex = 0)] out object ppUnk);
	}
}
