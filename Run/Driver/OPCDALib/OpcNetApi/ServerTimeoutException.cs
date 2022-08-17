using System;
using System.Runtime.Serialization;

namespace Opc
{
	// Token: 0x02000045 RID: 69
	public class ServerTimeoutException : ResultIDException
	{
		// Token: 0x060001A2 RID: 418 RVA: 0x0000621A File Offset: 0x0000521A
		public ServerTimeoutException() : base(ResultID.E_TIMEDOUT, "The server did not respond within the specified timeout period.")
		{
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000622C File Offset: 0x0000522C
		public ServerTimeoutException(string message) : base(ResultID.E_TIMEDOUT, "The server did not respond within the specified timeout period.\r\n" + message)
		{
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00006244 File Offset: 0x00005244
		public ServerTimeoutException(Exception e) : base(ResultID.E_TIMEDOUT, "The server did not respond within the specified timeout period.", e)
		{
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00006257 File Offset: 0x00005257
		public ServerTimeoutException(string message, Exception innerException) : base(ResultID.E_TIMEDOUT, "The server did not respond within the specified timeout period.\r\n" + message, innerException)
		{
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00006270 File Offset: 0x00005270
		protected ServerTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040000E7 RID: 231
		private const string Default = "The server did not respond within the specified timeout period.";
	}
}
