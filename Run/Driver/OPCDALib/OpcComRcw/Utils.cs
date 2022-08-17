using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using OpcRcw.Ae;
using OpcRcw.Da;
using OpcRcw.Hda;

namespace OpcRcw
{
	public static class Utils
	{
		//public static List<Type> RegisterComTypes(string filePath)
		//{
		//	Assembly assembly = Assembly.LoadFrom(filePath);
  //          VerifyCodebase(assembly, filePath);
		//	RegistrationServices registrationServices = new RegistrationServices();
		//	List<Type> list = new List<Type>(registrationServices.GetRegistrableTypesInAssembly(assembly));
		//	if (list.Count > 0)
		//	{
		//		if (!registrationServices.UnregisterAssembly(assembly))
		//		{
		//			throw new ApplicationException("Unregister COM Types Failed.");
		//		}
		//		if (!registrationServices.RegisterAssembly(assembly, AssemblyRegistrationFlags.SetCodeBase))
		//		{
		//			throw new ApplicationException("Register COM Types Failed.");
		//		}
		//	}
		//	return list;
		//}

		private static void VerifyCodebase(Assembly assembly, string filepath)
		{
			string text = assembly.CodeBase.ToLower();
			string text2 = filepath.Replace('\\', '/').Replace("//", "/").ToLower();
			if (!text2.StartsWith("file:///"))
			{
				text2 = "file:///" + text2;
			}
			if (text != text2)
			{
				throw new ApplicationException(string.Format("Duplicate assembly loaded. You need to restart the application.\r\n{0}\r\n{1}", text, text2));
			}
		}

		//public static List<Type> UnregisterComTypes(string filePath)
		//{
		//	Assembly assembly = Assembly.LoadFrom(filePath);
		//	Utils.VerifyCodebase(assembly, filePath);
		//	RegistrationServices registrationServices = new RegistrationServices();
		//	List<Type> result = new List<Type>(registrationServices.GetRegistrableTypesInAssembly(assembly));
		//	if (!registrationServices.UnregisterAssembly(assembly))
		//	{
		//		throw new ApplicationException("Unregister COM Types Failed.");
		//	}
		//	return result;
		//}

		public static string GetSystemMessage(int error, int localeId)
		{
			int dwLanguageId;
			if (localeId != 1024)
			{
				if (localeId == 2048)
				{
					dwLanguageId = Utils.GetSystemDefaultLangID();
				}
				else
				{
					dwLanguageId = (65535 & localeId);
				}
			}
			else
			{
				dwLanguageId = Utils.GetUserDefaultLangID();
			}
			IntPtr intPtr = Marshal.AllocCoTaskMem(1024);
			int num = Utils.FormatMessageW(4096, IntPtr.Zero, error, dwLanguageId, intPtr, 1023, IntPtr.Zero);
			if (num > 0)
			{
				string text = Marshal.PtrToStringUni(intPtr);
				Marshal.FreeCoTaskMem(intPtr);
				if (text != null && text.Length > 0)
				{
					return text.Trim();
				}
			}
			return string.Format("0x{0:X8}", error);
		}

		public static string ProgIDFromCLSID(Guid clsid)
		{
			RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(string.Format("CLSID\\{{{0}}}\\ProgId", clsid));
			if (registryKey != null)
			{
				try
				{
					return registryKey.GetValue("") as string;
				}
				finally
				{
					registryKey.Close();
				}
			}
			return null;
		}

		public static Guid CLSIDFromProgID(string progID)
		{
			if (string.IsNullOrEmpty(progID))
			{
				return Guid.Empty;
			}
			RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(string.Format("{0}\\CLSID", progID));
			if (registryKey != null)
			{
				try
				{
					string text = registryKey.GetValue(null) as string;
					if (text != null)
					{
						return new Guid(text.Substring(1, text.Length - 2));
					}
				}
				finally
				{
					registryKey.Close();
				}
			}
			return Guid.Empty;
		}

		public static List<Guid> GetImplementedCategories(Guid clsid)
		{
			List<Guid> list = new List<Guid>();
			string name = string.Format("CLSID\\{{{0}}}\\Implemented Categories", clsid);
			RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(name);
			if (registryKey != null)
			{
				try
				{
					foreach (string text in registryKey.GetSubKeyNames())
					{
						try
						{
							Guid item = new Guid(text.Substring(1, text.Length - 2));
							list.Add(item);
						}
						catch (Exception)
						{
						}
					}
				}
				finally
				{
					registryKey.Close();
				}
			}
			return list;
		}

