using System;
using System.Runtime.InteropServices;

namespace OpcRcw.Hda
{
	// Token: 0x02000033 RID: 51
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct OPCHDA_FILETIME
	{
		// Token: 0x040001D1 RID: 465
		public int dwLowDateTime;

		// Token: 0x040001D2 RID: 466
		public int dwHighDateTime;
	}
}
