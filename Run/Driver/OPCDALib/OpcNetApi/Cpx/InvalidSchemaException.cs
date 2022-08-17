using System;
using System.Runtime.Serialization;

namespace Opc.Cpx
{
	// Token: 0x02000068 RID: 104
	[Serializable]
	public class InvalidSchemaException : ApplicationException
	{
		// Token: 0x06000282 RID: 642 RVA: 0x00007265 File Offset: 0x00006265
		public InvalidSchemaException() : base("The schema cannot be used because it contains errors or inconsitencies.")
		{
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00007272 File Offset: 0x00006272
		public InvalidSchemaException(string message) : base("The schema cannot be used because it contains errors or inconsitencies.\r\n" + message)
		{
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00007285 File Offset: 0x00006285
		public InvalidSchemaException(Exception e) : base("The schema cannot be used because it contains errors or inconsitencies.", e)
		{
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00007293 File Offset: 0x00006293
		public InvalidSchemaException(string message, Exception innerException) : base("The schema cannot be used because it contains errors or inconsitencies.\r\n" + message, innerException)
		{
		}

		// Token: 0x06000286 RID: 646 RVA: 0x000072A7 File Offset: 0x000062A7
		protected InvalidSchemaException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x04000137 RID: 311
		private const string Default = "The schema cannot be used because it contains errors or inconsitencies.";
	}
}
