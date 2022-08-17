using System;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Opc;
using Opc.Da;

namespace OpcCom
{
    public class Interop
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SERVER_INFO_100
        {
            public uint sv100_platform_id;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string sv100_name;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SOLE_AUTHENTICATION_SERVICE
        {
            public uint dwAuthnSvc;

            public uint dwAuthzSvc;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string pPrincipalName;

            public int hr;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct COSERVERINFO
        {
            public uint dwReserved1;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string pwszName;

            public IntPtr pAuthInfo;

            public uint dwReserved2;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct COAUTHINFO
        {
            public uint dwAuthnSvc;

            public uint dwAuthzSvc;

            public IntPtr pwszServerPrincName;

            public uint dwAuthnLevel;

            public uint dwImpersonationLevel;

            public IntPtr pAuthIdentityData;

            public uint dwCapabilities;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct COAUTHIDENTITY
        {
            public IntPtr User;

            public uint UserLength;

            public IntPtr Domain;

            public uint DomainLength;

            public IntPtr Password;

            public uint PasswordLength;

            public uint Flags;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct MULTI_QI
        {
            public IntPtr iid;

            [MarshalAs(UnmanagedType.IUnknown)]
            public object pItf;

            public uint hr;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct LICINFO
        {
            public int cbLicInfo;

            [MarshalAs(UnmanagedType.Bool)]
            public bool fRuntimeKeyAvail;

            [MarshalAs(UnmanagedType.Bool)]
            public bool fLicVerified;
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("B196B28F-BAB4-101A-B69C-00AA00341D07")]
        private interface IClassFactory2
        {
            void CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object punkOuter, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);

            void LockServer([MarshalAs(UnmanagedType.Bool)] bool fLock);

            void GetLicInfo([In][Out] ref LICINFO pLicInfo);

            void RequestLicKey(int dwReserved, [MarshalAs(UnmanagedType.BStr)] string pbstrKey);

            void CreateInstanceLic([MarshalAs(UnmanagedType.IUnknown)] object punkOuter, [MarshalAs(UnmanagedType.IUnknown)] object punkReserved, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.BStr)] string bstrKey, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObject);
        }

        [ComImport]
        [Guid("0000013D-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IClientSecurity
        {
            void QueryBlanket([MarshalAs(UnmanagedType.IUnknown)] object pProxy, ref uint pAuthnSvc, ref uint pAuthzSvc, [MarshalAs(UnmanagedType.LPWStr)] ref string pServerPrincName, ref uint pAuthnLevel, ref uint pImpLevel, ref IntPtr pAuthInfo, ref uint pCapabilities);

            void SetBlanket([MarshalAs(UnmanagedType.IUnknown)] object pProxy, uint pAuthnSvc, uint pAuthzSvc, [MarshalAs(UnmanagedType.LPWStr)] string pServerPrincName, uint pAuthnLevel, uint pImpLevel, IntPtr pAuthInfo, uint pCapabilities);

            void CopyProxy([MarshalAs(UnmanagedType.IUnknown)] object pProxy, [MarshalAs(UnmanagedType.IUnknown)] out object ppCopy);
        }

        private class ServerInfo
        {
            private GCHandle m_hUserName;

            private GCHandle m_hPassword;

            private GCHandle m_hDomain;

            private GCHandle m_hIdentity;

            private GCHandle m_hAuthInfo;

            public COSERVERINFO Allocate(string hostName, NetworkCredential credential)
            {
                string text = null;
                string text2 = null;
                string text3 = null;
                if (credential != null)
                {
                    text = credential.UserName;
                    text2 = credential.Password;
                    text3 = credential.Domain;
                }

                m_hUserName = GCHandle.Alloc(text, GCHandleType.Pinned);
                m_hPassword = GCHandle.Alloc(text2, GCHandleType.Pinned);
                m_hDomain = GCHandle.Alloc(text3, GCHandleType.Pinned);
                m_hIdentity = default(GCHandle);
                if (text != null && text != string.Empty)
                {
                    COAUTHIDENTITY cOAUTHIDENTITY = default(COAUTHIDENTITY);
                    cOAUTHIDENTITY.User = m_hUserName.AddrOfPinnedObject();
                    cOAUTHIDENTITY.UserLength = (uint)(text?.Length ?? 0);
                    cOAUTHIDENTITY.Password = m_hPassword.AddrOfPinnedObject();
                    cOAUTHIDENTITY.PasswordLength = (uint)(text2?.Length ?? 0);
                    cOAUTHIDENTITY.Domain = m_hDomain.AddrOfPinnedObject();
                    cOAUTHIDENTITY.DomainLength = (uint)(text3?.Length ?? 0);
                    cOAUTHIDENTITY.Flags = 2u;
                    m_hIdentity = GCHandle.Alloc(cOAUTHIDENTITY, GCHandleType.Pinned);
                }

                COAUTHINFO cOAUTHINFO = default(COAUTHINFO);
                cOAUTHINFO.dwAuthnSvc = 10u;
                cOAUTHINFO.dwAuthzSvc = 0u;
                cOAUTHINFO.pwszServerPrincName = IntPtr.Zero;
                cOAUTHINFO.dwAuthnLevel = 2u;
                cOAUTHINFO.dwImpersonationLevel = 3u;
                cOAUTHINFO.pAuthIdentityData = (m_hIdentity.IsAllocated ? m_hIdentity.AddrOfPinnedObject() : IntPtr.Zero);
                cOAUTHINFO.dwCapabilities = 0u;
                m_hAuthInfo = GCHandle.Alloc(cOAUTHINFO, GCHandleType.Pinned);
                COSERVERINFO result = default(COSERVERINFO);
                result.pwszName = hostName;
                result.pAuthInfo = ((credential != null) ? m_hAuthInfo.AddrOfPinnedObject() : IntPtr.Zero);
                result.dwReserved1 = 0u;
                result.dwReserved2 = 0u;
                return result;
            }

            public void Deallocate()
            {
                if (m_hUserName.IsAllocated)
                {
                    m_hUserName.Free();
                }

                if (m_hPassword.IsAllocated)
                {
                    m_hPassword.Free();
                }

                if (m_hDomain.IsAllocated)
                {
                    m_hDomain.Free();
                }

                if (m_hIdentity.IsAllocated)
                {
                    m_hIdentity.Free();
                }

                if (m_hAuthInfo.IsAllocated)
                {
                    m_hAuthInfo.Free();
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct GUID
        {
            public int Data1;

            public short Data2;

            public short Data3;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] Data4;
        }

        private const uint LEVEL_SERVER_INFO_100 = 100u;

        private const uint LEVEL_SERVER_INFO_101 = 101u;

        private const int MAX_PREFERRED_LENGTH = -1;

        private const uint SV_TYPE_WORKSTATION = 1u;

        private const uint SV_TYPE_SERVER = 2u;

        private const int MAX_MESSAGE_LENGTH = 1024;

        private const uint FORMAT_MESSAGE_IGNORE_INSERTS = 512u;

        private const uint FORMAT_MESSAGE_FROM_SYSTEM = 4096u;

        private const int MAX_COMPUTERNAME_LENGTH = 31;

        private const uint RPC_C_AUTHN_NONE = 0u;

        private const uint RPC_C_AUTHN_DCE_PRIVATE = 1u;

        private const uint RPC_C_AUTHN_DCE_PUBLIC = 2u;

        private const uint RPC_C_AUTHN_DEC_PUBLIC = 4u;

        private const uint RPC_C_AUTHN_GSS_NEGOTIATE = 9u;

        private const uint RPC_C_AUTHN_WINNT = 10u;

        private const uint RPC_C_AUTHN_GSS_SCHANNEL = 14u;

        private const uint RPC_C_AUTHN_GSS_KERBEROS = 16u;

        private const uint RPC_C_AUTHN_DPA = 17u;

        private const uint RPC_C_AUTHN_MSN = 18u;

        private const uint RPC_C_AUTHN_DIGEST = 21u;

        private const uint RPC_C_AUTHN_MQ = 100u;

        private const uint RPC_C_AUTHN_DEFAULT = uint.MaxValue;

        private const uint RPC_C_AUTHZ_NONE = 0u;

        private const uint RPC_C_AUTHZ_NAME = 1u;

        private const uint RPC_C_AUTHZ_DCE = 2u;

        private const uint RPC_C_AUTHZ_DEFAULT = uint.MaxValue;

        private const uint RPC_C_AUTHN_LEVEL_DEFAULT = 0u;

        private const uint RPC_C_AUTHN_LEVEL_NONE = 1u;

        private const uint RPC_C_AUTHN_LEVEL_CONNECT = 2u;

        private const uint RPC_C_AUTHN_LEVEL_CALL = 3u;

        private const uint RPC_C_AUTHN_LEVEL_PKT = 4u;

        private const uint RPC_C_AUTHN_LEVEL_PKT_INTEGRITY = 5u;

        private const uint RPC_C_AUTHN_LEVEL_PKT_PRIVACY = 6u;

        private const uint RPC_C_IMP_LEVEL_ANONYMOUS = 1u;

        private const uint RPC_C_IMP_LEVEL_IDENTIFY = 2u;

        private const uint RPC_C_IMP_LEVEL_IMPERSONATE = 3u;

        private const uint RPC_C_IMP_LEVEL_DELEGATE = 4u;

        private const uint EOAC_NONE = 0u;

        private const uint EOAC_MUTUAL_AUTH = 1u;

        private const uint EOAC_CLOAKING = 16u;

        private const uint EOAC_SECURE_REFS = 2u;

        private const uint EOAC_ACCESS_CONTROL = 4u;

        private const uint EOAC_APPID = 8u;

        private const uint CLSCTX_INPROC_SERVER = 1u;

        private const uint CLSCTX_INPROC_HANDLER = 2u;

        private const uint CLSCTX_LOCAL_SERVER = 4u;

        private const uint CLSCTX_REMOTE_SERVER = 16u;

        private const uint SEC_WINNT_AUTH_IDENTITY_ANSI = 1u;

        private const uint SEC_WINNT_AUTH_IDENTITY_UNICODE = 2u;

        internal const int LOCALE_SYSTEM_DEFAULT = 2048;

        internal const int LOCALE_USER_DEFAULT = 1024;

        private static readonly Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

        private static bool m_preserveUTC = false;

        private static readonly DateTime FILETIME_BaseTime = new DateTime(1601, 1, 1);

        public static bool PreserveUTC
        {
            get
            {
                lock (typeof(Interop))
                {
                    return m_preserveUTC;
                }
            }
            set
            {
                lock (typeof(Interop))
                {
                    m_preserveUTC = value;
                }
            }
        }

        private static int VARIANT_SIZE
        {
            get
            {
                if (IntPtr.Size <= 4)
                {
                    return 16;
                }

                return 24;
            }
        }

        [DllImport("Netapi32.dll")]
        private static extern int NetServerEnum(IntPtr servername, uint level, out IntPtr bufptr, int prefmaxlen, out int entriesread, out int totalentries, uint servertype, IntPtr domain, IntPtr resume_handle);

        [DllImport("Netapi32.dll")]
        private static extern int NetApiBufferFree(IntPtr buffer);

        public static string[] EnumComputers()
        {
            int entriesread = 0;
            int totalentries = 0;
            IntPtr bufptr;
            int num = NetServerEnum(IntPtr.Zero, 100u, out bufptr, -1, out entriesread, out totalentries, 3u, IntPtr.Zero, IntPtr.Zero);
            if (num != 0)
            {
                throw new ApplicationException("NetApi Error = " + string.Format("0x{0,0:X}", num));
            }

            string[] array = new string[entriesread];
            IntPtr ptr = bufptr;
            for (int i = 0; i < entriesread; i++)
            {
                SERVER_INFO_100 sERVER_INFO_ = (SERVER_INFO_100)Marshal.PtrToStructure(ptr, typeof(SERVER_INFO_100));
                array[i] = sERVER_INFO_.sv100_name;
                ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(SERVER_INFO_100)));
            }

            NetApiBufferFree(bufptr);
            return array;
        }

        [DllImport("Kernel32.dll")]
        private static extern int FormatMessageW(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, IntPtr lpBuffer, int nSize, IntPtr Arguments);

        public static string GetSystemMessage(int error)
        {
            IntPtr intPtr = Marshal.AllocCoTaskMem(1024);
            FormatMessageW(4096, IntPtr.Zero, error, 0, intPtr, 1023, IntPtr.Zero);
            string text = Marshal.PtrToStringUni(intPtr);
            Marshal.FreeCoTaskMem(intPtr);
            if (text != null && text.Length > 0)
            {
                return text;
            }

            return string.Format("0x{0,0:X}", error);
        }

        [DllImport("Kernel32.dll")]
        private static extern int GetComputerNameW(IntPtr lpBuffer, ref int lpnSize);

        public static string GetComputerName()
        {
            string result = null;
            int lpnSize = 32;
            IntPtr intPtr = Marshal.AllocCoTaskMem(lpnSize * 2);
            if (GetComputerNameW(intPtr, ref lpnSize) != 0)
            {
                result = Marshal.PtrToStringUni(intPtr, lpnSize);
            }

            Marshal.FreeCoTaskMem(intPtr);
            return result;
        }

        [DllImport("ole32.dll")]
        private static extern int CoInitializeSecurity(IntPtr pSecDesc, int cAuthSvc, SOLE_AUTHENTICATION_SERVICE[] asAuthSvc, IntPtr pReserved1, uint dwAuthnLevel, uint dwImpLevel, IntPtr pAuthList, uint dwCapabilities, IntPtr pReserved3);

        [DllImport("ole32.dll")]
        private static extern void CoCreateInstanceEx(ref Guid clsid, [MarshalAs(UnmanagedType.IUnknown)] object punkOuter, uint dwClsCtx, [In] ref COSERVERINFO pServerInfo, uint dwCount, [In][Out] MULTI_QI[] pResults);

        [DllImport("ole32.dll")]
        private static extern void CoGetClassObject([MarshalAs(UnmanagedType.LPStruct)] Guid clsid, uint dwClsContext, [In] ref COSERVERINFO pServerInfo, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

        public static void InitializeSecurity()
        {
            int num = CoInitializeSecurity(IntPtr.Zero, -1, null, IntPtr.Zero, 1u, 2u, IntPtr.Zero, 0u, IntPtr.Zero);
            if (num != 0)
            {
                throw new ExternalException("CoInitializeSecurity: " + GetSystemMessage(num), num);
            }
        }

        public static object CreateInstance(Guid clsid, string hostName, NetworkCredential credential)
        {
            ServerInfo serverInfo = new ServerInfo();
            COSERVERINFO pServerInfo = serverInfo.Allocate(hostName, credential);
            GCHandle gCHandle = GCHandle.Alloc(IID_IUnknown, GCHandleType.Pinned);
            MULTI_QI[] array = new MULTI_QI[1];
            array[0].iid = gCHandle.AddrOfPinnedObject();
            array[0].pItf = null;
            array[0].hr = 0u;
            try
            {
                uint dwClsCtx = 5u;
                if (hostName != null && hostName.Length > 0 && hostName != "localhost")
                {
                    dwClsCtx = 20u;
                }

                CoCreateInstanceEx(ref clsid, null, dwClsCtx, ref pServerInfo, 1u, array);
            }
            finally
            {
                if (gCHandle.IsAllocated)
                {
                    gCHandle.Free();
                }

                serverInfo.Deallocate();
            }

            if (array[0].hr != 0)
            {
                throw new ExternalException("CoCreateInstanceEx: " + GetSystemMessage((int)array[0].hr));
            }

            return array[0].pItf;
        }

        public static object CreateInstanceWithLicenseKey(Guid clsid, string hostName, NetworkCredential credential, string licenseKey)
        {
            ServerInfo serverInfo = new ServerInfo();
            COSERVERINFO pServerInfo = serverInfo.Allocate(hostName, credential);
            object ppvObject = null;
            IClassFactory2 classFactory = null;
            try
            {
                uint dwClsContext = 5u;
                if (hostName != null && hostName.Length > 0)
                {
                    dwClsContext = 20u;
                }

                object ppv = null;
                CoGetClassObject(clsid, dwClsContext, ref pServerInfo, typeof(IClassFactory2).GUID, out ppv);
                classFactory = (IClassFactory2)ppv;
                IClientSecurity clientSecurity = (IClientSecurity)classFactory;
                uint pAuthnSvc = 0u;
                uint pAuthzSvc = 0u;
                string pServerPrincName = "";
                uint pAuthnLevel = 0u;
                uint pImpLevel = 0u;
                IntPtr pAuthInfo = IntPtr.Zero;
                uint pCapabilities = 0u;
                clientSecurity.QueryBlanket(classFactory, ref pAuthnSvc, ref pAuthzSvc, ref pServerPrincName, ref pAuthnLevel, ref pImpLevel, ref pAuthInfo, ref pCapabilities);
                pAuthnSvc = uint.MaxValue;
                pAuthnLevel = 2u;
                clientSecurity.SetBlanket(classFactory, pAuthnSvc, pAuthzSvc, pServerPrincName, pAuthnLevel, pImpLevel, pAuthInfo, pCapabilities);
                classFactory.CreateInstanceLic(null, null, IID_IUnknown, licenseKey, out ppvObject);
                return ppvObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                serverInfo.Deallocate();
            }
        }

        public static int[] GetInt32s(ref IntPtr pArray, int size, bool deallocate)
        {
            if (pArray == IntPtr.Zero || size <= 0)
            {
                return null;
            }

            int[] array = new int[size];
            Marshal.Copy(pArray, array, 0, size);
            if (deallocate)
            {
                Marshal.FreeCoTaskMem(pArray);
                pArray = IntPtr.Zero;
            }

            return array;
        }

        public static IntPtr GetInt32s(int[] input)
        {
            IntPtr intPtr = IntPtr.Zero;
            if (input != null)
            {
                intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * input.Length);
                Marshal.Copy(input, 0, intPtr, input.Length);
            }

            return intPtr;
        }

        public static short[] GetInt16s(ref IntPtr pArray, int size, bool deallocate)
        {
            if (pArray == IntPtr.Zero || size <= 0)
            {
                return null;
            }

            short[] array = new short[size];
            Marshal.Copy(pArray, array, 0, size);
            if (deallocate)
            {
                Marshal.FreeCoTaskMem(pArray);
                pArray = IntPtr.Zero;
            }

            return array;
        }

        public static IntPtr GetInt16s(short[] input)
        {
            IntPtr intPtr = IntPtr.Zero;
            if (input != null)
            {
                intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(short)) * input.Length);
                Marshal.Copy(input, 0, intPtr, input.Length);
            }

            return intPtr;
        }

        public static IntPtr GetUnicodeStrings(string[] values)
        {
            int num = (values != null) ? values.Length : 0;
            if (num <= 0)
            {
                return IntPtr.Zero;
            }

            IntPtr zero = IntPtr.Zero;
            IntPtr[] array = new IntPtr[num];
            for (int i = 0; i < num; i++)
            {
                ref IntPtr reference = ref array[i];
                reference = Marshal.StringToCoTaskMemUni(values[i]);
            }

            zero = Marshal.AllocCoTaskMem(values.Length * Marshal.SizeOf(typeof(IntPtr)));
            Marshal.Copy(array, 0, zero, num);
            return zero;
        }

        public static string[] GetUnicodeStrings(ref IntPtr pArray, int size, bool deallocate)
        {
            if (pArray == IntPtr.Zero || size <= 0)
            {
                return null;
            }

            IntPtr[] array = new IntPtr[size];
            Marshal.Copy(pArray, array, 0, size);
            string[] array2 = new string[size];
            for (int i = 0; i < size; i++)
            {
                IntPtr ptr = array[i];
                array2[i] = Marshal.PtrToStringUni(ptr);
                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(ptr);
                }
            }

            if (deallocate)
            {
                Marshal.FreeCoTaskMem(pArray);
                pArray = IntPtr.Zero;
            }

            return array2;
        }

        public static FILETIME GetFILETIME(DateTime datetime)
        {
            FILETIME result = default(FILETIME);
            if (datetime <= FILETIME_BaseTime)
            {
                result.dwHighDateTime = 0;
                result.dwLowDateTime = 0;
                return result;
            }

            long num = 0L;
            num = ((!m_preserveUTC) ? datetime.ToUniversalTime().Subtract(new TimeSpan(FILETIME_BaseTime.Ticks)).Ticks : datetime.Subtract(new TimeSpan(FILETIME_BaseTime.Ticks)).Ticks);
            result.dwHighDateTime = (int)((num >> 32) & uint.MaxValue);
            result.dwLowDateTime = (int)(num & uint.MaxValue);
            return result;
        }

        public static DateTime GetFILETIME(IntPtr pFiletime)
        {
            if (pFiletime == IntPtr.Zero)
            {
                return DateTime.MinValue;
            }

            return GetFILETIME((FILETIME)Marshal.PtrToStructure(pFiletime, typeof(FILETIME)));
        }

        public static DateTime GetFILETIME(FILETIME filetime)
        {
            long num = filetime.dwHighDateTime;
            if (num < 0)
            {
                num += 4294967296L;
            }

            long num2 = num << 32;
            num = filetime.dwLowDateTime;
            if (num < 0)
            {
                num += 4294967296L;
            }

            num2 += num;
            if (num2 == 0)
            {
                return DateTime.MinValue;
            }

            if (m_preserveUTC)
            {
                return FILETIME_BaseTime.Add(new TimeSpan(num2));
            }

            return FILETIME_BaseTime.Add(new TimeSpan(num2)).ToLocalTime();
        }

        public static IntPtr GetFILETIMEs(DateTime[] datetimes)
        {
            int num = (datetimes != null) ? datetimes.Length : 0;
            if (num <= 0)
            {
                return IntPtr.Zero;
            }

            IntPtr intPtr = Marshal.AllocCoTaskMem(num * Marshal.SizeOf(typeof(FILETIME)));
            IntPtr ptr = intPtr;
            for (int i = 0; i < num; i++)
            {
                Marshal.StructureToPtr((object)GetFILETIME(datetimes[i]), ptr, fDeleteOld: false);
                ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(FILETIME)));
            }

            return intPtr;
        }

