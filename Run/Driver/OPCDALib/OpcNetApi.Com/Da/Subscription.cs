using System;
using System.Collections;
using System.Runtime.InteropServices;
using Opc;
using Opc.Da;
using OpcRcw.Da;

namespace OpcCom.Da
{
    public class Subscription : ISubscription, IDisposable
    {
        private class ItemTable
        {
            private Hashtable m_items = new Hashtable();

            public ItemIdentifier this[object handle]
            {
                get
                {
                    if (handle != null)
                    {
                        return (ItemIdentifier)m_items[handle];
                    }

                    return null;
                }
                set
                {
                    if (handle != null)
                    {
                        if (value == null)
                        {
                            m_items.Remove(handle);
                        }
                        else
                        {
                            m_items[handle] = value;
                        }
                    }
                }
            }

            private int GetInvalidHandle()
            {
                int num = 0;
                foreach (ItemIdentifier value in m_items.Values)
                {
                    if (value.ServerHandle != null && (object)value.ServerHandle.GetType() == typeof(int) && num < (int)value.ServerHandle)
                    {
                        num = (int)value.ServerHandle + 1;
                    }
                }

                return num;
            }

            public ItemIdentifier[] GetItemIDs(ItemIdentifier[] items)
            {
                int invalidHandle = GetInvalidHandle();
                ItemIdentifier[] array = new ItemIdentifier[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    ItemIdentifier itemIdentifier = this[items[i].ServerHandle];
                    if (itemIdentifier != null)
                    {
                        array[i] = (ItemIdentifier)itemIdentifier.Clone();
                    }
                    else
                    {
                        array[i] = new ItemIdentifier();
                        array[i].ServerHandle = invalidHandle;
                    }

                    array[i].ClientHandle = items[i].ServerHandle;
                }

                return array;
            }

            public ItemResult[] CreateItems(Item[] items)
            {
                if (items == null)
                {
                    return null;
                }

                ItemResult[] array = new ItemResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = new ItemResult(items[i]);
                    ItemIdentifier itemIdentifier = this[items[i].ServerHandle];
                    if (itemIdentifier != null)
                    {
                        array[i].ItemName = itemIdentifier.ItemName;
                        array[i].ItemPath = itemIdentifier.ItemName;
                        array[i].ServerHandle = itemIdentifier.ServerHandle;
                        itemIdentifier.ClientHandle = items[i].ClientHandle;
                    }

                    if (array[i].ServerHandle == null)
                    {
                        array[i].ResultID = ResultID.Da.E_INVALIDHANDLE;
                        array[i].DiagnosticInfo = null;
                    }
                    else
                    {
                        array[i].ClientHandle = items[i].ServerHandle;
                    }
                }

                return array;
            }

            public ItemIdentifier[] ApplyFilters(int filters, ItemIdentifier[] results)
            {
                if (results == null)
                {
                    return null;
                }

                foreach (ItemIdentifier itemIdentifier in results)
                {
                    ItemIdentifier itemIdentifier2 = this[itemIdentifier.ClientHandle];
                    if (itemIdentifier2 != null)
                    {
                        itemIdentifier.ItemName = (((filters & 1) != 0) ? itemIdentifier2.ItemName : null);
                        itemIdentifier.ItemPath = (((filters & 2) != 0) ? itemIdentifier2.ItemPath : null);
                        itemIdentifier.ServerHandle = itemIdentifier.ClientHandle;
                        itemIdentifier.ClientHandle = (((filters & 4) != 0) ? itemIdentifier2.ClientHandle : null);
                    }

                    if ((filters & 8) == 0 && (object)itemIdentifier.GetType() == typeof(ItemValueResult))
                    {
                        ((ItemValueResult)itemIdentifier).Timestamp = DateTime.MinValue;
                        ((ItemValueResult)itemIdentifier).TimestampSpecified = false;
                    }
                }

                return results;
            }
        }

        private class Callback : IOPCDataCallback
        {
            private object m_handle;

            private int m_filters = 9;

            private ItemTable m_items;

            private Hashtable m_requests = new Hashtable();

            public event DataChangedEventHandler DataChanged
            {
                add
                {
                    lock (this)
                    {
                        this.m_dataChanged = (DataChangedEventHandler)Delegate.Combine(this.m_dataChanged, value);
                    }
                }
                remove
                {
                    lock (this)
                    {
                        this.m_dataChanged = (DataChangedEventHandler)Delegate.Remove(this.m_dataChanged, value);
                    }
                }
            }

            private event DataChangedEventHandler m_dataChanged;

            public Callback(object handle, int filters, ItemTable items)
            {
                m_handle = handle;
                m_filters = filters;
                m_items = items;
            }

            public void SetFilters(object handle, int filters)
            {
                lock (this)
                {
                    m_handle = handle;
                    m_filters = filters;
                }
            }

            public void BeginRequest(Request request)
            {
                lock (this)
                {
                    m_requests[request.RequestID] = request;
                }
            }

            public bool CancelRequest(Request request)
            {
                lock (this)
                {
                    return m_requests.ContainsKey(request.RequestID);
                }
            }

            public void EndRequest(Request request)
            {
                lock (this)
                {
                    m_requests.Remove(request.RequestID);
                }
            }

