using System;

namespace Opc
{
	// Token: 0x02000003 RID: 3
	public interface IServer : IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600000C RID: 12
		// (remove) Token: 0x0600000D RID: 13
		event ServerShutdownEventHandler ServerShutdown;

		// Token: 0x0600000E RID: 14
		string GetLocale();

		// Token: 0x0600000F RID: 15
		string SetLocale(string locale);

		// Token: 0x06000010 RID: 16
		string[] GetSupportedLocales();

		// Token: 0x06000011 RID: 17
		string GetErrorText(string locale, ResultID resultID);
	}
}