		public static List<Guid> EnumClassesInCategories(params Guid[] categories)
		{
			Utils.ICatInformation catInformation = (Utils.ICatInformation)Utils.CreateLocalServer(Utils.CLSID_StdComponentCategoriesMgr);
			object obj = null;
			List<Guid> result;
			try
			{
				catInformation.EnumClassesOfCategories(1, categories, 0, null, out obj);
				Utils.IEnumGUID enumGUID = (Utils.IEnumGUID)obj;
				List<Guid> list = new List<Guid>();
				Guid[] array = new Guid[10];
				for (;;)
				{
					int num = 0;
					IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(Guid)) * array.Length);
					try
					{
						enumGUID.Next(array.Length, intPtr, out num);
						if (num == 0)
						{
							break;
						}
						IntPtr ptr = intPtr;
						for (int i = 0; i < num; i++)
						{
							array[i] = (Guid)Marshal.PtrToStructure(ptr, typeof(Guid));
							ptr = (IntPtr)(ptr.ToInt64() + (long)Marshal.SizeOf(typeof(Guid)));
						}
					}
					finally
					{
						Marshal.FreeCoTaskMem(intPtr);
					}
					for (int j = 0; j < num; j++)
					{
						list.Add(array[j]);
					}
				}
				result = list;
			}
			finally
			{
				Utils.ReleaseServer(obj);
				Utils.ReleaseServer(catInformation);
			}
			return result;
		}

		public static string GetExecutablePath(Guid clsid)
		{
			RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(string.Format("CLSID\\{{{0}}}\\LocalServer32", clsid));
			if (registryKey == null)
			{
				registryKey = Registry.ClassesRoot.OpenSubKey(string.Format("CLSID\\{{{0}}}\\InprocServer32", clsid));
			}
			if (registryKey != null)
			{
				try
				{
					string text = registryKey.GetValue("Codebase") as string;
					if (text == null)
					{
						return registryKey.GetValue(null) as string;
					}
					return text;
				}
				finally
				{
					registryKey.Close();
				}
			}
			return null;
		}

		public static object CreateLocalServer(Guid clsid)
		{
			Utils.COSERVERINFO coserverinfo = default(Utils.COSERVERINFO);
			coserverinfo.pwszName = null;
			coserverinfo.pAuthInfo = IntPtr.Zero;
			coserverinfo.dwReserved1 = 0U;
			coserverinfo.dwReserved2 = 0U;
			GCHandle gchandle = GCHandle.Alloc(Utils.IID_IUnknown, GCHandleType.Pinned);
			Utils.MULTI_QI[] array = new Utils.MULTI_QI[1];
			array[0].iid = gchandle.AddrOfPinnedObject();
			array[0].pItf = null;
			array[0].hr = 0U;
			try
			{
				Utils.CoCreateInstanceEx(ref clsid, null, 5U, ref coserverinfo, 1U, array);
			}
			finally
			{
				gchandle.Free();
			}
			if (array[0].hr != 0U)
			{
				throw new ExternalException("CoCreateInstanceEx: 0x{0:X8}" + array[0].hr);
			}
			return array[0].pItf;
		}

		public static object CreateInstance(Guid clsid, string hostName, string username, string password, string domain)
		{
			Utils.ServerInfo serverInfo = new Utils.ServerInfo();
			Utils.COSERVERINFO coserverinfo = serverInfo.Allocate(hostName, username, password, domain);
			GCHandle gchandle = GCHandle.Alloc(Utils.IID_IUnknown, GCHandleType.Pinned);
			Utils.MULTI_QI[] array = new Utils.MULTI_QI[1];
			array[0].iid = gchandle.AddrOfPinnedObject();
			array[0].pItf = null;
			array[0].hr = 0U;
			try
			{
				uint dwClsCtx = 5U;
				if (!string.IsNullOrEmpty(hostName) && hostName != "localhost")
				{
					dwClsCtx = 20U;
				}
				Utils.CoCreateInstanceEx(ref clsid, null, dwClsCtx, ref coserverinfo, 1U, array);
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
				serverInfo.Deallocate();
			}
			if (array[0].hr != 0U)
			{
				throw Utils.CreateComException(-2147467259, "Could not create COM server '{0}' on host '{1}'. Reason: {2}.", new object[]
				{
					clsid,
					hostName,
					Utils.GetSystemMessage((int)array[0].hr, 2048)
				});
			}
			return array[0].pItf;
		}

		public static void ReleaseServer(object server)
		{
			if (server != null && server.GetType().IsCOMObject)
			{
				Marshal.ReleaseComObject(server);
			}
		}

		public static void RegisterClassInCategory(Guid clsid, Guid catid)
		{
			Utils.RegisterClassInCategory(clsid, catid, null);
		}

		public static void RegisterClassInCategory(Guid clsid, Guid catid, string description)
		{
			Utils.ICatRegister catRegister = (Utils.ICatRegister)Utils.CreateLocalServer(Utils.CLSID_StdComponentCategoriesMgr);
			try
			{
				string text = null;
				try
				{
					((Utils.ICatInformation)catRegister).GetCategoryDesc(catid, 0, out text);
				}
				catch (Exception innerException)
				{
					text = description;
					if (string.IsNullOrEmpty(text))
					{
						if (catid == Utils.CATID_OPCDAServer20)
						{
							text = "OPC Data Access Servers Version 2.0";
						}
						else if (catid == Utils.CATID_OPCDAServer30)
						{
							text = "OPC Data Access Servers Version 3.0";
						}
						else if (catid == Utils.CATID_OPCAEServer10)
						{
							text = "OPC Alarm & Event Server Version 1.0";
						}
						else
						{
							if (!(catid == Utils.CATID_OPCHDAServer10))
							{
								throw new ApplicationException("No description for category available", innerException);
							}
							text = "OPC History Data Access Servers Version 1.0";
						}
					}
					Utils.CATEGORYINFO categoryinfo;
					categoryinfo.catid = catid;
					categoryinfo.lcid = 0;
					categoryinfo.szDescription = text;
					catRegister.RegisterCategories(1, new Utils.CATEGORYINFO[]
					{
						categoryinfo
					});
				}
				catRegister.RegisterClassImplCategories(clsid, 1, new Guid[]
				{
					catid
				});
			}
			finally
			{
				Utils.ReleaseServer(catRegister);
			}
		}

		public static void UnregisterComServer(Guid clsid)
		{
			string name = string.Format("CLSID\\{{{0}}}\\Implemented Categories", clsid);
			RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(name);
			if (registryKey != null)
			{
				try
				{
					foreach (string text in registryKey.GetSubKeyNames())
					{
						try
						{
							Utils.UnregisterClassInCategory(clsid, new Guid(text.Substring(1, text.Length - 2)));
						}
						catch (Exception)
						{
						}
					}
				}
				finally
				{
					registryKey.Close();
				}
			}
			string name2 = string.Format("CLSID\\{{{0}}}\\ProgId", clsid);
			registryKey = Registry.ClassesRoot.OpenSubKey(name2);
			if (registryKey != null)
			{
				string text2 = registryKey.GetValue(null) as string;
				registryKey.Close();
				if (!string.IsNullOrEmpty(text2))
				{
					try
					{
						Registry.ClassesRoot.DeleteSubKeyTree(text2);
					}
					catch (Exception)
					{
					}
				}
			}
			try
			{
				Registry.ClassesRoot.DeleteSubKeyTree(string.Format("CLSID\\{{{0}}}", clsid));
			}
			catch (Exception)
			{
			}
		}

		public static void UnregisterClassInCategory(Guid clsid, Guid catid)
		{
			Utils.ICatRegister catRegister = (Utils.ICatRegister)Utils.CreateLocalServer(Utils.CLSID_StdComponentCategoriesMgr);
			try
			{
				catRegister.UnRegisterClassImplCategories(clsid, 1, new Guid[]
				{
					catid
				});
			}
			finally
			{
				Utils.ReleaseServer(catRegister);
			}
		}

		public static Exception CreateComException(Exception e)
		{
			return Utils.CreateComException(e, 0, null, new object[0]);
		}

		public static Exception CreateComException(int code, string message, params object[] args)
		{
			return Utils.CreateComException(null, code, message, args);
		}

		public static Exception CreateComException(Exception e, int code, string message, params object[] args)
		{
			if (code == 0)
			{
				if (e is COMException)
				{
					code = ((COMException)e).ErrorCode;
				}
				else
				{
					code = -2147467259;
				}
			}
			if (!string.IsNullOrEmpty(message))
			{
				if (args != null && args.Length > 0)
				{
					message = string.Format(CultureInfo.CurrentUICulture, message, args);
				}
			}
			else if (e != null)
			{
				message = e.Message;
			}
			else
			{
				message = Utils.GetSystemMessage(code, CultureInfo.CurrentUICulture.LCID);
			}
			return new COMException(message, code);
		}

		// Token: 0x0600009F RID: 159
		[DllImport("ole32.dll")]
		private static extern void CoCreateInstanceEx(ref Guid clsid, [MarshalAs(UnmanagedType.IUnknown)] object punkOuter, uint dwClsCtx, [In] ref Utils.COSERVERINFO pServerInfo, uint dwCount, [In] [Out] Utils.MULTI_QI[] pResults);

		// Token: 0x060000A0 RID: 160
		[DllImport("Kernel32.dll")]
		private static extern int FormatMessageW(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, IntPtr lpBuffer, int nSize, IntPtr Arguments);

		// Token: 0x060000A1 RID: 161
		[DllImport("Kernel32.dll")]
		private static extern int GetSystemDefaultLangID();

		// Token: 0x060000A2 RID: 162
		[DllImport("Kernel32.dll")]
		private static extern int GetUserDefaultLangID();

		// Token: 0x060000A3 RID: 163
		[DllImport("OleAut32.dll")]
		private static extern int VariantChangeTypeEx(IntPtr pvargDest, IntPtr pvarSrc, int lcid, ushort wFlags, short vt);

		// Token: 0x060000A4 RID: 164
		[DllImport("oleaut32.dll")]
		private static extern void VariantInit(IntPtr pVariant);

		// Token: 0x060000A5 RID: 165
		[DllImport("oleaut32.dll")]
		public static extern void VariantClear(IntPtr pVariant);

		// Token: 0x060000A6 RID: 166
		[DllImport("ole32.dll")]
		private static extern int CoInitializeSecurity(IntPtr pSecDesc, int cAuthSvc, Utils.SOLE_AUTHENTICATION_SERVICE[] asAuthSvc, IntPtr pReserved1, uint dwAuthnLevel, uint dwImpLevel, IntPtr pAuthList, uint dwCapabilities, IntPtr pReserved3);

		// Token: 0x060000A7 RID: 167
		[DllImport("ole32.dll")]
		private static extern int CoQueryProxyBlanket([MarshalAs(UnmanagedType.IUnknown)] object pProxy, ref uint pAuthnSvc, ref uint pAuthzSvc, [MarshalAs(UnmanagedType.LPWStr)] ref string pServerPrincName, ref uint pAuthnLevel, ref uint pImpLevel, ref IntPtr pAuthInfo, ref uint pCapabilities);

		// Token: 0x060000A8 RID: 168
		[DllImport("ole32.dll")]
		private static extern int CoSetProxyBlanket([MarshalAs(UnmanagedType.IUnknown)] object pProxy, uint pAuthnSvc, uint pAuthzSvc, IntPtr pServerPrincName, uint pAuthnLevel, uint pImpLevel, IntPtr pAuthInfo, uint pCapabilities);

		// Token: 0x060000A9 RID: 169
		[DllImport("ole32.dll")]
		private static extern void CoGetClassObject([MarshalAs(UnmanagedType.LPStruct)] Guid clsid, uint dwClsContext, [In] ref Utils.COSERVERINFO pServerInfo, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

		// Token: 0x060000AA RID: 170
		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

		// Token: 0x060000AB RID: 171
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern bool CloseHandle(IntPtr handle);

		// Token: 0x060000AC RID: 172
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool DuplicateToken(IntPtr ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);

		// Token: 0x04000273 RID: 627
		private const uint CLSCTX_INPROC_SERVER = 1U;

		// Token: 0x04000274 RID: 628
		private const uint CLSCTX_INPROC_HANDLER = 2U;

		// Token: 0x04000275 RID: 629
		private const uint CLSCTX_LOCAL_SERVER = 4U;

		// Token: 0x04000276 RID: 630
		private const uint CLSCTX_REMOTE_SERVER = 16U;

		// Token: 0x04000277 RID: 631
		private const string CATID_OPCDAServer20_Description = "OPC Data Access Servers Version 2.0";

		// Token: 0x04000278 RID: 632
		private const string CATID_OPCDAServer30_Description = "OPC Data Access Servers Version 3.0";

		// Token: 0x04000279 RID: 633
		private const string CATID_OPCAEServer10_Description = "OPC Alarm & Event Server Version 1.0";

		// Token: 0x0400027A RID: 634
		private const string CATID_OPCHDAServer10_Description = "OPC History Data Access Servers Version 1.0";

		// Token: 0x0400027B RID: 635
		private const int MAX_MESSAGE_LENGTH = 1024;

		// Token: 0x0400027C RID: 636
		private const uint FORMAT_MESSAGE_IGNORE_INSERTS = 512U;

		// Token: 0x0400027D RID: 637
		private const uint FORMAT_MESSAGE_FROM_SYSTEM = 4096U;

		// Token: 0x0400027E RID: 638
		public const int LOCALE_SYSTEM_DEFAULT = 2048;

		// Token: 0x0400027F RID: 639
		public const int LOCALE_USER_DEFAULT = 1024;

		// Token: 0x04000280 RID: 640
		private const int VARIANT_SIZE = 16;

		// Token: 0x04000281 RID: 641
		private const int DISP_E_TYPEMISMATCH = -2147352571;

		// Token: 0x04000282 RID: 642
		private const int DISP_E_OVERFLOW = -2147352566;

		// Token: 0x04000283 RID: 643
		private const int VARIANT_NOVALUEPROP = 1;

		// Token: 0x04000284 RID: 644
		private const int VARIANT_ALPHABOOL = 2;

		// Token: 0x04000285 RID: 645
		private const uint RPC_C_AUTHN_NONE = 0U;

		// Token: 0x04000286 RID: 646
		private const uint RPC_C_AUTHN_DCE_PRIVATE = 1U;

		// Token: 0x04000287 RID: 647
		private const uint RPC_C_AUTHN_DCE_PUBLIC = 2U;

		// Token: 0x04000288 RID: 648
		private const uint RPC_C_AUTHN_DEC_PUBLIC = 4U;

		// Token: 0x04000289 RID: 649
		private const uint RPC_C_AUTHN_GSS_NEGOTIATE = 9U;

		// Token: 0x0400028A RID: 650
		private const uint RPC_C_AUTHN_WINNT = 10U;

		// Token: 0x0400028B RID: 651
		private const uint RPC_C_AUTHN_GSS_SCHANNEL = 14U;

		// Token: 0x0400028C RID: 652
		private const uint RPC_C_AUTHN_GSS_KERBEROS = 16U;

		// Token: 0x0400028D RID: 653
		private const uint RPC_C_AUTHN_DPA = 17U;

		// Token: 0x0400028E RID: 654
		private const uint RPC_C_AUTHN_MSN = 18U;

		// Token: 0x0400028F RID: 655
		private const uint RPC_C_AUTHN_DIGEST = 21U;

		// Token: 0x04000290 RID: 656
		private const uint RPC_C_AUTHN_MQ = 100U;

		// Token: 0x04000291 RID: 657
		private const uint RPC_C_AUTHN_DEFAULT = 4294967295U;

		// Token: 0x04000292 RID: 658
		private const uint RPC_C_AUTHZ_NONE = 0U;

		// Token: 0x04000293 RID: 659
		private const uint RPC_C_AUTHZ_NAME = 1U;

		// Token: 0x04000294 RID: 660
		private const uint RPC_C_AUTHZ_DCE = 2U;

		// Token: 0x04000295 RID: 661
		private const uint RPC_C_AUTHZ_DEFAULT = 4294967295U;

		// Token: 0x04000296 RID: 662
		private const uint RPC_C_AUTHN_LEVEL_DEFAULT = 0U;

		// Token: 0x04000297 RID: 663
		private const uint RPC_C_AUTHN_LEVEL_NONE = 1U;

		// Token: 0x04000298 RID: 664
		private const uint RPC_C_AUTHN_LEVEL_CONNECT = 2U;

		// Token: 0x04000299 RID: 665
		private const uint RPC_C_AUTHN_LEVEL_CALL = 3U;

		// Token: 0x0400029A RID: 666
		private const uint RPC_C_AUTHN_LEVEL_PKT = 4U;

		// Token: 0x0400029B RID: 667
		private const uint RPC_C_AUTHN_LEVEL_PKT_INTEGRITY = 5U;

		// Token: 0x0400029C RID: 668
		private const uint RPC_C_AUTHN_LEVEL_PKT_PRIVACY = 6U;

		// Token: 0x0400029D RID: 669
		private const uint RPC_C_IMP_LEVEL_ANONYMOUS = 1U;

		// Token: 0x0400029E RID: 670
		private const uint RPC_C_IMP_LEVEL_IDENTIFY = 2U;

		// Token: 0x0400029F RID: 671
		private const uint RPC_C_IMP_LEVEL_IMPERSONATE = 3U;

		// Token: 0x040002A0 RID: 672
		private const uint RPC_C_IMP_LEVEL_DELEGATE = 4U;

		// Token: 0x040002A1 RID: 673
		private const uint EOAC_NONE = 0U;

		// Token: 0x040002A2 RID: 674
		private const uint EOAC_MUTUAL_AUTH = 1U;

		// Token: 0x040002A3 RID: 675
		private const uint EOAC_CLOAKING = 16U;

		// Token: 0x040002A4 RID: 676
		private const uint EOAC_STATIC_CLOAKING = 32U;

		// Token: 0x040002A5 RID: 677
		private const uint EOAC_DYNAMIC_CLOAKING = 64U;

		// Token: 0x040002A6 RID: 678
		private const uint EOAC_SECURE_REFS = 2U;

		// Token: 0x040002A7 RID: 679
		private const uint EOAC_ACCESS_CONTROL = 4U;

		// Token: 0x040002A8 RID: 680
		private const uint EOAC_APPID = 8U;

		// Token: 0x040002A9 RID: 681
		private const uint SEC_WINNT_AUTH_IDENTITY_ANSI = 1U;

		// Token: 0x040002AA RID: 682
		private const uint SEC_WINNT_AUTH_IDENTITY_UNICODE = 2U;

		// Token: 0x040002AB RID: 683
		private const int LOGON32_PROVIDER_DEFAULT = 0;

		// Token: 0x040002AC RID: 684
		private const int LOGON32_LOGON_INTERACTIVE = 2;

		// Token: 0x040002AD RID: 685
		private const int LOGON32_LOGON_NETWORK = 3;

		// Token: 0x040002AE RID: 686
		private const int SECURITY_ANONYMOUS = 0;

		// Token: 0x040002AF RID: 687
		private const int SECURITY_IDENTIFICATION = 1;

		// Token: 0x040002B0 RID: 688
		private const int SECURITY_IMPERSONATION = 2;

		// Token: 0x040002B1 RID: 689
		private const int SECURITY_DELEGATION = 3;

		// Token: 0x040002B2 RID: 690
		public static readonly Guid CATID_OPCDAServer20 = typeof(CATID_OPCDAServer20).GUID;

		// Token: 0x040002B3 RID: 691
		public static readonly Guid CATID_OPCDAServer30 = typeof(CATID_OPCDAServer30).GUID;

		// Token: 0x040002B4 RID: 692
		public static readonly Guid CATID_OPCAEServer10 = typeof(CATID_OPCAEServer10).GUID;

		// Token: 0x040002B5 RID: 693
		public static readonly Guid CATID_OPCHDAServer10 = typeof(CATID_OPCHDAServer10).GUID;

		// Token: 0x040002B6 RID: 694
		private static readonly Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

		// Token: 0x040002B7 RID: 695
		private static readonly Guid CLSID_StdComponentCategoriesMgr = new Guid("0002E005-0000-0000-C000-000000000046");

		// Token: 0x040002B8 RID: 696
		private static readonly DateTime FILETIME_BaseTime = new DateTime(1601, 1, 1);

		// Token: 0x040002B9 RID: 697
		private static readonly IntPtr COLE_DEFAULT_PRINCIPAL = new IntPtr(-1);

		// Token: 0x040002BA RID: 698
		private static readonly IntPtr COLE_DEFAULT_AUTHINFO = new IntPtr(-1);

		// Token: 0x0200004D RID: 77
		private class ServerInfo
		{
			// Token: 0x060000AE RID: 174 RVA: 0x00002BE8 File Offset: 0x00000DE8
			public Utils.COSERVERINFO Allocate(string hostName, string username, string password, string domain)
			{
				Utils.COSERVERINFO result = default(Utils.COSERVERINFO);
				result.pwszName = hostName;
				result.pAuthInfo = IntPtr.Zero;
				result.dwReserved1 = 0U;
				result.dwReserved2 = 0U;
				if (string.IsNullOrEmpty(username))
				{
					return result;
				}
				this.m_hUserName = GCHandle.Alloc(username, GCHandleType.Pinned);
				this.m_hPassword = GCHandle.Alloc(password, GCHandleType.Pinned);
				this.m_hDomain = GCHandle.Alloc(domain, GCHandleType.Pinned);
				this.m_hIdentity = default(GCHandle);
				this.m_hIdentity = GCHandle.Alloc(new Utils.COAUTHIDENTITY
				{
					User = this.m_hUserName.AddrOfPinnedObject(),
					UserLength = (uint)((username != null) ? username.Length : 0),
					Password = this.m_hPassword.AddrOfPinnedObject(),
					PasswordLength = (uint)((password != null) ? password.Length : 0),
					Domain = this.m_hDomain.AddrOfPinnedObject(),
					DomainLength = (uint)((domain != null) ? domain.Length : 0),
					Flags = 2U
				}, GCHandleType.Pinned);
				this.m_hAuthInfo = GCHandle.Alloc(new Utils.COAUTHINFO
				{
					dwAuthnSvc = 10U,
					dwAuthzSvc = 0U,
					pwszServerPrincName = IntPtr.Zero,
					dwAuthnLevel = 2U,
					dwImpersonationLevel = 3U,
					pAuthIdentityData = this.m_hIdentity.AddrOfPinnedObject(),
					dwCapabilities = 0U
				}, GCHandleType.Pinned);
				result.pAuthInfo = this.m_hAuthInfo.AddrOfPinnedObject();
				return result;
			}

			// Token: 0x060000AF RID: 175 RVA: 0x00002D68 File Offset: 0x00000F68
			public void Deallocate()
			{
				if (this.m_hUserName.IsAllocated)
				{
					this.m_hUserName.Free();
				}
				if (this.m_hPassword.IsAllocated)
				{
					this.m_hPassword.Free();
				}
				if (this.m_hDomain.IsAllocated)
				{
					this.m_hDomain.Free();
				}
				if (this.m_hIdentity.IsAllocated)
				{
					this.m_hIdentity.Free();
				}
				if (this.m_hAuthInfo.IsAllocated)
				{
					this.m_hAuthInfo.Free();
				}
			}

			// Token: 0x040002BB RID: 699
			private GCHandle m_hUserName;

			// Token: 0x040002BC RID: 700
			private GCHandle m_hPassword;

			// Token: 0x040002BD RID: 701
			private GCHandle m_hDomain;

			// Token: 0x040002BE RID: 702
			private GCHandle m_hIdentity;

			// Token: 0x040002BF RID: 703
			private GCHandle m_hAuthInfo;
		}

		// Token: 0x0200004E RID: 78
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct MULTI_QI
		{
			// Token: 0x040002C0 RID: 704
			public IntPtr iid;

			// Token: 0x040002C1 RID: 705
			[MarshalAs(UnmanagedType.IUnknown)]
			public object pItf;

			// Token: 0x040002C2 RID: 706
			public uint hr;
		}

		// Token: 0x0200004F RID: 79
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0002E000-0000-0000-C000-000000000046")]
		[ComImport]
		private interface IEnumGUID
		{
			// Token: 0x060000B1 RID: 177
			void Next([MarshalAs(UnmanagedType.I4)] int celt, [Out] IntPtr rgelt, [MarshalAs(UnmanagedType.I4)] out int pceltFetched);

			// Token: 0x060000B2 RID: 178
			void Skip([MarshalAs(UnmanagedType.I4)] int celt);

			// Token: 0x060000B3 RID: 179
			void Reset();

			// Token: 0x060000B4 RID: 180
			void Clone(out Utils.IEnumGUID ppenum);
		}

		// Token: 0x02000050 RID: 80
		[Guid("0002E013-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		private interface ICatInformation
		{
			// Token: 0x060000B5 RID: 181
			void EnumCategories(int lcid, [MarshalAs(UnmanagedType.Interface)] out object ppenumCategoryInfo);

			// Token: 0x060000B6 RID: 182
			void GetCategoryDesc([MarshalAs(UnmanagedType.LPStruct)] Guid rcatid, int lcid, [MarshalAs(UnmanagedType.LPWStr)] out string pszDesc);

			// Token: 0x060000B7 RID: 183
			void EnumClassesOfCategories(int cImplemented, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] Guid[] rgcatidImpl, int cRequired, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 2)] Guid[] rgcatidReq, [MarshalAs(UnmanagedType.Interface)] out object ppenumClsid);

			// Token: 0x060000B8 RID: 184
			void IsClassOfCategories([MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, int cImplemented, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] Guid[] rgcatidImpl, int cRequired, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 3)] Guid[] rgcatidReq);

			// Token: 0x060000B9 RID: 185
			void EnumImplCategoriesOfClass([MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, [MarshalAs(UnmanagedType.Interface)] out object ppenumCatid);

			// Token: 0x060000BA RID: 186
			void EnumReqCategoriesOfClass([MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, [MarshalAs(UnmanagedType.Interface)] out object ppenumCatid);
		}

		// Token: 0x02000051 RID: 81
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct CATEGORYINFO
		{
			// Token: 0x040002C3 RID: 707
			public Guid catid;

			// Token: 0x040002C4 RID: 708
			public int lcid;

			// Token: 0x040002C5 RID: 709
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 127)]
			public string szDescription;
		}

		// Token: 0x02000052 RID: 82
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0002E012-0000-0000-C000-000000000046")]
		[ComImport]
		private interface ICatRegister
		{
			// Token: 0x060000BB RID: 187
			void RegisterCategories(int cCategories, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] Utils.CATEGORYINFO[] rgCategoryInfo);

			// Token: 0x060000BC RID: 188
			void UnRegisterCategories(int cCategories, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 0)] Guid[] rgcatid);

			// Token: 0x060000BD RID: 189
			void RegisterClassImplCategories([MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, int cCategories, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] Guid[] rgcatid);

			// Token: 0x060000BE RID: 190
			void UnRegisterClassImplCategories([MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, int cCategories, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] Guid[] rgcatid);

			// Token: 0x060000BF RID: 191
			void RegisterClassReqCategories([MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, int cCategories, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] Guid[] rgcatid);

			// Token: 0x060000C0 RID: 192
			void UnRegisterClassReqCategories([MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, int cCategories, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] Guid[] rgcatid);
		}

		// Token: 0x02000053 RID: 83
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct GUID
		{
			// Token: 0x040002C6 RID: 710
			public int Data1;

			// Token: 0x040002C7 RID: 711
			public short Data2;

			// Token: 0x040002C8 RID: 712
			public short Data3;

			// Token: 0x040002C9 RID: 713
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public byte[] Data4;
		}

		// Token: 0x02000054 RID: 84
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct SOLE_AUTHENTICATION_SERVICE
		{
			// Token: 0x040002CA RID: 714
			public uint dwAuthnSvc;

			// Token: 0x040002CB RID: 715
			public uint dwAuthzSvc;

			// Token: 0x040002CC RID: 716
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pPrincipalName;

			// Token: 0x040002CD RID: 717
			public int hr;
		}

		// Token: 0x02000055 RID: 85
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct COSERVERINFO
		{
			// Token: 0x040002CE RID: 718
			public uint dwReserved1;

			// Token: 0x040002CF RID: 719
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwszName;

			// Token: 0x040002D0 RID: 720
			public IntPtr pAuthInfo;

			// Token: 0x040002D1 RID: 721
			public uint dwReserved2;
		}

		// Token: 0x02000056 RID: 86
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct COAUTHINFO
		{
			// Token: 0x040002D2 RID: 722
			public uint dwAuthnSvc;

			// Token: 0x040002D3 RID: 723
			public uint dwAuthzSvc;

			// Token: 0x040002D4 RID: 724
			public IntPtr pwszServerPrincName;

			// Token: 0x040002D5 RID: 725
			public uint dwAuthnLevel;

			// Token: 0x040002D6 RID: 726
			public uint dwImpersonationLevel;

			// Token: 0x040002D7 RID: 727
			public IntPtr pAuthIdentityData;

			// Token: 0x040002D8 RID: 728
			public uint dwCapabilities;
		}

		// Token: 0x02000057 RID: 87
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct COAUTHIDENTITY
		{
			// Token: 0x040002D9 RID: 729
			public IntPtr User;

			// Token: 0x040002DA RID: 730
			public uint UserLength;

			// Token: 0x040002DB RID: 731
			public IntPtr Domain;

			// Token: 0x040002DC RID: 732
			public uint DomainLength;

			// Token: 0x040002DD RID: 733
			public IntPtr Password;

			// Token: 0x040002DE RID: 734
			public uint PasswordLength;

			// Token: 0x040002DF RID: 735
			public uint Flags;
		}

		// Token: 0x02000058 RID: 88
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct LICINFO
		{
			// Token: 0x040002E0 RID: 736
			public int cbLicInfo;

			// Token: 0x040002E1 RID: 737
			[MarshalAs(UnmanagedType.Bool)]
			public bool fRuntimeKeyAvail;

			// Token: 0x040002E2 RID: 738
			[MarshalAs(UnmanagedType.Bool)]
			public bool fLicVerified;
		}

		// Token: 0x02000059 RID: 89
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000001-0000-0000-C000-000000000046")]
		[ComImport]
		private interface IClassFactory
		{
			// Token: 0x060000C1 RID: 193
			void CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object punkOuter, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);

			// Token: 0x060000C2 RID: 194
			void LockServer([MarshalAs(UnmanagedType.Bool)] bool fLock);
		}

		// Token: 0x0200005A RID: 90
		[Guid("B196B28F-BAB4-101A-B69C-00AA00341D07")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		private interface IClassFactory2
		{
			// Token: 0x060000C3 RID: 195
			void CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object punkOuter, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);

			// Token: 0x060000C4 RID: 196
			void LockServer([MarshalAs(UnmanagedType.Bool)] bool fLock);

			// Token: 0x060000C5 RID: 197
			void GetLicInfo([In] [Out] ref Utils.LICINFO pLicInfo);

			// Token: 0x060000C6 RID: 198
			void RequestLicKey(int dwReserved, [MarshalAs(UnmanagedType.BStr)] string pbstrKey);

			// Token: 0x060000C7 RID: 199
			void CreateInstanceLic([MarshalAs(UnmanagedType.IUnknown)] object punkOuter, [MarshalAs(UnmanagedType.IUnknown)] object punkReserved, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.BStr)] string bstrKey, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObject);
		}
	}
}
