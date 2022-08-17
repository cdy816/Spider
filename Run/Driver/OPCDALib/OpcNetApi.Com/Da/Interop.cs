using Opc;
using Opc.Da;
using OpcRcw.Da;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace OpcCom.Da
{
    // Token: 0x0200002F RID: 47
    public class Interop
    {
        internal static OpcRcw.Da.FILETIME Convert(FILETIME input)
        {
            OpcRcw.Da.FILETIME result = default(OpcRcw.Da.FILETIME);
            result.dwLowDateTime = input.dwLowDateTime;
            result.dwHighDateTime = input.dwHighDateTime;
            return result;
        }

        internal static FILETIME Convert(OpcRcw.Da.FILETIME input)
        {
            FILETIME result = default(FILETIME);
            result.dwLowDateTime = input.dwLowDateTime;
            result.dwHighDateTime = input.dwHighDateTime;
            return result;
        }

        internal static OPCSERVERSTATUS GetServerStatus(ServerStatus input, int groupCount)
        {
            OPCSERVERSTATUS result = default(OPCSERVERSTATUS);
            if (input != null)
            {
                result.szVendorInfo = input.VendorInfo;
                result.wMajorVersion = 0;
                result.wMinorVersion = 0;
                result.wBuildNumber = 0;
                result.dwServerState = (OPCSERVERSTATE)input.ServerState;
                result.ftStartTime = Convert(OpcCom.Interop.GetFILETIME(input.StartTime));
                result.ftCurrentTime = Convert(OpcCom.Interop.GetFILETIME(input.CurrentTime));
                result.ftLastUpdateTime = Convert(OpcCom.Interop.GetFILETIME(input.LastUpdateTime));
                result.dwBandWidth = -1;
                result.dwGroupCount = groupCount;
                result.wReserved = 0;
                if (input.ProductVersion != null)
                {
                    string[] array = input.ProductVersion.Split('.');
                    if (array.Length > 0)
                    {
                        try
                        {
                            result.wMajorVersion = System.Convert.ToInt16(array[0]);
                        }
                        catch
                        {
                            result.wMajorVersion = 0;
                        }
                    }

                    if (array.Length > 1)
                    {
                        try
                        {
                            result.wMinorVersion = System.Convert.ToInt16(array[1]);
                        }
                        catch
                        {
                            result.wMinorVersion = 0;
                        }
                    }

                    result.wBuildNumber = 0;
                    for (int i = 2; i < array.Length; i++)
                    {
                        try
                        {
                            result.wBuildNumber = (short)(result.wBuildNumber * 100 + System.Convert.ToInt16(array[i]));
                        }
                        catch
                        {
                            result.wBuildNumber = 0;
                            return result;
                        }
                    }
                }
            }

            return result;
        }

        internal static ServerStatus GetServerStatus(ref IntPtr pInput, bool deallocate)
        {
            ServerStatus serverStatus = null;
            if (pInput != IntPtr.Zero)
            {
                OPCSERVERSTATUS oPCSERVERSTATUS = (OPCSERVERSTATUS)Marshal.PtrToStructure(pInput, typeof(OPCSERVERSTATUS));
                serverStatus = new ServerStatus();
                serverStatus.VendorInfo = oPCSERVERSTATUS.szVendorInfo;
                serverStatus.ProductVersion = $"{oPCSERVERSTATUS.wMajorVersion}.{oPCSERVERSTATUS.wMinorVersion}.{oPCSERVERSTATUS.wBuildNumber}";
                serverStatus.ServerState = (serverState)oPCSERVERSTATUS.dwServerState;
                serverStatus.StatusInfo = null;
                serverStatus.StartTime = OpcCom.Interop.GetFILETIME(Convert(oPCSERVERSTATUS.ftStartTime));
                serverStatus.CurrentTime = OpcCom.Interop.GetFILETIME(Convert(oPCSERVERSTATUS.ftCurrentTime));
                serverStatus.LastUpdateTime = OpcCom.Interop.GetFILETIME(Convert(oPCSERVERSTATUS.ftLastUpdateTime));
                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCSERVERSTATUS));
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }

            return serverStatus;
        }

        internal static OPCBROWSEFILTER GetBrowseFilter(browseFilter input)
        {
            switch (input)
            {
                case browseFilter.all:
                    return OPCBROWSEFILTER.OPC_BROWSE_FILTER_ALL;
                case browseFilter.branch:
                    return OPCBROWSEFILTER.OPC_BROWSE_FILTER_BRANCHES;
                case browseFilter.item:
                    return OPCBROWSEFILTER.OPC_BROWSE_FILTER_ITEMS;
                default:
                    return OPCBROWSEFILTER.OPC_BROWSE_FILTER_ALL;
            }
        }

        internal static browseFilter GetBrowseFilter(OPCBROWSEFILTER input)
        {
            switch (input)
            {
                case OPCBROWSEFILTER.OPC_BROWSE_FILTER_ALL:
                    return browseFilter.all;
                case OPCBROWSEFILTER.OPC_BROWSE_FILTER_BRANCHES:
                    return browseFilter.branch;
                case OPCBROWSEFILTER.OPC_BROWSE_FILTER_ITEMS:
                    return browseFilter.item;
                default:
                    return browseFilter.all;
            }
        }

        internal static IntPtr GetHRESULTs(IResult[] results)
        {
            int[] array = new int[results.Length];
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i] != null)
                {
                    array[i] = OpcCom.Interop.GetResultID(results[i].ResultID);
                }
                else
                {
                    array[i] = -1073479679;
                }
            }

            IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * results.Length);
            Marshal.Copy(array, 0, intPtr, results.Length);
            return intPtr;
        }

        internal static BrowseElement[] GetBrowseElements(ref IntPtr pInput, int count, bool deallocate)
        {
            BrowseElement[] array = null;
            if (pInput != IntPtr.Zero && count > 0)
            {
                array = new BrowseElement[count];
                IntPtr pInput2 = pInput;
                for (int i = 0; i < count; i++)
                {
                    array[i] = GetBrowseElement(pInput2, deallocate);
                    pInput2 = (IntPtr)(pInput2.ToInt64() + Marshal.SizeOf(typeof(OPCBROWSEELEMENT)));
                }

                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }

            return array;
        }

        internal static IntPtr GetBrowseElements(BrowseElement[] input, bool propertiesRequested)
        {
            IntPtr intPtr = IntPtr.Zero;
            if (input != null && input.Length > 0)
            {
                intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(OPCBROWSEELEMENT)) * input.Length);
                IntPtr ptr = intPtr;
                for (int i = 0; i < input.Length; i++)
                {
                    OPCBROWSEELEMENT browseElement = GetBrowseElement(input[i], propertiesRequested);
                    Marshal.StructureToPtr((object)browseElement, ptr, fDeleteOld: false);
                    ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(OPCBROWSEELEMENT)));
                }
            }

            return intPtr;
        }

        internal static BrowseElement GetBrowseElement(IntPtr pInput, bool deallocate)
        {
            BrowseElement browseElement = null;
            if (pInput != IntPtr.Zero)
            {
                OPCBROWSEELEMENT oPCBROWSEELEMENT = (OPCBROWSEELEMENT)Marshal.PtrToStructure(pInput, typeof(OPCBROWSEELEMENT));
                browseElement = new BrowseElement();
                browseElement.Name = oPCBROWSEELEMENT.szName;
                browseElement.ItemPath = null;
                browseElement.ItemName = oPCBROWSEELEMENT.szItemID;
                browseElement.IsItem = ((oPCBROWSEELEMENT.dwFlagValue & 2) != 0);
                browseElement.HasChildren = ((oPCBROWSEELEMENT.dwFlagValue & 1) != 0);
                browseElement.Properties = GetItemProperties(ref oPCBROWSEELEMENT.ItemProperties, deallocate);
                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCBROWSEELEMENT));
                }
            }

            return browseElement;
        }

        internal static OPCBROWSEELEMENT GetBrowseElement(BrowseElement input, bool propertiesRequested)
        {
            OPCBROWSEELEMENT result = default(OPCBROWSEELEMENT);
            if (input != null)
            {
                result.szName = input.Name;
                result.szItemID = input.ItemName;
                result.dwFlagValue = 0;
                result.ItemProperties = GetItemProperties(input.Properties);
                if (input.IsItem)
                {
                    result.dwFlagValue |= 2;
                }

                if (input.HasChildren)
                {
                    result.dwFlagValue |= 1;
                }
            }

            return result;
        }

        internal static int[] GetPropertyIDs(PropertyID[] propertyIDs)
        {
            ArrayList arrayList = new ArrayList();
            if (propertyIDs != null)
            {
                foreach (PropertyID propertyID in propertyIDs)
                {
                    arrayList.Add(propertyID.Code);
                }
            }

            return (int[])arrayList.ToArray(typeof(int));
        }

        internal static PropertyID[] GetPropertyIDs(int[] propertyIDs)
        {
            ArrayList arrayList = new ArrayList();
            if (propertyIDs != null)
            {
                foreach (int input in propertyIDs)
                {
                    arrayList.Add(GetPropertyID(input));
                }
            }

            return (PropertyID[])arrayList.ToArray(typeof(PropertyID));
        }

        internal static ItemPropertyCollection[] GetItemPropertyCollections(ref IntPtr pInput, int count, bool deallocate)
        {
            ItemPropertyCollection[] array = null;
            if (pInput != IntPtr.Zero && count > 0)
            {
                array = new ItemPropertyCollection[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    OPCITEMPROPERTIES input = (OPCITEMPROPERTIES)Marshal.PtrToStructure(ptr, typeof(OPCITEMPROPERTIES));
                    array[i] = new ItemPropertyCollection();
                    array[i].ItemPath = null;
                    array[i].ItemName = null;
                    array[i].ResultID = OpcCom.Interop.GetResultID(input.hrErrorID);
                    ItemProperty[] itemProperties = GetItemProperties(ref input, deallocate);
                    if (itemProperties != null)
                    {
                        array[i].AddRange(itemProperties);
                    }

                    if (deallocate)
                    {
                        Marshal.DestroyStructure(ptr, typeof(OPCITEMPROPERTIES));
                    }

                    ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(OPCITEMPROPERTIES)));
                }

                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }

            return array;
        }

        internal static IntPtr GetItemPropertyCollections(ItemPropertyCollection[] input)
        {
            IntPtr intPtr = IntPtr.Zero;
            if (input != null && input.Length > 0)
            {
                intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(OPCITEMPROPERTIES)) * input.Length);
                IntPtr ptr = intPtr;
                for (int i = 0; i < input.Length; i++)
                {
                    OPCITEMPROPERTIES oPCITEMPROPERTIES = default(OPCITEMPROPERTIES);
                    if (input[i].Count > 0)
                    {
                        oPCITEMPROPERTIES = GetItemProperties((ItemProperty[])input[i].ToArray(typeof(ItemProperty)));
                    }

                    oPCITEMPROPERTIES.hrErrorID = OpcCom.Interop.GetResultID(input[i].ResultID);
                    Marshal.StructureToPtr((object)oPCITEMPROPERTIES, ptr, fDeleteOld: false);
                    ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(OPCITEMPROPERTIES)));
                }
            }

            return intPtr;
        }

        internal static ItemProperty[] GetItemProperties(ref OPCITEMPROPERTIES input, bool deallocate)
        {
            ItemProperty[] array = null;
            if (input.dwNumProperties > 0)
            {
                array = new ItemProperty[input.dwNumProperties];
                IntPtr pInput = input.pItemProperties;
                for (int i = 0; i < array.Length; i++)
                {
                    try
                    {
                        array[i] = GetItemProperty(pInput, deallocate);
                    }
                    catch (Exception ex)
                    {
                        array[i] = new ItemProperty();
                        array[i].Description = ex.Message;
                        array[i].ResultID = ResultID.E_FAIL;
                    }

                    pInput = (IntPtr)(pInput.ToInt64() + Marshal.SizeOf(typeof(OPCITEMPROPERTY)));
                }

                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(input.pItemProperties);
                    input.pItemProperties = IntPtr.Zero;
                }
            }

            return array;
        }

        internal static OPCITEMPROPERTIES GetItemProperties(ItemProperty[] input)
        {
            OPCITEMPROPERTIES result = default(OPCITEMPROPERTIES);
            if (input != null && input.Length > 0)
            {
                result.hrErrorID = 0;
                result.dwReserved = 0;
                result.dwNumProperties = input.Length;
                result.pItemProperties = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(OPCITEMPROPERTY)) * input.Length);
                bool flag = false;
                IntPtr ptr = result.pItemProperties;
                for (int i = 0; i < input.Length; i++)
                {
                    OPCITEMPROPERTY itemProperty = GetItemProperty(input[i]);
                    Marshal.StructureToPtr((object)itemProperty, ptr, fDeleteOld: false);
                    ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(OPCITEMPROPERTY)));
                    if (input[i].ResultID.Failed())
                    {
                        flag = true;
                    }
                }

                if (flag)
                {
                    result.hrErrorID = 1;
                }
            }

            return result;
        }

        internal static ItemProperty GetItemProperty(IntPtr pInput, bool deallocate)
        {
            ItemProperty itemProperty = null;
            if (pInput != IntPtr.Zero)
            {
                OPCITEMPROPERTY oPCITEMPROPERTY = (OPCITEMPROPERTY)Marshal.PtrToStructure(pInput, typeof(OPCITEMPROPERTY));
                itemProperty = new ItemProperty();
                itemProperty.ID = GetPropertyID(oPCITEMPROPERTY.dwPropertyID);
                itemProperty.Description = oPCITEMPROPERTY.szDescription;
                itemProperty.DataType = OpcCom.Interop.GetType((VarEnum)oPCITEMPROPERTY.vtDataType);
                itemProperty.ItemPath = null;
                itemProperty.ItemName = oPCITEMPROPERTY.szItemID;
                itemProperty.Value = UnmarshalPropertyValue(itemProperty.ID, oPCITEMPROPERTY.vValue);
                itemProperty.ResultID = OpcCom.Interop.GetResultID(oPCITEMPROPERTY.hrErrorID);
                if (oPCITEMPROPERTY.hrErrorID == -1073479674)
                {
                    itemProperty.ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                }

                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCITEMPROPERTY));
                }
            }

            return itemProperty;
        }

        internal static OPCITEMPROPERTY GetItemProperty(ItemProperty input)
        {
            OPCITEMPROPERTY result = default(OPCITEMPROPERTY);
            if (input != null)
            {
                result.dwPropertyID = input.ID.Code;
                result.szDescription = input.Description;
                result.vtDataType = (short)OpcCom.Interop.GetType(input.DataType);
                result.vValue = MarshalPropertyValue(input.ID, input.Value);
                result.wReserved = 0;
                result.hrErrorID = OpcCom.Interop.GetResultID(input.ResultID);
                PropertyDescription propertyDescription = PropertyDescription.Find(input.ID);
                if (propertyDescription != null)
                {
                    result.vtDataType = (short)OpcCom.Interop.GetType(propertyDescription.Type);
                }

                if (input.ResultID == ResultID.Da.E_WRITEONLY)
                {
                    result.hrErrorID = -1073479674;
                }
            }

            return result;
        }

        public static PropertyID GetPropertyID(int input)
        {
            FieldInfo[] fields = typeof(Property).GetFields(BindingFlags.Static | BindingFlags.Public);
            FieldInfo[] array = fields;
            foreach (FieldInfo fieldInfo in array)
            {
                PropertyID result = (PropertyID)fieldInfo.GetValue(typeof(PropertyID));
                if (input == result.Code)
                {
                    return result;
                }
            }

            return new PropertyID(input);
        }

        internal static object UnmarshalPropertyValue(PropertyID propertyID, object input)
        {
            if (input == null)
            {
                return null;
            }

            try
            {
                if (propertyID == Property.DATATYPE)
                {
                    return OpcCom.Interop.GetType((VarEnum)System.Convert.ToUInt16(input));
                }

                if (propertyID == Property.ACCESSRIGHTS)
                {
                    switch (System.Convert.ToInt32(input))
                    {
                        case 1:
                            return accessRights.readable;
                        case 2:
                            return accessRights.writable;
                        case 3:
                            return accessRights.readWritable;
                        default:
                            return null;
                    }
                }

                if (propertyID == Property.EUTYPE)
                {
                    switch ((OPCEUTYPE)input)
                    {
                        case OPCEUTYPE.OPC_NOENUM:
                            return euType.noEnum;
                        case OPCEUTYPE.OPC_ANALOG:
                            return euType.analog;
                        case OPCEUTYPE.OPC_ENUMERATED:
                            return euType.enumerated;
                        default:
                            return null;
                    }
                }

                if (propertyID == Property.QUALITY)
                {
                    return new Quality(System.Convert.ToInt16(input));
                }

                if (propertyID == Property.TIMESTAMP)
                {
                    if ((object)input.GetType() == typeof(DateTime))
                    {
                        DateTime dateTime = (DateTime)input;
                        if (dateTime != DateTime.MinValue)
                        {
                            return dateTime.ToLocalTime();
                        }

                        return dateTime;
                    }

                    return input;
                }

                return input;
            }
            catch
            {
                return input;
            }
        }

        internal static object MarshalPropertyValue(PropertyID propertyID, object input)
        {
            if (input == null)
            {
                return null;
            }

            try
            {
                if (propertyID == Property.DATATYPE)
                {
                    return (short)OpcCom.Interop.GetType((System.Type)input);
                }

                if (propertyID == Property.ACCESSRIGHTS)
                {
                    switch ((accessRights)input)
                    {
                        case accessRights.readable:
                            return 1;
                        case accessRights.writable:
                            return 2;
                        case accessRights.readWritable:
                            return 3;
                        default:
                            return null;
                    }
                }

                if (propertyID == Property.EUTYPE)
                {
                    switch ((euType)input)
                    {
                        case euType.noEnum:
                            return OPCEUTYPE.OPC_NOENUM;
                        case euType.analog:
                            return OPCEUTYPE.OPC_ANALOG;
                        case euType.enumerated:
                            return OPCEUTYPE.OPC_ENUMERATED;
                        default:
                            return null;
                    }
                }

                if (propertyID == Property.QUALITY)
                {
                    return ((Quality)input).GetCode();
                }

                if (propertyID == Property.TIMESTAMP)
                {
                    if ((object)input.GetType() == typeof(DateTime))
                    {
                        DateTime dateTime = (DateTime)input;
                        if (dateTime != DateTime.MinValue)
                        {
                            return dateTime.ToUniversalTime();
                        }

                        return dateTime;
                    }

                    return input;
                }

                return input;
            }
            catch
            {
                return input;
            }
        }

        internal static OPCITEMVQT[] GetOPCITEMVQTs(ItemValue[] input)
        {
            OPCITEMVQT[] array = null;
            if (input != null)
            {
                array = new OPCITEMVQT[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    array[i] = default(OPCITEMVQT);
                    DateTime datetime = input[i].TimestampSpecified ? input[i].Timestamp : DateTime.MinValue;
                    array[i].vDataValue = OpcCom.Interop.GetVARIANT(input[i].Value);
                    array[i].bQualitySpecified = (input[i].QualitySpecified ? 1 : 0);
                    array[i].wQuality = (short)(input[i].QualitySpecified ? input[i].Quality.GetCode() : 0);
                    array[i].bTimeStampSpecified = (input[i].TimestampSpecified ? 1 : 0);
                    array[i].ftTimeStamp = Convert(OpcCom.Interop.GetFILETIME(datetime));
                }
            }

            return array;
        }

        internal static OPCITEMDEF[] GetOPCITEMDEFs(Item[] input)
        {
            OPCITEMDEF[] array = null;
            if (input != null)
            {
                array = new OPCITEMDEF[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    array[i] = default(OPCITEMDEF);
                    array[i].szItemID = input[i].ItemName;
                    array[i].szAccessPath = ((input[i].ItemPath == null) ? string.Empty : input[i].ItemPath);
                    array[i].bActive = ((!input[i].ActiveSpecified) ? 1 : (input[i].Active ? 1 : 0));
                    array[i].vtRequestedDataType = (short)OpcCom.Interop.GetType(input[i].ReqType);
                    array[i].hClient = 0;
                    array[i].dwBlobSize = 0;
                    array[i].pBlob = IntPtr.Zero;
                }
            }

            return array;
        }

        internal static ItemValue[] GetItemValues(ref IntPtr pInput, int count, bool deallocate)
        {
            ItemValue[] array = null;
            if (pInput != IntPtr.Zero && count > 0)
            {
                array = new ItemValue[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    OPCITEMSTATE oPCITEMSTATE = (OPCITEMSTATE)Marshal.PtrToStructure(ptr, typeof(OPCITEMSTATE));
                    array[i] = new ItemValue();
                    array[i].ClientHandle = oPCITEMSTATE.hClient;
                    array[i].Value = oPCITEMSTATE.vDataValue;
                    array[i].Quality = new Quality(oPCITEMSTATE.wQuality);
                    array[i].QualitySpecified = true;
                    array[i].Timestamp = OpcCom.Interop.GetFILETIME(Convert(oPCITEMSTATE.ftTimeStamp));
                    array[i].TimestampSpecified = (array[i].Timestamp != DateTime.MinValue);
                    if (deallocate)
                    {
                        Marshal.DestroyStructure(ptr, typeof(OPCITEMSTATE));
                    }

                    ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(OPCITEMSTATE)));
                }

                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }

            return array;
        }

        internal static int[] GetItemResults(ref IntPtr pInput, int count, bool deallocate)
        {
            int[] array = null;
            if (pInput != IntPtr.Zero && count > 0)
            {
                array = new int[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    OPCITEMRESULT oPCITEMRESULT = (OPCITEMRESULT)Marshal.PtrToStructure(ptr, typeof(OPCITEMRESULT));
                    array[i] = oPCITEMRESULT.hServer;
                    if (deallocate)
                    {
                        Marshal.FreeCoTaskMem(oPCITEMRESULT.pBlob);
                        oPCITEMRESULT.pBlob = IntPtr.Zero;
                        Marshal.DestroyStructure(ptr, typeof(OPCITEMRESULT));
                    }

                    ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                }

                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }

            return array;
        }

        internal static IntPtr GetItemStates(ItemValueResult[] input)
        {
            IntPtr intPtr = IntPtr.Zero;
            if (input != null && input.Length > 0)
            {
                intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(OPCITEMSTATE)) * input.Length);
                IntPtr ptr = intPtr;
                for (int i = 0; i < input.Length; i++)
                {
                    OPCITEMSTATE oPCITEMSTATE = default(OPCITEMSTATE);
                    oPCITEMSTATE.hClient = System.Convert.ToInt32(input[i].ClientHandle);
                    oPCITEMSTATE.vDataValue = input[i].Value;
                    oPCITEMSTATE.wQuality = (short)(input[i].QualitySpecified ? input[i].Quality.GetCode() : 0);
                    oPCITEMSTATE.ftTimeStamp = Convert(OpcCom.Interop.GetFILETIME(input[i].Timestamp));
                    oPCITEMSTATE.wReserved = 0;
                    Marshal.StructureToPtr((object)oPCITEMSTATE, ptr, fDeleteOld: false);
                    ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(OPCITEMSTATE)));
                }
            }

            return intPtr;
        }
    }
}