        public static DateTime[] GetFILETIMEs(ref IntPtr pArray, int size, bool deallocate)
        {
            if (pArray == IntPtr.Zero || size <= 0)
            {
                return null;
            }

            DateTime[] array = new DateTime[size];
            IntPtr pFiletime = pArray;
            for (int i = 0; i < size; i++)
            {
                ref DateTime reference = ref array[i];
                reference = GetFILETIME(pFiletime);
                pFiletime = (IntPtr)(pFiletime.ToInt64() + Marshal.SizeOf(typeof(FILETIME)));
            }

            if (deallocate)
            {
                Marshal.FreeCoTaskMem(pArray);
                pArray = IntPtr.Zero;
            }

            return array;
        }

        public static Guid[] GetGUIDs(ref IntPtr pInput, int size, bool deallocate)
        {
            if (pInput == IntPtr.Zero || size <= 0)
            {
                return null;
            }

            Guid[] array = new Guid[size];
            IntPtr intPtr = pInput;
            for (int i = 0; i < size; i++)
            {
                GUID gUID = (GUID)Marshal.PtrToStructure(pInput, typeof(GUID));
                ref Guid reference = ref array[i];
                reference = new Guid(gUID.Data1, gUID.Data2, gUID.Data3, gUID.Data4);
                intPtr = (IntPtr)(intPtr.ToInt64() + Marshal.SizeOf(typeof(GUID)));
            }

            if (deallocate)
            {
                Marshal.FreeCoTaskMem(pInput);
                pInput = IntPtr.Zero;
            }

            return array;
        }

