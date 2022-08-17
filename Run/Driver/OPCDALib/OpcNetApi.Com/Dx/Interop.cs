using System;
using System.Runtime.InteropServices;
using Opc;
using Opc.Dx;
using OpcRcw.Dx;

namespace OpcCom.Dx
{
    public class Interop
    {
        internal static ResultID[] GetResultIDs(ref IntPtr pInput, int count, bool deallocate)
        {
            ResultID[] array = null;
            if (pInput != IntPtr.Zero && count > 0)
            {
                array = new ResultID[count];
                int[] int32s = OpcCom.Interop.GetInt32s(ref pInput, count, deallocate);
                for (int i = 0; i < count; i++)
                {
                    ref ResultID reference = ref array[i];
                    reference = OpcCom.Interop.GetResultID(int32s[i]);
                }
            }

            return array;
        }

        internal static Opc.Dx.SourceServer[] GetSourceServers(ref IntPtr pInput, int count, bool deallocate)
        {
            Opc.Dx.SourceServer[] array = null;
            if (pInput != IntPtr.Zero && count > 0)
            {
                array = new Opc.Dx.SourceServer[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    OpcRcw.Dx.SourceServer sourceServer = (OpcRcw.Dx.SourceServer)Marshal.PtrToStructure(ptr, typeof(OpcRcw.Dx.SourceServer));
                    array[i] = new Opc.Dx.SourceServer();
                    array[i].ItemName = sourceServer.szItemName;
                    array[i].ItemPath = sourceServer.szItemPath;
                    array[i].Version = sourceServer.szVersion;
                    array[i].Name = sourceServer.szName;
                    array[i].Description = sourceServer.szDescription;
                    array[i].ServerType = sourceServer.szServerType;
                    array[i].ServerURL = sourceServer.szServerURL;
                    array[i].DefaultConnected = (sourceServer.bDefaultSourceServerConnected != 0);
                    ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(OpcRcw.Dx.SourceServer)));
                }

                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }

