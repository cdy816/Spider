using System;
using System.Collections;
using System.Reflection;

namespace Opc
{
	// Token: 0x0200007A RID: 122
	public class Type
	{
		// Token: 0x0600031C RID: 796 RVA: 0x00008348 File Offset: 0x00007348
		public static Type[] Enumerate()
		{
			ArrayList arrayList = new ArrayList();
			FieldInfo[] fields = typeof(Type).GetFields(BindingFlags.Static | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				arrayList.Add(fieldInfo.GetValue(typeof(Type)));
			}
			return (Type[])arrayList.ToArray(typeof(Type));
		}

		// Token: 0x04000168 RID: 360
		public static System.Type SBYTE = typeof(sbyte);

		// Token: 0x04000169 RID: 361
		public static System.Type BYTE = typeof(byte);

		// Token: 0x0400016A RID: 362
		public static System.Type SHORT = typeof(short);

		// Token: 0x0400016B RID: 363
		public static System.Type USHORT = typeof(ushort);

		// Token: 0x0400016C RID: 364
		public static System.Type INT = typeof(int);

		// Token: 0x0400016D RID: 365
		public static System.Type UINT = typeof(uint);

		// Token: 0x0400016E RID: 366
		public static System.Type LONG = typeof(long);

		// Token: 0x0400016F RID: 367
		public static System.Type ULONG = typeof(ulong);

		// Token: 0x04000170 RID: 368
		public static System.Type FLOAT = typeof(float);

		// Token: 0x04000171 RID: 369
		public static System.Type DOUBLE = typeof(double);

		// Token: 0x04000172 RID: 370
		public static System.Type DECIMAL = typeof(decimal);

		// Token: 0x04000173 RID: 371
		public static System.Type BOOLEAN = typeof(bool);

		// Token: 0x04000174 RID: 372
		public static System.Type DATETIME = typeof(DateTime);

		// Token: 0x04000175 RID: 373
		public static System.Type DURATION = typeof(TimeSpan);

		// Token: 0x04000176 RID: 374
		public static System.Type STRING = typeof(string);

		// Token: 0x04000177 RID: 375
		public static System.Type ANY_TYPE = typeof(object);

		// Token: 0x04000178 RID: 376
		public static System.Type BINARY = typeof(byte[]);

		// Token: 0x04000179 RID: 377
		public static System.Type ARRAY_SHORT = typeof(short[]);

		// Token: 0x0400017A RID: 378
		public static System.Type ARRAY_USHORT = typeof(ushort[]);

		// Token: 0x0400017B RID: 379
		public static System.Type ARRAY_INT = typeof(int[]);

		// Token: 0x0400017C RID: 380
		public static System.Type ARRAY_UINT = typeof(uint[]);

		// Token: 0x0400017D RID: 381
		public static System.Type ARRAY_LONG = typeof(long[]);

		// Token: 0x0400017E RID: 382
		public static System.Type ARRAY_ULONG = typeof(ulong[]);

		// Token: 0x0400017F RID: 383
		public static System.Type ARRAY_FLOAT = typeof(float[]);

		// Token: 0x04000180 RID: 384
		public static System.Type ARRAY_DOUBLE = typeof(double[]);

		// Token: 0x04000181 RID: 385
		public static System.Type ARRAY_DECIMAL = typeof(decimal[]);

		// Token: 0x04000182 RID: 386
		public static System.Type ARRAY_BOOLEAN = typeof(bool[]);

		// Token: 0x04000183 RID: 387
		public static System.Type ARRAY_DATETIME = typeof(DateTime[]);

		// Token: 0x04000184 RID: 388
		public static System.Type ARRAY_STRING = typeof(string[]);

		// Token: 0x04000185 RID: 389
		public static System.Type ARRAY_ANY_TYPE = typeof(object[]);

		// Token: 0x04000186 RID: 390
		public static System.Type ILLEGAL_TYPE = typeof(Type);
	}
}
