using System;

namespace Opc.Dx
{
	// Token: 0x0200007C RID: 124
	[Serializable]
	public class SourceServer : ItemIdentifier
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000328 RID: 808 RVA: 0x00008651 File Offset: 0x00007651
		// (set) Token: 0x06000329 RID: 809 RVA: 0x00008659 File Offset: 0x00007659
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600032A RID: 810 RVA: 0x00008662 File Offset: 0x00007662
		// (set) Token: 0x0600032B RID: 811 RVA: 0x0000866A File Offset: 0x0000766A
		public string Description
		{
			get
			{
				return this.m_description;
			}
			set
			{
				this.m_description = value;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600032C RID: 812 RVA: 0x00008673 File Offset: 0x00007673
		// (set) Token: 0x0600032D RID: 813 RVA: 0x0000867B File Offset: 0x0000767B
		public string ServerType
		{
			get
			{
				return this.m_serverType;
			}
			set
			{
				this.m_serverType = value;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600032E RID: 814 RVA: 0x00008684 File Offset: 0x00007684
		// (set) Token: 0x0600032F RID: 815 RVA: 0x0000868C File Offset: 0x0000768C
		public string ServerURL
		{
			get
			{
				return this.m_serverURL;
			}
			set
			{
				this.m_serverURL = value;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000330 RID: 816 RVA: 0x00008695 File Offset: 0x00007695
		// (set) Token: 0x06000331 RID: 817 RVA: 0x0000869D File Offset: 0x0000769D
		public bool DefaultConnected
		{
			get
			{
				return this.m_defaultConnected;
			}
			set
			{
				this.m_defaultConnected = value;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000332 RID: 818 RVA: 0x000086A6 File Offset: 0x000076A6
		// (set) Token: 0x06000333 RID: 819 RVA: 0x000086AE File Offset: 0x000076AE
		public bool DefaultConnectedSpecified
		{
			get
			{
				return this.m_defaultConnectedSpecified;
			}
			set
			{
				this.m_defaultConnectedSpecified = value;
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x000086B7 File Offset: 0x000076B7
		public SourceServer()
		{
		}

		// Token: 0x06000335 RID: 821 RVA: 0x000086BF File Offset: 0x000076BF
		public SourceServer(ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x06000336 RID: 822 RVA: 0x000086C8 File Offset: 0x000076C8
		public SourceServer(SourceServer server) : base(server)
		{
			if (server != null)
			{
				this.m_name = server.m_name;
				this.m_description = server.m_description;
				this.m_serverType = server.m_serverType;
				this.m_serverURL = server.m_serverURL;
				this.m_defaultConnected = server.m_defaultConnected;
				this.m_defaultConnectedSpecified = server.m_defaultConnectedSpecified;
			}
		}

		// Token: 0x04000188 RID: 392
		private string m_name;

		// Token: 0x04000189 RID: 393
		private string m_description;

		// Token: 0x0400018A RID: 394
		private string m_serverType;

		// Token: 0x0400018B RID: 395
		private string m_serverURL;

		// Token: 0x0400018C RID: 396
		private bool m_defaultConnected;

		// Token: 0x0400018D RID: 397
		private bool m_defaultConnectedSpecified;
	}
}