            return array;
        }

        internal static OpcRcw.Dx.SourceServer[] GetSourceServers(Opc.Dx.SourceServer[] input)
        {
            OpcRcw.Dx.SourceServer[] array = null;
            if (input != null && input.Length > 0)
            {
                array = new OpcRcw.Dx.SourceServer[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    array[i] = default(OpcRcw.Dx.SourceServer);
                    array[i].dwMask = 2147483647u;
                    array[i].szItemName = input[i].ItemName;
                    array[i].szItemPath = input[i].ItemPath;
                    array[i].szVersion = input[i].Version;
                    array[i].szName = input[i].Name;
                    array[i].szDescription = input[i].Description;
                    array[i].szServerType = input[i].ServerType;
                    array[i].szServerURL = input[i].ServerURL;
                    array[i].bDefaultSourceServerConnected = (input[i].DefaultConnected ? 1 : 0);
                }
            }

            return array;
        }

        internal static GeneralResponse GetGeneralResponse(DXGeneralResponse input, bool deallocate)
        {
            Opc.Dx.IdentifiedResult[] identifiedResults = GetIdentifiedResults(ref input.pIdentifiedResults, input.dwCount, deallocate);
            return new GeneralResponse(input.szConfigurationVersion, identifiedResults);
        }

        internal static Opc.Dx.IdentifiedResult[] GetIdentifiedResults(ref IntPtr pInput, int count, bool deallocate)
        {
            Opc.Dx.IdentifiedResult[] array = null;
            if (pInput != IntPtr.Zero && count > 0)
            {
                array = new Opc.Dx.IdentifiedResult[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    OpcRcw.Dx.IdentifiedResult identifiedResult = (OpcRcw.Dx.IdentifiedResult)Marshal.PtrToStructure(ptr, typeof(OpcRcw.Dx.IdentifiedResult));
                    array[i] = new Opc.Dx.IdentifiedResult();
                    array[i].ItemName = identifiedResult.szItemName;
                    array[i].ItemPath = identifiedResult.szItemPath;
                    array[i].Version = identifiedResult.szVersion;
                    array[i].ResultID = OpcCom.Interop.GetResultID(identifiedResult.hResultCode);
                    if (deallocate)
                    {
                        Marshal.DestroyStructure(ptr, typeof(OpcRcw.Dx.IdentifiedResult));
                    }

                    ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(OpcRcw.Dx.IdentifiedResult)));
                }

                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }

            return array;
        }

        internal static Opc.Dx.DXConnection[] GetDXConnections(ref IntPtr pInput, int count, bool deallocate)
        {
            Opc.Dx.DXConnection[] array = null;
            if (pInput != IntPtr.Zero && count > 0)
            {
                array = new Opc.Dx.DXConnection[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    OpcRcw.Dx.DXConnection input = (OpcRcw.Dx.DXConnection)Marshal.PtrToStructure(ptr, typeof(OpcRcw.Dx.DXConnection));
                    array[i] = GetDXConnection(input, deallocate);
                    if (deallocate)
                    {
                        Marshal.DestroyStructure(ptr, typeof(OpcRcw.Dx.DXConnection));
                    }

                    ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(OpcRcw.Dx.DXConnection)));
                }

                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }

            return array;
        }

        internal static OpcRcw.Dx.DXConnection[] GetDXConnections(Opc.Dx.DXConnection[] input)
        {
            OpcRcw.Dx.DXConnection[] array = null;
            if (input != null && input.Length > 0)
            {
                array = new OpcRcw.Dx.DXConnection[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    ref OpcRcw.Dx.DXConnection reference = ref array[i];
                    reference = GetDXConnection(input[i]);
                }
            }

            return array;
        }

        internal static OpcRcw.Dx.DXConnection GetDXConnection(Opc.Dx.DXConnection input)
        {
            OpcRcw.Dx.DXConnection result = default(OpcRcw.Dx.DXConnection);
            result.dwMask = 0u;
            result.szItemPath = null;
            result.szItemName = null;
            result.szVersion = null;
            result.dwBrowsePathCount = 0;
            result.pszBrowsePaths = IntPtr.Zero;
            result.szName = null;
            result.szDescription = null;
            result.szKeyword = null;
            result.bDefaultSourceItemConnected = 0;
            result.bDefaultTargetItemConnected = 0;
            result.bDefaultOverridden = 0;
            result.vDefaultOverrideValue = null;
            result.vSubstituteValue = null;
            result.bEnableSubstituteValue = 0;
            result.szTargetItemPath = null;
            result.szTargetItemName = null;
            result.szSourceServerName = null;
            result.szSourceItemPath = null;
            result.szSourceItemName = null;
            result.dwSourceItemQueueSize = 0;
            result.dwUpdateRate = 0;
            result.fltDeadBand = 0f;
            result.szVendorData = null;
            if (input.ItemName != null)
            {
                result.dwMask |= 2u;
                result.szItemName = input.ItemName;
            }

            if (input.ItemPath != null)
            {
                result.dwMask |= 1u;
                result.szItemPath = input.ItemPath;
            }

            if (input.Version != null)
            {
                result.dwMask |= 4u;
                result.szVersion = input.Version;
            }

            if (input.BrowsePaths.Count > 0)
            {
                result.dwMask |= 8u;
                result.dwBrowsePathCount = input.BrowsePaths.Count;
                result.pszBrowsePaths = OpcCom.Interop.GetUnicodeStrings(input.BrowsePaths.ToArray());
            }

            if (input.Name != null)
            {
                result.dwMask |= 16u;
                result.szName = input.Name;
            }

            if (input.Description != null)
            {
                result.dwMask |= 32u;
                result.szDescription = input.Description;
            }

            if (input.Keyword != null)
            {
                result.dwMask |= 64u;
                result.szKeyword = input.Keyword;
            }

            if (input.DefaultSourceItemConnectedSpecified)
            {
                result.dwMask |= 128u;
                result.bDefaultSourceItemConnected = (input.DefaultSourceItemConnected ? 1 : 0);
            }

            if (input.DefaultTargetItemConnectedSpecified)
            {
                result.dwMask |= 256u;
                result.bDefaultTargetItemConnected = (input.DefaultTargetItemConnected ? 1 : 0);
            }

            if (input.DefaultOverriddenSpecified)
            {
                result.dwMask |= 512u;
                result.bDefaultOverridden = (input.DefaultOverridden ? 1 : 0);
            }

            if (input.DefaultOverrideValue != null)
            {
                result.dwMask |= 1024u;
                result.vDefaultOverrideValue = input.DefaultOverrideValue;
            }

            if (input.SubstituteValue != null)
            {
                result.dwMask |= 2048u;
                result.vSubstituteValue = input.SubstituteValue;
            }

            if (input.EnableSubstituteValueSpecified)
            {
                result.dwMask |= 4096u;
                result.bEnableSubstituteValue = (input.EnableSubstituteValue ? 1 : 0);
            }

            if (input.TargetItemName != null)
            {
                result.dwMask |= 16384u;
                result.szTargetItemName = input.TargetItemName;
            }

            if (input.TargetItemPath != null)
            {
                result.dwMask |= 8192u;
                result.szTargetItemPath = input.TargetItemPath;
            }

            if (input.SourceServerName != null)
            {
                result.dwMask |= 32768u;
                result.szSourceServerName = input.SourceServerName;
            }

            if (input.SourceItemName != null)
            {
                result.dwMask |= 131072u;
                result.szSourceItemName = input.SourceItemName;
            }

            if (input.SourceItemPath != null)
            {
                result.dwMask |= 65536u;
                result.szSourceItemPath = input.SourceItemPath;
            }

            if (input.SourceItemQueueSizeSpecified)
            {
                result.dwMask |= 262144u;
                result.dwSourceItemQueueSize = input.SourceItemQueueSize;
            }

            if (input.UpdateRateSpecified)
            {
                result.dwMask |= 524288u;
                result.dwUpdateRate = input.UpdateRate;
            }

            if (input.DeadbandSpecified)
            {
                result.dwMask |= 1048576u;
                result.fltDeadBand = input.Deadband;
            }

            if (input.VendorData != null)
            {
                result.dwMask |= 2097152u;
                result.szVendorData = input.VendorData;
            }

            return result;
        }

        internal static Opc.Dx.DXConnection GetDXConnection(OpcRcw.Dx.DXConnection input, bool deallocate)
        {
            Opc.Dx.DXConnection dXConnection = new Opc.Dx.DXConnection();
            dXConnection.ItemPath = null;
            dXConnection.ItemName = null;
            dXConnection.Version = null;
            dXConnection.BrowsePaths.Clear();
            dXConnection.Name = null;
            dXConnection.Description = null;
            dXConnection.Keyword = null;
            dXConnection.DefaultSourceItemConnected = false;
            dXConnection.DefaultSourceItemConnectedSpecified = false;
            dXConnection.DefaultTargetItemConnected = false;
            dXConnection.DefaultTargetItemConnectedSpecified = false;
            dXConnection.DefaultOverridden = false;
            dXConnection.DefaultOverriddenSpecified = false;
            dXConnection.DefaultOverrideValue = null;
            dXConnection.SubstituteValue = null;
            dXConnection.EnableSubstituteValue = false;
            dXConnection.EnableSubstituteValueSpecified = false;
            dXConnection.TargetItemPath = null;
            dXConnection.TargetItemName = null;
            dXConnection.SourceServerName = null;
            dXConnection.SourceItemPath = null;
            dXConnection.SourceItemName = null;
            dXConnection.SourceItemQueueSize = 0;
            dXConnection.SourceItemQueueSizeSpecified = false;
            dXConnection.UpdateRate = 0;
            dXConnection.UpdateRateSpecified = false;
            dXConnection.Deadband = 0f;
            dXConnection.DeadbandSpecified = false;
            dXConnection.VendorData = null;
            if ((input.dwMask & 2) != 0)
            {
                dXConnection.ItemName = input.szItemName;
            }

            if ((input.dwMask & 1) != 0)
            {
                dXConnection.ItemPath = input.szItemPath;
            }

            if ((input.dwMask & 4) != 0)
            {
                dXConnection.Version = input.szVersion;
            }

            if ((input.dwMask & 8) != 0)
            {
                string[] unicodeStrings = OpcCom.Interop.GetUnicodeStrings(ref input.pszBrowsePaths, input.dwBrowsePathCount, deallocate);
                if (unicodeStrings != null)
                {
                    dXConnection.BrowsePaths.AddRange(unicodeStrings);
                }
            }

            if ((input.dwMask & 0x10) != 0)
            {
                dXConnection.Name = input.szName;
            }

            if ((input.dwMask & 0x20) != 0)
            {
                dXConnection.Description = input.szDescription;
            }

            if ((input.dwMask & 0x40) != 0)
            {
                dXConnection.Keyword = input.szKeyword;
            }

            if ((input.dwMask & 0x80) != 0)
            {
                dXConnection.DefaultSourceItemConnected = (input.bDefaultSourceItemConnected != 0);
                dXConnection.DefaultSourceItemConnectedSpecified = true;
            }

            if ((input.dwMask & 0x100) != 0)
            {
                dXConnection.DefaultTargetItemConnected = (input.bDefaultTargetItemConnected != 0);
                dXConnection.DefaultTargetItemConnectedSpecified = true;
            }

            if ((input.dwMask & 0x200) != 0)
            {
                dXConnection.DefaultOverridden = (input.bDefaultOverridden != 0);
                dXConnection.DefaultOverriddenSpecified = true;
            }

            if ((input.dwMask & 0x400) != 0)
            {
                dXConnection.DefaultOverrideValue = input.vDefaultOverrideValue;
            }

            if ((input.dwMask & 0x800) != 0)
            {
                dXConnection.SubstituteValue = input.vSubstituteValue;
            }

            if ((input.dwMask & 0x1000) != 0)
            {
                dXConnection.EnableSubstituteValue = (input.bEnableSubstituteValue != 0);
                dXConnection.EnableSubstituteValueSpecified = true;
            }

            if ((input.dwMask & 0x4000) != 0)
            {
                dXConnection.TargetItemName = input.szTargetItemName;
            }

            if ((input.dwMask & 0x2000) != 0)
            {
                dXConnection.TargetItemPath = input.szTargetItemPath;
            }

            if ((input.dwMask & 0x8000) != 0)
            {
                dXConnection.SourceServerName = input.szSourceServerName;
            }

            if ((input.dwMask & 0x20000) != 0)
            {
                dXConnection.SourceItemName = input.szSourceItemName;
            }

            if ((input.dwMask & 0x10000) != 0)
            {
                dXConnection.SourceItemPath = input.szSourceItemPath;
            }

            if ((input.dwMask & 0x40000) != 0)
            {
                dXConnection.SourceItemQueueSize = input.dwSourceItemQueueSize;
                dXConnection.SourceItemQueueSizeSpecified = true;
            }

            if ((input.dwMask & 0x80000) != 0)
            {
                dXConnection.UpdateRate = input.dwUpdateRate;
                dXConnection.UpdateRateSpecified = true;
            }

            if ((input.dwMask & 0x100000) != 0)
            {
                dXConnection.Deadband = input.fltDeadBand;
                dXConnection.DeadbandSpecified = true;
            }

            if ((input.dwMask & 0x200000) != 0)
            {
                dXConnection.VendorData = input.szVendorData;
            }

            return dXConnection;
        }

        internal static OpcRcw.Dx.ItemIdentifier[] GetItemIdentifiers(Opc.Dx.ItemIdentifier[] input)
        {
            OpcRcw.Dx.ItemIdentifier[] array = null;
            if (input != null && input.Length > 0)
            {
                array = new OpcRcw.Dx.ItemIdentifier[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    array[i] = default(OpcRcw.Dx.ItemIdentifier);
                    array[i].szItemName = input[i].ItemName;
                    array[i].szItemPath = input[i].ItemPath;
                    array[i].szVersion = input[i].Version;
                }
            }

            return array;
        }
    }
}
