using System;
using System.Collections;
using System.Reflection;

namespace Opc.Dx
{
	// Token: 0x0200007D RID: 125
	public class ServerType
	{
		// Token: 0x06000337 RID: 823 RVA: 0x00008728 File Offset: 0x00007728
		public static string[] Enumerate()
		{
			ArrayList arrayList = new ArrayList();
			FieldInfo[] fields = typeof(ServerType).GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				arrayList.Add(fieldInfo.GetValue(typeof(string)));
			}
			return (string[])arrayList.ToArray(typeof(string));
		}

		// Token: 0x0400018E RID: 398
		public const string COM_DA10 = "COM-DA1.0";

		// Token: 0x0400018F RID: 399
		public const string COM_DA204 = "COM-DA2.04";

		// Token: 0x04000190 RID: 400
		public const string COM_DA205 = "COM-DA2.05";

		// Token: 0x04000191 RID: 401
		public const string COM_DA30 = "COM-DA3.0";

		// Token: 0x04000192 RID: 402
		public const string XML_DA10 = "XML-DA1.0";
	}
}
