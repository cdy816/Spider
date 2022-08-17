using System;
using System.Net;
using System.Runtime.Serialization;

namespace Opc
{
	// Token: 0x02000082 RID: 130
	[Serializable]
	public class ConnectData : ISerializable, ICredentials
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0000A41E File Offset: 0x0000941E
		// (set) Token: 0x06000373 RID: 883 RVA: 0x0000A426 File Offset: 0x00009426
		public NetworkCredential Credentials
		{
			get
			{
				return this.m_credentials;
			}
			set
			{
				this.m_credentials = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000374 RID: 884 RVA: 0x0000A42F File Offset: 0x0000942F
		// (set) Token: 0x06000375 RID: 885 RVA: 0x0000A437 File Offset: 0x00009437
		public string LicenseKey
		{
			get
			{
				return this.m_licenseKey;
			}
			set
			{
				this.m_licenseKey = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000376 RID: 886 RVA: 0x0000A440 File Offset: 0x00009440
		// (set) Token: 0x06000377 RID: 887 RVA: 0x0000A448 File Offset: 0x00009448
		public bool AlwaysUseDA20 { get; set; }

		// Token: 0x06000378 RID: 888 RVA: 0x0000A451 File Offset: 0x00009451
		public NetworkCredential GetCredential(Uri uri, string authenticationType)
		{
			if (this.m_credentials != null)
			{
				return new NetworkCredential(this.m_credentials.UserName, this.m_credentials.Password, this.m_credentials.Domain);
			}
			return null;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000A483 File Offset: 0x00009483
		public IWebProxy GetProxy()
		{
			if (this.m_proxy != null)
			{
				return this.m_proxy;
			}
			return new WebProxy();
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000A499 File Offset: 0x00009499
		public void SetProxy(WebProxy proxy)
		{
			this.m_proxy = proxy;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000A4A2 File Offset: 0x000094A2
		public ConnectData(NetworkCredential credentials)
		{
			this.m_credentials = credentials;
			this.m_proxy = null;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000A4B8 File Offset: 0x000094B8
		public ConnectData(NetworkCredential credentials, WebProxy proxy)
		{
			this.m_credentials = credentials;
			this.m_proxy = proxy;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000A4D0 File Offset: 0x000094D0
		protected ConnectData(SerializationInfo info, StreamingContext context)
		{
			string @string = info.GetString("UN");
			string string2 = info.GetString("PW");
			string string3 = info.GetString("DO");
			string string4 = info.GetString("PU");
			info.GetString("LK");
			if (string3 != null)
			{
				this.m_credentials = new NetworkCredential(@string, string2, string3);
			}
			else
			{
				this.m_credentials = new NetworkCredential(@string, string2);
			}
			if (string4 != null)
			{
				this.m_proxy = new WebProxy(string4);
				return;
			}
			this.m_proxy = null;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000A558 File Offset: 0x00009558
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (this.m_credentials != null)
			{
				info.AddValue("UN", this.m_credentials.UserName);
				info.AddValue("PW", this.m_credentials.Password);
				info.AddValue("DO", this.m_credentials.Domain);
			}
			else
			{
				info.AddValue("UN", null);
				info.AddValue("PW", null);
				info.AddValue("DO", null);
			}
			if (this.m_proxy != null)
			{
				info.AddValue("PU", this.m_proxy.Address);
				return;
			}
			info.AddValue("PU", null);
		}

		// Token: 0x040001A5 RID: 421
		private NetworkCredential m_credentials;

		// Token: 0x040001A6 RID: 422
		private WebProxy m_proxy;

		// Token: 0x040001A7 RID: 423
		private string m_licenseKey;

		// Token: 0x02000083 RID: 131
		private class Names
		{
			// Token: 0x040001A9 RID: 425
			internal const string USER_NAME = "UN";

			// Token: 0x040001AA RID: 426
			internal const string PASSWORD = "PW";

			// Token: 0x040001AB RID: 427
			internal const string DOMAIN = "DO";

			// Token: 0x040001AC RID: 428
			internal const string PROXY_URI = "PU";

			// Token: 0x040001AD RID: 429
			internal const string LICENSE_KEY = "LK";
		}
	}
}