            public void OnDataChange(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
            {
                try
                {
                    Request request = null;
                    lock (this)
                    {
                        if (dwTransid != 0)
                        {
                            request = (Request)m_requests[dwTransid];
                            if (request != null)
                            {
                                m_requests.Remove(dwTransid);
                            }
                        }

                        if (this.m_dataChanged != null)
                        {
                            ItemValueResult[] array = UnmarshalValues(dwCount, phClientItems, pvValues, pwQualities, pftTimeStamps, pErrors);
                            lock (m_items)
                            {
                                m_items.ApplyFilters(m_filters | 4, array);
                            }

                            this.m_dataChanged(m_handle, request?.Handle, array);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _ = ex.StackTrace;
                }
            }

            public void OnReadComplete(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
            {
                try
                {
                    Request request = null;
                    ItemValueResult[] results = null;
                    lock (this)
                    {
                        request = (Request)m_requests[dwTransid];
                        if (request == null)
                        {
                            return;
                        }

                        m_requests.Remove(dwTransid);
                        results = UnmarshalValues(dwCount, phClientItems, pvValues, pwQualities, pftTimeStamps, pErrors);
                        lock (m_items)
                        {
                            m_items.ApplyFilters(m_filters | 4, results);
                        }
                    }

                    lock (request)
                    {
                        request.EndRequest(results);
                    }
                }
                catch (Exception ex)
                {
                    _ = ex.StackTrace;
                }
            }

            public void OnWriteComplete(int dwTransid, int hGroup, int hrMastererror, int dwCount, int[] phClientItems, int[] pErrors)
            {
                try
                {
                    Request request = null;
                    IdentifiedResult[] array = null;
                    lock (this)
                    {
                        request = (Request)m_requests[dwTransid];
                        if (request == null)
                        {
                            return;
                        }

                        m_requests.Remove(dwTransid);
                        array = new IdentifiedResult[dwCount];
                        for (int i = 0; i < array.Length; i++)
                        {
                            ItemIdentifier item = m_items[phClientItems[i]];
                            array[i] = new IdentifiedResult(item);
                            array[i].ClientHandle = phClientItems[i];
                            array[i].ResultID = OpcCom.Interop.GetResultID(pErrors[i]);
                            array[i].DiagnosticInfo = null;
                            if (pErrors[i] == -1073479674)
                            {
                                array[i].ResultID = new ResultID(ResultID.Da.E_READONLY, -1073479674L);
                            }
                        }

                        lock (m_items)
                        {
                            m_items.ApplyFilters(m_filters | 4, array);
                        }
                    }

                    lock (request)
                    {
                        request.EndRequest(array);
                    }
                }
                catch (Exception ex)
                {
                    _ = ex.StackTrace;
                }
            }

            public void OnCancelComplete(int dwTransid, int hGroup)
            {
                try
                {
                    Request request = null;
                    lock (this)
                    {
                        request = (Request)m_requests[dwTransid];
                        if (request == null)
                        {
                            return;
                        }

                        m_requests.Remove(dwTransid);
                    }

                    lock (request)
                    {
                        request.EndRequest();
                    }
                }
                catch (Exception ex)
                {
                    _ = ex.StackTrace;
                }
            }

            private ItemValueResult[] UnmarshalValues(int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
            {
                ItemValueResult[] array = new ItemValueResult[dwCount];
                for (int i = 0; i < array.Length; i++)
                {
                    ItemIdentifier item = m_items[phClientItems[i]];
                    array[i] = new ItemValueResult(item);
                    array[i].ClientHandle = phClientItems[i];
                    array[i].Value = pvValues[i];
                    array[i].Quality = new Quality(pwQualities[i]);
                    array[i].QualitySpecified = true;
                    array[i].Timestamp = OpcCom.Interop.GetFILETIME(Interop.Convert(pftTimeStamps[i]));
                    array[i].TimestampSpecified = (array[i].Timestamp != DateTime.MinValue);
                    array[i].ResultID = OpcCom.Interop.GetResultID(pErrors[i]);
                    array[i].DiagnosticInfo = null;
                    if (pErrors[i] == -1073479674)
                    {
                        array[i].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                    }
                }

                return array;
            }
        }

        private bool m_disposed;

        protected object m_group;

        protected ConnectionPoint m_connection;

        private Callback m_callback;

        protected string m_name;

        protected object m_handle;

        protected int m_filters = 9;

        private ItemTable m_items = new ItemTable();

        protected int m_counter;

        public event DataChangedEventHandler DataChanged
        {
            add
            {
                lock (this)
                {
                    m_callback.DataChanged += value;
                    Advise();
                }
            }
            remove
            {
                lock (this)
                {
                    m_callback.DataChanged -= value;
                    Unadvise();
                }
            }
        }

        internal Subscription(object group, SubscriptionState state, int filters)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }

            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            m_group = group;
            m_name = state.Name;
            m_handle = state.ClientHandle;
            m_filters = filters;
            m_callback = new Callback(state.ClientHandle, m_filters, m_items);
        }

        ~Subscription()
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
                if (disposing && m_group != null && m_connection != null)
                {
                    m_connection.Dispose();
                    m_connection = null;
                }

                if (m_group != null)
                {
                    OpcCom.Interop.ReleaseServer(m_group);
                    m_group = null;
                }
            }

            m_disposed = true;
        }

        public int GetResultFilters()
        {
            lock (this)
            {
                return m_filters;
            }
        }

