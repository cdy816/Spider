using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;

namespace Opc
{
	// Token: 0x02000004 RID: 4
	[Serializable]
	public class Server : IServer, IDisposable, ISerializable, ICloneable
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002928 File Offset: 0x00001928
		public Server(Factory factory, URL url)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			this.m_factory = (IFactory)factory.Clone();
			this.m_server = null;
			this.m_url = null;
			this.m_name = null;
			this.m_supportedLocales = null;
			this.m_resourceManager = new ResourceManager("Opc.Resources.Strings", Assembly.GetExecutingAssembly());
			if (url != null)
			{
				this.SetUrl(url);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002998 File Offset: 0x00001998
		~Server()
		{
			this.Dispose(false);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000029C8 File Offset: 0x000019C8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000029D8 File Offset: 0x000019D8
		protected virtual void Dispose(bool disposing)
		{
			if (!this.m_disposed)
			{
				if (disposing)
				{
					if (this.m_factory != null)
					{
						this.m_factory.Dispose();
						this.m_factory = null;
					}
					if (this.m_server != null)
					{
						try
						{
							this.Disconnect();
						}
						catch
						{
						}
						this.m_server = null;
					}
				}
				this.m_disposed = true;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002A3C File Offset: 0x00001A3C
		protected Server(SerializationInfo info, StreamingContext context)
		{
			this.m_name = info.GetString("Name");
			this.m_url = (URL)info.GetValue("Url", typeof(URL));
			this.m_factory = (IFactory)info.GetValue("Factory", typeof(IFactory));
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002AA0 File Offset: 0x00001AA0
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Name", this.m_name);
			info.AddValue("Url", this.m_url);
			info.AddValue("Factory", this.m_factory);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002AD8 File Offset: 0x00001AD8
		public virtual object Clone()
		{
			Server server = (Server)base.MemberwiseClone();
			server.m_server = null;
			server.m_supportedLocales = null;
			server.m_locale = null;
			server.m_resourceManager = new ResourceManager("Opc.Resources.Strings", Assembly.GetExecutingAssembly());
			return server;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002B1C File Offset: 0x00001B1C
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002B24 File Offset: 0x00001B24
		public virtual string Name
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

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002B2D File Offset: 0x00001B2D
		// (set) Token: 0x0600001C RID: 28 RVA: 0x00002B49 File Offset: 0x00001B49
		public virtual URL Url
		{
			get
			{
				if (this.m_url == null)
				{
					return null;
				}
				return (URL)this.m_url.Clone();
			}
			set
			{
				this.SetUrl(value);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002B52 File Offset: 0x00001B52
		public virtual string Locale
		{
			get
			{
				return this.m_locale;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002B5A File Offset: 0x00001B5A
		public virtual string[] SupportedLocales
		{
			get
			{
				if (this.m_supportedLocales == null)
				{
					return null;
				}
				return (string[])this.m_supportedLocales.Clone();
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002B76 File Offset: 0x00001B76
		public virtual bool IsConnected
		{
			get
			{
				return this.m_server != null;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002B84 File Offset: 0x00001B84
		public virtual void Connect()
		{
			this.Connect(this.m_url, null);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002B93 File Offset: 0x00001B93
		public virtual void Connect(ConnectData connectData)
		{
			this.Connect(this.m_url, connectData);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002BA4 File Offset: 0x00001BA4
		public virtual void Connect(URL url, ConnectData connectData)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (this.m_server != null)
			{
				throw new AlreadyConnectedException();
			}
			this.SetUrl(url);
			try
			{
				this.m_server = this.m_factory.CreateInstance(url, connectData);
				this.m_connectData = connectData;
				this.GetSupportedLocales();
				this.SetLocale(this.m_locale);
			}
			catch (Exception ex)
			{
				if (this.m_server != null)
				{
					try
					{
						this.Disconnect();
					}
					catch
					{
					}
				}
				throw ex;
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002C38 File Offset: 0x00001C38
		public virtual void Disconnect()
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			this.m_server.Dispose();
			this.m_server = null;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002C5C File Offset: 0x00001C5C
		public virtual Server Duplicate()
		{
			Server server = (Server)Activator.CreateInstance(base.GetType(), new object[]
			{
				this.m_factory,
				this.m_url
			});
			server.m_connectData = this.m_connectData;
			server.m_locale = this.m_locale;
			return server;
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000025 RID: 37 RVA: 0x00002CAD File Offset: 0x00001CAD
		// (remove) Token: 0x06000026 RID: 38 RVA: 0x00002CBB File Offset: 0x00001CBB
		public virtual event ServerShutdownEventHandler ServerShutdown
		{
			add
			{
				this.m_server.ServerShutdown += value;
			}
			remove
			{
				this.m_server.ServerShutdown -= value;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002CC9 File Offset: 0x00001CC9
		public virtual string GetLocale()
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			this.m_locale = this.m_server.GetLocale();
			return this.m_locale;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002CF0 File Offset: 0x00001CF0
		public virtual string SetLocale(string locale)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			try
			{
				this.m_locale = this.m_server.SetLocale(locale);
			}
			catch
			{
				string text = Server.FindBestLocale(locale, this.m_supportedLocales);
				if (text != locale)
				{
					this.m_server.SetLocale(text);
				}
				this.m_locale = text;
			}
			return this.m_locale;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002D64 File Offset: 0x00001D64
		public virtual string[] GetSupportedLocales()
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			this.m_supportedLocales = this.m_server.GetSupportedLocales();
			return this.SupportedLocales;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002D8B File Offset: 0x00001D8B
		public virtual string GetErrorText(string locale, ResultID resultID)
		{
			if (this.m_server == null)
			{
				throw new NotConnectedException();
			}
			return this.m_server.GetErrorText((locale == null) ? this.m_locale : locale, resultID);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002DB4 File Offset: 0x00001DB4
		protected string GetString(string name)
		{
			CultureInfo culture = null;
			try
			{
				culture = new CultureInfo(this.Locale);
			}
			catch
			{
				culture = new CultureInfo("");
			}
			string result;
			try
			{
				result = this.m_resourceManager.GetString(name, culture);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002E14 File Offset: 0x00001E14
		protected void SetUrl(URL url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (this.m_server != null)
			{
				throw new AlreadyConnectedException();
			}
			this.m_url = (URL)url.Clone();
			string text = "";
			if (this.m_url.HostName != null)
			{
				text = this.m_url.HostName.ToLower();
				if (text == "localhost" || text == "127.0.0.1")
				{
					text = "";
				}
			}
			if (this.m_url.Port != 0)
			{
				text += string.Format(".{0}", this.m_url.Port);
			}
			if (text != "")
			{
				text += ".";
			}
			if (this.m_url.Scheme != "http")
			{
				string text2 = this.m_url.Path;
				int num = text2.LastIndexOf('/');
				if (num != -1)
				{
					text2 = text2.Substring(0, num);
				}
				text += text2;
			}
			else
			{
				string text3 = this.m_url.Path;
				int num2 = text3.LastIndexOf('.');
				if (num2 != -1)
				{
					text3 = text3.Substring(0, num2);
				}
				while (text3.IndexOf('/') != -1)
				{
					text3 = text3.Replace('/', '-');
				}
				text += text3;
			}
			this.m_name = text;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002F68 File Offset: 0x00001F68
		public static string FindBestLocale(string requestedLocale, string[] supportedLocales)
		{
			string result;
			try
			{
				foreach (string a in supportedLocales)
				{
					if (a == requestedLocale)
					{
						return requestedLocale;
					}
				}
				CultureInfo cultureInfo = new CultureInfo(requestedLocale);
				foreach (string name in supportedLocales)
				{
					try
					{
						CultureInfo cultureInfo2 = new CultureInfo(name);
						if (cultureInfo.Parent.Name == cultureInfo2.Name)
						{
							return cultureInfo2.Name;
						}
					}
					catch
					{
					}
				}
				result = ((supportedLocales != null && supportedLocales.Length > 0) ? supportedLocales[0] : "");
			}
			catch
			{
				result = ((supportedLocales != null && supportedLocales.Length > 0) ? supportedLocales[0] : "");
			}
			return result;
		}

		// Token: 0x04000001 RID: 1
		private bool m_disposed;

		// Token: 0x04000002 RID: 2
		protected IServer m_server;

		// Token: 0x04000003 RID: 3
		private URL m_url;

		// Token: 0x04000004 RID: 4
		protected IFactory m_factory;

		// Token: 0x04000005 RID: 5
		private ConnectData m_connectData;

		// Token: 0x04000006 RID: 6
		private string m_name;

		// Token: 0x04000007 RID: 7
		private string m_locale;

		// Token: 0x04000008 RID: 8
		private string[] m_supportedLocales;

		// Token: 0x04000009 RID: 9
		protected ResourceManager m_resourceManager;

		// Token: 0x02000005 RID: 5
		private class Names
		{
			// Token: 0x0400000A RID: 10
			internal const string NAME = "Name";

			// Token: 0x0400000B RID: 11
			internal const string URL = "Url";

			// Token: 0x0400000C RID: 12
			internal const string FACTORY = "Factory";
		}
	}
}
