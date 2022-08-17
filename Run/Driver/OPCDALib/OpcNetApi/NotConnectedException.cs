using System;
using System.Runtime.Serialization;

namespace Opc
{
	// Token: 0x02000042 RID: 66
	[Serializable]
	public class NotConnectedException : ApplicationException
	{
		// Token: 0x06000193 RID: 403 RVA: 0x0000610E File Offset: 0x0000510E
		public NotConnectedException() : base("The remote server is not currently connected.")
		{
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000611B File Offset: 0x0000511B
		public NotConnectedException(string message) : base("The remote server is not currently connected.\r\n" + message)
		{
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000612E File Offset: 0x0000512E
		public NotConnectedException(Exception e) : base("The remote server is not currently connected.", e)
		{
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000613C File Offset: 0x0000513C
		public NotConnectedException(string message, Exception innerException) : base("The remote server is not currently connected.\r\n" + message, innerException)
		{
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00006150 File Offset: 0x00005150
		protected NotConnectedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040000E4 RID: 228
		private const string Default = "The remote server is not currently connected.";
	}
}