        public void SetResultFilters(int filters)
        {
            lock (this)
            {
                m_filters = filters;
                m_callback.SetFilters(m_handle, m_filters);
            }
        }

        public SubscriptionState GetState()
        {
            lock (this)
            {
                SubscriptionState subscriptionState = new SubscriptionState();
                subscriptionState.ClientHandle = m_handle;
                try
                {
                    string ppName = null;
                    int pActive = 0;
                    int pUpdateRate = 0;
                    float pPercentDeadband = 0f;
                    int pTimeBias = 0;
                    int pLCID = 0;
                    int phClientGroup = 0;
                    int phServerGroup = 0;
                    ((IOPCGroupStateMgt)m_group).GetState(out pUpdateRate, out pActive, out ppName, out pTimeBias, out pPercentDeadband, out pLCID, out phClientGroup, out phServerGroup);
                    subscriptionState.Name = ppName;
                    subscriptionState.ServerHandle = phServerGroup;
                    subscriptionState.Active = (pActive != 0);
                    subscriptionState.UpdateRate = pUpdateRate;
                    subscriptionState.Deadband = pPercentDeadband;
                    subscriptionState.Locale = OpcCom.Interop.GetLocale(pLCID);
                    m_name = subscriptionState.Name;
                    try
                    {
                        int pdwKeepAliveTime = 0;
                        ((IOPCGroupStateMgt2)m_group).GetKeepAlive(out pdwKeepAliveTime);
                        subscriptionState.KeepAlive = pdwKeepAliveTime;
                    }
                    catch
                    {
                        subscriptionState.KeepAlive = 0;
                    }
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCGroupStateMgt.GetState", e);
                }

                return subscriptionState;
            }
        }

        public SubscriptionState ModifyState(int masks, SubscriptionState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            lock (this)
            {
                if ((masks & 1) != 0 && state.Name != m_name)
                {
                    try
                    {
                        ((IOPCGroupStateMgt)m_group).SetName(state.Name);
                        m_name = state.Name;
                    }
                    catch (Exception e)
                    {
                        throw OpcCom.Interop.CreateException("IOPCGroupStateMgt.SetName", e);
                    }
                }

                if ((masks & 2) != 0)
                {
                    m_handle = state.ClientHandle;
                    m_callback.SetFilters(m_handle, m_filters);
                }

                int num = state.Active ? 1 : 0;
                int num2 = ((masks & 4) != 0) ? OpcCom.Interop.GetLocale(state.Locale) : 0;
                GCHandle gCHandle = GCHandle.Alloc(num, GCHandleType.Pinned);
                GCHandle gCHandle2 = GCHandle.Alloc(num2, GCHandleType.Pinned);
                GCHandle gCHandle3 = GCHandle.Alloc(state.UpdateRate, GCHandleType.Pinned);
                GCHandle gCHandle4 = GCHandle.Alloc(state.Deadband, GCHandleType.Pinned);
                int pRevisedUpdateRate = 0;
                try
                {
                    ((IOPCGroupStateMgt)m_group).SetState(((masks & 0x10) != 0) ? gCHandle3.AddrOfPinnedObject() : IntPtr.Zero, out pRevisedUpdateRate, ((masks & 8) != 0) ? gCHandle.AddrOfPinnedObject() : IntPtr.Zero, IntPtr.Zero, ((masks & 0x80) != 0) ? gCHandle4.AddrOfPinnedObject() : IntPtr.Zero, ((masks & 4) != 0) ? gCHandle2.AddrOfPinnedObject() : IntPtr.Zero, IntPtr.Zero);
                }
                catch (Exception e2)
                {
                    throw OpcCom.Interop.CreateException("IOPCGroupStateMgt.SetState", e2);
                }
                finally
                {
                    if (gCHandle.IsAllocated)
                    {
                        gCHandle.Free();
                    }

                    if (gCHandle2.IsAllocated)
                    {
                        gCHandle2.Free();
                    }

                    if (gCHandle3.IsAllocated)
                    {
                        gCHandle3.Free();
                    }

                    if (gCHandle4.IsAllocated)
                    {
                        gCHandle4.Free();
                    }
                }

                if ((masks & 0x20) != 0)
                {
                    int pdwRevisedKeepAliveTime = 0;
                    try
                    {
                        ((IOPCGroupStateMgt2)m_group).SetKeepAlive(state.KeepAlive, out pdwRevisedKeepAliveTime);
                    }
                    catch
                    {
                        state.KeepAlive = 0;
                    }
                }

                return GetState();
            }
        }