        [DllImport("oleaut32.dll")]
        public static extern void VariantClear(IntPtr pVariant);

        public static object GetVARIANT(object source)
        {
            if (source == null || (object)source.GetType() == null)
            {
                return null;
            }

            if ((object)source.GetType() == typeof(decimal[]))
            {
                decimal[] array = (decimal[])source;
                object[] array2 = new object[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    try
                    {
                        array2[i] = array[i];
                    }
                    catch (Exception)
                    {
                        array2[i] = double.NaN;
                    }
                }

                return array2;
            }

            return source;
        }

        public static IntPtr GetVARIANTs(object[] values, bool preprocess)
        {
            int num = (values != null) ? values.Length : 0;
            if (num <= 0)
            {
                return IntPtr.Zero;
            }

            IntPtr intPtr = Marshal.AllocCoTaskMem(num * VARIANT_SIZE);
            IntPtr pDstNativeVariant = intPtr;
            for (int i = 0; i < num; i++)
            {
                if (preprocess)
                {
                    Marshal.GetNativeVariantForObject(GetVARIANT(values[i]), pDstNativeVariant);
                }
                else
                {
                    Marshal.GetNativeVariantForObject(values[i], pDstNativeVariant);
                }

                pDstNativeVariant = (IntPtr)(pDstNativeVariant.ToInt64() + VARIANT_SIZE);
            }

            return intPtr;
        }

