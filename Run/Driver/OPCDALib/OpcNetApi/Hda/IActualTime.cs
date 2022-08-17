using System;

namespace Opc.Hda
{
	// Token: 0x02000053 RID: 83
	public interface IActualTime
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001FA RID: 506
		// (set) Token: 0x060001FB RID: 507
		DateTime StartTime { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001FC RID: 508
		// (set) Token: 0x060001FD RID: 509
		DateTime EndTime { get; set; }
	}
}