        public ItemResult[] AddItems(Item[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (items.Length == 0)
            {
                return new ItemResult[0];
            }

            lock (this)
            {
                if (m_group == null)
                {
                    throw new NotConnectedException();
                }

                int num = items.Length;
                OPCITEMDEF[] oPCITEMDEFs = Interop.GetOPCITEMDEFs(items);
                ItemResult[] array = null;
                lock (m_items)
                {
                    for (int i = 0; i < num; i++)
                    {
                        oPCITEMDEFs[i].hClient = ++m_counter;
                    }

                    IntPtr ppAddResults = IntPtr.Zero;
                    IntPtr ppErrors = IntPtr.Zero;
                    try
                    {
                        ((IOPCItemMgt)m_group).AddItems(num, oPCITEMDEFs, out ppAddResults, out ppErrors);
                    }
                    catch (Exception e)
                    {
                        throw OpcCom.Interop.CreateException("IOPCItemMgt.AddItems", e);
                    }

                    int[] itemResults = Interop.GetItemResults(ref ppAddResults, num, deallocate: true);
                    int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, num, deallocate: true);
                    array = new ItemResult[num];
                    for (int j = 0; j < num; j++)
                    {
                        array[j] = new ItemResult(items[j]);
                        array[j].ServerHandle = itemResults[j];
                        array[j].ClientHandle = oPCITEMDEFs[j].hClient;
                        if (!array[j].ActiveSpecified)
                        {
                            array[j].Active = true;
                            array[j].ActiveSpecified = true;
                        }

                        array[j].ResultID = OpcCom.Interop.GetResultID(int32s[j]);
                        array[j].DiagnosticInfo = null;
                        if (array[j].ResultID.Succeeded())
                        {
                            array[j].ClientHandle = items[j].ClientHandle;
                            m_items[oPCITEMDEFs[j].hClient] = new ItemIdentifier(array[j]);
                            array[j].ClientHandle = oPCITEMDEFs[j].hClient;
                        }
                    }
                }

                UpdateDeadbands(array);
                UpdateSamplingRates(array);
                SetEnableBuffering(array);
                lock (m_items)
                {
                    ItemResult[] array2 = (ItemResult[])m_items.ApplyFilters(m_filters, array);
                    if ((m_filters & 4) != 0)
                    {
                        for (int k = 0; k < num; k++)
                        {
                            if (array2[k].ResultID.Failed())
                            {
                                array2[k].ClientHandle = items[k].ClientHandle;
                            }
                        }
                    }

                    return array2;
                }
            }
        }

        public ItemResult[] ModifyItems(int masks, Item[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (items.Length == 0)
            {
                return new ItemResult[0];
            }

            lock (this)
            {
                if (m_group == null)
                {
                    throw new NotConnectedException();
                }

                ItemResult[] array = null;
                lock (m_items)
                {
                    array = m_items.CreateItems(items);
                }

                if ((masks & 0x40) != 0)
                {
                    SetReqTypes(array);
                }

                if ((masks & 8) != 0)
                {
                    UpdateActive(array);
                }

                if ((masks & 0x80) != 0)
                {
                    UpdateDeadbands(array);
                }

                if ((masks & 0x100) != 0)
                {
                    UpdateSamplingRates(array);
                }

                if ((masks & 0x200) != 0)
                {
                    SetEnableBuffering(array);
                }

                lock (m_items)
                {
                    return (ItemResult[])m_items.ApplyFilters(m_filters, array);
                }
            }
        }

        public IdentifiedResult[] RemoveItems(ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (items.Length == 0)
            {
                return new IdentifiedResult[0];
            }

            lock (this)
            {
                if (m_group == null)
                {
                    throw new NotConnectedException();
                }

                ItemIdentifier[] array = null;
                lock (m_items)
                {
                    array = m_items.GetItemIDs(items);
                }

                int[] array2 = new int[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    array2[i] = (int)array[i].ServerHandle;
                }

                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCItemMgt)m_group).RemoveItems(array.Length, array2, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCItemMgt.RemoveItems", e);
                }

                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, array.Length, deallocate: true);
                IdentifiedResult[] array3 = new IdentifiedResult[array.Length];
                ArrayList arrayList = new ArrayList(array.Length);
                for (int j = 0; j < array.Length; j++)
                {
                    array3[j] = new IdentifiedResult(array[j]);
                    array3[j].ResultID = OpcCom.Interop.GetResultID(int32s[j]);
                    array3[j].DiagnosticInfo = null;
                    if (array3[j].ResultID.Succeeded())
                    {
                        arrayList.Add(array3[j].ClientHandle);
                    }
                }

