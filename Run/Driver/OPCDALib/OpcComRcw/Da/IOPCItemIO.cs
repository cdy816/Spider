using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Da
{
	// Token: 0x020000A7 RID: 167
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("85C0B427-2893-4cbc-BD78-E5FC5146F08F")]
	[ComImport]
	public interface IOPCItemIO
	{
		// Token: 0x0600014B RID: 331
		void Read([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszItemIDs, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 0)] int[] pdwMaxAge, out IntPtr ppvValues, out IntPtr ppwQualities, out IntPtr ppftTimeStamps, out IntPtr ppErrors);

		// Token: 0x0600014C RID: 332
		void WriteVQT([MarshalAs(UnmanagedType.I4)] int dwCount, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] pszItemIDs, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] OPCITEMVQT[] pItemVQT, out IntPtr ppErrors);
	}
}
