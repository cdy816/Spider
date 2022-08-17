using System;
using System.Collections;
using System.Runtime.InteropServices;
using OpcRcw.Comn;

namespace OpcCom
{
	// Token: 0x02000038 RID: 56
	public class EnumGuid
	{
		// Token: 0x06000226 RID: 550 RVA: 0x00019A30 File Offset: 0x00018A30
		public EnumGuid(object server)
		{
			this.m_enumerator = (IEnumGUID)server;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00019A44 File Offset: 0x00018A44
		public void Release()
		{
			Interop.ReleaseServer(this.m_enumerator);
			this.m_enumerator = null;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00019A58 File Offset: 0x00018A58
		public object GetEnumerator()
		{
			return this.m_enumerator;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00019A60 File Offset: 0x00018A60
		public Guid[] GetAll()
		{
			this.Reset();
			ArrayList arrayList = new ArrayList();
			for (;;)
			{
				Guid[] array = this.Next(1);
				if (array == null)
				{
					break;
				}
				arrayList.AddRange(array);
			}
			return (Guid[])arrayList.ToArray(typeof(Guid));
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00019AA4 File Offset: 0x00018AA4
		public Guid[] Next(int count)
		{
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(Guid)) * count);
			Guid[] result;
			try
			{
				int num = 0;
				try
				{
					this.m_enumerator.Next(count, intPtr, out num);
				}
				catch (Exception)
				{
					return null;
				}
				if (num == 0)
				{
					result = null;
				}
				else
				{
					IntPtr ptr = intPtr;
					Guid[] array = new Guid[num];
					for (int i = 0; i < num; i++)
					{
						array[i] = (Guid)Marshal.PtrToStructure(ptr, typeof(Guid));
						ptr = (IntPtr)(ptr.ToInt64() + (long)Marshal.SizeOf(typeof(Guid)));
					}
					result = array;
				}
			}
			finally
			{
				Marshal.FreeCoTaskMem(intPtr);
			}
			return result;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00019B6C File Offset: 0x00018B6C
		public void Skip(int count)
		{
			this.m_enumerator.Skip(count);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00019B7A File Offset: 0x00018B7A
		public void Reset()
		{
			this.m_enumerator.Reset();
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00019B88 File Offset: 0x00018B88
		public EnumGuid Clone()
		{
			IEnumGUID server = null;
			this.m_enumerator.Clone(out server);
			return new EnumGuid(server);
		}

		// Token: 0x0400014A RID: 330
		private IEnumGUID m_enumerator;
	}
}
