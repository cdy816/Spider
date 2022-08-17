using System;
using System.Collections;

namespace Opc.Dx
{
	// Token: 0x02000086 RID: 134
	[Serializable]
	public class BrowsePathCollection : ArrayList
	{
		// Token: 0x170000DC RID: 220
		public string this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = value;
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000AACC File Offset: 0x00009ACC
		public new string[] ToArray()
		{
			return (string[])this.ToArray(typeof(string));
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000AAE3 File Offset: 0x00009AE3
		public int Add(string browsePath)
		{
			return base.Add(browsePath);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000AAEC File Offset: 0x00009AEC
		public void Insert(int index, string browsePath)
		{
			if (browsePath == null)
			{
				throw new ArgumentNullException("browsePath");
			}
			base.Insert(index, browsePath);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000AB04 File Offset: 0x00009B04
		public BrowsePathCollection()
		{
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000AB0C File Offset: 0x00009B0C
		public BrowsePathCollection(ICollection browsePaths)
		{
			if (browsePaths != null)
			{
				foreach (object obj in browsePaths)
				{
					string browsePath = (string)obj;
					this.Add(browsePath);
				}
			}
		}
	}
}