        public static object[] GetVARIANTs(ref IntPtr pArray, int size, bool deallocate)
        {
            if (pArray == IntPtr.Zero || size <= 0)
            {
                return null;
            }

            object[] array = new object[size];
            IntPtr intPtr = pArray;
            byte[] array2 = new byte[size * VARIANT_SIZE];
            Marshal.Copy(intPtr, array2, 0, array2.Length);
            for (int i = 0; i < size; i++)
            {
                try
                {
                    array[i] = Marshal.GetObjectForNativeVariant(intPtr);
                    if (deallocate)
                    {
                        VariantClear(intPtr);
                    }
                }
                catch (Exception)
                {
                    array[i] = null;
                }

                intPtr = (IntPtr)(intPtr.ToInt64() + VARIANT_SIZE);
            }

            if (deallocate)
            {
                Marshal.FreeCoTaskMem(pArray);
                pArray = IntPtr.Zero;
            }

            return array;
        }

        internal static string GetLocale(int input)
        {
            try
            {
                if (input == 2048 || input == 1024 || input == 0)
                {
                    return CultureInfo.InvariantCulture.Name;
                }

                return new CultureInfo(input).Name;
            }
            catch
            {
                throw new ExternalException("Invalid LCID", -2147024809);
            }
        }

        internal static int GetLocale(string input)
        {
            if (input == null || input == "")
            {
                return 0;
            }

            CultureInfo cultureInfo = null;
            try
            {
                cultureInfo = new CultureInfo(input);
            }
            catch
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }

