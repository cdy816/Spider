using System;
using System.Collections;

namespace Opc.Da
{
	// Token: 0x020000EB RID: 235
	[Serializable]
	public class ItemPropertyCollection : ArrayList, IResult
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x00012DA7 File Offset: 0x00011DA7
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x00012DAF File Offset: 0x00011DAF
		public string ItemName
		{
			get
			{
				return this.m_itemName;
			}
			set
			{
				this.m_itemName = value;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x00012DB8 File Offset: 0x00011DB8
		// (set) Token: 0x06000802 RID: 2050 RVA: 0x00012DC0 File Offset: 0x00011DC0
		public string ItemPath
		{
			get
			{
				return this.m_itemPath;
			}
			set
			{
				this.m_itemPath = value;
			}
		}

		// Token: 0x170001F6 RID: 502
		public ItemProperty this[int index]
		{
			get
			{
				return (ItemProperty)base[index];
			}
			set
			{
				base[index] = value;
			}
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00012DE1 File Offset: 0x00011DE1
		public ItemPropertyCollection()
		{
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00012DF4 File Offset: 0x00011DF4
		public ItemPropertyCollection(ItemIdentifier itemID)
		{
			if (itemID != null)
			{
				this.m_itemName = itemID.ItemName;
				this.m_itemPath = itemID.ItemPath;
			}
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00012E22 File Offset: 0x00011E22
		public ItemPropertyCollection(ItemIdentifier itemID, ResultID resultID)
		{
			if (itemID != null)
			{
				this.m_itemName = itemID.ItemName;
				this.m_itemPath = itemID.ItemPath;
			}
			this.ResultID = resultID;
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x00012E57 File Offset: 0x00011E57
		// (set) Token: 0x06000809 RID: 2057 RVA: 0x00012E5F File Offset: 0x00011E5F
		public ResultID ResultID
		{
			get
			{
				return this.m_resultID;
			}
			set
			{
				this.m_resultID = value;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x00012E68 File Offset: 0x00011E68
		// (set) Token: 0x0600080B RID: 2059 RVA: 0x00012E70 File Offset: 0x00011E70
		public string DiagnosticInfo
		{
			get
			{
				return this.m_diagnosticInfo;
			}
			set
			{
				this.m_diagnosticInfo = value;
			}
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00012E79 File Offset: 0x00011E79
		public void CopyTo(ItemProperty[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00012E83 File Offset: 0x00011E83
		public void Insert(int index, ItemProperty value)
		{
			this.Insert(index, value);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00012E8D File Offset: 0x00011E8D
		public void Remove(ItemProperty value)
		{
			this.Remove(value);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00012E96 File Offset: 0x00011E96
		public bool Contains(ItemProperty value)
		{
			return this.Contains(value);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00012E9F File Offset: 0x00011E9F
		public int IndexOf(ItemProperty value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00012EA8 File Offset: 0x00011EA8
		public int Add(ItemProperty value)
		{
			return this.Add(value);
		}

		// Token: 0x0400039B RID: 923
		private string m_itemName;

		// Token: 0x0400039C RID: 924
		private string m_itemPath;

		// Token: 0x0400039D RID: 925
		private ResultID m_resultID = ResultID.S_OK;

		// Token: 0x0400039E RID: 926
		private string m_diagnosticInfo;
	}
}
