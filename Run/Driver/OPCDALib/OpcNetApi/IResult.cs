using System;

namespace Opc
{
	// Token: 0x02000052 RID: 82
	public interface IResult
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001F6 RID: 502
		// (set) Token: 0x060001F7 RID: 503
		ResultID ResultID { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001F8 RID: 504
		// (set) Token: 0x060001F9 RID: 505
		string DiagnosticInfo { get; set; }
	}
}
