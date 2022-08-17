using System;
using System.Collections;
using System.Runtime.InteropServices;
using Opc;
using Opc.Da;
using OpcRcw.Comn;
using OpcRcw.Da;

namespace OpcCom.Da.Wrapper
{
    [CLSCompliant(false)]
    public class Server : ConnectionPointContainer, IOPCCommon, IOPCServer, IOPCBrowseServerAddressSpace, IOPCItemProperties, IOPCBrowse, IOPCItemIO, IOPCWrappedServer
    {
        private class ContinuationPoint
        {
            public DateTime Timestamp;

            public Opc.Da.BrowsePosition Position;

            public ContinuationPoint(Opc.Da.BrowsePosition position)
            {
                Timestamp = DateTime.UtcNow;
                Position = position;
            }
        }

        private Opc.Da.IServer m_server;

        private Hashtable m_groups = new Hashtable();

        private Hashtable m_continuationPoints = new Hashtable();

        private Stack m_browseStack = new Stack();

        private int m_lcid = 2048;

        private int m_nextHandle = 1;

        public Opc.Da.IServer IServer
        {
            get
            {
                return m_server;
            }
            set
            {
                m_server = value;
            }
        }

        protected Server()
        {
            RegisterInterface(typeof(IOPCShutdown).GUID);
        }

        public int SetGroupName(string oldName, string newName)
        {
            lock (this)
            {
                Group group = (Group)m_groups[oldName];
                if (newName == null || newName.Length == 0 || group == null)
                {
                    return -2147024809;
                }

                if (m_groups.Contains(newName))
                {
                    return -1073479668;
                }

                m_groups.Remove(oldName);
                m_groups[newName] = group;
                return 0;
            }
        }

        public static Exception CreateException(Exception e)
        {
            if (typeof(ExternalException).IsInstanceOfType(e))
            {
                return e;
            }

            if (typeof(ResultIDException).IsInstanceOfType(e))
            {
                return new ExternalException(e.Message, OpcCom.Interop.GetResultID(((ResultIDException)e).Result));
            }

            return new ExternalException(e.Message, -2147467259);
        }

        public static Exception CreateException(int code)
        {
            return new ExternalException($"0x{code:X8}", code);
        }

        internal Group CreateGroup(ref SubscriptionState state, int lcid, int timebias)
        {
            lock (this)
            {
                ISubscription subscription = m_server.CreateSubscription(state);
                state = subscription.GetState();
                if (state == null)
                {
                    throw CreateException(-2147467259);
                }

                if (m_groups.Contains(state.Name))
                {
                    m_server.CancelSubscription(subscription);
                    throw new ExternalException("E_DUPLICATENAME", -1073479668);
                }

                Group group = new Group(this, state.Name, ++m_nextHandle, lcid, timebias, subscription);
                m_groups[state.Name] = group;
                return group;
            }
        }

        public virtual void Load(Guid clsid)
        {
        }

        public virtual void Unload()
        {
        }

