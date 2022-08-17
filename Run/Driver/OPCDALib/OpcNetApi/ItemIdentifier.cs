using System;
using System.Text;

namespace Opc
{
	// Token: 0x0200004B RID: 75
	[Serializable]
	public class ItemIdentifier : ICloneable
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000636B File Offset: 0x0000536B
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00006373 File Offset: 0x00005373
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

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000637C File Offset: 0x0000537C
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00006384 File Offset: 0x00005384
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000638D File Offset: 0x0000538D
		// (set) Token: 0x060001BE RID: 446 RVA: 0x00006395 File Offset: 0x00005395
		public object ClientHandle
		{
			get
			{
				return this.m_clientHandle;
			}
			set
			{
				this.m_clientHandle = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000639E File Offset: 0x0000539E
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x000063A6 File Offset: 0x000053A6
		public object ServerHandle
		{
			get
			{
				return this.m_serverHandle;
			}
			set
			{
				this.m_serverHandle = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x000063B0 File Offset: 0x000053B0
		public string Key
		{
			get
			{
				return new StringBuilder(64).Append((this.ItemName == null) ? "null" : this.ItemName).Append("\r\n").Append((this.ItemPath == null) ? "null" : this.ItemPath).ToString();
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00006407 File Offset: 0x00005407
		public ItemIdentifier()
		{
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000640F File Offset: 0x0000540F
		public ItemIdentifier(string itemName)
		{
			this.ItemPath = null;
			this.ItemName = itemName;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00006425 File Offset: 0x00005425
		public ItemIdentifier(string itemPath, string itemName)
		{
			this.ItemPath = itemPath;
			this.ItemName = itemName;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000643B File Offset: 0x0000543B
		public ItemIdentifier(ItemIdentifier itemID)
		{
			if (itemID != null)
			{
				this.ItemPath = itemID.ItemPath;
				this.ItemName = itemID.ItemName;
				this.ClientHandle = itemID.ClientHandle;
				this.ServerHandle = itemID.ServerHandle;
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00006476 File Offset: 0x00005476
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x040000EB RID: 235
		private string m_itemName;

		// Token: 0x040000EC RID: 236
		private string m_itemPath;

		// Token: 0x040000ED RID: 237
		private object m_clientHandle;

		// Token: 0x040000EE RID: 238
		private object m_serverHandle;
	}
}
