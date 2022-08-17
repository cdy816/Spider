using System;
using System.Collections;
using System.Runtime.InteropServices;
using Opc;
using Opc.Da;
using OpcRcw.Da;

namespace OpcCom.Da.Wrapper
{
    [CLSCompliant(false)]
    public class Group : ConnectionPointContainer, IDisposable, IOPCItemMgt, IOPCSyncIO, IOPCSyncIO2, IOPCAsyncIO2, IOPCAsyncIO3, IOPCGroupStateMgt, IOPCGroupStateMgt2, IOPCItemDeadbandMgt, IOPCItemSamplingMgt
    {
        private const int LOCALE_SYSTEM_DEFAULT = 2048;

        private bool m_disposed;

        private Server m_server;

        private int m_serverHandle;

        private int m_clientHandle;

        private string m_name;

        private ISubscription m_subscription;

        private int m_timebias;

        private int m_lcid = 2048;

        private Hashtable m_items = new Hashtable();

        private Hashtable m_requests = new Hashtable();

        private int m_nextHandle = 1000;

        private DataChangedEventHandler m_dataChanged;

        public string Name
        {
            get
            {
                lock (this)
                {
                    return m_name;
                }
            }
        }

        public int ServerHandle
        {
            get
            {
                lock (this)
                {
                    return m_serverHandle;
                }
            }
        }

        public Group(Server server, string name, int handle, int lcid, int timebias, ISubscription subscription)
        {
            RegisterInterface(typeof(IOPCDataCallback).GUID);
            m_server = server;
            m_name = name;
            m_serverHandle = handle;
            m_lcid = lcid;
            m_timebias = timebias;
            m_subscription = subscription;
        }

        public override void OnAdvise(Guid riid)
        {
            lock (this)
            {
                m_dataChanged = OnDataChanged;
                m_subscription.DataChanged += m_dataChanged;
            }
        }

        public override void OnUnadvise(Guid riid)
        {
            lock (this)
            {
                if (m_dataChanged != null)
                {
                    m_subscription.DataChanged -= m_dataChanged;
                    m_dataChanged = null;
                }
            }
        }

        ~Group()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (m_disposed)
            {
                return;
            }

            lock (this)
            {
                if (disposing && m_subscription != null)
                {
                    m_subscription.DataChanged -= m_dataChanged;
                    m_server.IServer.CancelSubscription(m_subscription);
                    m_subscription = null;
                }
            }

            m_disposed = true;
        }

