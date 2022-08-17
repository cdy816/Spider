using System;

namespace Opc.Cpx
{
	// Token: 0x02000065 RID: 101
	internal struct Context
	{
		// Token: 0x06000277 RID: 631 RVA: 0x00006F9C File Offset: 0x00005F9C
		public Context(byte[] buffer)
		{
			this.Buffer = buffer;
			this.Index = 0;
			this.Dictionary = null;
			this.Type = null;
			this.BigEndian = false;
			this.CharWidth = 2;
			this.StringEncoding = "UCS-2";
			this.FloatFormat = "IEEE-754";
		}

		// Token: 0x0400012B RID: 299
		public const string STRING_ENCODING_ACSII = "ASCII";

		// Token: 0x0400012C RID: 300
		public const string STRING_ENCODING_UCS2 = "UCS-2";

		// Token: 0x0400012D RID: 301
		public const string FLOAT_FORMAT_IEEE754 = "IEEE-754";

		// Token: 0x0400012E RID: 302
		public byte[] Buffer;

		// Token: 0x0400012F RID: 303
		public int Index;

		// Token: 0x04000130 RID: 304
		public TypeDictionary Dictionary;

		// Token: 0x04000131 RID: 305
		public TypeDescription Type;

		// Token: 0x04000132 RID: 306
		public bool BigEndian;

		// Token: 0x04000133 RID: 307
		public int CharWidth;

		// Token: 0x04000134 RID: 308
		public string StringEncoding;

		// Token: 0x04000135 RID: 309
		public string FloatFormat;
	}
}
