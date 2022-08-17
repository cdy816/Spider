using System;
using System.Net;
using System.Net.Sockets;

namespace Opc
{
    // Token: 0x02000094 RID: 148
    [Serializable]
	public class URL : ICloneable
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x0000CD49 File Offset: 0x0000BD49
		// (set) Token: 0x0600042A RID: 1066 RVA: 0x0000CD51 File Offset: 0x0000BD51
		public string Scheme
		{
			get
			{
				return this.m_scheme;
			}
			set
			{
				this.m_scheme = value;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x0000CD5A File Offset: 0x0000BD5A
		// (set) Token: 0x0600042C RID: 1068 RVA: 0x0000CD62 File Offset: 0x0000BD62
		public string HostName
		{
			get
			{
				return this.m_hostName;
			}
			set
			{
				this.m_hostName = value;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x0000CD6B File Offset: 0x0000BD6B
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x0000CD73 File Offset: 0x0000BD73
		public int Port
		{
			get
			{
				return this.m_port;
			}
			set
			{
				this.m_port = value;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x0000CD7C File Offset: 0x0000BD7C
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x0000CD84 File Offset: 0x0000BD84
		public string Path
		{
			get
			{
				return this.m_path;
			}
			set
			{
				this.m_path = value;
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000CD8D File Offset: 0x0000BD8D
		public URL()
		{
			this.Scheme = "http";
			this.HostName = "localhost";
			this.Port = 0;
			this.Path = null;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000CDBC File Offset: 0x0000BDBC
		public URL(string url)
		{
			this.Scheme = "http";
			this.HostName = "localhost";
			this.Port = 0;
			this.Path = null;
			string text = url;
			int num = text.IndexOf("://");
			if (num >= 0)
			{
				this.Scheme = text.Substring(0, num);
				text = text.Substring(num + 3);
			}
			num = text.IndexOfAny(new char[]
			{
				'/'
			});
			if (num < 0)
			{
				this.Path = text;
				return;
			}
			string text2 = text.Substring(0, num);
			IPAddress ipaddress;
			try
			{
				ipaddress = IPAddress.Parse(text2);
			}
			catch
			{
				ipaddress = null;
			}
			if (ipaddress != null && ipaddress.AddressFamily == AddressFamily.InterNetworkV6)
			{
				if (text2.Contains("]"))
				{
					this.HostName = text2.Substring(0, text2.IndexOf("]") + 1);
					if (text2.Substring(text2.IndexOf(']')).Contains(":"))
					{
						string text3 = text2.Substring(text2.LastIndexOf(':') + 1);
						if (text3 != "")
						{
							try
							{
								this.Port = (int)System.Convert.ToUInt16(text3);
								goto IL_12E;
							}
							catch
							{
								this.Port = 0;
								goto IL_12E;
							}
						}
						this.Port = 0;
					}
					else
					{
						this.Port = 0;
					}
					IL_12E:
					this.Path = text.Substring(num + 1);
				}
				else
				{
					this.HostName = "[" + text2 + "]";
					this.Port = 0;
				}
				this.Path = text.Substring(num + 1);
				return;
			}
			num = text.IndexOfAny(new char[]
			{
				':',
				'/'
			});
			if (num < 0)
			{
				this.Path = text;
				return;
			}
			this.HostName = text.Substring(0, num);
			if (text[num] == ':')
			{
				text = text.Substring(num + 1);
				num = text.IndexOf("/");
				string value;
				if (num >= 0)
				{
					value = text.Substring(0, num);
					text = text.Substring(num + 1);
				}
				else
				{
					value = text;
					text = "";
				}
				try
				{
					this.Port = (int)System.Convert.ToUInt16(value);
					goto IL_20D;
				}
				catch
				{
					this.Port = 0;
					goto IL_20D;
				}
			}
			text = text.Substring(num + 1);
			IL_20D:
			this.Path = text;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000D008 File Offset: 0x0000C008
		public override string ToString()
		{
			string text = (this.HostName == null || this.HostName == "") ? "localhost" : this.HostName;
			if (this.Port > 0)
			{
				return string.Format("{0}://{1}:{2}/{3}", new object[]
				{
					this.Scheme,
					text,
					this.Port,
					this.Path
				});
			}
			return string.Format("{0}://{1}/{2}", new object[]
			{
				this.Scheme,
				text,
				this.Path
			});
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000D0A8 File Offset: 0x0000C0A8
		public override bool Equals(object target)
		{
			URL url = null;
			if (target != null && target.GetType() == typeof(URL))
			{
				url = (URL)target;
			}
			if (target != null && target.GetType() == typeof(string))
			{
				url = new URL((string)target);
			}
			return url != null && !(url.Path != this.Path) && !(url.Scheme != this.Scheme) && !(url.HostName != this.HostName) && url.Port == this.Port;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000D149 File Offset: 0x0000C149
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000D156 File Offset: 0x0000C156
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x04000241 RID: 577
		private string m_scheme;

		// Token: 0x04000242 RID: 578
		private string m_hostName;

		// Token: 0x04000243 RID: 579
		private int m_port;

		// Token: 0x04000244 RID: 580
		private string m_path;
	}
}
