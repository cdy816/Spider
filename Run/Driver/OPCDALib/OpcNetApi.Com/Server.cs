using System;
using System.Collections;
using Opc;
using OpcRcw.Comn;

namespace OpcCom
{
	// Token: 0x02000018 RID: 24
	public class Server : IServer, IDisposable
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x000096BC File Offset: 0x000086BC
		internal Server()
		{
			this.m_url = null;
			this.m_server = null;
			this.m_callback = new Server.Callback(this);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000096DE File Offset: 0x000086DE
		internal Server(URL url, object server)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			this.m_url = (URL)url.Clone();
			this.m_server = server;
			this.m_callback = new Server.Callback(this);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00009718 File Offset: 0x00008718
		~Server()
		{
			this.Dispose(false);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00009748 File Offset: 0x00008748
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00009758 File Offset: 0x00008758
		protected virtual void Dispose(bool disposing)
		{
			if (!this.m_disposed)
			{
				lock (this)
				{
					if (disposing && this.m_connection != null)
					{
						this.m_connection.Dispose();
						this.m_connection = null;
					}
					Interop.ReleaseServer(this.m_server);
					this.m_server = null;
				}
				this.m_disposed = true;
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000097C4 File Offset: 0x000087C4
		public virtual void Initialize(URL url, ConnectData connectData)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			lock (this)
			{
				if (this.m_url == null || !this.m_url.Equals(url))
				{
					if (this.m_server != null)
					{
						this.Uninitialize();
					}
					this.m_server = (IOPCCommon)Factory.Connect(url, connectData);
				}
				this.m_url = (URL)url.Clone();
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00009848 File Offset: 0x00008848
		public virtual void Uninitialize()
		{
			lock (this)
			{
				this.Dispose();
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060000CE RID: 206 RVA: 0x0000987C File Offset: 0x0000887C
		// (remove) Token: 0x060000CF RID: 207 RVA: 0x000098D0 File Offset: 0x000088D0
		public virtual event ServerShutdownEventHandler ServerShutdown
		{
			add
			{
				lock (this)
				{
					try
					{
						this.Advise();
						this.m_callback.ServerShutdown += value;
					}
					catch
					{
					}
				}
			}
			remove
			{
				lock (this)
				{
					this.m_callback.ServerShutdown -= value;
					this.Unadvise();
				}
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00009910 File Offset: 0x00008910
		public virtual string GetLocale()
		{
			string locale;
			lock (this)
			{
				if (this.m_server == null)
				{
					throw new NotConnectedException();
				}
				try
				{
					int input = 0;
					((IOPCCommon)this.m_server).GetLocaleID(out input);
					locale = Interop.GetLocale(input);
				}
				catch (Exception e)
				{
					throw Interop.CreateException("IOPCCommon.GetLocaleID", e);
				}
			}
			return locale;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00009984 File Offset: 0x00008984
		public virtual string SetLocale(string locale)
		{
			string locale3;
			lock (this)
			{
				if (this.m_server == null)
				{
					throw new NotConnectedException();
				}
				int locale2 = Interop.GetLocale(locale);
				try
				{
					((IOPCCommon)this.m_server).SetLocaleID(locale2);
				}
				catch (Exception e)
				{
					if (locale2 != 0)
					{
						throw Interop.CreateException("IOPCCommon.SetLocaleID", e);
					}
					try
					{
						((IOPCCommon)this.m_server).SetLocaleID(2048);
					}
					catch
					{
					}
				}
				locale3 = this.GetLocale();
			}
			return locale3;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00009A28 File Offset: 0x00008A28
		public virtual string[] GetSupportedLocales()
		{
			string[] result;
			lock (this)
			{
				if (this.m_server == null)
				{
					throw new NotConnectedException();
				}
				try
				{
					int size = 0;
					IntPtr zero = IntPtr.Zero;
					((IOPCCommon)this.m_server).QueryAvailableLocaleIDs(out size, out zero);
					int[] int32s = Interop.GetInt32s(ref zero, size, true);
					if (int32s != null)
					{
						ArrayList arrayList = new ArrayList();
						foreach (int input in int32s)
						{
							try
							{
								arrayList.Add(Interop.GetLocale(input));
							}
							catch
							{
							}
						}
						result = (string[])arrayList.ToArray(typeof(string));
					}
					else
					{
						result = null;
					}
				}
				catch (Exception e)
				{
					throw Interop.CreateException("IOPCCommon.QueryAvailableLocaleIDs", e);
				}
			}
			return result;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00009B0C File Offset: 0x00008B0C
		public virtual string GetErrorText(string locale, ResultID resultID)
		{
			string result;
			lock (this)
			{
				if (this.m_server == null)
				{
					throw new NotConnectedException();
				}
				try
				{
					string locale2 = this.GetLocale();
					if (locale2 != locale)
					{
						this.SetLocale(locale);
					}
					string text = null;
					((IOPCCommon)this.m_server).GetErrorString(resultID.Code, out text);
					if (locale2 != locale)
					{
						this.SetLocale(locale2);
					}
					result = text;
				}
				catch (Exception e)
				{
					throw Interop.CreateException("IOPCServer.GetErrorString", e);
				}
			}
			return result;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00009BAC File Offset: 0x00008BAC
		private void Advise()
		{
			if (this.m_connection == null)
			{
				this.m_connection = new ConnectionPoint(this.m_server, typeof(IOPCShutdown).GUID);
				this.m_connection.Advise(this.m_callback);
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00009BE8 File Offset: 0x00008BE8
		private void Unadvise()
		{
			if (this.m_connection != null && this.m_connection.Unadvise() == 0)
			{
				this.m_connection.Dispose();
				this.m_connection = null;
			}
		}

		// Token: 0x04000083 RID: 131
		private bool m_disposed;

		// Token: 0x04000084 RID: 132
		protected object m_server;

		// Token: 0x04000085 RID: 133
		protected URL m_url;

		// Token: 0x04000086 RID: 134
		private ConnectionPoint m_connection;

		// Token: 0x04000087 RID: 135
		private Server.Callback m_callback;

		// Token: 0x02000019 RID: 25
		private class Callback : IOPCShutdown
		{
			// Token: 0x060000D6 RID: 214 RVA: 0x00009C11 File Offset: 0x00008C11
			public Callback(Server server)
			{
				this.m_server = server;
			}

			// Token: 0x1400000A RID: 10
			// (add) Token: 0x060000D7 RID: 215 RVA: 0x00009C20 File Offset: 0x00008C20
			// (remove) Token: 0x060000D8 RID: 216 RVA: 0x00009C68 File Offset: 0x00008C68
			public event ServerShutdownEventHandler ServerShutdown
			{
				add
				{
					lock (this)
					{
						this.m_serverShutdown = (ServerShutdownEventHandler)Delegate.Combine(this.m_serverShutdown, value);
					}
				}
				remove
				{
					lock (this)
					{
						this.m_serverShutdown = (ServerShutdownEventHandler)Delegate.Remove(this.m_serverShutdown, value);
					}
				}
			}

			// Token: 0x1400000B RID: 11
			// (add) Token: 0x060000D9 RID: 217 RVA: 0x00009CB0 File Offset: 0x00008CB0
			// (remove) Token: 0x060000DA RID: 218 RVA: 0x00009CC9 File Offset: 0x00008CC9
			private event ServerShutdownEventHandler m_serverShutdown;

			// Token: 0x060000DB RID: 219 RVA: 0x00009CE4 File Offset: 0x00008CE4
			public void ShutdownRequest(string reason)
			{
				try
				{
					lock (this)
					{
						if (this.m_serverShutdown != null)
						{
							this.m_serverShutdown(reason);
						}
					}
				}
				catch (Exception ex)
				{
					string stackTrace = ex.StackTrace;
				}
			}

			// Token: 0x04000088 RID: 136
			private Server m_server;
		}
	}
}