                lock (m_items)
                {
                    array3 = (IdentifiedResult[])m_items.ApplyFilters(m_filters, array3);
                    foreach (int item in arrayList)
                    {
                        m_items[item] = null;
                    }

                    return array3;
                }
            }
        }

        public ItemValueResult[] Read(Item[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (items.Length == 0)
            {
                return new ItemValueResult[0];
            }

            lock (this)
            {
                if (m_group == null)
                {
                    throw new NotConnectedException();
                }

                ItemIdentifier[] itemIDs = null;
                lock (m_items)
                {
                    itemIDs = m_items.GetItemIDs(items);
                }

                ItemValueResult[] results = Read(itemIDs, items);
                lock (m_items)
                {
                    return (ItemValueResult[])m_items.ApplyFilters(m_filters, results);
                }
            }
        }

        public IdentifiedResult[] Write(ItemValue[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (items.Length == 0)
            {
                return new IdentifiedResult[0];
            }

            lock (this)
            {
                if (m_group == null)
                {
                    throw new NotConnectedException();
                }

                ItemIdentifier[] itemIDs = null;
                lock (m_items)
                {
                    itemIDs = m_items.GetItemIDs(items);
                }

                IdentifiedResult[] results = Write(itemIDs, items);
                lock (m_items)
                {
                    return (IdentifiedResult[])m_items.ApplyFilters(m_filters, results);
                }
            }
        }

        public IdentifiedResult[] Read(Item[] items, object requestHandle, ReadCompleteEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            request = null;
            if (items.Length == 0)
            {
                return new IdentifiedResult[0];
            }

            lock (this)
            {
                if (m_group == null)
                {
                    throw new NotConnectedException();
                }

                if (m_connection == null)
                {
                    Advise();
                }

                ItemIdentifier[] itemIDs = null;
                lock (m_items)
                {
                    itemIDs = m_items.GetItemIDs(items);
                }

                Request request2 = new Request(this, requestHandle, m_filters, m_counter++, callback);
                m_callback.BeginRequest(request2);
                request = request2;
                IdentifiedResult[] array = null;
                int cancelID = 0;
                try
                {
                    array = BeginRead(itemIDs, items, request2.RequestID, out cancelID);
                }
                catch (Exception ex)
                {
                    m_callback.EndRequest(request2);
                    throw ex;
                }

                lock (m_items)
                {
                    m_items.ApplyFilters(m_filters | 4, array);
                }

                lock (request2)
                {
                    if (request2.BeginRead(cancelID, array))
                    {
                        m_callback.EndRequest(request2);
                        request = null;
                    }
                }

                return array;
            }
        }

        public IdentifiedResult[] Write(ItemValue[] items, object requestHandle, WriteCompleteEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            request = null;
            if (items.Length == 0)
            {
                return new IdentifiedResult[0];
            }

            lock (this)
            {
                if (m_group == null)
                {
                    throw new NotConnectedException();
                }

                if (m_connection == null)
                {
                    Advise();
                }

                ItemIdentifier[] itemIDs = null;
                lock (m_items)
                {
                    itemIDs = m_items.GetItemIDs(items);
                }

                Request request2 = new Request(this, requestHandle, m_filters, m_counter++, callback);
                m_callback.BeginRequest(request2);
                request = request2;
                IdentifiedResult[] array = null;
                int cancelID = 0;
                try
                {
                    array = BeginWrite(itemIDs, items, request2.RequestID, out cancelID);
                }
                catch (Exception ex)
                {
                    m_callback.EndRequest(request2);
                    throw ex;
                }

                lock (m_items)
                {
                    m_items.ApplyFilters(m_filters | 4, array);
                }

                lock (request2)
                {
                    if (request2.BeginWrite(cancelID, array))
                    {
                        m_callback.EndRequest(request2);
                        request = null;
                    }
                }

                return array;
            }
        }

        public void Cancel(IRequest request, CancelCompleteEventHandler callback)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            lock (this)
            {
                lock (request)
                {
                    if (m_callback.CancelRequest((Request)request))
                    {
                        ((Request)request).Callback = callback;
                        try
                        {
                            ((IOPCAsyncIO2)m_group).Cancel2(((Request)request).CancelID);
                        }
                        catch (Exception e)
                        {
                            throw OpcCom.Interop.CreateException("IOPCAsyncIO2.Cancel2", e);
                        }
                    }
                }
            }
        }

        public virtual void Refresh()
        {
            lock (this)
            {
                if (m_group == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    int pdwCancelID = 0;
                    ((IOPCAsyncIO3)m_group).RefreshMaxAge(int.MaxValue, ++m_counter, out pdwCancelID);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCAsyncIO3.RefreshMaxAge", e);
                }
            }
        }

        public virtual void Refresh(object requestHandle, out IRequest request)
        {
            lock (this)
            {
                if (m_group == null)
                {
                    throw new NotConnectedException();
                }

                if (m_connection == null)
                {
                    Advise();
                }

                Request request2 = new Request(this, requestHandle, m_filters, m_counter++, null);
                int pdwCancelID = 0;
                try
                {
                    ((IOPCAsyncIO3)m_group).RefreshMaxAge(0, request2.RequestID, out pdwCancelID);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCAsyncIO3.RefreshMaxAge", e);
                }

                request = request2;
                lock (request)
                {
                    request2.BeginRefresh(pdwCancelID);
                }
            }
        }

        public virtual void SetEnabled(bool enabled)
        {
            lock (this)
            {
                if (m_group == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    ((IOPCAsyncIO3)m_group).SetEnable(enabled ? 1 : 0);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCAsyncIO3.SetEnable", e);
                }
            }
        }

        public virtual bool GetEnabled()
        {
            lock (this)
            {
                if (m_group == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    int pbEnable = 0;
                    ((IOPCAsyncIO3)m_group).GetEnable(out pbEnable);
                    return pbEnable != 0;
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCAsyncIO3.GetEnable", e);
                }
            }
        }

        protected virtual ItemValueResult[] Read(ItemIdentifier[] itemIDs, Item[] items)
        {
            try
            {
                int[] array = new int[itemIDs.Length];
                int[] array2 = new int[itemIDs.Length];
                for (int i = 0; i < itemIDs.Length; i++)
                {
                    array[i] = (int)itemIDs[i].ServerHandle;
                    array2[i] = (items[i].MaxAgeSpecified ? items[i].MaxAge : 0);
                }

                IntPtr ppvValues = IntPtr.Zero;
                IntPtr ppwQualities = IntPtr.Zero;
                IntPtr ppftTimeStamps = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCSyncIO2)m_group).ReadMaxAge(itemIDs.Length, array, array2, out ppvValues, out ppwQualities, out ppftTimeStamps, out ppErrors);
                object[] vARIANTs = OpcCom.Interop.GetVARIANTs(ref ppvValues, itemIDs.Length, deallocate: true);
                short[] int16s = OpcCom.Interop.GetInt16s(ref ppwQualities, itemIDs.Length, deallocate: true);
                DateTime[] fILETIMEs = OpcCom.Interop.GetFILETIMEs(ref ppftTimeStamps, itemIDs.Length, deallocate: true);
                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, itemIDs.Length, deallocate: true);
                ItemValueResult[] array3 = new ItemValueResult[itemIDs.Length];
                for (int j = 0; j < itemIDs.Length; j++)
                {
                    array3[j] = new ItemValueResult(itemIDs[j]);
                    array3[j].Value = vARIANTs[j];
                    array3[j].Quality = new Quality(int16s[j]);
                    array3[j].QualitySpecified = (vARIANTs[j] != null);
                    array3[j].Timestamp = fILETIMEs[j];
                    array3[j].TimestampSpecified = (vARIANTs[j] != null);
                    array3[j].ResultID = OpcCom.Interop.GetResultID(int32s[j]);
                    array3[j].DiagnosticInfo = null;
                    if (int32s[j] == -1073479674)
                    {
                        array3[j].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                    }
                }

                return array3;
            }
            catch (Exception e)
            {
                throw OpcCom.Interop.CreateException("IOPCSyncIO2.ReadMaxAge", e);
            }
        }

        protected virtual IdentifiedResult[] Write(ItemIdentifier[] itemIDs, ItemValue[] items)
        {
            try
            {
                int[] array = new int[itemIDs.Length];
                for (int i = 0; i < itemIDs.Length; i++)
                {
                    array[i] = (int)itemIDs[i].ServerHandle;
                }

                OPCITEMVQT[] oPCITEMVQTs = Interop.GetOPCITEMVQTs(items);
                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCSyncIO2)m_group).WriteVQT(itemIDs.Length, array, oPCITEMVQTs, out ppErrors);
                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, itemIDs.Length, deallocate: true);
                IdentifiedResult[] array2 = new IdentifiedResult[itemIDs.Length];
                for (int j = 0; j < itemIDs.Length; j++)
                {
                    array2[j] = new IdentifiedResult(itemIDs[j]);
                    array2[j].ResultID = OpcCom.Interop.GetResultID(int32s[j]);
                    array2[j].DiagnosticInfo = null;
                    if (int32s[j] == -1073479674)
                    {
                        array2[j].ResultID = new ResultID(ResultID.Da.E_READONLY, -1073479674L);
                    }
                }

                return array2;
            }
            catch (Exception e)
            {
                throw OpcCom.Interop.CreateException("IOPCSyncIO2.WriteVQT", e);
            }
        }

        protected virtual IdentifiedResult[] BeginRead(ItemIdentifier[] itemIDs, Item[] items, int requestID, out int cancelID)
        {
            try
            {
                int[] array = new int[itemIDs.Length];
                int[] array2 = new int[itemIDs.Length];
                for (int i = 0; i < itemIDs.Length; i++)
                {
                    array[i] = (int)itemIDs[i].ServerHandle;
                    array2[i] = (items[i].MaxAgeSpecified ? items[i].MaxAge : 0);
                }

                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCAsyncIO3)m_group).ReadMaxAge(itemIDs.Length, array, array2, requestID, out cancelID, out ppErrors);
                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, itemIDs.Length, deallocate: true);
                IdentifiedResult[] array3 = new IdentifiedResult[itemIDs.Length];
                for (int j = 0; j < itemIDs.Length; j++)
                {
                    array3[j] = new IdentifiedResult(itemIDs[j]);
                    array3[j].ResultID = OpcCom.Interop.GetResultID(int32s[j]);
                    array3[j].DiagnosticInfo = null;
                    if (int32s[j] == -1073479674)
                    {
                        array3[j].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                    }
                }

                return array3;
            }
            catch (Exception e)
            {
                throw OpcCom.Interop.CreateException("IOPCAsyncIO3.ReadMaxAge", e);
            }
        }

        protected virtual IdentifiedResult[] BeginWrite(ItemIdentifier[] itemIDs, ItemValue[] items, int requestID, out int cancelID)
        {
            try
            {
                int[] array = new int[itemIDs.Length];
                for (int i = 0; i < itemIDs.Length; i++)
                {
                    array[i] = (int)itemIDs[i].ServerHandle;
                }

                OPCITEMVQT[] oPCITEMVQTs = Interop.GetOPCITEMVQTs(items);
                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCAsyncIO3)m_group).WriteVQT(itemIDs.Length, array, oPCITEMVQTs, requestID, out cancelID, out ppErrors);
                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, itemIDs.Length, deallocate: true);
                IdentifiedResult[] array2 = new IdentifiedResult[itemIDs.Length];
                for (int j = 0; j < itemIDs.Length; j++)
                {
                    array2[j] = new IdentifiedResult(itemIDs[j]);
                    array2[j].ResultID = OpcCom.Interop.GetResultID(int32s[j]);
                    array2[j].DiagnosticInfo = null;
                    if (int32s[j] == -1073479674)
                    {
                        array2[j].ResultID = new ResultID(ResultID.Da.E_READONLY, -1073479674L);
                    }
                }

                return array2;
            }
            catch (Exception e)
            {
                throw OpcCom.Interop.CreateException("IOPCAsyncIO3.WriteVQT", e);
            }
        }

        private void SetReqTypes(ItemResult[] items)
        {
            if (items == null || items.Length == 0)
            {
                return;
            }

            ArrayList arrayList = new ArrayList();
            foreach (ItemResult itemResult in items)
            {
                if (itemResult.ResultID.Succeeded() && (object)itemResult.ReqType != null)
                {
                    arrayList.Add(itemResult);
                }
            }

            if (arrayList.Count == 0)
            {
                return;
            }

            try
            {
                int[] array = new int[arrayList.Count];
                short[] array2 = new short[arrayList.Count];
                for (int j = 0; j < arrayList.Count; j++)
                {
                    ItemResult itemResult2 = (ItemResult)arrayList[j];
                    array[j] = System.Convert.ToInt32(itemResult2.ServerHandle);
                    array2[j] = (short)OpcCom.Interop.GetType(itemResult2.ReqType);
                }

                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCItemMgt)m_group).SetDatatypes(arrayList.Count, array, array2, out ppErrors);
                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, array.Length, deallocate: true);
                for (int k = 0; k < int32s.Length; k++)
                {
                    if (OpcCom.Interop.GetResultID(int32s[k]).Failed())
                    {
                        ItemResult itemResult3 = (ItemResult)arrayList[k];
                        itemResult3.ResultID = ResultID.Da.E_BADTYPE;
                        itemResult3.DiagnosticInfo = null;
                    }
                }
            }
            catch
            {
                for (int l = 0; l < arrayList.Count; l++)
                {
                    ItemResult itemResult4 = (ItemResult)arrayList[l];
                    itemResult4.ResultID = ResultID.Da.E_BADTYPE;
                    itemResult4.DiagnosticInfo = null;
                }
            }
        }

        private void SetActive(ItemResult[] items, bool active)
        {
            if (items == null || items.Length == 0)
            {
                return;
            }

            try
            {
                int[] array = new int[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = System.Convert.ToInt32(items[i].ServerHandle);
                }

                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCItemMgt)m_group).SetActiveState(items.Length, array, active ? 1 : 0, out ppErrors);
                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, array.Length, deallocate: true);
                for (int j = 0; j < int32s.Length; j++)
                {
                    if (OpcCom.Interop.GetResultID(int32s[j]).Failed())
                    {
                        items[j].Active = false;
                        items[j].ActiveSpecified = true;
                    }
                }
            }
            catch
            {
                for (int k = 0; k < items.Length; k++)
                {
                    items[k].Active = false;
                    items[k].ActiveSpecified = true;
                }
            }
        }

        private void UpdateActive(ItemResult[] items)
        {
            if (items == null || items.Length == 0)
            {
                return;
            }

            ArrayList arrayList = new ArrayList();
            ArrayList arrayList2 = new ArrayList();
            foreach (ItemResult itemResult in items)
            {
                if (itemResult.ResultID.Succeeded() && itemResult.ActiveSpecified)
                {
                    if (itemResult.Active)
                    {
                        arrayList.Add(itemResult);
                    }
                    else
                    {
                        arrayList2.Add(itemResult);
                    }
                }
            }

            SetActive((ItemResult[])arrayList.ToArray(typeof(ItemResult)), active: true);
            SetActive((ItemResult[])arrayList2.ToArray(typeof(ItemResult)), active: false);
        }

        private void SetDeadbands(ItemResult[] items)
        {
            if (items == null || items.Length == 0)
            {
                return;
            }

            try
            {
                int[] array = new int[items.Length];
                float[] array2 = new float[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = System.Convert.ToInt32(items[i].ServerHandle);
                    array2[i] = items[i].Deadband;
                }

                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCItemDeadbandMgt)m_group).SetItemDeadband(array.Length, array, array2, out ppErrors);
                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, array.Length, deallocate: true);
                for (int j = 0; j < int32s.Length; j++)
                {
                    if (OpcCom.Interop.GetResultID(int32s[j]).Failed())
                    {
                        items[j].Deadband = 0f;
                        items[j].DeadbandSpecified = false;
                    }
                }
            }
            catch
            {
                for (int k = 0; k < items.Length; k++)
                {
                    items[k].Deadband = 0f;
                    items[k].DeadbandSpecified = false;
                }
            }
        }

        private void ClearDeadbands(ItemResult[] items)
        {
            if (items == null || items.Length == 0)
            {
                return;
            }

            try
            {
                int[] array = new int[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = System.Convert.ToInt32(items[i].ServerHandle);
                }

                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCItemDeadbandMgt)m_group).ClearItemDeadband(array.Length, array, out ppErrors);
                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, array.Length, deallocate: true);
                for (int j = 0; j < int32s.Length; j++)
                {
                    if (OpcCom.Interop.GetResultID(int32s[j]).Failed())
                    {
                        items[j].Deadband = 0f;
                        items[j].DeadbandSpecified = false;
                    }
                }
            }
            catch
            {
                for (int k = 0; k < items.Length; k++)
                {
                    items[k].Deadband = 0f;
                    items[k].DeadbandSpecified = false;
                }
            }
        }

        private void UpdateDeadbands(ItemResult[] items)
        {
            if (items == null || items.Length == 0)
            {
                return;
            }

            ArrayList arrayList = new ArrayList();
            ArrayList arrayList2 = new ArrayList();
            foreach (ItemResult itemResult in items)
            {
                if (itemResult.ResultID.Succeeded())
                {
                    if (itemResult.DeadbandSpecified)
                    {
                        arrayList.Add(itemResult);
                    }
                    else
                    {
                        arrayList2.Add(itemResult);
                    }
                }
            }

            SetDeadbands((ItemResult[])arrayList.ToArray(typeof(ItemResult)));
            ClearDeadbands((ItemResult[])arrayList2.ToArray(typeof(ItemResult)));
        }

        private void SetSamplingRates(ItemResult[] items)
        {
            if (items == null || items.Length == 0)
            {
                return;
            }

            try
            {
                int[] array = new int[items.Length];
                int[] array2 = new int[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = System.Convert.ToInt32(items[i].ServerHandle);
                    array2[i] = items[i].SamplingRate;
                }

                IntPtr ppdwRevisedSamplingRate = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCItemSamplingMgt)m_group).SetItemSamplingRate(array.Length, array, array2, out ppdwRevisedSamplingRate, out ppErrors);
                int[] int32s = OpcCom.Interop.GetInt32s(ref ppdwRevisedSamplingRate, array.Length, deallocate: true);
                int[] int32s2 = OpcCom.Interop.GetInt32s(ref ppErrors, array.Length, deallocate: true);
                for (int j = 0; j < int32s2.Length; j++)
                {
                    if (items[j].SamplingRate != int32s[j])
                    {
                        items[j].SamplingRate = int32s[j];
                        items[j].SamplingRateSpecified = true;
                    }
                    else if (OpcCom.Interop.GetResultID(int32s2[j]).Failed())
                    {
                        items[j].SamplingRate = 0;
                        items[j].SamplingRateSpecified = false;
                    }
                }
            }
            catch
            {
                for (int k = 0; k < items.Length; k++)
                {
                    items[k].SamplingRate = 0;
                    items[k].SamplingRateSpecified = false;
                }
            }
        }

        private void ClearSamplingRates(ItemResult[] items)
        {
            if (items == null || items.Length == 0)
            {
                return;
            }

            try
            {
                int[] array = new int[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = System.Convert.ToInt32(items[i].ServerHandle);
                }

                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCItemSamplingMgt)m_group).ClearItemSamplingRate(array.Length, array, out ppErrors);
                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, array.Length, deallocate: true);
                for (int j = 0; j < int32s.Length; j++)
                {
                    if (OpcCom.Interop.GetResultID(int32s[j]).Failed())
                    {
                        items[j].SamplingRate = 0;
                        items[j].SamplingRateSpecified = false;
                    }
                }
            }
            catch
            {
                for (int k = 0; k < items.Length; k++)
                {
                    items[k].SamplingRate = 0;
                    items[k].SamplingRateSpecified = false;
                }
            }
        }

        private void UpdateSamplingRates(ItemResult[] items)
        {
            if (items == null || items.Length == 0)
            {
                return;
            }

            ArrayList arrayList = new ArrayList();
            ArrayList arrayList2 = new ArrayList();
            foreach (ItemResult itemResult in items)
            {
                if (itemResult.ResultID.Succeeded())
                {
                    if (itemResult.SamplingRateSpecified)
                    {
                        arrayList.Add(itemResult);
                    }
                    else
                    {
                        arrayList2.Add(itemResult);
                    }
                }
            }

            SetSamplingRates((ItemResult[])arrayList.ToArray(typeof(ItemResult)));
            ClearSamplingRates((ItemResult[])arrayList2.ToArray(typeof(ItemResult)));
        }

        private void SetEnableBuffering(ItemResult[] items)
        {
            if (items == null || items.Length == 0)
            {
                return;
            }

            ArrayList arrayList = new ArrayList();
            foreach (ItemResult itemResult in items)
            {
                if (itemResult.ResultID.Succeeded())
                {
                    arrayList.Add(itemResult);
                }
            }

            if (arrayList.Count == 0)
            {
                return;
            }

            try
            {
                int[] array = new int[arrayList.Count];
                int[] array2 = new int[arrayList.Count];
                for (int j = 0; j < arrayList.Count; j++)
                {
                    ItemResult itemResult2 = (ItemResult)arrayList[j];
                    array[j] = System.Convert.ToInt32(itemResult2.ServerHandle);
                    array2[j] = ((itemResult2.EnableBufferingSpecified && itemResult2.EnableBuffering) ? 1 : 0);
                }

                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCItemSamplingMgt)m_group).SetItemBufferEnable(array.Length, array, array2, out ppErrors);
                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, array.Length, deallocate: true);
                for (int k = 0; k < int32s.Length; k++)
                {
                    ItemResult itemResult3 = (ItemResult)arrayList[k];
                    if (OpcCom.Interop.GetResultID(int32s[k]).Failed())
                    {
                        itemResult3.EnableBuffering = false;
                        itemResult3.EnableBufferingSpecified = true;
                    }
                }
            }
            catch
            {
                foreach (ItemResult item in arrayList)
                {
                    item.EnableBuffering = false;
                    item.EnableBufferingSpecified = true;
                }
            }
        }

        private void Advise()
        {
            if (m_connection == null)
            {
                m_connection = new ConnectionPoint(m_group, typeof(IOPCDataCallback).GUID);
                m_connection.Advise(m_callback);
            }
        }

        private void Unadvise()
        {
            if (m_connection != null && m_connection.Unadvise() == 0)
            {
                m_connection.Dispose();
                m_connection = null;
            }
        }
    }

}