        public void SetLocaleID(int dwLcid)
        {
            lock (this)
            {
                try
                {
                    m_server.SetLocale(OpcCom.Interop.GetLocale(dwLcid));
                    m_lcid = dwLcid;
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void QueryAvailableLocaleIDs(out int pdwCount, out IntPtr pdwLcid)
        {
            lock (this)
            {
                try
                {
                    pdwCount = 0;
                    pdwLcid = IntPtr.Zero;
                    string[] supportedLocales = m_server.GetSupportedLocales();
                    if (supportedLocales != null && supportedLocales.Length > 0)
                    {
                        pdwLcid = Marshal.AllocCoTaskMem(supportedLocales.Length * Marshal.SizeOf(typeof(int)));
                        int[] array = new int[supportedLocales.Length];
                        for (int i = 0; i < supportedLocales.Length; i++)
                        {
                            array[i] = OpcCom.Interop.GetLocale(supportedLocales[i]);
                        }

                        Marshal.Copy(array, 0, pdwLcid, supportedLocales.Length);
                        pdwCount = supportedLocales.Length;
                    }
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void GetLocaleID(out int pdwLcid)
        {
            lock (this)
            {
                try
                {
                    pdwLcid = m_lcid;
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        void IOPCCommon.GetErrorString(int dwError, out string ppString)
        {
            lock (this)
            {
                try
                {
                    ppString = m_server.GetErrorText(m_server.GetLocale(), OpcCom.Interop.GetResultID(dwError));
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void SetClientName(string szName)
        {
        }

        public void GetGroupByName(string szName, ref Guid riid, out object ppUnk)
        {
            lock (this)
            {
                try
                {
                    foreach (Group value in m_groups.Values)
                    {
                        if (value.Name == szName)
                        {
                            ppUnk = value;
                            return;
                        }
                    }

                    throw new ExternalException("E_INVALIDARG", -2147024809);
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void GetErrorString(int dwError, int dwLocale, out string ppString)
        {
            lock (this)
            {
                try
                {
                    ppString = m_server.GetErrorText(OpcCom.Interop.GetLocale(dwLocale), OpcCom.Interop.GetResultID(dwError));
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void RemoveGroup(int hServerGroup, int bForce)
        {
            lock (this)
            {
                try
                {
                    foreach (Group value in m_groups.Values)
                    {
                        if (value.ServerHandle == hServerGroup)
                        {
                            m_groups.Remove(value.Name);
                            value.Dispose();
                            return;
                        }
                    }

                    throw new ExternalException("E_FAIL", -2147467259);
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void CreateGroupEnumerator(OPCENUMSCOPE dwScope, ref Guid riid, out object ppUnk)
        {
            lock (this)
            {
                try
                {
                    if (dwScope == OPCENUMSCOPE.OPC_ENUM_PUBLIC_CONNECTIONS || dwScope == OPCENUMSCOPE.OPC_ENUM_PUBLIC)
                    {
                        if (riid == typeof(IEnumString).GUID)
                        {
                            ppUnk = new EnumString(null);
                            return;
                        }

                        if (riid == typeof(IEnumUnknown).GUID)
                        {
                            ppUnk = new EnumUnknown(null);
                            return;
                        }

                        throw new ExternalException("E_NOINTERFACE", -2147467262);
                    }

                    if (riid == typeof(IEnumUnknown).GUID)
                    {
                        ppUnk = new EnumUnknown(m_groups);
                        return;
                    }

                    if (riid == typeof(IEnumString).GUID)
                    {
                        ArrayList arrayList = new ArrayList(m_groups.Count);
                        foreach (Group value in m_groups.Values)
                        {
                            arrayList.Add(value.Name);
                        }

                        ppUnk = new EnumString(arrayList);
                        return;
                    }

                    throw new ExternalException("E_NOINTERFACE", -2147467262);
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void AddGroup(string szName, int bActive, int dwRequestedUpdateRate, int hClientGroup, IntPtr pTimeBias, IntPtr pPercentDeadband, int dwLCID, out int phServerGroup, out int pRevisedUpdateRate, ref Guid riid, out object ppUnk)
        {
            lock (this)
            {
                try
                {
                    SubscriptionState state = new SubscriptionState();
                    state.Name = szName;
                    state.ServerHandle = null;
                    state.ClientHandle = hClientGroup;
                    state.Active = (bActive != 0);
                    state.Deadband = 0f;
                    state.KeepAlive = 0;
                    state.Locale = OpcCom.Interop.GetLocale(dwLCID);
                    state.UpdateRate = dwRequestedUpdateRate;
                    if (pPercentDeadband != IntPtr.Zero)
                    {
                        float[] array = new float[1];
                        Marshal.Copy(pPercentDeadband, array, 0, 1);
                        state.Deadband = array[0];
                    }

                    DateTime now = DateTime.Now;
                    int num = (int)(0.0 - TimeZone.CurrentTimeZone.GetUtcOffset(now).TotalMinutes);
                    if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(now))
                    {
                        num += 60;
                    }

                    if (pTimeBias != IntPtr.Zero)
                    {
                        num = Marshal.ReadInt32(pTimeBias);
                    }

                    Group group = CreateGroup(ref state, dwLCID, num);
                    phServerGroup = group.ServerHandle;
                    pRevisedUpdateRate = state.UpdateRate;
                    ppUnk = group;
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void GetStatus(out IntPtr ppServerStatus)
        {
            lock (this)
            {
                try
                {
                    OPCSERVERSTATUS serverStatus = Interop.GetServerStatus(m_server.GetStatus(), m_groups.Count);
                    ppServerStatus = Marshal.AllocCoTaskMem(Marshal.SizeOf(serverStatus.GetType()));
                    Marshal.StructureToPtr((object)serverStatus, ppServerStatus, fDeleteOld: false);
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void Browse(string szItemID, ref IntPtr pszContinuationPoint, int dwMaxElementsReturned, OPCBROWSEFILTER dwBrowseFilter, string szElementNameFilter, string szVendorFilter, int bReturnAllProperties, int bReturnPropertyValues, int dwPropertyCount, int[] pdwPropertyIDs, out int pbMoreElements, out int pdwCount, out IntPtr ppBrowseElements)
        {
            lock (this)
            {
                try
                {
                    ItemIdentifier itemID = new ItemIdentifier(szItemID);
                    BrowseFilters browseFilters = new BrowseFilters();
                    browseFilters.MaxElementsReturned = dwMaxElementsReturned;
                    browseFilters.BrowseFilter = Interop.GetBrowseFilter(dwBrowseFilter);
                    browseFilters.ElementNameFilter = szElementNameFilter;
                    browseFilters.VendorFilter = szVendorFilter;
                    browseFilters.ReturnAllProperties = (bReturnAllProperties != 0);
                    browseFilters.ReturnPropertyValues = (bReturnPropertyValues != 0);
                    browseFilters.PropertyIDs = Interop.GetPropertyIDs(pdwPropertyIDs);
                    Opc.Da.BrowsePosition position = null;
                    BrowseElement[] array = null;
                    string text = null;
                    if (pszContinuationPoint != IntPtr.Zero)
                    {
                        text = Marshal.PtrToStringUni(pszContinuationPoint);
                    }

                    if (text == null || text.Length == 0)
                    {
                        array = m_server.Browse(itemID, browseFilters, out position);
                    }
                    else
                    {
                        ContinuationPoint continuationPoint = (ContinuationPoint)m_continuationPoints[text];
                        if (continuationPoint != null)
                        {
                            position = continuationPoint.Position;
                            m_continuationPoints.Remove(text);
                        }

                        if (position == null)
                        {
                            throw new ExternalException("E_INVALIDCONTINUATIONPOINT", -1073478653);
                        }

                        Marshal.FreeCoTaskMem(pszContinuationPoint);
                        pszContinuationPoint = IntPtr.Zero;
                        position.MaxElementsReturned = dwMaxElementsReturned;
                        array = m_server.BrowseNext(ref position);
                    }

                    CleanupContinuationPoints();
                    if (position != null)
                    {
                        text = Guid.NewGuid().ToString();
                        m_continuationPoints[text] = new ContinuationPoint(position);
                        pszContinuationPoint = Marshal.StringToCoTaskMemUni(text);
                    }

                    if (pszContinuationPoint == IntPtr.Zero)
                    {
                        pszContinuationPoint = Marshal.StringToCoTaskMemUni(string.Empty);
                    }

                    pbMoreElements = 0;
                    pdwCount = 0;
                    ppBrowseElements = IntPtr.Zero;
                    if (array != null)
                    {
                        pdwCount = array.Length;
                        ppBrowseElements = Interop.GetBrowseElements(array, dwPropertyCount > 0);
                    }
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void GetProperties(int dwItemCount, string[] pszItemIDs, int bReturnPropertyValues, int dwPropertyCount, int[] pdwPropertyIDs, out IntPtr ppItemProperties)
        {
            lock (this)
            {
                try
                {
                    if (dwItemCount == 0 || pszItemIDs == null)
                    {
                        throw new ExternalException("E_INVALIDARG", -2147024809);
                    }

                    ppItemProperties = IntPtr.Zero;
                    ItemIdentifier[] array = new ItemIdentifier[dwItemCount];
                    for (int i = 0; i < dwItemCount; i++)
                    {
                        array[i] = new ItemIdentifier(pszItemIDs[i]);
                    }

                    PropertyID[] propertyIDs = null;
                    if (dwPropertyCount > 0 && pdwPropertyIDs != null)
                    {
                        propertyIDs = Interop.GetPropertyIDs(pdwPropertyIDs);
                    }

                    ItemPropertyCollection[] properties = m_server.GetProperties(array, propertyIDs, bReturnPropertyValues != 0);
                    if (properties != null)
                    {
                        ppItemProperties = Interop.GetItemPropertyCollections(properties);
                    }
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void GetItemID(string szItemDataID, out string szItemID)
        {
            lock (this)
            {
                try
                {
                    if (szItemDataID == null || szItemDataID.Length == 0)
                    {
                        if (m_browseStack.Count == 0)
                        {
                            szItemID = "";
                        }
                        else
                        {
                            szItemID = ((ItemIdentifier)m_browseStack.Peek()).ItemName;
                        }

                        return;
                    }

                    if (IsItem(szItemDataID))
                    {
                        szItemID = szItemDataID;
                        return;
                    }

                    BrowseElement browseElement = FindChild(szItemDataID);
                    if (browseElement == null)
                    {
                        throw CreateException(-2147024809);
                    }

                    szItemID = browseElement.ItemName;
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void BrowseAccessPaths(string szItemID, out IEnumString ppIEnumString)
        {
            lock (this)
            {
                try
                {
                    throw new ExternalException("BrowseAccessPaths", -2147467263);
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void QueryOrganization(out OPCNAMESPACETYPE pNameSpaceType)
        {
            lock (this)
            {
                try
                {
                    pNameSpaceType = OPCNAMESPACETYPE.OPC_NS_HIERARCHIAL;
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void ChangeBrowsePosition(OPCBROWSEDIRECTION dwBrowseDirection, string szString)
        {
            lock (this)
            {
                try
                {
                    BrowseFilters browseFilters = new BrowseFilters();
                    browseFilters.MaxElementsReturned = 0;
                    browseFilters.BrowseFilter = browseFilter.all;
                    browseFilters.ElementNameFilter = null;
                    browseFilters.VendorFilter = null;
                    browseFilters.ReturnAllProperties = false;
                    browseFilters.PropertyIDs = null;
                    browseFilters.ReturnPropertyValues = false;
                    ItemIdentifier itemIdentifier = null;
                    Opc.Da.BrowsePosition position = null;
                    switch (dwBrowseDirection)
                    {
                        case OPCBROWSEDIRECTION.OPC_BROWSE_TO:
                            {
                                if (szString == null || szString.Length == 0)
                                {
                                    m_browseStack.Clear();
                                    break;
                                }

                                itemIdentifier = new ItemIdentifier(szString);
                                BrowseElement[] array = null;
                                try
                                {
                                    array = m_server.Browse(itemIdentifier, browseFilters, out position);
                                }
                                catch (Exception)
                                {
                                    throw CreateException(-2147024809);
                                }

                                if (array == null || array.Length == 0)
                                {
                                    throw CreateException(-2147024809);
                                }

                                m_browseStack.Clear();
                                m_browseStack.Push(null);
                                m_browseStack.Push(itemIdentifier);
                                break;
                            }
                        case OPCBROWSEDIRECTION.OPC_BROWSE_DOWN:
                            {
                                if (szString == null || szString.Length == 0)
                                {
                                    throw CreateException(-2147024809);
                                }

                                BrowseElement browseElement = FindChild(szString);
                                if (browseElement == null || !browseElement.HasChildren)
                                {
                                    throw CreateException(-2147024809);
                                }

                                m_browseStack.Push(new ItemIdentifier(browseElement.ItemName));
                                break;
                            }
                        case OPCBROWSEDIRECTION.OPC_BROWSE_UP:
                            if (m_browseStack.Count == 0)
                            {
                                throw CreateException(-2147467259);
                            }

                            itemIdentifier = (ItemIdentifier)m_browseStack.Pop();
                            if (m_browseStack.Count > 0 && m_browseStack.Peek() == null)
                            {
                                BuildBrowseStack(itemIdentifier);
                            }

                            break;
                    }

                    if (position != null)
                    {
                        position.Dispose();
                        position = null;
                    }
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void BrowseOPCItemIDs(OPCBROWSETYPE dwBrowseFilterType, string szFilterCriteria, short vtDataTypeFilter, int dwAccessRightsFilter, out IEnumString ppIEnumString)
        {
            lock (this)
            {
                try
                {
                    ItemIdentifier itemID = null;
                    if (m_browseStack.Count > 0)
                    {
                        itemID = (ItemIdentifier)m_browseStack.Peek();
                    }

                    ArrayList arrayList = new ArrayList();
                    Browse(itemID, dwBrowseFilterType, szFilterCriteria, vtDataTypeFilter, dwAccessRightsFilter, arrayList);
                    ppIEnumString = new EnumString(arrayList);
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void LookupItemIDs(string szItemID, int dwCount, int[] pdwPropertyIDs, out IntPtr ppszNewItemIDs, out IntPtr ppErrors)
        {
            lock (this)
            {
                try
                {
                    if (szItemID == null || szItemID.Length == 0 || dwCount == 0 || pdwPropertyIDs == null)
                    {
                        throw CreateException(-2147024809);
                    }

                    ItemIdentifier[] itemIDs = new ItemIdentifier[1]
                    {
                        new ItemIdentifier(szItemID)
                    };
                    PropertyID[] array = new PropertyID[pdwPropertyIDs.Length];
                    for (int i = 0; i < array.Length; i++)
                    {
                        ref PropertyID reference = ref array[i];
                        reference = Interop.GetPropertyID(pdwPropertyIDs[i]);
                    }

                    ItemPropertyCollection[] properties = m_server.GetProperties(itemIDs, array, returnValues: false);
                    if (properties == null || properties.Length != 1)
                    {
                        throw CreateException(-2147467259);
                    }

                    if (properties[0].ResultID.Failed())
                    {
                        throw new ResultIDException(properties[0].ResultID);
                    }

                    string[] array2 = new string[properties[0].Count];
                    for (int j = 0; j < properties[0].Count; j++)
                    {
                        ItemProperty itemProperty = properties[0][j];
                        if (itemProperty.ID.Code <= Property.EUINFO.Code)
                        {
                            itemProperty.ResultID = ResultID.Da.E_INVALID_PID;
                        }

                        if (itemProperty.ResultID.Succeeded())
                        {
                            array2[j] = itemProperty.ItemName;
                        }
                    }

                    ppszNewItemIDs = OpcCom.Interop.GetUnicodeStrings(array2);
                    ppErrors = Interop.GetHRESULTs((IResult[])properties[0].ToArray(typeof(IResult)));
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void QueryAvailableProperties(string szItemID, out int pdwCount, out IntPtr ppPropertyIDs, out IntPtr ppDescriptions, out IntPtr ppvtDataTypes)
        {
            lock (this)
            {
                try
                {
                    if (szItemID == null || szItemID.Length == 0)
                    {
                        throw new ExternalException("QueryAvailableProperties", -2147024809);
                    }

                    ItemIdentifier[] itemIDs = new ItemIdentifier[1]
                    {
                        new ItemIdentifier(szItemID)
                    };
                    ItemPropertyCollection[] properties = m_server.GetProperties(itemIDs, null, returnValues: false);
                    if (properties == null || properties.Length != 1)
                    {
                        throw new ExternalException("LookupItemIDs", -2147467259);
                    }

                    if (properties[0].ResultID.Failed())
                    {
                        throw new ResultIDException(properties[0].ResultID);
                    }

                    int[] array = new int[properties[0].Count];
                    string[] array2 = new string[properties[0].Count];
                    short[] array3 = new short[properties[0].Count];
                    for (int i = 0; i < properties[0].Count; i++)
                    {
                        ItemProperty itemProperty = properties[0][i];
                        if (itemProperty.ResultID.Succeeded())
                        {
                            array[i] = itemProperty.ID.Code;
                            PropertyDescription propertyDescription = PropertyDescription.Find(itemProperty.ID);
                            if (propertyDescription != null)
                            {
                                array2[i] = propertyDescription.Name;
                                array3[i] = (short)OpcCom.Interop.GetType(propertyDescription.Type);
                            }
                        }
                    }

                    pdwCount = array.Length;
                    ppPropertyIDs = OpcCom.Interop.GetInt32s(array);
                    ppDescriptions = OpcCom.Interop.GetUnicodeStrings(array2);
                    ppvtDataTypes = OpcCom.Interop.GetInt16s(array3);
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void GetItemProperties(string szItemID, int dwCount, int[] pdwPropertyIDs, out IntPtr ppvData, out IntPtr ppErrors)
        {
            lock (this)
            {
                try
                {
                    if (dwCount == 0 || pdwPropertyIDs == null)
                    {
                        throw CreateException(-2147024809);
                    }

                    if (szItemID == null || szItemID.Length == 0)
                    {
                        throw CreateException(-1073479672);
                    }

                    ItemIdentifier[] itemIDs = new ItemIdentifier[1]
                    {
                        new ItemIdentifier(szItemID)
                    };
                    PropertyID[] array = new PropertyID[pdwPropertyIDs.Length];
                    for (int i = 0; i < array.Length; i++)
                    {
                        ref PropertyID reference = ref array[i];
                        reference = Interop.GetPropertyID(pdwPropertyIDs[i]);
                    }

                    ItemPropertyCollection[] properties = m_server.GetProperties(itemIDs, array, returnValues: true);
                    if (properties == null || properties.Length != 1)
                    {
                        throw CreateException(-2147467259);
                    }

                    if (properties[0].ResultID.Failed())
                    {
                        throw new ResultIDException(properties[0].ResultID);
                    }

                    object[] array2 = new object[properties[0].Count];
                    for (int j = 0; j < properties[0].Count; j++)
                    {
                        ItemProperty itemProperty = properties[0][j];
                        if (itemProperty.ResultID.Succeeded())
                        {
                            array2[j] = Interop.MarshalPropertyValue(itemProperty.ID, itemProperty.Value);
                        }
                    }

                    ppvData = OpcCom.Interop.GetVARIANTs(array2, preprocess: false);
                    ppErrors = Interop.GetHRESULTs((IResult[])properties[0].ToArray(typeof(IResult)));
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void WriteVQT(int dwCount, string[] pszItemIDs, OPCITEMVQT[] pItemVQT, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (dwCount == 0 || pszItemIDs == null || pItemVQT == null)
                {
                    throw CreateException(-2147024809);
                }

                try
                {
                    ItemValue[] array = new ItemValue[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new ItemValue(new ItemIdentifier(pszItemIDs[i]));
                        array[i].Value = pItemVQT[i].vDataValue;
                        array[i].Quality = new Quality(pItemVQT[i].wQuality);
                        array[i].QualitySpecified = (pItemVQT[i].bQualitySpecified != 0);
                        array[i].Timestamp = OpcCom.Interop.GetFILETIME(Interop.Convert(pItemVQT[i].ftTimeStamp));
                        array[i].TimestampSpecified = (pItemVQT[i].bTimeStampSpecified != 0);
                    }

                    IdentifiedResult[] array2 = m_server.Write(array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw CreateException(-2147467259);
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        public void Read(int dwCount, string[] pszItemIDs, int[] pdwMaxAge, out IntPtr ppvValues, out IntPtr ppwQualities, out IntPtr ppftTimeStamps, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (dwCount == 0 || pszItemIDs == null || pdwMaxAge == null)
                {
                    throw CreateException(-2147024809);
                }

                try
                {
                    Item[] array = new Item[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new Item(new ItemIdentifier(pszItemIDs[i]));
                        array[i].MaxAge = ((pdwMaxAge[i] < 0) ? int.MaxValue : pdwMaxAge[i]);
                        array[i].MaxAgeSpecified = true;
                    }

                    ItemValueResult[] array2 = m_server.Read(array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw CreateException(-2147467259);
                    }

                    object[] array3 = new object[array2.Length];
                    short[] array4 = new short[array2.Length];
                    DateTime[] array5 = new DateTime[array2.Length];
                    for (int j = 0; j < array2.Length; j++)
                    {
                        array3[j] = array2[j].Value;
                        array4[j] = (short)(array2[j].QualitySpecified ? array2[j].Quality.GetCode() : 0);
                        ref DateTime reference = ref array5[j];
                        reference = (array2[j].TimestampSpecified ? array2[j].Timestamp : DateTime.MinValue);
                    }

                    ppvValues = OpcCom.Interop.GetVARIANTs(array3, preprocess: false);
                    ppwQualities = OpcCom.Interop.GetInt16s(array4);
                    ppftTimeStamps = OpcCom.Interop.GetFILETIMEs(array5);
                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw CreateException(e);
                }
            }
        }

        private void CleanupContinuationPoints()
        {
            ArrayList arrayList = new ArrayList();
            foreach (DictionaryEntry continuationPoint3 in m_continuationPoints)
            {
                try
                {
                    ContinuationPoint continuationPoint = continuationPoint3.Value as ContinuationPoint;
                    if (DateTime.UtcNow.Ticks - continuationPoint.Timestamp.Ticks > 6000000000L)
                    {
                        arrayList.Add(continuationPoint3.Key);
                    }
                }
                catch
                {
                    arrayList.Add(continuationPoint3.Key);
                }
            }

            foreach (string item in arrayList)
            {
                ContinuationPoint continuationPoint2 = (ContinuationPoint)m_continuationPoints[item];
                m_continuationPoints.Remove(item);
                continuationPoint2.Position.Dispose();
            }
        }

        private bool IsItem(string name)
        {
            ItemIdentifier itemIdentifier = new ItemIdentifier(name);
            try
            {
                ItemPropertyCollection[] properties = m_server.GetProperties(new ItemIdentifier[1]
                {
                    itemIdentifier
                }, new PropertyID[1]
                {
                    Property.DATATYPE
                }, returnValues: false);
                if (properties == null || properties.Length != 1)
                {
                    return false;
                }

                if (properties[0].ResultID.Failed() || properties[0][0].ResultID.Failed())
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private BrowseElement FindChild(string name)
        {
            ItemIdentifier itemID = null;
            if (m_browseStack.Count > 0)
            {
                itemID = (ItemIdentifier)m_browseStack.Peek();
            }

            BrowseElement[] array = null;
            try
            {
                BrowseFilters browseFilters = new BrowseFilters();
                browseFilters.MaxElementsReturned = 0;
                browseFilters.BrowseFilter = browseFilter.all;
                browseFilters.ElementNameFilter = name;
                browseFilters.VendorFilter = null;
                browseFilters.ReturnAllProperties = false;
                browseFilters.PropertyIDs = null;
                browseFilters.ReturnPropertyValues = false;
                Opc.Da.BrowsePosition position = null;
                array = m_server.Browse(itemID, browseFilters, out position);
                if (position != null)
                {
                    position.Dispose();
                    position = null;
                }
            }
            catch (Exception)
            {
                return null;
            }

            if (array != null && array.Length > 0)
            {
                return array[0];
            }

            return null;
        }

        private void BuildBrowseStack(ItemIdentifier itemID)
        {
            m_browseStack.Clear();
            BuildBrowseStack(null, itemID);
        }

        private bool BuildBrowseStack(ItemIdentifier itemID, ItemIdentifier targetID)
        {
            BrowseFilters browseFilters = new BrowseFilters();
            browseFilters.MaxElementsReturned = 0;
            browseFilters.BrowseFilter = browseFilter.all;
            browseFilters.ElementNameFilter = null;
            browseFilters.VendorFilter = null;
            browseFilters.ReturnAllProperties = false;
            browseFilters.PropertyIDs = null;
            browseFilters.ReturnPropertyValues = false;
            BrowseElement[] array = null;
            Opc.Da.BrowsePosition position = null;
            try
            {
                array = m_server.Browse(itemID, browseFilters, out position);
            }
            catch (Exception)
            {
                m_browseStack.Clear();
                return false;
            }

            if (position != null)
            {
                position.Dispose();
                position = null;
            }

            if (array == null || array.Length == 0)
            {
                m_browseStack.Clear();
                return false;
            }

            BrowseElement[] array2 = array;
            foreach (BrowseElement browseElement in array2)
            {
                if (browseElement.ItemName == targetID.ItemName)
                {
                    return true;
                }

                if (targetID.ItemName.StartsWith(browseElement.ItemName))
                {
                    ItemIdentifier itemIdentifier = new ItemIdentifier(targetID.ItemName);
                    m_browseStack.Push(itemIdentifier);
                    return BuildBrowseStack(itemIdentifier, targetID);
                }
            }

            return false;
        }

        private void Browse(ItemIdentifier itemID, OPCBROWSETYPE dwBrowseFilterType, string szFilterCriteria, short vtDataTypeFilter, int dwAccessRightsFilter, ArrayList hits)
        {
            BrowseFilters browseFilters = new BrowseFilters();
            browseFilters.MaxElementsReturned = 0;
            browseFilters.BrowseFilter = browseFilter.all;
            browseFilters.ElementNameFilter = ((dwBrowseFilterType != OPCBROWSETYPE.OPC_FLAT) ? szFilterCriteria : "");
            browseFilters.VendorFilter = null;
            browseFilters.ReturnAllProperties = false;
            browseFilters.PropertyIDs = new PropertyID[2]
            {
                Property.DATATYPE,
                Property.ACCESSRIGHTS
            };
            browseFilters.ReturnPropertyValues = true;
            BrowseElement[] array = null;
            try
            {
                Opc.Da.BrowsePosition position = null;
                array = m_server.Browse(itemID, browseFilters, out position);
                if (position != null)
                {
                    position.Dispose();
                    position = null;
                }
            }
            catch
            {
                throw new ExternalException("BrowseOPCItemIDs", -2147467259);
            }

            BrowseElement[] array2 = array;
            foreach (BrowseElement browseElement in array2)
            {
                switch (dwBrowseFilterType)
                {
                    case OPCBROWSETYPE.OPC_FLAT:
                        if (browseElement.HasChildren)
                        {
                            Browse(new ItemIdentifier(browseElement.ItemName), dwBrowseFilterType, szFilterCriteria, vtDataTypeFilter, dwAccessRightsFilter, hits);
                        }

                        break;
                    case OPCBROWSETYPE.OPC_BRANCH:
                        if (!browseElement.HasChildren)
                        {
                            continue;
                        }

                        break;
                    case OPCBROWSETYPE.OPC_LEAF:
                        if (browseElement.HasChildren)
                        {
                            continue;
                        }

                        break;
                }

                if (browseElement.IsItem)
                {
                    if (vtDataTypeFilter != 0)
                    {
                        short num = (short)OpcCom.Interop.GetType((System.Type)browseElement.Properties[0].Value);
                        if (num != vtDataTypeFilter)
                        {
                            continue;
                        }
                    }

                    if (dwAccessRightsFilter != 0)
                    {
                        accessRights accessRights = (accessRights)browseElement.Properties[1].Value;
                        if ((dwAccessRightsFilter == 1 && accessRights == accessRights.writable) || (dwAccessRightsFilter == 2 && accessRights == accessRights.readable))
                        {
                            continue;
                        }
                    }
                }

                if (dwBrowseFilterType != OPCBROWSETYPE.OPC_FLAT)
                {
                    hits.Add(browseElement.Name);
                }
                else if (browseElement.IsItem && (szFilterCriteria.Length == 0 || Opc.Convert.Match(browseElement.ItemName, szFilterCriteria, caseSensitive: true)))
                {
                    hits.Add(browseElement.ItemName);
                }
            }
        }
    }
}