            return cultureInfo.LCID;
        }

        internal static System.Type GetType(VarEnum input)
        {
            switch (input)
            {
                case VarEnum.VT_EMPTY:
                    return null;
                case VarEnum.VT_I1:
                    return typeof(sbyte);
                case VarEnum.VT_UI1:
                    return typeof(byte);
                case VarEnum.VT_I2:
                    return typeof(short);
                case VarEnum.VT_UI2:
                    return typeof(ushort);
                case VarEnum.VT_I4:
                    return typeof(int);
                case VarEnum.VT_UI4:
                    return typeof(uint);
                case VarEnum.VT_I8:
                    return typeof(long);
                case VarEnum.VT_UI8:
                    return typeof(ulong);
                case VarEnum.VT_R4:
                    return typeof(float);
                case VarEnum.VT_R8:
                    return typeof(double);
                case VarEnum.VT_CY:
                    return typeof(decimal);
                case VarEnum.VT_BOOL:
                    return typeof(bool);
                case VarEnum.VT_DATE:
                    return typeof(DateTime);
                case VarEnum.VT_BSTR:
                    return typeof(string);
                case (VarEnum)8208:
                    return typeof(sbyte[]);
                case (VarEnum)8209:
                    return typeof(byte[]);
                case (VarEnum)8194:
                    return typeof(short[]);
                case (VarEnum)8210:
                    return typeof(ushort[]);
                case (VarEnum)8195:
                    return typeof(int[]);
                case (VarEnum)8211:
                    return typeof(uint[]);
                case (VarEnum)8212:
                    return typeof(long[]);
                case (VarEnum)8213:
                    return typeof(ulong[]);
                case (VarEnum)8196:
                    return typeof(float[]);
                case (VarEnum)8197:
                    return typeof(double[]);
                case (VarEnum)8198:
                    return typeof(decimal[]);
                case (VarEnum)8203:
                    return typeof(bool[]);
                case (VarEnum)8199:
                    return typeof(DateTime[]);
                case (VarEnum)8200:
                    return typeof(string[]);
                case (VarEnum)8204:
                    return typeof(object[]);
                default:
                    return Opc.Type.ILLEGAL_TYPE;
            }
        }

        internal static VarEnum GetType(System.Type input)
        {
            if ((object)input == null)
            {
                return VarEnum.VT_EMPTY;
            }

            if ((object)input == typeof(sbyte))
            {
                return VarEnum.VT_I1;
            }

            if ((object)input == typeof(byte))
            {
                return VarEnum.VT_UI1;
            }

            if ((object)input == typeof(short))
            {
                return VarEnum.VT_I2;
            }

            if ((object)input == typeof(ushort))
            {
                return VarEnum.VT_UI2;
            }

            if ((object)input == typeof(int))
            {
                return VarEnum.VT_I4;
            }

            if ((object)input == typeof(uint))
            {
                return VarEnum.VT_UI4;
            }

            if ((object)input == typeof(long))
            {
                return VarEnum.VT_I8;
            }

            if ((object)input == typeof(ulong))
            {
                return VarEnum.VT_UI8;
            }

            if ((object)input == typeof(float))
            {
                return VarEnum.VT_R4;
            }

            if ((object)input == typeof(double))
            {
                return VarEnum.VT_R8;
            }

            if ((object)input == typeof(decimal))
            {
                return VarEnum.VT_CY;
            }

            if ((object)input == typeof(bool))
            {
                return VarEnum.VT_BOOL;
            }

            if ((object)input == typeof(DateTime))
            {
                return VarEnum.VT_DATE;
            }

            if ((object)input == typeof(string))
            {
                return VarEnum.VT_BSTR;
            }

            if ((object)input == typeof(object))
            {
                return VarEnum.VT_EMPTY;
            }

            if ((object)input == typeof(sbyte[]))
            {
                return (VarEnum)8208;
            }

            if ((object)input == typeof(byte[]))
            {
                return (VarEnum)8209;
            }

            if ((object)input == typeof(short[]))
            {
                return (VarEnum)8194;
            }

            if ((object)input == typeof(ushort[]))
            {
                return (VarEnum)8210;
            }

            if ((object)input == typeof(int[]))
            {
                return (VarEnum)8195;
            }

            if ((object)input == typeof(uint[]))
            {
                return (VarEnum)8211;
            }

            if ((object)input == typeof(long[]))
            {
                return (VarEnum)8212;
            }

            if ((object)input == typeof(ulong[]))
            {
                return (VarEnum)8213;
            }

            if ((object)input == typeof(float[]))
            {
                return (VarEnum)8196;
            }

            if ((object)input == typeof(double[]))
            {
                return (VarEnum)8197;
            }

            if ((object)input == typeof(decimal[]))
            {
                return (VarEnum)8198;
            }

            if ((object)input == typeof(bool[]))
            {
                return (VarEnum)8203;
            }

            if ((object)input == typeof(DateTime[]))
            {
                return (VarEnum)8199;
            }

            if ((object)input == typeof(string[]))
            {
                return (VarEnum)8200;
            }

            if ((object)input == typeof(object[]))
            {
                return (VarEnum)8204;
            }

            if ((object)input == Opc.Type.ILLEGAL_TYPE)
            {
                return (VarEnum)Enum.ToObject(typeof(VarEnum), 32767);
            }

            if ((object)input == typeof(System.Type))
            {
                return VarEnum.VT_I2;
            }

            if ((object)input == typeof(Quality))
            {
                return VarEnum.VT_I2;
            }

            if ((object)input == typeof(accessRights))
            {
                return VarEnum.VT_I4;
            }

            if ((object)input == typeof(euType))
            {
                return VarEnum.VT_I4;
            }

            return VarEnum.VT_EMPTY;
        }

        internal static ResultID GetResultID(int input)
        {
            switch (input)
            {
                case 0:
                    return new ResultID(ResultID.S_OK, input);
                case -2147467259:
                    return new ResultID(ResultID.E_FAIL, input);
                case -2147024809:
                    return new ResultID(ResultID.E_INVALIDARG, input);
                case -2147352571:
                    return new ResultID(ResultID.Da.E_BADTYPE, input);
                case -2147352566:
                    return new ResultID(ResultID.Da.E_RANGE, input);
                case -2147024882:
                    return new ResultID(ResultID.E_OUTOFMEMORY, input);
                case -2147467262:
                    return new ResultID(ResultID.E_NOTSUPPORTED, input);
                case -1073479679:
                    return new ResultID(ResultID.Da.E_INVALIDHANDLE, input);
                case -1073479676:
                    return new ResultID(ResultID.Da.E_BADTYPE, input);
                case -1073479673:
                    return new ResultID(ResultID.Da.E_UNKNOWN_ITEM_NAME, input);
                case -1073479672:
                    return new ResultID(ResultID.Da.E_INVALID_ITEM_NAME, input);
                case -1073479670:
                    return new ResultID(ResultID.Da.E_UNKNOWN_ITEM_PATH, input);
                case -1073479671:
                    return new ResultID(ResultID.Da.E_INVALID_FILTER, input);
                case -1073479669:
                    return new ResultID(ResultID.Da.E_RANGE, input);
                case 262157:
                    return new ResultID(ResultID.Da.S_UNSUPPORTEDRATE, input);
                case 262158:
                    return new ResultID(ResultID.Da.S_CLAMP, input);
                case -1073479165:
                    return new ResultID(ResultID.Da.E_INVALID_PID, input);
                case -1073478655:
                    return new ResultID(ResultID.Da.E_NO_ITEM_DEADBAND, input);
                case -1073478654:
                    return new ResultID(ResultID.Da.E_NO_ITEM_BUFFERING, input);
                case -1073478650:
                    return new ResultID(ResultID.Da.E_NO_WRITEQT, input);
                case -1073478653:
                    return new ResultID(ResultID.Da.E_INVALIDCONTINUATIONPOINT, input);
                case 263172:
                    return new ResultID(ResultID.Da.S_DATAQUEUEOVERFLOW, input);
                case -1073478649:
                    return new ResultID(ResultID.Cpx.E_TYPE_CHANGED, input);
                case -1073478648:
                    return new ResultID(ResultID.Cpx.E_FILTER_DUPLICATE, input);
                case -1073478647:
                    return new ResultID(ResultID.Cpx.E_FILTER_INVALID, input);
                case -1073478646:
                    return new ResultID(ResultID.Cpx.E_FILTER_ERROR, input);
                case 263179:
                    return new ResultID(ResultID.Cpx.S_FILTER_NO_DATA, input);
                case -1073475583:
                    return new ResultID(ResultID.Hda.E_MAXEXCEEDED, input);
                case 1074008066:
                    return new ResultID(ResultID.Hda.S_NODATA, input);
                case 1074008067:
                    return new ResultID(ResultID.Hda.S_MOREDATA, input);
                case -1073475580:
                    return new ResultID(ResultID.Hda.E_INVALIDAGGREGATE, input);
                case 1074008069:
                    return new ResultID(ResultID.Hda.S_CURRENTVALUE, input);
                case 1074008070:
                    return new ResultID(ResultID.Hda.S_EXTRADATA, input);
                case -2147217401:
                    return new ResultID(ResultID.Hda.W_NOFILTER, input);
                case -1073475576:
                    return new ResultID(ResultID.Hda.E_UNKNOWNATTRID, input);
                case -1073475575:
                    return new ResultID(ResultID.Hda.E_NOT_AVAIL, input);
                case -1073475574:
                    return new ResultID(ResultID.Hda.E_INVALIDDATATYPE, input);
                case -1073475573:
                    return new ResultID(ResultID.Hda.E_DATAEXISTS, input);
                case -1073475572:
                    return new ResultID(ResultID.Hda.E_INVALIDATTRID, input);
                case -1073475571:
                    return new ResultID(ResultID.Hda.E_NODATAEXISTS, input);
                case 1074008078:
                    return new ResultID(ResultID.Hda.S_INSERTED, input);
                case 1074008079:
                    return new ResultID(ResultID.Hda.S_REPLACED, input);
                case -1073477888:
                    return new ResultID(ResultID.Dx.E_PERSISTING, input);
                case -1073477887:
                    return new ResultID(ResultID.Dx.E_NOITEMLIST, input);
                case -1073477886:
                    return new ResultID(ResultID.Dx.E_VERSION_MISMATCH, input);
                case -1073477885:
                    return new ResultID(ResultID.Dx.E_VERSION_MISMATCH, input);
                case -1073477884:
                    return new ResultID(ResultID.Dx.E_UNKNOWN_ITEM_PATH, input);
                case -1073477883:
                    return new ResultID(ResultID.Dx.E_UNKNOWN_ITEM_NAME, input);
                case -1073477882:
                    return new ResultID(ResultID.Dx.E_INVALID_ITEM_PATH, input);
                case -1073477881:
                    return new ResultID(ResultID.Dx.E_INVALID_ITEM_NAME, input);
                case -1073477880:
                    return new ResultID(ResultID.Dx.E_INVALID_NAME, input);
                case -1073477879:
                    return new ResultID(ResultID.Dx.E_DUPLICATE_NAME, input);
                case -1073477878:
                    return new ResultID(ResultID.Dx.E_INVALID_BROWSE_PATH, input);
                case -1073477877:
                    return new ResultID(ResultID.Dx.E_INVALID_SERVER_URL, input);
                case -1073477876:
                    return new ResultID(ResultID.Dx.E_INVALID_SERVER_TYPE, input);
                case -1073477875:
                    return new ResultID(ResultID.Dx.E_UNSUPPORTED_SERVER_TYPE, input);
                case -1073477874:
                    return new ResultID(ResultID.Dx.E_CONNECTIONS_EXIST, input);
                case -1073477873:
                    return new ResultID(ResultID.Dx.E_TOO_MANY_CONNECTIONS, input);
                case -1073477872:
                    return new ResultID(ResultID.Dx.E_OVERRIDE_BADTYPE, input);
                case -1073477871:
                    return new ResultID(ResultID.Dx.E_OVERRIDE_RANGE, input);
                case -1073477870:
                    return new ResultID(ResultID.Dx.E_SUBSTITUTE_BADTYPE, input);
                case -1073477869:
                    return new ResultID(ResultID.Dx.E_SUBSTITUTE_RANGE, input);
                case -1073477868:
                    return new ResultID(ResultID.Dx.E_INVALID_TARGET_ITEM, input);
                case -1073477867:
                    return new ResultID(ResultID.Dx.E_UNKNOWN_TARGET_ITEM, input);
                case -1073477866:
                    return new ResultID(ResultID.Dx.E_TARGET_ALREADY_CONNECTED, input);
                case -1073477865:
                    return new ResultID(ResultID.Dx.E_UNKNOWN_SERVER_NAME, input);
                case -1073477864:
                    return new ResultID(ResultID.Dx.E_UNKNOWN_SOURCE_ITEM, input);
                case -1073477863:
                    return new ResultID(ResultID.Dx.E_INVALID_SOURCE_ITEM, input);
                case -1073477862:
                    return new ResultID(ResultID.Dx.E_INVALID_QUEUE_SIZE, input);
                case -1073477861:
                    return new ResultID(ResultID.Dx.E_INVALID_DEADBAND, input);
                case -1073477860:
                    return new ResultID(ResultID.Dx.E_INVALID_CONFIG_FILE, input);
                case -1073477859:
                    return new ResultID(ResultID.Dx.E_PERSIST_FAILED, input);
                case -1073477858:
                    return new ResultID(ResultID.Dx.E_TARGET_FAULT, input);
                case -1073477857:
                    return new ResultID(ResultID.Dx.E_TARGET_NO_ACCESSS, input);
                case -1073477856:
                    return new ResultID(ResultID.Dx.E_SOURCE_SERVER_FAULT, input);
                case -1073477855:
                    return new ResultID(ResultID.Dx.E_SOURCE_SERVER_NO_ACCESSS, input);
                case -1073477854:
                    return new ResultID(ResultID.Dx.E_SUBSCRIPTION_FAULT, input);
                case -1073477853:
                    return new ResultID(ResultID.Dx.E_SOURCE_ITEM_BADRIGHTS, input);
                case -1073477852:
                    return new ResultID(ResultID.Dx.E_SOURCE_ITEM_BAD_QUALITY, input);
                case -1073477851:
                    return new ResultID(ResultID.Dx.E_SOURCE_ITEM_BADTYPE, input);
                case -1073477850:
                    return new ResultID(ResultID.Dx.E_SOURCE_ITEM_RANGE, input);
                case -1073477849:
                    return new ResultID(ResultID.Dx.E_SOURCE_SERVER_NOT_CONNECTED, input);
                case -1073477848:
                    return new ResultID(ResultID.Dx.E_SOURCE_SERVER_TIMEOUT, input);
                case -1073477847:
                    return new ResultID(ResultID.Dx.E_TARGET_ITEM_DISCONNECTED, input);
                case -1073477846:
                    return new ResultID(ResultID.Dx.E_TARGET_NO_WRITES_ATTEMPTED, input);
                case -1073477845:
                    return new ResultID(ResultID.Dx.E_TARGET_ITEM_BADTYPE, input);
                case -1073477844:
                    return new ResultID(ResultID.Dx.E_TARGET_ITEM_RANGE, input);
                case 264064:
                    return new ResultID(ResultID.Dx.S_TARGET_SUBSTITUTED, input);
                case 264065:
                    return new ResultID(ResultID.Dx.S_TARGET_OVERRIDEN, input);
                case 264066:
                    return new ResultID(ResultID.Dx.S_CLAMP, input);
                case 262656:
                    return new ResultID(ResultID.Ae.S_ALREADYACKED, input);
                case 262657:
                    return new ResultID(ResultID.Ae.S_INVALIDBUFFERTIME, input);
                case 262658:
                    return new ResultID(ResultID.Ae.S_INVALIDMAXSIZE, input);
                case 262659:
                    return new ResultID(ResultID.Ae.S_INVALIDKEEPALIVETIME, input);
                case -1073479164:
                    return new ResultID(ResultID.Ae.E_INVALIDTIME, input);
                case -1073479163:
                    return new ResultID(ResultID.Ae.E_BUSY, input);
                case -1073479162:
                    return new ResultID(ResultID.Ae.E_NOINFO, input);
                default:
                    if ((input & 0x7FFF0000) == 65536)
                    {
                        return new ResultID(ResultID.E_NETWORK_ERROR, input);
                    }

                    if (input >= 0)
                    {
                        return new ResultID(ResultID.S_FALSE, input);
                    }

                    return new ResultID(ResultID.E_FAIL, input);
            }
        }

        internal static int GetResultID(ResultID input)
        {
            if (input.Name != null && input.Name.Namespace == "http://opcfoundation.org/DataAccess/")
            {
                if (input == ResultID.S_OK)
                {
                    return 0;
                }

                if (input == ResultID.E_FAIL)
                {
                    return -2147467259;
                }

                if (input == ResultID.E_INVALIDARG)
                {
                    return -2147024809;
                }

                if (input == ResultID.Da.E_BADTYPE)
                {
                    return -1073479676;
                }

                if (input == ResultID.Da.E_READONLY)
                {
                    return -1073479674;
                }

                if (input == ResultID.Da.E_WRITEONLY)
                {
                    return -1073479674;
                }

                if (input == ResultID.Da.E_RANGE)
                {
                    return -1073479669;
                }

                if (input == ResultID.E_OUTOFMEMORY)
                {
                    return -2147024882;
                }

                if (input == ResultID.E_NOTSUPPORTED)
                {
                    return -2147467262;
                }

                if (input == ResultID.Da.E_INVALIDHANDLE)
                {
                    return -1073479679;
                }

                if (input == ResultID.Da.E_UNKNOWN_ITEM_NAME)
                {
                    return -1073479673;
                }

                if (input == ResultID.Da.E_INVALID_ITEM_NAME)
                {
                    return -1073479672;
                }

                if (input == ResultID.Da.E_INVALID_ITEM_PATH)
                {
                    return -1073479672;
                }

                if (input == ResultID.Da.E_UNKNOWN_ITEM_PATH)
                {
                    return -1073479670;
                }

                if (input == ResultID.Da.E_INVALID_FILTER)
                {
                    return -1073479671;
                }

                if (input == ResultID.Da.S_UNSUPPORTEDRATE)
                {
                    return 262157;
                }

                if (input == ResultID.Da.S_CLAMP)
                {
                    return 262158;
                }

                if (input == ResultID.Da.E_INVALID_PID)
                {
                    return -1073479165;
                }

                if (input == ResultID.Da.E_NO_ITEM_DEADBAND)
                {
                    return -1073478655;
                }

                if (input == ResultID.Da.E_NO_ITEM_BUFFERING)
                {
                    return -1073478654;
                }

                if (input == ResultID.Da.E_NO_WRITEQT)
                {
                    return -1073478650;
                }

                if (input == ResultID.Da.E_INVALIDCONTINUATIONPOINT)
                {
                    return -1073478653;
                }

                if (input == ResultID.Da.S_DATAQUEUEOVERFLOW)
                {
                    return 263172;
                }
            }
            else if (input.Name != null && input.Name.Namespace == "http://opcfoundation.org/ComplexData/")
            {
                if (input == ResultID.Cpx.E_TYPE_CHANGED)
                {
                    return -1073478649;
                }

                if (input == ResultID.Cpx.E_FILTER_DUPLICATE)
                {
                    return -1073478648;
                }

                if (input == ResultID.Cpx.E_FILTER_INVALID)
                {
                    return -1073478647;
                }

                if (input == ResultID.Cpx.E_FILTER_ERROR)
                {
                    return -1073478646;
                }

                if (input == ResultID.Cpx.S_FILTER_NO_DATA)
                {
                    return 263179;
                }
            }
            else if (input.Name != null && input.Name.Namespace == "http://opcfoundation.org/HistoricalDataAccess/")
            {
                if (input == ResultID.Hda.E_MAXEXCEEDED)
                {
                    return -1073475583;
                }

                if (input == ResultID.Hda.S_NODATA)
                {
                    return 1074008066;
                }

                if (input == ResultID.Hda.S_MOREDATA)
                {
                    return 1074008067;
                }

                if (input == ResultID.Hda.E_INVALIDAGGREGATE)
                {
                    return -1073475580;
                }

                if (input == ResultID.Hda.S_CURRENTVALUE)
                {
                    return 1074008069;
                }

                if (input == ResultID.Hda.S_EXTRADATA)
                {
                    return 1074008070;
                }

                if (input == ResultID.Hda.E_UNKNOWNATTRID)
                {
                    return -1073475576;
                }

                if (input == ResultID.Hda.E_NOT_AVAIL)
                {
                    return -1073475575;
                }

                if (input == ResultID.Hda.E_INVALIDDATATYPE)
                {
                    return -1073475574;
                }

                if (input == ResultID.Hda.E_DATAEXISTS)
                {
                    return -1073475573;
                }

                if (input == ResultID.Hda.E_INVALIDATTRID)
                {
                    return -1073475572;
                }

                if (input == ResultID.Hda.E_NODATAEXISTS)
                {
                    return -1073475571;
                }

                if (input == ResultID.Hda.S_INSERTED)
                {
                    return 1074008078;
                }

                if (input == ResultID.Hda.S_REPLACED)
                {
                    return 1074008079;
                }
            }

            if (input.Name != null && input.Name.Namespace == "http://opcfoundation.org/DataExchange/")
            {
                if (input == ResultID.Dx.E_PERSISTING)
                {
                    return -1073477888;
                }

                if (input == ResultID.Dx.E_NOITEMLIST)
                {
                    return -1073477887;
                }

                if (input == ResultID.Dx.E_SERVER_STATE)
                {
                    return -1073477885;
                }

                if (input == ResultID.Dx.E_VERSION_MISMATCH)
                {
                    return -1073477885;
                }

                if (input == ResultID.Dx.E_UNKNOWN_ITEM_PATH)
                {
                    return -1073477884;
                }

                if (input == ResultID.Dx.E_UNKNOWN_ITEM_NAME)
                {
                    return -1073477883;
                }

                if (input == ResultID.Dx.E_INVALID_ITEM_PATH)
                {
                    return -1073477882;
                }

                if (input == ResultID.Dx.E_INVALID_ITEM_NAME)
                {
                    return -1073477881;
                }

                if (input == ResultID.Dx.E_INVALID_NAME)
                {
                    return -1073477880;
                }

                if (input == ResultID.Dx.E_DUPLICATE_NAME)
                {
                    return -1073477879;
                }

                if (input == ResultID.Dx.E_INVALID_BROWSE_PATH)
                {
                    return -1073477878;
                }

                if (input == ResultID.Dx.E_INVALID_SERVER_URL)
                {
                    return -1073477877;
                }

                if (input == ResultID.Dx.E_INVALID_SERVER_TYPE)
                {
                    return -1073477876;
                }

                if (input == ResultID.Dx.E_UNSUPPORTED_SERVER_TYPE)
                {
                    return -1073477875;
                }

                if (input == ResultID.Dx.E_CONNECTIONS_EXIST)
                {
                    return -1073477874;
                }

                if (input == ResultID.Dx.E_TOO_MANY_CONNECTIONS)
                {
                    return -1073477873;
                }

                if (input == ResultID.Dx.E_OVERRIDE_BADTYPE)
                {
                    return -1073477872;
                }

                if (input == ResultID.Dx.E_OVERRIDE_RANGE)
                {
                    return -1073477871;
                }

                if (input == ResultID.Dx.E_SUBSTITUTE_BADTYPE)
                {
                    return -1073477870;
                }

                if (input == ResultID.Dx.E_SUBSTITUTE_RANGE)
                {
                    return -1073477869;
                }

                if (input == ResultID.Dx.E_INVALID_TARGET_ITEM)
                {
                    return -1073477868;
                }

                if (input == ResultID.Dx.E_UNKNOWN_TARGET_ITEM)
                {
                    return -1073477867;
                }

                if (input == ResultID.Dx.E_TARGET_ALREADY_CONNECTED)
                {
                    return -1073477866;
                }

                if (input == ResultID.Dx.E_UNKNOWN_SERVER_NAME)
                {
                    return -1073477865;
                }

                if (input == ResultID.Dx.E_UNKNOWN_SOURCE_ITEM)
                {
                    return -1073477864;
                }

                if (input == ResultID.Dx.E_INVALID_SOURCE_ITEM)
                {
                    return -1073477863;
                }

                if (input == ResultID.Dx.E_INVALID_QUEUE_SIZE)
                {
                    return -1073477862;
                }

                if (input == ResultID.Dx.E_INVALID_DEADBAND)
                {
                    return -1073477861;
                }

                if (input == ResultID.Dx.E_INVALID_CONFIG_FILE)
                {
                    return -1073477860;
                }

                if (input == ResultID.Dx.E_PERSIST_FAILED)
                {
                    return -1073477859;
                }

                if (input == ResultID.Dx.E_TARGET_FAULT)
                {
                    return -1073477858;
                }

                if (input == ResultID.Dx.E_TARGET_NO_ACCESSS)
                {
                    return -1073477857;
                }

                if (input == ResultID.Dx.E_SOURCE_SERVER_FAULT)
                {
                    return -1073477856;
                }

                if (input == ResultID.Dx.E_SOURCE_SERVER_NO_ACCESSS)
                {
                    return -1073477855;
                }

                if (input == ResultID.Dx.E_SUBSCRIPTION_FAULT)
                {
                    return -1073477854;
                }

                if (input == ResultID.Dx.E_SOURCE_ITEM_BADRIGHTS)
                {
                    return -1073477853;
                }

                if (input == ResultID.Dx.E_SOURCE_ITEM_BAD_QUALITY)
                {
                    return -1073477852;
                }

                if (input == ResultID.Dx.E_SOURCE_ITEM_BADTYPE)
                {
                    return -1073477851;
                }

                if (input == ResultID.Dx.E_SOURCE_ITEM_RANGE)
                {
                    return -1073477850;
                }

                if (input == ResultID.Dx.E_SOURCE_SERVER_NOT_CONNECTED)
                {
                    return -1073477849;
                }

                if (input == ResultID.Dx.E_SOURCE_SERVER_TIMEOUT)
                {
                    return -1073477848;
                }

                if (input == ResultID.Dx.E_TARGET_ITEM_DISCONNECTED)
                {
                    return -1073477847;
                }

                if (input == ResultID.Dx.E_TARGET_NO_WRITES_ATTEMPTED)
                {
                    return -1073477846;
                }

                if (input == ResultID.Dx.E_TARGET_ITEM_BADTYPE)
                {
                    return -1073477845;
                }

                if (input == ResultID.Dx.E_TARGET_ITEM_RANGE)
                {
                    return -1073477844;
                }

                if (input == ResultID.Dx.S_TARGET_SUBSTITUTED)
                {
                    return 264064;
                }

                if (input == ResultID.Dx.S_TARGET_OVERRIDEN)
                {
                    return 264065;
                }

                if (input == ResultID.Dx.S_CLAMP)
                {
                    return 264066;
                }
            }
            else if (input.Code == -1)
            {
                if (input.Succeeded())
                {
                    return 1;
                }

                return -2147467259;
            }

            return input.Code;
        }

        public static Exception CreateException(string message, Exception e)
        {
            return CreateException(message, Marshal.GetHRForException(e));
        }

        public static Exception CreateException(string message, int code)
        {
            return new ResultIDException(GetResultID(code), message);
        }

        public static void ReleaseServer(object server)
        {
            if (server != null && server.GetType().IsCOMObject)
            {
                Marshal.ReleaseComObject(server);
            }
        }
    }
}