        public void SetActiveState(int dwCount, int[] phServer, int bActive, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    Item[] array = new Item[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new Item((ItemIdentifier)m_items[phServer[i]]);
                        array[i].Active = (bActive != 0);
                        array[i].ActiveSpecified = true;
                    }

                    ItemResult[] array2 = m_subscription.ModifyItems(8, array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    for (int j = 0; j < dwCount; j++)
                    {
                        if (array2[j].ResultID.Succeeded())
                        {
                            m_items[phServer[j]] = array2[j];
                        }
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void AddItems(int dwCount, OPCITEMDEF[] pItemArray, out IntPtr ppAddResults, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || pItemArray == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    Item[] array = new Item[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new Item();
                        array[i].ItemName = pItemArray[i].szItemID;
                        array[i].ItemPath = pItemArray[i].szAccessPath;
                        array[i].ClientHandle = pItemArray[i].hClient;
                        array[i].ServerHandle = null;
                        array[i].Active = (pItemArray[i].bActive != 0);
                        array[i].ActiveSpecified = true;
                        array[i].ReqType = OpcCom.Interop.GetType((VarEnum)pItemArray[i].vtRequestedDataType);
                    }

                    ItemResult[] array2 = m_subscription.AddItems(array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    ItemPropertyCollection[] properties = m_server.IServer.GetProperties(array, new PropertyID[2]
                    {
                        Property.DATATYPE,
                        Property.ACCESSRIGHTS
                    }, returnValues: true);
                    ppAddResults = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(OPCITEMRESULT)) * array2.Length);
                    IntPtr ptr = ppAddResults;
                    for (int j = 0; j < array2.Length; j++)
                    {
                        OPCITEMRESULT oPCITEMRESULT = default(OPCITEMRESULT);
                        oPCITEMRESULT.hServer = 0;
                        oPCITEMRESULT.dwBlobSize = 0;
                        oPCITEMRESULT.pBlob = IntPtr.Zero;
                        oPCITEMRESULT.vtCanonicalDataType = 0;
                        oPCITEMRESULT.dwAccessRights = 0;
                        oPCITEMRESULT.wReserved = 0;
                        if (array2[j].ResultID.Succeeded())
                        {
                            oPCITEMRESULT.hServer = ++m_nextHandle;
                            oPCITEMRESULT.vtCanonicalDataType = (short)Interop.MarshalPropertyValue(Property.DATATYPE, properties[j][0].Value);
                            oPCITEMRESULT.dwAccessRights = (int)Interop.MarshalPropertyValue(Property.ACCESSRIGHTS, properties[j][1].Value);
                            m_items[m_nextHandle] = array2[j];
                        }

                        Marshal.StructureToPtr((object)oPCITEMRESULT, ptr, fDeleteOld: false);
                        ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void SetClientHandles(int dwCount, int[] phServer, int[] phClient, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null || phClient == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    Item[] array = new Item[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new Item((ItemIdentifier)m_items[phServer[i]]);
                        array[i].ClientHandle = phClient[i];
                    }

                    ItemResult[] array2 = m_subscription.ModifyItems(2, array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    for (int j = 0; j < dwCount; j++)
                    {
                        if (array2[j].ResultID.Succeeded())
                        {
                            m_items[phServer[j]] = array2[j];
                        }
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void SetDatatypes(int dwCount, int[] phServer, short[] pRequestedDatatypes, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null || pRequestedDatatypes == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    Item[] array = new Item[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new Item((ItemIdentifier)m_items[phServer[i]]);
                        array[i].ReqType = OpcCom.Interop.GetType((VarEnum)pRequestedDatatypes[i]);
                    }

                    ItemResult[] array2 = m_subscription.ModifyItems(64, array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    for (int j = 0; j < dwCount; j++)
                    {
                        if (array2[j].ResultID.Succeeded())
                        {
                            m_items[phServer[j]] = array2[j];
                        }
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void ValidateItems(int dwCount, OPCITEMDEF[] pItemArray, int bBlobUpdate, out IntPtr ppValidationResults, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || pItemArray == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    Item[] array = new Item[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new Item();
                        array[i].ItemName = pItemArray[i].szItemID;
                        array[i].ItemPath = pItemArray[i].szAccessPath;
                        array[i].ClientHandle = pItemArray[i].hClient;
                        array[i].ServerHandle = null;
                        array[i].Active = false;
                        array[i].ActiveSpecified = true;
                        array[i].ReqType = OpcCom.Interop.GetType((VarEnum)pItemArray[i].vtRequestedDataType);
                    }

                    ItemResult[] array2 = m_subscription.AddItems(array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    m_subscription.RemoveItems(array2);
                    ItemPropertyCollection[] properties = m_server.IServer.GetProperties(array, new PropertyID[2]
                    {
                        Property.DATATYPE,
                        Property.ACCESSRIGHTS
                    }, returnValues: true);
                    ppValidationResults = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(OPCITEMRESULT)) * array2.Length);
                    IntPtr ptr = ppValidationResults;
                    for (int j = 0; j < array2.Length; j++)
                    {
                        OPCITEMRESULT oPCITEMRESULT = default(OPCITEMRESULT);
                        oPCITEMRESULT.hServer = 0;
                        oPCITEMRESULT.dwBlobSize = 0;
                        oPCITEMRESULT.pBlob = IntPtr.Zero;
                        oPCITEMRESULT.vtCanonicalDataType = 0;
                        oPCITEMRESULT.dwAccessRights = 0;
                        oPCITEMRESULT.wReserved = 0;
                        if (array2[j].ResultID.Succeeded())
                        {
                            oPCITEMRESULT.vtCanonicalDataType = (short)Interop.MarshalPropertyValue(Property.DATATYPE, properties[j][0].Value);
                            oPCITEMRESULT.dwAccessRights = (int)Interop.MarshalPropertyValue(Property.ACCESSRIGHTS, properties[j][1].Value);
                        }

                        Marshal.StructureToPtr((object)oPCITEMRESULT, ptr, fDeleteOld: false);
                        ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void CreateEnumerator(ref Guid riid, out object ppUnk)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (riid != typeof(IEnumOPCItemAttributes).GUID)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    int[] array = new int[m_items.Count];
                    Item[] array2 = new Item[m_items.Count];
                    int num = 0;
                    IDictionaryEnumerator enumerator = m_items.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        array[num] = (int)enumerator.Key;
                        array2[num] = (Item)enumerator.Value;
                        num++;
                    }

                    PropertyID[] propertyIDs = new PropertyID[6]
                    {
                        Property.ACCESSRIGHTS,
                        Property.DATATYPE,
                        Property.EUTYPE,
                        Property.EUINFO,
                        Property.HIGHEU,
                        Property.LOWEU
                    };
                    ItemPropertyCollection[] properties = m_server.IServer.GetProperties(array2, propertyIDs, returnValues: true);
                    EnumOPCItemAttributes.ItemAttributes[] array3 = new EnumOPCItemAttributes.ItemAttributes[m_items.Count];
                    for (int i = 0; i < array2.Length; i++)
                    {
                        array3[i] = new EnumOPCItemAttributes.ItemAttributes();
                        array3[i].ItemID = array2[i].ItemName;
                        array3[i].AccessPath = array2[i].ItemPath;
                        array3[i].ClientHandle = (int)array2[i].ClientHandle;
                        array3[i].ServerHandle = array[i];
                        array3[i].Active = array2[i].Active;
                        array3[i].RequestedDataType = array2[i].ReqType;
                        array3[i].AccessRights = (accessRights)properties[i][0].Value;
                        array3[i].CanonicalDataType = (System.Type)properties[i][1].Value;
                        array3[i].EuType = (euType)properties[i][2].Value;
                        array3[i].EuInfo = (string[])properties[i][3].Value;
                        if (array3[i].EuType == euType.analog)
                        {
                            array3[i].MaxValue = (double)properties[i][4].Value;
                            array3[i].MinValue = (double)properties[i][5].Value;
                        }
                    }

                    ppUnk = new EnumOPCItemAttributes(array3);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void RemoveItems(int dwCount, int[] phServer, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                _ = (int[])new ArrayList(m_items.Keys).ToArray(typeof(int));
                if (dwCount == 0 || phServer == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    ItemIdentifier[] array = new ItemIdentifier[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new ItemIdentifier((ItemIdentifier)m_items[phServer[i]]);
                    }

                    IdentifiedResult[] array2 = m_subscription.RemoveItems(array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    for (int j = 0; j < dwCount; j++)
                    {
                        if (array2[j].ResultID.Succeeded())
                        {
                            m_items.Remove(phServer[j]);
                        }
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void Read(OPCDATASOURCE dwSource, int dwCount, int[] phServer, out IntPtr ppItemValues, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    Item[] array = new Item[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new Item((ItemIdentifier)m_items[phServer[i]]);
                        array[i].MaxAge = ((dwSource != OPCDATASOURCE.OPC_DS_DEVICE) ? int.MaxValue : 0);
                        array[i].MaxAgeSpecified = true;
                    }

                    ItemValueResult[] array2 = m_subscription.Read(array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    ppItemValues = Interop.GetItemStates(array2);
                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void Write(int dwCount, int[] phServer, object[] pItemValues, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null || pItemValues == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    ItemValue[] array = new ItemValue[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new ItemValue((ItemIdentifier)m_items[phServer[i]]);
                        array[i].Value = pItemValues[i];
                        array[i].Quality = Quality.Bad;
                        array[i].QualitySpecified = false;
                        array[i].Timestamp = DateTime.MinValue;
                        array[i].TimestampSpecified = false;
                    }

                    IdentifiedResult[] array2 = m_subscription.Write(array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void ReadMaxAge(int dwCount, int[] phServer, int[] pdwMaxAge, out IntPtr ppvValues, out IntPtr ppwQualities, out IntPtr ppftTimeStamps, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null || pdwMaxAge == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    Item[] array = new Item[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new Item((ItemIdentifier)m_items[phServer[i]]);
                        array[i].MaxAge = ((pdwMaxAge[i] < 0) ? int.MaxValue : pdwMaxAge[i]);
                        array[i].MaxAgeSpecified = true;
                    }

                    ItemValueResult[] array2 = m_subscription.Read(array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
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
                    throw Server.CreateException(e);
                }
            }
        }

        public void WriteVQT(int dwCount, int[] phServer, OPCITEMVQT[] pItemVQT, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null || pItemVQT == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    ItemValue[] array = new ItemValue[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new ItemValue((ItemIdentifier)m_items[phServer[i]]);
                        array[i].Value = pItemVQT[i].vDataValue;
                        array[i].Quality = new Quality(pItemVQT[i].wQuality);
                        array[i].QualitySpecified = (pItemVQT[i].bQualitySpecified != 0);
                        array[i].Timestamp = OpcCom.Interop.GetFILETIME(Interop.Convert(pItemVQT[i].ftTimeStamp));
                        array[i].TimestampSpecified = (pItemVQT[i].bTimeStampSpecified != 0);
                    }

                    IdentifiedResult[] array2 = m_subscription.Write(array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void Read(int dwCount, int[] phServer, int dwTransactionID, out int pdwCancelID, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                if (!IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw Server.CreateException(-2147220992);
                }

                try
                {
                    Item[] array = new Item[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new Item((ItemIdentifier)m_items[phServer[i]]);
                        array[i].MaxAge = 0;
                        array[i].MaxAgeSpecified = true;
                    }

                    pdwCancelID = AssignHandle();
                    IRequest request = null;
                    IdentifiedResult[] array2 = m_subscription.Read(array, pdwCancelID, OnReadComplete, out request);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    if (request != null)
                    {
                        m_requests[request] = dwTransactionID;
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void Write(int dwCount, int[] phServer, object[] pItemValues, int dwTransactionID, out int pdwCancelID, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null || pItemValues == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                if (!IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw Server.CreateException(-2147220992);
                }

                try
                {
                    ItemValue[] array = new ItemValue[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new ItemValue((ItemIdentifier)m_items[phServer[i]]);
                        array[i].Value = pItemValues[i];
                        array[i].Quality = Quality.Bad;
                        array[i].QualitySpecified = false;
                        array[i].Timestamp = DateTime.MinValue;
                        array[i].TimestampSpecified = false;
                    }

                    pdwCancelID = AssignHandle();
                    IRequest request = null;
                    IdentifiedResult[] array2 = m_subscription.Write(array, pdwCancelID, OnWriteComplete, out request);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    if (request != null)
                    {
                        m_requests[request] = dwTransactionID;
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void Cancel2(int dwCancelID)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (!IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw Server.CreateException(-2147220992);
                }

                try
                {
                    IDictionaryEnumerator enumerator = m_requests.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        Opc.Da.Request request = (Opc.Da.Request)enumerator.Key;
                        if (request.Handle.Equals(dwCancelID))
                        {
                            m_requests.Remove(request);
                            return;
                        }
                    }

                    throw Server.CreateException(-2147467259);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void Refresh2(OPCDATASOURCE dwSource, int dwTransactionID, out int pdwCancelID)
        {
            lock (this)
            {
                if (!IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw Server.CreateException(-2147220992);
                }

                int dwMaxAge = (dwSource != OPCDATASOURCE.OPC_DS_DEVICE) ? int.MaxValue : 0;
                RefreshMaxAge(dwMaxAge, dwTransactionID, out pdwCancelID);
            }
        }

        public void SetEnable(int bEnable)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (!IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw Server.CreateException(-2147220992);
                }

                try
                {
                    m_subscription.SetEnabled(bEnable != 0);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void GetEnable(out int pbEnable)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (!IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw Server.CreateException(-2147220992);
                }

                try
                {
                    pbEnable = (m_subscription.GetEnabled() ? 1 : 0);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void ReadMaxAge(int dwCount, int[] phServer, int[] pdwMaxAge, int dwTransactionID, out int pdwCancelID, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null || pdwMaxAge == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                if (!IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw Server.CreateException(-2147220992);
                }

                try
                {
                    Item[] array = new Item[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new Item((ItemIdentifier)m_items[phServer[i]]);
                        array[i].MaxAge = ((pdwMaxAge[i] < 0) ? int.MaxValue : pdwMaxAge[i]);
                        array[i].MaxAgeSpecified = true;
                    }

                    pdwCancelID = AssignHandle();
                    IRequest request = null;
                    IdentifiedResult[] array2 = m_subscription.Read(array, pdwCancelID, OnReadComplete, out request);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    if (request != null)
                    {
                        m_requests[request] = dwTransactionID;
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void WriteVQT(int dwCount, int[] phServer, OPCITEMVQT[] pItemVQT, int dwTransactionID, out int pdwCancelID, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null || pItemVQT == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                if (!IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw Server.CreateException(-2147220992);
                }

                try
                {
                    ItemValue[] array = new ItemValue[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new ItemValue((ItemIdentifier)m_items[phServer[i]]);
                        array[i].Value = pItemVQT[i].vDataValue;
                        array[i].Quality = new Quality(pItemVQT[i].wQuality);
                        array[i].QualitySpecified = (pItemVQT[i].bQualitySpecified != 0);
                        array[i].Timestamp = OpcCom.Interop.GetFILETIME(Interop.Convert(pItemVQT[i].ftTimeStamp));
                        array[i].TimestampSpecified = (pItemVQT[i].bTimeStampSpecified != 0);
                    }

                    pdwCancelID = AssignHandle();
                    IRequest request = null;
                    IdentifiedResult[] array2 = m_subscription.Write(array, pdwCancelID, OnWriteComplete, out request);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    if (request != null)
                    {
                        m_requests[request] = dwTransactionID;
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void RefreshMaxAge(int dwMaxAge, int dwTransactionID, out int pdwCancelID)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (!IsConnected(typeof(IOPCDataCallback).GUID))
                {
                    throw Server.CreateException(-2147220992);
                }

                try
                {
                    pdwCancelID = AssignHandle();
                    IRequest request = null;
                    m_subscription.Refresh(pdwCancelID, out request);
                    if (request != null)
                    {
                        m_requests[request] = dwTransactionID;
                    }
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void GetState(out int pUpdateRate, out int pActive, out string ppName, out int pTimeBias, out float pPercentDeadband, out int pLCID, out int phClientGroup, out int phServerGroup)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                try
                {
                    SubscriptionState state = m_subscription.GetState();
                    if (state == null)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    pUpdateRate = state.UpdateRate;
                    pActive = (state.Active ? 1 : 0);
                    ppName = state.Name;
                    pTimeBias = m_timebias;
                    pPercentDeadband = state.Deadband;
                    pLCID = m_lcid;
                    phClientGroup = (m_clientHandle = (int)state.ClientHandle);
                    phServerGroup = m_serverHandle;
                    m_name = state.Name;
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void CloneGroup(string szName, ref Guid riid, out object ppUnk)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                Group group = null;
                try
                {
                    SubscriptionState state = m_subscription.GetState();
                    state.Name = szName;
                    state.Active = false;
                    group = m_server.CreateGroup(ref state, m_lcid, m_timebias);
                    Item[] array = new Item[m_items.Count];
                    int num = 0;
                    foreach (Item value in m_items.Values)
                    {
                        array[num++] = value;
                    }

                    group.AddItems(array);
                    ppUnk = group;
                }
                catch (Exception e)
                {
                    if (group != null)
                    {
                        try
                        {
                            m_server.RemoveGroup(group.ServerHandle, 0);
                        }
                        catch
                        {
                        }
                    }

                    throw Server.CreateException(e);
                }
            }
        }

        public void SetName(string szName)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                try
                {
                    SubscriptionState state = m_subscription.GetState();
                    if (state == null)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    int num = m_server.SetGroupName(state.Name, szName);
                    if (num != 0)
                    {
                        throw new ExternalException("SetName", num);
                    }

                    string text2 = m_name = (state.Name = szName);
                    m_subscription.ModifyState(1, state);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void SetState(IntPtr pRequestedUpdateRate, out int pRevisedUpdateRate, IntPtr pActive, IntPtr pTimeBias, IntPtr pPercentDeadband, IntPtr pLCID, IntPtr phClientGroup)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                try
                {
                    SubscriptionState subscriptionState = new SubscriptionState();
                    if (subscriptionState == null)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    int num = 0;
                    if (pRequestedUpdateRate != IntPtr.Zero)
                    {
                        subscriptionState.UpdateRate = Marshal.ReadInt32(pRequestedUpdateRate);
                        num |= 0x10;
                    }

                    if (pActive != IntPtr.Zero)
                    {
                        subscriptionState.Active = (Marshal.ReadInt32(pActive) != 0);
                        num |= 8;
                    }

                    if (pTimeBias != IntPtr.Zero)
                    {
                        m_timebias = Marshal.ReadInt32(pTimeBias);
                    }

                    if (pPercentDeadband != IntPtr.Zero)
                    {
                        float[] array = new float[1];
                        Marshal.Copy(pPercentDeadband, array, 0, 1);
                        subscriptionState.Deadband = array[0];
                        num |= 0x80;
                    }

                    if (pLCID != IntPtr.Zero)
                    {
                        m_lcid = Marshal.ReadInt32(pLCID);
                        subscriptionState.Locale = OpcCom.Interop.GetLocale(m_lcid);
                        num |= 4;
                    }

                    if (phClientGroup != IntPtr.Zero)
                    {
                        subscriptionState.ClientHandle = (m_clientHandle = Marshal.ReadInt32(phClientGroup));
                        num |= 2;
                    }

                    subscriptionState = m_subscription.ModifyState(num, subscriptionState);
                    pRevisedUpdateRate = subscriptionState.UpdateRate;
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void GetKeepAlive(out int pdwKeepAliveTime)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                try
                {
                    SubscriptionState state = m_subscription.GetState();
                    if (state == null)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    pdwKeepAliveTime = state.KeepAlive;
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void SetKeepAlive(int dwKeepAliveTime, out int pdwRevisedKeepAliveTime)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                try
                {
                    SubscriptionState subscriptionState = new SubscriptionState();
                    if (subscriptionState == null)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    subscriptionState.KeepAlive = dwKeepAliveTime;
                    subscriptionState = m_subscription.ModifyState(32, subscriptionState);
                    pdwRevisedKeepAliveTime = subscriptionState.KeepAlive;
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void SetItemDeadband(int dwCount, int[] phServer, float[] pPercentDeadband, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null || pPercentDeadband == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    Item[] array = new Item[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new Item((ItemIdentifier)m_items[phServer[i]]);
                        array[i].Deadband = pPercentDeadband[i];
                        array[i].DeadbandSpecified = true;
                    }

                    ItemResult[] array2 = m_subscription.ModifyItems(128, array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    for (int j = 0; j < dwCount; j++)
                    {
                        if (array2[j].ResultID.Succeeded())
                        {
                            m_items[phServer[j]] = array2[j];
                        }
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void GetItemDeadband(int dwCount, int[] phServer, out IntPtr ppPercentDeadband, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    float[] array = new float[dwCount];
                    int[] array2 = new int[dwCount];
                    for (int i = 0; i < dwCount; i++)
                    {
                        ItemResult itemResult = (ItemResult)m_items[phServer[i]];
                        array2[i] = -1073479679;
                        if (itemResult != null && itemResult.ResultID.Succeeded())
                        {
                            if (itemResult.DeadbandSpecified)
                            {
                                array[i] = itemResult.Deadband;
                                array2[i] = 0;
                            }
                            else
                            {
                                array2[i] = -1073478656;
                            }
                        }
                    }

                    ppPercentDeadband = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(float)) * dwCount);
                    Marshal.Copy(array, 0, ppPercentDeadband, dwCount);
                    ppErrors = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * dwCount);
                    Marshal.Copy(array2, 0, ppErrors, dwCount);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void ClearItemDeadband(int dwCount, int[] phServer, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    ArrayList arrayList = new ArrayList();
                    Item[] array = new Item[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        Item item = (Item)m_items[phServer[i]];
                        array[i] = new Item((ItemIdentifier)item);
                        if (item != null)
                        {
                            if (item.DeadbandSpecified)
                            {
                                array[i].Deadband = 0f;
                                array[i].DeadbandSpecified = false;
                            }
                            else
                            {
                                arrayList.Add(i);
                            }
                        }
                    }

                    ItemResult[] array2 = m_subscription.ModifyItems(128, array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    foreach (int item2 in arrayList)
                    {
                        if (array2[item2].ResultID.Succeeded())
                        {
                            array2[item2].ResultID = new ResultID(-1073478656L);
                        }
                    }

                    for (int j = 0; j < dwCount; j++)
                    {
                        if (array2[j].ResultID.Succeeded())
                        {
                            m_items[phServer[j]] = array2[j];
                        }
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void SetItemSamplingRate(int dwCount, int[] phServer, int[] pdwRequestedSamplingRate, out IntPtr ppdwRevisedSamplingRate, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null || pdwRequestedSamplingRate == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    Item[] array = new Item[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new Item((ItemIdentifier)m_items[phServer[i]]);
                        array[i].SamplingRate = pdwRequestedSamplingRate[i];
                        array[i].SamplingRateSpecified = true;
                    }

                    ItemResult[] array2 = m_subscription.ModifyItems(256, array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    int[] array3 = new int[dwCount];
                    for (int j = 0; j < dwCount; j++)
                    {
                        if (array2[j].ResultID.Succeeded())
                        {
                            m_items[phServer[j]] = array2[j];
                            array3[j] = array2[j].SamplingRate;
                        }
                    }

                    ppdwRevisedSamplingRate = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * dwCount);
                    Marshal.Copy(array3, 0, ppdwRevisedSamplingRate, dwCount);
                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void GetItemSamplingRate(int dwCount, int[] phServer, out IntPtr ppdwSamplingRate, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    int[] array = new int[dwCount];
                    int[] array2 = new int[dwCount];
                    for (int i = 0; i < dwCount; i++)
                    {
                        ItemResult itemResult = (ItemResult)m_items[phServer[i]];
                        array2[i] = -1073479679;
                        if (itemResult != null && itemResult.ResultID.Succeeded())
                        {
                            if (itemResult.SamplingRateSpecified)
                            {
                                array[i] = itemResult.SamplingRate;
                                array2[i] = 0;
                            }
                            else
                            {
                                array2[i] = -1073478651;
                            }
                        }
                    }

                    ppdwSamplingRate = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * dwCount);
                    Marshal.Copy(array, 0, ppdwSamplingRate, dwCount);
                    ppErrors = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * dwCount);
                    Marshal.Copy(array2, 0, ppErrors, dwCount);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void ClearItemSamplingRate(int dwCount, int[] phServer, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    Item[] array = new Item[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new Item((ItemIdentifier)m_items[phServer[i]]);
                        array[i].SamplingRate = 0;
                        array[i].SamplingRateSpecified = false;
                    }

                    ItemResult[] array2 = m_subscription.ModifyItems(256, array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    for (int j = 0; j < dwCount; j++)
                    {
                        if (array2[j].ResultID.Succeeded())
                        {
                            m_items[phServer[j]] = array2[j];
                        }
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void SetItemBufferEnable(int dwCount, int[] phServer, int[] pbEnable, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null || pbEnable == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    Item[] array = new Item[dwCount];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new Item((ItemIdentifier)m_items[phServer[i]]);
                        array[i].EnableBuffering = ((pbEnable[i] != 0) ? true : false);
                        array[i].EnableBufferingSpecified = ((pbEnable[i] != 0) ? true : false);
                    }

                    ItemResult[] array2 = m_subscription.ModifyItems(512, array);
                    if (array2 == null || array2.Length != array.Length)
                    {
                        throw Server.CreateException(-2147467259);
                    }

                    for (int j = 0; j < dwCount; j++)
                    {
                        if (array2[j].ResultID.Succeeded())
                        {
                            m_items[phServer[j]] = array2[j];
                        }
                    }

                    ppErrors = Interop.GetHRESULTs(array2);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void GetItemBufferEnable(int dwCount, int[] phServer, out IntPtr ppbEnable, out IntPtr ppErrors)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (dwCount == 0 || phServer == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                try
                {
                    int[] array = new int[dwCount];
                    int[] array2 = new int[dwCount];
                    for (int i = 0; i < dwCount; i++)
                    {
                        ItemResult itemResult = (ItemResult)m_items[phServer[i]];
                        array2[i] = -1073479679;
                        if (itemResult != null && itemResult.ResultID.Succeeded())
                        {
                            array[i] = ((itemResult.EnableBuffering && itemResult.EnableBufferingSpecified) ? 1 : 0);
                            array2[i] = 0;
                        }
                    }

                    ppbEnable = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * dwCount);
                    Marshal.Copy(array, 0, ppbEnable, dwCount);
                    ppErrors = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * dwCount);
                    Marshal.Copy(array2, 0, ppErrors, dwCount);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        private int AssignHandle()
        {
            return ++m_nextHandle;
        }

        private void OnDataChanged(object subscriptionHandle, object requestHandle, ItemValueResult[] results)
        {
            InvokeCallback(requestHandle, results, dataChanged: true);
        }

        private void OnReadComplete(object requestHandle, ItemValueResult[] results)
        {
            InvokeCallback(requestHandle, results, dataChanged: false);
        }

        private void InvokeCallback(object requestHandle, ItemValueResult[] results, bool dataChanged)
        {
            try
            {
                object obj = null;
                int dwTransid = 0;
                int hGroup = 0;
                int hrMastererror = 0;
                int hrMasterquality = 0;
                int[] array = null;
                object[] array2 = null;
                short[] array3 = null;
                OpcRcw.Da.FILETIME[] array4 = null;
                int[] array5 = null;
                lock (this)
                {
                    bool flag = false;
                    IDictionaryEnumerator enumerator = m_requests.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        Opc.Da.Request request = (Opc.Da.Request)enumerator.Key;
                        if (request.Handle.Equals(requestHandle))
                        {
                            dwTransid = (int)enumerator.Value;
                            m_requests.Remove(request);
                            flag = true;
                            break;
                        }
                    }

                    if (!dataChanged && !flag)
                    {
                        return;
                    }

                    obj = GetCallback(typeof(IOPCDataCallback).GUID);
                    if (obj == null)
                    {
                        return;
                    }

                    hGroup = m_clientHandle;
                    if (results != null)
                    {
                        array = new int[results.Length];
                        array2 = new object[results.Length];
                        array3 = new short[results.Length];
                        array4 = new OpcRcw.Da.FILETIME[results.Length];
                        array5 = new int[results.Length];
                        for (int i = 0; i < results.Length; i++)
                        {
                            array[i] = (int)results[i].ClientHandle;
                            array2[i] = results[i].Value;
                            array3[i] = (short)(results[i].QualitySpecified ? results[i].Quality.GetCode() : 0);
                            ref OpcRcw.Da.FILETIME reference = ref array4[i];
                            reference = Interop.Convert(OpcCom.Interop.GetFILETIME(results[i].Timestamp));
                            array5[i] = OpcCom.Interop.GetResultID(results[i].ResultID);
                            if (results[i].Quality.QualityBits != qualityBits.good)
                            {
                                hrMasterquality = 1;
                            }

                            if (results[i].ResultID != ResultID.S_OK)
                            {
                                hrMastererror = 1;
                            }
                        }
                    }
                }

                if (dataChanged)
                {
                    ((IOPCDataCallback)obj).OnDataChange(dwTransid, hGroup, hrMasterquality, hrMastererror, array.Length, array, array2, array3, array4, array5);
                }
                else
                {
                    ((IOPCDataCallback)obj).OnReadComplete(dwTransid, hGroup, hrMasterquality, hrMastererror, array.Length, array, array2, array3, array4, array5);
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        private void OnWriteComplete(object clientHandle, IdentifiedResult[] results)
        {
            try
            {
                object obj = null;
                int dwTransid = -1;
                int hGroup = -1;
                int hrMastererr = 0;
                int[] array = null;
                int[] array2 = null;
                lock (this)
                {
                    bool flag = false;
                    IDictionaryEnumerator enumerator = m_requests.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        Opc.Da.Request request = (Opc.Da.Request)enumerator.Key;
                        if (request.Handle.Equals(clientHandle))
                        {
                            dwTransid = (int)enumerator.Value;
                            m_requests.Remove(request);
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        return;
                    }

                    obj = GetCallback(typeof(IOPCDataCallback).GUID);
                    if (obj == null)
                    {
                        return;
                    }

                    hGroup = m_clientHandle;
                    if (results != null)
                    {
                        array = new int[results.Length];
                        array2 = new int[results.Length];
                        for (int i = 0; i < results.Length; i++)
                        {
                            array[i] = (int)results[i].ClientHandle;
                            array2[i] = OpcCom.Interop.GetResultID(results[i].ResultID);
                            if (results[i].ResultID != ResultID.S_OK)
                            {
                                hrMastererr = 1;
                            }
                        }
                    }
                }

                ((IOPCDataCallback)obj).OnWriteComplete(dwTransid, hGroup, hrMastererr, array.Length, array, array2);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        private void AddItems(Item[] items)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw Server.CreateException(-2147467259);
                }

                if (items == null)
                {
                    throw Server.CreateException(-2147024809);
                }

                ItemResult[] array = m_subscription.AddItems(items);
                if (array == null || array.Length != items.Length)
                {
                    throw Server.CreateException(-2147467259);
                }

                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].ResultID.Succeeded())
                    {
                        m_items[++m_nextHandle] = array[i];
                    }
                }
            }
        }
    }
}
