using System;
using System.Runtime.Serialization;

namespace Opc
{
	// Token: 0x02000047 RID: 71
	[Serializable]
	public class BrowseCannotContinueException : ApplicationException
	{
		// Token: 0x060001AC RID: 428 RVA: 0x000062C6 File Offset: 0x000052C6
		public BrowseCannotContinueException() : base("The browse operation cannot continue.")
		{
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000062D3 File Offset: 0x000052D3
		public BrowseCannotContinueException(string message) : base("The browse operation cannot continue.\r\n" + message)
		{
		}

		// Token: 0x060001AE RID: 430 RVA: 0x000062E6 File Offset: 0x000052E6
		public BrowseCannotContinueException(string message, Exception innerException) : base("The browse operation cannot continue.\r\n" + message, innerException)
		{
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000062FA File Offset: 0x000052FA
		protected BrowseCannotContinueException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040000E9 RID: 233
		private const string Default = "The browse operation cannot continue.";
	}
}
