using System;
using System.Runtime.Serialization;

namespace Opc
{
	// Token: 0x02000046 RID: 70
	[Serializable]
	public class InvalidResponseException : ApplicationException
	{
		// Token: 0x060001A7 RID: 423 RVA: 0x0000627A File Offset: 0x0000527A
		public InvalidResponseException() : base("The response from the server was invalid or incomplete.")
		{
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00006287 File Offset: 0x00005287
		public InvalidResponseException(string message) : base("The response from the server was invalid or incomplete.\r\n" + message)
		{
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000629A File Offset: 0x0000529A
		public InvalidResponseException(Exception e) : base("The response from the server was invalid or incomplete.", e)
		{
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000062A8 File Offset: 0x000052A8
		public InvalidResponseException(string message, Exception innerException) : base("The response from the server was invalid or incomplete.\r\n" + message, innerException)
		{
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000062BC File Offset: 0x000052BC
		protected InvalidResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040000E8 RID: 232
		private const string Default = "The response from the server was invalid or incomplete.";
	}
}
