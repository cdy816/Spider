using System;

namespace Opc.Dx
{
	// Token: 0x02000085 RID: 133
	[Serializable]
	public class DXConnection : ItemIdentifier
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0000A760 File Offset: 0x00009760
		// (set) Token: 0x06000390 RID: 912 RVA: 0x0000A768 File Offset: 0x00009768
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

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0000A771 File Offset: 0x00009771
		public BrowsePathCollection BrowsePaths
		{
			get
			{
				return this.m_browsePaths;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0000A779 File Offset: 0x00009779
		// (set) Token: 0x06000393 RID: 915 RVA: 0x0000A781 File Offset: 0x00009781
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

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000A78A File Offset: 0x0000978A
		// (set) Token: 0x06000395 RID: 917 RVA: 0x0000A792 File Offset: 0x00009792
		public string Keyword
		{
			get
			{
				return this.m_keyword;
			}
			set
			{
				this.m_keyword = value;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000396 RID: 918 RVA: 0x0000A79B File Offset: 0x0000979B
		// (set) Token: 0x06000397 RID: 919 RVA: 0x0000A7A3 File Offset: 0x000097A3
		public bool DefaultSourceItemConnected
		{
			get
			{
				return this.m_defaultSourceItemConnected;
			}
			set
			{
				this.m_defaultSourceItemConnected = value;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000398 RID: 920 RVA: 0x0000A7AC File Offset: 0x000097AC
		// (set) Token: 0x06000399 RID: 921 RVA: 0x0000A7B4 File Offset: 0x000097B4
		public bool DefaultSourceItemConnectedSpecified
		{
			get
			{
				return this.m_defaultSourceItemConnectedSpecified;
			}
			set
			{
				this.m_defaultSourceItemConnectedSpecified = value;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0000A7BD File Offset: 0x000097BD
		// (set) Token: 0x0600039B RID: 923 RVA: 0x0000A7C5 File Offset: 0x000097C5
		public bool DefaultTargetItemConnected
		{
			get
			{
				return this.m_defaultTargetItemConnected;
			}
			set
			{
				this.m_defaultTargetItemConnected = value;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600039C RID: 924 RVA: 0x0000A7CE File Offset: 0x000097CE
		// (set) Token: 0x0600039D RID: 925 RVA: 0x0000A7D6 File Offset: 0x000097D6
		public bool DefaultTargetItemConnectedSpecified
		{
			get
			{
				return this.m_defaultTargetItemConnectedSpecified;
			}
			set
			{
				this.m_defaultTargetItemConnectedSpecified = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600039E RID: 926 RVA: 0x0000A7DF File Offset: 0x000097DF
		// (set) Token: 0x0600039F RID: 927 RVA: 0x0000A7E7 File Offset: 0x000097E7
		public bool DefaultOverridden
		{
			get
			{
				return this.m_defaultOverridden;
			}
			set
			{
				this.m_defaultOverridden = value;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000A7F0 File Offset: 0x000097F0
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x0000A7F8 File Offset: 0x000097F8
		public bool DefaultOverriddenSpecified
		{
			get
			{
				return this.m_defaultOverriddenSpecified;
			}
			set
			{
				this.m_defaultOverriddenSpecified = value;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x0000A801 File Offset: 0x00009801
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x0000A809 File Offset: 0x00009809
		public object DefaultOverrideValue
		{
			get
			{
				return this.m_defaultOverrideValue;
			}
			set
			{
				this.m_defaultOverrideValue = value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0000A812 File Offset: 0x00009812
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x0000A81A File Offset: 0x0000981A
		public bool EnableSubstituteValue
		{
			get
			{
				return this.m_enableSubstituteValue;
			}
			set
			{
				this.m_enableSubstituteValue = value;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x0000A823 File Offset: 0x00009823
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x0000A82B File Offset: 0x0000982B
		public bool EnableSubstituteValueSpecified
		{
			get
			{
				return this.m_enableSubstituteValueSpecified;
			}
			set
			{
				this.m_enableSubstituteValueSpecified = value;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000A834 File Offset: 0x00009834
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x0000A83C File Offset: 0x0000983C
		public object SubstituteValue
		{
			get
			{
				return this.m_substituteValue;
			}
			set
			{
				this.m_substituteValue = value;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060003AA RID: 938 RVA: 0x0000A845 File Offset: 0x00009845
		// (set) Token: 0x060003AB RID: 939 RVA: 0x0000A84D File Offset: 0x0000984D
		public string TargetItemName
		{
			get
			{
				return this.m_targetItemName;
			}
			set
			{
				this.m_targetItemName = value;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0000A856 File Offset: 0x00009856
		// (set) Token: 0x060003AD RID: 941 RVA: 0x0000A85E File Offset: 0x0000985E
		public string TargetItemPath
		{
			get
			{
				return this.m_targetItemPath;
			}
			set
			{
				this.m_targetItemPath = value;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060003AE RID: 942 RVA: 0x0000A867 File Offset: 0x00009867
		// (set) Token: 0x060003AF RID: 943 RVA: 0x0000A86F File Offset: 0x0000986F
		public string SourceServerName
		{
			get
			{
				return this.m_sourceServerName;
			}
			set
			{
				this.m_sourceServerName = value;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x0000A878 File Offset: 0x00009878
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x0000A880 File Offset: 0x00009880
		public string SourceItemName
		{
			get
			{
				return this.m_sourceItemName;
			}
			set
			{
				this.m_sourceItemName = value;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x0000A889 File Offset: 0x00009889
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x0000A891 File Offset: 0x00009891
		public string SourceItemPath
		{
			get
			{
				return this.m_sourceItemPath;
			}
			set
			{
				this.m_sourceItemPath = value;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0000A89A File Offset: 0x0000989A
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x0000A8A2 File Offset: 0x000098A2
		public int SourceItemQueueSize
		{
			get
			{
				return this.m_sourceItemQueueSize;
			}
			set
			{
				this.m_sourceItemQueueSize = value;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x0000A8AB File Offset: 0x000098AB
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x0000A8B3 File Offset: 0x000098B3
		public bool SourceItemQueueSizeSpecified
		{
			get
			{
				return this.m_sourceItemQueueSizeSpecified;
			}
			set
			{
				this.m_sourceItemQueueSizeSpecified = value;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x0000A8BC File Offset: 0x000098BC
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x0000A8C4 File Offset: 0x000098C4
		public int UpdateRate
		{
			get
			{
				return this.m_updateRate;
			}
			set
			{
				this.m_updateRate = value;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060003BA RID: 954 RVA: 0x0000A8CD File Offset: 0x000098CD
		// (set) Token: 0x060003BB RID: 955 RVA: 0x0000A8D5 File Offset: 0x000098D5
		public bool UpdateRateSpecified
		{
			get
			{
				return this.m_updateRateSpecified;
			}
			set
			{
				this.m_updateRateSpecified = value;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060003BC RID: 956 RVA: 0x0000A8DE File Offset: 0x000098DE
		// (set) Token: 0x060003BD RID: 957 RVA: 0x0000A8E6 File Offset: 0x000098E6
		public float Deadband
		{
			get
			{
				return this.m_deadband;
			}
			set
			{
				this.m_deadband = value;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060003BE RID: 958 RVA: 0x0000A8EF File Offset: 0x000098EF
		// (set) Token: 0x060003BF RID: 959 RVA: 0x0000A8F7 File Offset: 0x000098F7
		public bool DeadbandSpecified
		{
			get
			{
				return this.m_deadbandSpecified;
			}
			set
			{
				this.m_deadbandSpecified = value;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0000A900 File Offset: 0x00009900
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x0000A908 File Offset: 0x00009908
		public string VendorData
		{
			get
			{
				return this.m_vendorData;
			}
			set
			{
				this.m_vendorData = value;
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000A911 File Offset: 0x00009911
		public DXConnection()
		{
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000A92B File Offset: 0x0000992B
		public DXConnection(ItemIdentifier item) : base(item)
		{
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000A948 File Offset: 0x00009948
		public DXConnection(DXConnection connection) : base(connection)
		{
			if (connection != null)
			{
				this.BrowsePaths.AddRange(connection.BrowsePaths);
				this.Name = connection.Name;
				this.Description = connection.Description;
				this.Keyword = connection.Keyword;
				this.DefaultSourceItemConnected = connection.DefaultSourceItemConnected;
				this.DefaultSourceItemConnectedSpecified = connection.DefaultSourceItemConnectedSpecified;
				this.DefaultTargetItemConnected = connection.DefaultTargetItemConnected;
				this.DefaultTargetItemConnectedSpecified = connection.DefaultTargetItemConnectedSpecified;
				this.DefaultOverridden = connection.DefaultOverridden;
				this.DefaultOverriddenSpecified = connection.DefaultOverriddenSpecified;
				this.DefaultOverrideValue = connection.DefaultOverrideValue;
				this.EnableSubstituteValue = connection.EnableSubstituteValue;
				this.EnableSubstituteValueSpecified = connection.EnableSubstituteValueSpecified;
				this.SubstituteValue = connection.SubstituteValue;
				this.TargetItemName = connection.TargetItemName;
				this.TargetItemPath = connection.TargetItemPath;
				this.SourceServerName = connection.SourceServerName;
				this.SourceItemName = connection.SourceItemName;
				this.SourceItemPath = connection.SourceItemPath;
				this.SourceItemQueueSize = connection.SourceItemQueueSize;
				this.SourceItemQueueSizeSpecified = connection.SourceItemQueueSizeSpecified;
				this.UpdateRate = connection.UpdateRate;
				this.UpdateRateSpecified = connection.UpdateRateSpecified;
				this.Deadband = connection.Deadband;
				this.DeadbandSpecified = connection.DeadbandSpecified;
				this.VendorData = connection.VendorData;
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000AAB1 File Offset: 0x00009AB1
		public override object Clone()
		{
			return new DXConnection(this);
		}

		// Token: 0x040001B0 RID: 432
		private string m_name;

		// Token: 0x040001B1 RID: 433
		private BrowsePathCollection m_browsePaths = new BrowsePathCollection();

		// Token: 0x040001B2 RID: 434
		private string m_description;

		// Token: 0x040001B3 RID: 435
		private string m_keyword;

		// Token: 0x040001B4 RID: 436
		private bool m_defaultSourceItemConnected;

		// Token: 0x040001B5 RID: 437
		private bool m_defaultSourceItemConnectedSpecified;

		// Token: 0x040001B6 RID: 438
		private bool m_defaultTargetItemConnected;

		// Token: 0x040001B7 RID: 439
		private bool m_defaultTargetItemConnectedSpecified;

		// Token: 0x040001B8 RID: 440
		private bool m_defaultOverridden;

		// Token: 0x040001B9 RID: 441
		private bool m_defaultOverriddenSpecified;

		// Token: 0x040001BA RID: 442
		private object m_defaultOverrideValue;

		// Token: 0x040001BB RID: 443
		private bool m_enableSubstituteValue;

		// Token: 0x040001BC RID: 444
		private bool m_enableSubstituteValueSpecified;

		// Token: 0x040001BD RID: 445
		private object m_substituteValue;

		// Token: 0x040001BE RID: 446
		private string m_targetItemName;

		// Token: 0x040001BF RID: 447
		private string m_targetItemPath;

		// Token: 0x040001C0 RID: 448
		private string m_sourceServerName;

		// Token: 0x040001C1 RID: 449
		private string m_sourceItemName;

		// Token: 0x040001C2 RID: 450
		private string m_sourceItemPath;

		// Token: 0x040001C3 RID: 451
		private int m_sourceItemQueueSize = 1;

		// Token: 0x040001C4 RID: 452
		private bool m_sourceItemQueueSizeSpecified;

		// Token: 0x040001C5 RID: 453
		private int m_updateRate;

		// Token: 0x040001C6 RID: 454
		private bool m_updateRateSpecified;

		// Token: 0x040001C7 RID: 455
		private float m_deadband;

		// Token: 0x040001C8 RID: 456
		private bool m_deadbandSpecified;

		// Token: 0x040001C9 RID: 457
		private string m_vendorData;
	}
}
