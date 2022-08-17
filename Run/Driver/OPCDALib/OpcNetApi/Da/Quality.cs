using System;

namespace Opc.Da
{
	// Token: 0x02000063 RID: 99
	[Serializable]
	public struct Quality
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000266 RID: 614 RVA: 0x00006D95 File Offset: 0x00005D95
		// (set) Token: 0x06000267 RID: 615 RVA: 0x00006D9D File Offset: 0x00005D9D
		public qualityBits QualityBits
		{
			get
			{
				return this.m_qualityBits;
			}
			set
			{
				this.m_qualityBits = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000268 RID: 616 RVA: 0x00006DA6 File Offset: 0x00005DA6
		// (set) Token: 0x06000269 RID: 617 RVA: 0x00006DAE File Offset: 0x00005DAE
		public limitBits LimitBits
		{
			get
			{
				return this.m_limitBits;
			}
			set
			{
				this.m_limitBits = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600026A RID: 618 RVA: 0x00006DB7 File Offset: 0x00005DB7
		// (set) Token: 0x0600026B RID: 619 RVA: 0x00006DBF File Offset: 0x00005DBF
		public byte VendorBits
		{
			get
			{
				return this.m_vendorBits;
			}
			set
			{
				this.m_vendorBits = value;
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00006DC8 File Offset: 0x00005DC8
		public short GetCode()
		{
			ushort num = 0;
			num |= (ushort)this.QualityBits;
			num |= (ushort)this.LimitBits;
			num |= (ushort)(this.VendorBits << 8);
			if (num > 32767)
			{
				return (short)(-(short)(65536 - (int)num));
			}
			return (short)num;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00006E0E File Offset: 0x00005E0E
		public void SetCode(short code)
		{
			this.m_qualityBits = (qualityBits)(code & 252);
			this.m_limitBits = (limitBits)(code & 3);
			this.m_vendorBits = (byte)((code & -253) >> 8);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00006E36 File Offset: 0x00005E36
		public static bool operator ==(Quality a, Quality b)
		{
			return a.Equals(b);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00006E4B File Offset: 0x00005E4B
		public static bool operator !=(Quality a, Quality b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00006E63 File Offset: 0x00005E63
		public Quality(qualityBits quality)
		{
			this.m_qualityBits = quality;
			this.m_limitBits = limitBits.none;
			this.m_vendorBits = 0;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00006E7A File Offset: 0x00005E7A
		public Quality(short code)
		{
			this.m_qualityBits = (qualityBits)(code & 252);
			this.m_limitBits = (limitBits)(code & 3);
			this.m_vendorBits = (byte)((code & -253) >> 8);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00006EA4 File Offset: 0x00005EA4
		public override string ToString()
		{
			string text = this.QualityBits.ToString();
			if (this.LimitBits != limitBits.none)
			{
				text += string.Format("[{0}]", this.LimitBits.ToString());
			}
			if (this.VendorBits != 0)
			{
				text += string.Format(":{0,0:X}", this.VendorBits);
			}
			return text;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00006F10 File Offset: 0x00005F10
		public override bool Equals(object target)
		{
			if (target == null || target.GetType() != typeof(Quality))
			{
				return false;
			}
			Quality quality = (Quality)target;
			return this.QualityBits == quality.QualityBits && this.LimitBits == quality.LimitBits && this.VendorBits == quality.VendorBits;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00006F6F File Offset: 0x00005F6F
		public override int GetHashCode()
		{
			return (int)this.GetCode();
		}

		// Token: 0x04000123 RID: 291
		private qualityBits m_qualityBits;

		// Token: 0x04000124 RID: 292
		private limitBits m_limitBits;

		// Token: 0x04000125 RID: 293
		private byte m_vendorBits;

		// Token: 0x04000126 RID: 294
		public static readonly Quality Good = new Quality(qualityBits.good);

		// Token: 0x04000127 RID: 295
		public static readonly Quality Bad = new Quality(qualityBits.bad);
	}
}
