using System;
using System.Collections;
using System.Runtime.InteropServices;
using Opc;
using Opc.Hda;
using OpcRcw.Hda;

namespace OpcCom.Hda
{
    public class Server : OpcCom.Server, Opc.Hda.IServer, Opc.IServer, IDisposable
    {
        private bool m_disposed;

        private static int NextHandle = 1;

        private Hashtable m_items = new Hashtable();

        private DataCallback m_callback = new DataCallback();

        private ConnectionPoint m_connection;

        internal Server()
        {
        }

        public Server(URL url, object server)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            m_url = (URL)url.Clone();
            m_server = server;
            Advise();
        }

        protected override void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                lock (this)
                {
                    if (disposing)
                    {
                        Unadvise();
                    }

                    m_disposed = true;
                }
            }

            base.Dispose(disposing);
        }

        public ServerStatus GetStatus()
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                _ = IntPtr.Zero;
                OPCHDA_SERVERSTATUS pwStatus = OPCHDA_SERVERSTATUS.OPCHDA_INDETERMINATE;
                IntPtr pftCurrentTime = IntPtr.Zero;
                IntPtr pftStartTime = IntPtr.Zero;
                short pwMajorVersion = 0;
                short wMinorVersion = 0;
                short pwBuildNumber = 0;
                int pdwMaxReturnValues = 0;
                string ppszStatusString = null;
                string ppszVendorInfo = null;
                try
                {
                    ((IOPCHDA_Server)m_server).GetHistorianStatus(out pwStatus, out pftCurrentTime, out pftStartTime, out pwMajorVersion, out wMinorVersion, out pwBuildNumber, out pdwMaxReturnValues, out ppszStatusString, out ppszVendorInfo);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Server.GetHistorianStatus", e);
                }

                ServerStatus serverStatus = new ServerStatus();
                serverStatus.VendorInfo = ppszVendorInfo;
                serverStatus.ProductVersion = $"{pwMajorVersion}.{wMinorVersion}.{pwBuildNumber}";
                serverStatus.ServerState = (ServerState)pwStatus;
                serverStatus.StatusInfo = ppszStatusString;
                serverStatus.StartTime = DateTime.MinValue;
                serverStatus.CurrentTime = DateTime.MinValue;
                serverStatus.MaxReturnValues = pdwMaxReturnValues;
                if (pftStartTime != IntPtr.Zero)
                {
                    serverStatus.StartTime = OpcCom.Interop.GetFILETIME(pftStartTime);
                    Marshal.FreeCoTaskMem(pftStartTime);
                }

                if (pftCurrentTime != IntPtr.Zero)
                {
                    serverStatus.CurrentTime = OpcCom.Interop.GetFILETIME(pftCurrentTime);
                    Marshal.FreeCoTaskMem(pftCurrentTime);
                }

                return serverStatus;
            }
        }

        public Opc.Hda.Attribute[] GetAttributes()
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                int pdwCount = 0;
                IntPtr ppdwAttrID = IntPtr.Zero;
                IntPtr ppszAttrName = IntPtr.Zero;
                IntPtr ppszAttrDesc = IntPtr.Zero;
                IntPtr ppvtAttrDataType = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Server)m_server).GetItemAttributes(out pdwCount, out ppdwAttrID, out ppszAttrName, out ppszAttrDesc, out ppvtAttrDataType);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Server.GetItemAttributes", e);
                }

                if (pdwCount == 0)
                {
                    return new Opc.Hda.Attribute[0];
                }

                int[] int32s = OpcCom.Interop.GetInt32s(ref ppdwAttrID, pdwCount, deallocate: true);
                string[] unicodeStrings = OpcCom.Interop.GetUnicodeStrings(ref ppszAttrName, pdwCount, deallocate: true);
                string[] unicodeStrings2 = OpcCom.Interop.GetUnicodeStrings(ref ppszAttrDesc, pdwCount, deallocate: true);
                short[] int16s = OpcCom.Interop.GetInt16s(ref ppvtAttrDataType, pdwCount, deallocate: true);
                if (int32s == null || unicodeStrings == null || unicodeStrings2 == null || int16s == null)
                {
                    throw new InvalidResponseException();
                }

                Opc.Hda.Attribute[] array = new Opc.Hda.Attribute[pdwCount];
                for (int i = 0; i < pdwCount; i++)
                {
                    array[i] = new Opc.Hda.Attribute();
                    array[i].ID = int32s[i];
                    array[i].Name = unicodeStrings[i];
                    array[i].Description = unicodeStrings2[i];
                    array[i].DataType = OpcCom.Interop.GetType((VarEnum)Enum.ToObject(typeof(VarEnum), int16s[i]));
                }

                return array;
            }
        }

        public Aggregate[] GetAggregates()
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                int pdwCount = 0;
                IntPtr ppdwAggrID = IntPtr.Zero;
                IntPtr ppszAggrName = IntPtr.Zero;
                IntPtr ppszAggrDesc = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Server)m_server).GetAggregates(out pdwCount, out ppdwAggrID, out ppszAggrName, out ppszAggrDesc);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Server.GetAggregates", e);
                }

                if (pdwCount == 0)
                {
                    return new Aggregate[0];
                }

                int[] int32s = OpcCom.Interop.GetInt32s(ref ppdwAggrID, pdwCount, deallocate: true);
                string[] unicodeStrings = OpcCom.Interop.GetUnicodeStrings(ref ppszAggrName, pdwCount, deallocate: true);
                string[] unicodeStrings2 = OpcCom.Interop.GetUnicodeStrings(ref ppszAggrDesc, pdwCount, deallocate: true);
                if (int32s == null || unicodeStrings == null || unicodeStrings2 == null)
                {
                    throw new InvalidResponseException();
                }

                Aggregate[] array = new Aggregate[pdwCount];
                for (int i = 0; i < pdwCount; i++)
                {
                    array[i] = new Aggregate();
                    array[i].ID = int32s[i];
                    array[i].Name = unicodeStrings[i];
                    array[i].Description = unicodeStrings2[i];
                }

                return array;
            }
        }

        public IBrowser CreateBrowser(BrowseFilter[] filters, out ResultID[] results)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                int num = (filters != null) ? filters.Length : 0;
                int[] array = new int[num];
                object[] array2 = new object[num];
                OPCHDA_OPERATORCODES[] array3 = new OPCHDA_OPERATORCODES[num];
                for (int i = 0; i < num; i++)
                {
                    array[i] = filters[i].AttributeID;
                    array3[i] = (OPCHDA_OPERATORCODES)Enum.ToObject(typeof(OPCHDA_OPERATORCODES), filters[i].Operator);
                    array2[i] = OpcCom.Interop.GetVARIANT(filters[i].FilterValue);
                }

                IOPCHDA_Browser pphBrowser = null;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Server)m_server).CreateBrowse(num, array, array3, array2, out pphBrowser, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Server.CreateBrowse", e);
                }

                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, num, deallocate: true);
                if ((num > 0 && int32s == null) || pphBrowser == null)
                {
                    throw new InvalidResponseException();
                }

                results = new ResultID[num];
                for (int j = 0; j < num; j++)
                {
                    ref ResultID reference = ref results[j];
                    reference = OpcCom.Interop.GetResultID(int32s[j]);
                }

                return new Browser(this, pphBrowser, filters, results);
            }
        }

        public IdentifiedResult[] CreateItems(ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                string[] array = new string[items.Length];
                int[] array2 = new int[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null)
                    {
                        array[i] = items[i].ItemName;
                        array2[i] = CreateHandle();
                    }
                }

                IntPtr pphServer = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Server)m_server).GetItemHandles(items.Length, array, array2, out pphServer, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Server.GetItemHandles", e);
                }

                int[] int32s = OpcCom.Interop.GetInt32s(ref pphServer, items.Length, deallocate: true);
                int[] int32s2 = OpcCom.Interop.GetInt32s(ref ppErrors, items.Length, deallocate: true);
                if (int32s == null || int32s2 == null)
                {
                    throw new InvalidResponseException();
                }

                IdentifiedResult[] array3 = new IdentifiedResult[items.Length];
                for (int j = 0; j < array3.Length; j++)
                {
                    array3[j] = new IdentifiedResult(items[j]);
                    array3[j].ResultID = OpcCom.Interop.GetResultID(int32s2[j]);
                    if (array3[j].ResultID.Succeeded())
                    {
                        ItemIdentifier itemIdentifier = new ItemIdentifier();
                        itemIdentifier.ItemName = items[j].ItemName;
                        itemIdentifier.ItemPath = items[j].ItemPath;
                        itemIdentifier.ServerHandle = int32s[j];
                        itemIdentifier.ClientHandle = items[j].ClientHandle;
                        m_items.Add(array2[j], itemIdentifier);
                        array3[j].ServerHandle = array2[j];
                        array3[j].ClientHandle = items[j].ClientHandle;
                    }
                }

                return array3;
            }
        }

        public IdentifiedResult[] ReleaseItems(ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                int[] serverHandles = GetServerHandles(items);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Server)m_server).ReleaseItemHandles(items.Length, serverHandles, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Server.ReleaseItemHandles", e);
                }

                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, items.Length, deallocate: true);
                if (int32s == null)
                {
                    throw new InvalidResponseException();
                }

                IdentifiedResult[] array = new IdentifiedResult[items.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = new IdentifiedResult(items[i]);
                    array[i].ResultID = OpcCom.Interop.GetResultID(int32s[i]);
                    if (array[i].ResultID.Succeeded() && items[i].ServerHandle != null)
                    {
                        ItemIdentifier itemIdentifier = (ItemIdentifier)m_items[items[i].ServerHandle];
                        if (itemIdentifier != null)
                        {
                            array[i].ItemName = itemIdentifier.ItemName;
                            array[i].ItemPath = itemIdentifier.ItemPath;
                            array[i].ClientHandle = itemIdentifier.ClientHandle;
                            m_items.Remove(items[i].ServerHandle);
                        }
                    }
                }

                return array;
            }
        }

        public IdentifiedResult[] ValidateItems(ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                string[] array = new string[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null)
                    {
                        array[i] = items[i].ItemName;
                    }
                }

                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Server)m_server).ValidateItemIDs(items.Length, array, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Server.ValidateItemIDs", e);
                }

                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, items.Length, deallocate: true);
                if (int32s == null)
                {
                    throw new InvalidResponseException();
                }

                IdentifiedResult[] array2 = new IdentifiedResult[items.Length];
                for (int j = 0; j < array2.Length; j++)
                {
                    array2[j] = new IdentifiedResult(items[j]);
                    array2[j].ResultID = OpcCom.Interop.GetResultID(int32s[j]);
                }

                return array2;
            }
        }

        public ItemValueCollection[] ReadRaw(Time startTime, Time endTime, int maxValues, bool includeBounds, ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new ItemValueCollection[0];
                }

                int[] serverHandles = GetServerHandles(items);
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = Interop.GetTime(endTime);
                IntPtr ppItemValues = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncRead)m_server).ReadRaw(ref htStartTime, ref htEndTime, maxValues, includeBounds ? 1 : 0, serverHandles.Length, serverHandles, out ppItemValues, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncRead.ReadRaw", e);
                }

                ItemValueCollection[] itemValueCollections = Interop.GetItemValueCollections(ref ppItemValues, items.Length, deallocate: true);
                UpdateResults(items, itemValueCollections, ref ppErrors);
                UpdateActualTimes(itemValueCollections, htStartTime, htEndTime);
                return itemValueCollections;
            }
        }

        public IdentifiedResult[] ReadRaw(Time startTime, Time endTime, int maxValues, bool includeBounds, ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
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
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                Request request2 = m_callback.CreateRequest(requestHandle, callback);
                _ = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = GetServerHandles(items);
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = Interop.GetTime(endTime);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncRead)m_server).ReadRaw(request2.RequestID, ref htStartTime, ref htEndTime, maxValues, includeBounds ? 1 : 0, serverHandles.Length, serverHandles, out pdwCancelID, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.ReadRaw", e);
                }

                IdentifiedResult[] array = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = new IdentifiedResult();
                }

                UpdateResults(items, array, ref ppErrors);
                if (request2.Update(pdwCancelID, array))
                {
                    request = null;
                    m_callback.CancelRequest(request2, null);
                    return array;
                }

                UpdateActualTimes(new IActualTime[1]
                {
                    request2
                }, htStartTime, htEndTime);
                request = request2;
                return array;
            }
        }

        public IdentifiedResult[] AdviseRaw(Time startTime, decimal updateInterval, ItemIdentifier[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
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
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                Request request2 = m_callback.CreateRequest(requestHandle, callback);
                _ = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = GetServerHandles(items);
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_FILETIME fILETIME = Interop.GetFILETIME(updateInterval);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncRead)m_server).AdviseRaw(request2.RequestID, ref htStartTime, fILETIME, serverHandles.Length, serverHandles, out pdwCancelID, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.AdviseRaw", e);
                }

                IdentifiedResult[] array = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = new IdentifiedResult();
                }

                UpdateResults(items, array, ref ppErrors);
                request2.Update(pdwCancelID, array);
                request = request2;
                return array;
            }
        }

        public IdentifiedResult[] PlaybackRaw(Time startTime, Time endTime, int maxValues, decimal updateInterval, decimal playbackDuration, ItemIdentifier[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
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
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                Request request2 = m_callback.CreateRequest(requestHandle, callback);
                _ = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = GetServerHandles(items);
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = Interop.GetTime(endTime);
                OPCHDA_FILETIME fILETIME = Interop.GetFILETIME(updateInterval);
                OPCHDA_FILETIME fILETIME2 = Interop.GetFILETIME(playbackDuration);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Playback)m_server).ReadRawWithUpdate(request2.RequestID, ref htStartTime, ref htEndTime, maxValues, fILETIME2, fILETIME, serverHandles.Length, serverHandles, out pdwCancelID, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Playback.ReadRawWithUpdate", e);
                }

                IdentifiedResult[] array = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = new IdentifiedResult();
                }

                UpdateResults(items, array, ref ppErrors);
                request2.Update(pdwCancelID, array);
                request = request2;
                return array;
            }
        }

        public ItemValueCollection[] ReadProcessed(Time startTime, Time endTime, decimal resampleInterval, Item[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new ItemValueCollection[0];
                }

                int[] serverHandles = GetServerHandles(items);
                int[] aggregateIDs = GetAggregateIDs(items);
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = Interop.GetTime(endTime);
                OPCHDA_FILETIME fILETIME = Interop.GetFILETIME(resampleInterval);
                IntPtr ppItemValues = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncRead)m_server).ReadProcessed(ref htStartTime, ref htEndTime, fILETIME, serverHandles.Length, serverHandles, aggregateIDs, out ppItemValues, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncRead.ReadProcessed", e);
                }

                ItemValueCollection[] itemValueCollections = Interop.GetItemValueCollections(ref ppItemValues, items.Length, deallocate: true);
                UpdateResults(items, itemValueCollections, ref ppErrors);
                UpdateActualTimes(itemValueCollections, htStartTime, htEndTime);
                return itemValueCollections;
            }
        }

        public IdentifiedResult[] ReadProcessed(Time startTime, Time endTime, decimal resampleInterval, Item[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
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
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                Request request2 = m_callback.CreateRequest(requestHandle, callback);
                _ = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = GetServerHandles(items);
                int[] aggregateIDs = GetAggregateIDs(items);
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = Interop.GetTime(endTime);
                OPCHDA_FILETIME fILETIME = Interop.GetFILETIME(resampleInterval);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncRead)m_server).ReadProcessed(request2.RequestID, ref htStartTime, ref htEndTime, fILETIME, serverHandles.Length, serverHandles, aggregateIDs, out pdwCancelID, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.ReadProcessed", e);
                }

                IdentifiedResult[] array = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = new IdentifiedResult();
                }

                UpdateResults(items, array, ref ppErrors);
                if (request2.Update(pdwCancelID, array))
                {
                    request = null;
                    m_callback.CancelRequest(request2, null);
                    return array;
                }

                UpdateActualTimes(new IActualTime[1]
                {
                    request2
                }, htStartTime, htEndTime);
                request = request2;
                return array;
            }
        }

        public IdentifiedResult[] AdviseProcessed(Time startTime, decimal resampleInterval, int numberOfIntervals, Item[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
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
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                Request request2 = m_callback.CreateRequest(requestHandle, callback);
                _ = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = GetServerHandles(items);
                int[] aggregateIDs = GetAggregateIDs(items);
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_FILETIME fILETIME = Interop.GetFILETIME(resampleInterval);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncRead)m_server).AdviseProcessed(request2.RequestID, ref htStartTime, fILETIME, serverHandles.Length, serverHandles, aggregateIDs, numberOfIntervals, out pdwCancelID, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.AdviseProcessed", e);
                }

                IdentifiedResult[] array = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = new IdentifiedResult();
                }

                UpdateResults(items, array, ref ppErrors);
                request2.Update(pdwCancelID, array);
                request = request2;
                return array;
            }
        }

        public IdentifiedResult[] PlaybackProcessed(Time startTime, Time endTime, decimal resampleInterval, int numberOfIntervals, decimal updateInterval, Item[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request)
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
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                Request request2 = m_callback.CreateRequest(requestHandle, callback);
                _ = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = GetServerHandles(items);
                int[] aggregateIDs = GetAggregateIDs(items);
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = Interop.GetTime(endTime);
                OPCHDA_FILETIME fILETIME = Interop.GetFILETIME(resampleInterval);
                OPCHDA_FILETIME fILETIME2 = Interop.GetFILETIME(updateInterval);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_Playback)m_server).ReadProcessedWithUpdate(request2.RequestID, ref htStartTime, ref htEndTime, fILETIME, numberOfIntervals, fILETIME2, serverHandles.Length, serverHandles, aggregateIDs, out pdwCancelID, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Playback.ReadProcessedWithUpdate", e);
                }

                IdentifiedResult[] array = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = new IdentifiedResult();
                }

                UpdateResults(items, array, ref ppErrors);
                request2.Update(pdwCancelID, array);
                request = request2;
                return array;
            }
        }

        public ItemValueCollection[] ReadAtTime(DateTime[] timestamps, ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new ItemValueCollection[0];
                }

                int[] serverHandles = GetServerHandles(items);
                OPCHDA_FILETIME[] fILETIMEs = Interop.GetFILETIMEs(timestamps);
                IntPtr ppItemValues = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncRead)m_server).ReadAtTime(fILETIMEs.Length, fILETIMEs, serverHandles.Length, serverHandles, out ppItemValues, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncRead.ReadAtTime", e);
                }

                ItemValueCollection[] itemValueCollections = Interop.GetItemValueCollections(ref ppItemValues, items.Length, deallocate: true);
                UpdateResults(items, itemValueCollections, ref ppErrors);
                return itemValueCollections;
            }
        }

        public IdentifiedResult[] ReadAtTime(DateTime[] timestamps, ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
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
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                Request request2 = m_callback.CreateRequest(requestHandle, callback);
                _ = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = GetServerHandles(items);
                OPCHDA_FILETIME[] fILETIMEs = Interop.GetFILETIMEs(timestamps);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncRead)m_server).ReadAtTime(request2.RequestID, fILETIMEs.Length, fILETIMEs, serverHandles.Length, serverHandles, out pdwCancelID, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.ReadAtTime", e);
                }

                IdentifiedResult[] array = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = new IdentifiedResult();
                }

                UpdateResults(items, array, ref ppErrors);
                if (request2.Update(pdwCancelID, array))
                {
                    request = null;
                    m_callback.CancelRequest(request2, null);
                    return array;
                }

                request = request2;
                return array;
            }
        }

        public ModifiedValueCollection[] ReadModified(Time startTime, Time endTime, int maxValues, ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new ModifiedValueCollection[0];
                }

                int[] serverHandles = GetServerHandles(items);
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = Interop.GetTime(endTime);
                IntPtr ppItemValues = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncRead)m_server).ReadModified(ref htStartTime, ref htEndTime, maxValues, serverHandles.Length, serverHandles, out ppItemValues, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncRead.ReadModified", e);
                }

                ModifiedValueCollection[] modifiedValueCollections = Interop.GetModifiedValueCollections(ref ppItemValues, items.Length, deallocate: true);
                UpdateResults(items, modifiedValueCollections, ref ppErrors);
                UpdateActualTimes(modifiedValueCollections, htStartTime, htEndTime);
                return modifiedValueCollections;
            }
        }

        public IdentifiedResult[] ReadModified(Time startTime, Time endTime, int maxValues, ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request)
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
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                Request request2 = m_callback.CreateRequest(requestHandle, callback);
                _ = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = GetServerHandles(items);
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = Interop.GetTime(endTime);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncRead)m_server).ReadModified(request2.RequestID, ref htStartTime, ref htEndTime, maxValues, serverHandles.Length, serverHandles, out pdwCancelID, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.ReadModified", e);
                }

                IdentifiedResult[] array = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = new IdentifiedResult();
                }

                UpdateResults(items, array, ref ppErrors);
                if (request2.Update(pdwCancelID, array))
                {
                    request = null;
                    m_callback.CancelRequest(request2, null);
                    return array;
                }

                UpdateActualTimes(new IActualTime[1]
                {
                    request2
                }, htStartTime, htEndTime);
                request = request2;
                return array;
            }
        }

        public ItemAttributeCollection ReadAttributes(Time startTime, Time endTime, ItemIdentifier item, int[] attributeIDs)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (attributeIDs == null)
            {
                throw new ArgumentNullException("attributeIDs");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (attributeIDs.Length == 0)
                {
                    return new ItemAttributeCollection(item);
                }

                int[] serverHandles = GetServerHandles(new ItemIdentifier[1]
                {
                    item
                });
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = Interop.GetTime(endTime);
                IntPtr ppAttributeValues = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncRead)m_server).ReadAttribute(ref htStartTime, ref htEndTime, serverHandles[0], attributeIDs.Length, attributeIDs, out ppAttributeValues, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncRead.ReadAttribute", e);
                }

                AttributeValueCollection[] attributeValueCollections = Interop.GetAttributeValueCollections(ref ppAttributeValues, attributeIDs.Length, deallocate: true);
                ItemAttributeCollection itemAttributeCollection = UpdateResults(item, attributeValueCollections, ref ppErrors);
                UpdateActualTimes(new IActualTime[1]
                {
                    itemAttributeCollection
                }, htStartTime, htEndTime);
                return itemAttributeCollection;
            }
        }

        public ResultCollection ReadAttributes(Time startTime, Time endTime, ItemIdentifier item, int[] attributeIDs, object requestHandle, ReadAttributesEventHandler callback, out IRequest request)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (attributeIDs == null)
            {
                throw new ArgumentNullException("attributeIDs");
            }

            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            request = null;
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (attributeIDs.Length == 0)
                {
                    return new ResultCollection();
                }

                Request request2 = m_callback.CreateRequest(requestHandle, callback);
                _ = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = GetServerHandles(new ItemIdentifier[1]
                {
                    item
                });
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = Interop.GetTime(endTime);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncRead)m_server).ReadAttribute(request2.RequestID, ref htStartTime, ref htEndTime, serverHandles[0], attributeIDs.Length, attributeIDs, out pdwCancelID, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.ReadAttribute", e);
                }

                ResultCollection resultCollection = new ResultCollection(item);
                UpdateResult(item, resultCollection, 0);
                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, attributeIDs.Length, deallocate: true);
                if (int32s == null)
                {
                    throw new InvalidResponseException();
                }

                int[] array = int32s;
                foreach (int input in array)
                {
                    Result value = new Result(OpcCom.Interop.GetResultID(input));
                    resultCollection.Add(value);
                }

                if (request2.Update(pdwCancelID, new ResultCollection[1]
                {
                    resultCollection
                }))
                {
                    request = null;
                    m_callback.CancelRequest(request2, null);
                    return resultCollection;
                }

                UpdateActualTimes(new IActualTime[1]
                {
                    request2
                }, htStartTime, htEndTime);
                request = request2;
                return resultCollection;
            }
        }

        public AnnotationValueCollection[] ReadAnnotations(Time startTime, Time endTime, ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new AnnotationValueCollection[0];
                }

                int[] serverHandles = GetServerHandles(items);
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = Interop.GetTime(endTime);
                IntPtr ppAnnotationValues = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncAnnotations)m_server).Read(ref htStartTime, ref htEndTime, serverHandles.Length, serverHandles, out ppAnnotationValues, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncAnnotations.Read", e);
                }

                AnnotationValueCollection[] annotationValueCollections = Interop.GetAnnotationValueCollections(ref ppAnnotationValues, items.Length, deallocate: true);
                UpdateResults(items, annotationValueCollections, ref ppErrors);
                UpdateActualTimes(annotationValueCollections, htStartTime, htEndTime);
                return annotationValueCollections;
            }
        }

        public IdentifiedResult[] ReadAnnotations(Time startTime, Time endTime, ItemIdentifier[] items, object requestHandle, ReadAnnotationsEventHandler callback, out IRequest request)
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
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                Request request2 = m_callback.CreateRequest(requestHandle, callback);
                _ = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = GetServerHandles(items);
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = Interop.GetTime(endTime);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncAnnotations)m_server).Read(request2.RequestID, ref htStartTime, ref htEndTime, serverHandles.Length, serverHandles, out pdwCancelID, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncAnnotations.Read", e);
                }

                IdentifiedResult[] array = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = new IdentifiedResult();
                }

                UpdateResults(items, array, ref ppErrors);
                if (request2.Update(pdwCancelID, array))
                {
                    request = null;
                    m_callback.CancelRequest(request2, null);
                    return array;
                }

                UpdateActualTimes(new IActualTime[1]
                {
                    request2
                }, htStartTime, htEndTime);
                request = request2;
                return array;
            }
        }

        public ResultCollection[] InsertAnnotations(AnnotationValueCollection[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new ResultCollection[0];
                }

                ResultCollection[] array = CreateResultCollections(items);
                int[] serverHandles = null;
                OPCHDA_ANNOTATION[] annotations = null;
                OPCHDA_FILETIME[] ftTimestamps = null;
                int num = MarshalAnnotatations(items, ref serverHandles, ref ftTimestamps, ref annotations);
                if (num == 0)
                {
                    return array;
                }

                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncAnnotations)m_server).Insert(serverHandles.Length, serverHandles, ftTimestamps, annotations, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncAnnotations.Insert", e);
                }

                for (int i = 0; i < annotations.Length; i++)
                {
                    OpcCom.Interop.GetFILETIMEs(ref annotations[i].ftTimeStamps, 1, deallocate: true);
                    OpcCom.Interop.GetUnicodeStrings(ref annotations[i].szAnnotation, 1, deallocate: true);
                    OpcCom.Interop.GetFILETIMEs(ref annotations[i].ftAnnotationTime, 1, deallocate: true);
                    OpcCom.Interop.GetUnicodeStrings(ref annotations[i].szUser, 1, deallocate: true);
                }

                UpdateResults(items, array, num, ref ppErrors);
                return array;
            }
        }

        public IdentifiedResult[] InsertAnnotations(AnnotationValueCollection[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            request = null;
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                ResultCollection[] results = CreateResultCollections(items);
                int[] serverHandles = null;
                OPCHDA_ANNOTATION[] annotations = null;
                OPCHDA_FILETIME[] ftTimestamps = null;
                int num = MarshalAnnotatations(items, ref serverHandles, ref ftTimestamps, ref annotations);
                if (num == 0)
                {
                    return GetIdentifiedResults(results);
                }

                Request request2 = m_callback.CreateRequest(requestHandle, callback);
                IntPtr ppErrors = IntPtr.Zero;
                int pdwCancelID = 0;
                try
                {
                    ((IOPCHDA_AsyncAnnotations)m_server).Insert(request2.RequestID, serverHandles.Length, serverHandles, ftTimestamps, annotations, out pdwCancelID, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncAnnotations.Insert", e);
                }

                for (int i = 0; i < annotations.Length; i++)
                {
                    OpcCom.Interop.GetFILETIMEs(ref annotations[i].ftTimeStamps, 1, deallocate: true);
                    OpcCom.Interop.GetUnicodeStrings(ref annotations[i].szAnnotation, 1, deallocate: true);
                    OpcCom.Interop.GetFILETIMEs(ref annotations[i].ftAnnotationTime, 1, deallocate: true);
                    OpcCom.Interop.GetUnicodeStrings(ref annotations[i].szUser, 1, deallocate: true);
                }

                UpdateResults(items, results, num, ref ppErrors);
                if (request2.Update(pdwCancelID, results))
                {
                    request = null;
                    m_callback.CancelRequest(request2, null);
                    return GetIdentifiedResults(results);
                }

                request = request2;
                return GetIdentifiedResults(results);
            }
        }

        public ResultCollection[] Insert(ItemValueCollection[] items, bool replace)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new ResultCollection[0];
                }

                ResultCollection[] array = CreateResultCollections(items);
                int[] handles = null;
                object[] values = null;
                int[] qualities = null;
                DateTime[] timestamps = null;
                int num = MarshalValues(items, ref handles, ref values, ref qualities, ref timestamps);
                if (num == 0)
                {
                    return array;
                }

                OPCHDA_FILETIME[] fILETIMEs = Interop.GetFILETIMEs(timestamps);
                IntPtr ppErrors = IntPtr.Zero;
                if (replace)
                {
                    try
                    {
                        ((IOPCHDA_SyncUpdate)m_server).InsertReplace(handles.Length, handles, fILETIMEs, values, qualities, out ppErrors);
                    }
                    catch (Exception e)
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_SyncUpdate.InsertReplace", e);
                    }
                }
                else
                {
                    try
                    {
                        ((IOPCHDA_SyncUpdate)m_server).Insert(handles.Length, handles, fILETIMEs, values, qualities, out ppErrors);
                    }
                    catch (Exception e2)
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_SyncUpdate.Insert", e2);
                    }
                }

                UpdateResults(items, array, num, ref ppErrors);
                return array;
            }
        }

        public IdentifiedResult[] Insert(ItemValueCollection[] items, bool replace, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            request = null;
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                ResultCollection[] results = CreateResultCollections(items);
                int[] handles = null;
                object[] values = null;
                int[] qualities = null;
                DateTime[] timestamps = null;
                int num = MarshalValues(items, ref handles, ref values, ref qualities, ref timestamps);
                if (num == 0)
                {
                    return GetIdentifiedResults(results);
                }

                OPCHDA_FILETIME[] fILETIMEs = Interop.GetFILETIMEs(timestamps);
                Request request2 = m_callback.CreateRequest(requestHandle, callback);
                IntPtr ppErrors = IntPtr.Zero;
                int pdwCancelID = 0;
                if (replace)
                {
                    try
                    {
                        ((IOPCHDA_AsyncUpdate)m_server).InsertReplace(request2.RequestID, handles.Length, handles, fILETIMEs, values, qualities, out pdwCancelID, out ppErrors);
                    }
                    catch (Exception e)
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_AsyncUpdate.InsertReplace", e);
                    }
                }
                else
                {
                    try
                    {
                        ((IOPCHDA_AsyncUpdate)m_server).Insert(request2.RequestID, handles.Length, handles, fILETIMEs, values, qualities, out pdwCancelID, out ppErrors);
                    }
                    catch (Exception e2)
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_AsyncUpdate.Insert", e2);
                    }
                }

                UpdateResults(items, results, num, ref ppErrors);
                if (request2.Update(pdwCancelID, results))
                {
                    request = null;
                    m_callback.CancelRequest(request2, null);
                    return GetIdentifiedResults(results);
                }

                request = request2;
                return GetIdentifiedResults(results);
            }
        }

        public ResultCollection[] Replace(ItemValueCollection[] items)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new ResultCollection[0];
                }

                ResultCollection[] array = CreateResultCollections(items);
                int[] handles = null;
                object[] values = null;
                int[] qualities = null;
                DateTime[] timestamps = null;
                int num = MarshalValues(items, ref handles, ref values, ref qualities, ref timestamps);
                if (num == 0)
                {
                    return array;
                }

                OPCHDA_FILETIME[] fILETIMEs = Interop.GetFILETIMEs(timestamps);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncUpdate)m_server).Replace(handles.Length, handles, fILETIMEs, values, qualities, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncUpdate.Replace", e);
                }

                UpdateResults(items, array, num, ref ppErrors);
                return array;
            }
        }

        public IdentifiedResult[] Replace(ItemValueCollection[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            request = null;
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                ResultCollection[] results = CreateResultCollections(items);
                int[] handles = null;
                object[] values = null;
                int[] qualities = null;
                DateTime[] timestamps = null;
                int num = MarshalValues(items, ref handles, ref values, ref qualities, ref timestamps);
                if (num == 0)
                {
                    return GetIdentifiedResults(results);
                }

                OPCHDA_FILETIME[] fILETIMEs = Interop.GetFILETIMEs(timestamps);
                Request request2 = m_callback.CreateRequest(requestHandle, callback);
                IntPtr ppErrors = IntPtr.Zero;
                int pdwCancelID = 0;
                try
                {
                    ((IOPCHDA_AsyncUpdate)m_server).Replace(request2.RequestID, handles.Length, handles, fILETIMEs, values, qualities, out pdwCancelID, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncUpdate.Replace", e);
                }

                UpdateResults(items, results, num, ref ppErrors);
                if (request2.Update(pdwCancelID, results))
                {
                    request = null;
                    m_callback.CancelRequest(request2, null);
                    return GetIdentifiedResults(results);
                }

                request = request2;
                return GetIdentifiedResults(results);
            }
        }

        public IdentifiedResult[] Delete(Time startTime, Time endTime, ItemIdentifier[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                int[] serverHandles = GetServerHandles(items);
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = Interop.GetTime(endTime);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncUpdate)m_server).DeleteRaw(ref htStartTime, ref htEndTime, serverHandles.Length, serverHandles, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncUpdate.DeleteRaw", e);
                }

                IdentifiedResult[] array = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = new IdentifiedResult();
                }

                UpdateResults(items, array, ref ppErrors);
                return array;
            }
        }

        public IdentifiedResult[] Delete(Time startTime, Time endTime, ItemIdentifier[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
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
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                Request request2 = m_callback.CreateRequest(requestHandle, callback);
                _ = request2.RequestID;
                int pdwCancelID = 0;
                int[] serverHandles = GetServerHandles(items);
                OPCHDA_TIME htStartTime = Interop.GetTime(startTime);
                OPCHDA_TIME htEndTime = Interop.GetTime(endTime);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_AsyncUpdate)m_server).DeleteRaw(request2.RequestID, ref htStartTime, ref htEndTime, serverHandles.Length, serverHandles, out pdwCancelID, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncUpdate.DeleteRaw", e);
                }

                IdentifiedResult[] array = new IdentifiedResult[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = new IdentifiedResult();
                }

                UpdateResults(items, array, ref ppErrors);
                if (request2.Update(pdwCancelID, array))
                {
                    request = null;
                    m_callback.CancelRequest(request2, null);
                    return array;
                }

                UpdateActualTimes(new IActualTime[1]
                {
                    request2
                }, htStartTime, htEndTime);
                request = request2;
                return array;
            }
        }

        public ResultCollection[] DeleteAtTime(ItemTimeCollection[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new ResultCollection[0];
                }

                ResultCollection[] array = CreateResultCollections(items);
                int[] handles = null;
                DateTime[] timestamps = null;
                int num = MarshalTimestamps(items, ref handles, ref timestamps);
                if (num == 0)
                {
                    return array;
                }

                OPCHDA_FILETIME[] fILETIMEs = Interop.GetFILETIMEs(timestamps);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCHDA_SyncUpdate)m_server).DeleteAtTime(handles.Length, handles, fILETIMEs, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_SyncUpdate.DeleteAtTime", e);
                }

                UpdateResults(items, array, num, ref ppErrors);
                return array;
            }
        }

        public IdentifiedResult[] DeleteAtTime(ItemTimeCollection[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            request = null;
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (items.Length == 0)
                {
                    return new IdentifiedResult[0];
                }

                ResultCollection[] results = CreateResultCollections(items);
                int[] handles = null;
                DateTime[] timestamps = null;
                int num = MarshalTimestamps(items, ref handles, ref timestamps);
                if (num == 0)
                {
                    return GetIdentifiedResults(results);
                }

                OPCHDA_FILETIME[] fILETIMEs = Interop.GetFILETIMEs(timestamps);
                Request request2 = m_callback.CreateRequest(requestHandle, callback);
                IntPtr ppErrors = IntPtr.Zero;
                int pdwCancelID = 0;
                try
                {
                    ((IOPCHDA_AsyncUpdate)m_server).DeleteAtTime(request2.RequestID, handles.Length, handles, fILETIMEs, out pdwCancelID, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_AsyncUpdate.DeleteAtTime", e);
                }

                UpdateResults(items, results, num, ref ppErrors);
                if (request2.Update(pdwCancelID, results))
                {
                    request = null;
                    m_callback.CancelRequest(request2, null);
                    return GetIdentifiedResults(results);
                }

                request = request2;
                return GetIdentifiedResults(results);
            }
        }

        public void CancelRequest(IRequest request)
        {
            CancelRequest(request, null);
        }

        public void CancelRequest(IRequest request, CancelCompleteEventHandler callback)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                Request request2 = (Request)request;
                m_callback.CancelRequest(request2, callback);
                try
                {
                    ((IOPCHDA_AsyncRead)m_server).Cancel(request2.CancelID);
                }
                catch (Exception e)
                {
                    if (-2147467259 != Marshal.GetHRForException(e))
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_AsyncRead.Cancel", e);
                    }
                }
            }
        }

        private void Advise()
        {
            if (m_connection == null)
            {
                try
                {
                    m_connection = new ConnectionPoint(m_server, typeof(IOPCHDA_DataCallback).GUID);
                    m_connection.Advise(m_callback);
                }
                catch
                {
                    m_connection = null;
                }
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

        private int CreateHandle()
        {
            return NextHandle++;
        }

        private int GetInvalidHandle()
        {
            int num = 0;
            foreach (ItemIdentifier value in m_items.Values)
            {
                int num2 = (int)value.ServerHandle;
                if (num < num2)
                {
                    num = num2;
                }
            }

            return num + 1;
        }

        private int GetCount(ICollection[] collections)
        {
            int num = 0;
            if (collections != null)
            {
                foreach (ICollection collection in collections)
                {
                    if (collection != null)
                    {
                        num += collection.Count;
                    }
                }
            }

            return num;
        }

        private ResultCollection[] CreateResultCollections(ItemIdentifier[] items)
        {
            ResultCollection[] array = null;
            if (items != null)
            {
                array = new ResultCollection[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = new ResultCollection();
                    if (items[i] != null)
                    {
                        UpdateResult(items[i], array[i], 0);
                    }
                }
            }

            return array;
        }

        private int[] GetServerHandles(ItemIdentifier[] items)
        {
            int invalidHandle = GetInvalidHandle();
            int[] array = new int[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                array[i] = invalidHandle;
                if (items[i] != null && items[i].ServerHandle != null)
                {
                    ItemIdentifier itemIdentifier = (ItemIdentifier)m_items[items[i].ServerHandle];
                    if (itemIdentifier != null)
                    {
                        array[i] = (int)itemIdentifier.ServerHandle;
                    }
                }
            }

            return array;
        }

        private int[] GetAggregateIDs(Item[] items)
        {
            int[] array = new int[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                array[i] = 0;
                if (items[i].AggregateID != 0)
                {
                    array[i] = items[i].AggregateID;
                }
            }

            return array;
        }

        private void UpdateResult(ItemIdentifier item, ItemIdentifier result, int error)
        {
            result.ItemName = item.ItemName;
            result.ItemPath = item.ItemPath;
            result.ClientHandle = item.ClientHandle;
            result.ServerHandle = item.ServerHandle;
            if (error >= 0 && item.ServerHandle != null)
            {
                ItemIdentifier itemIdentifier = (ItemIdentifier)m_items[item.ServerHandle];
                if (itemIdentifier != null)
                {
                    result.ItemName = itemIdentifier.ItemName;
                    result.ItemPath = itemIdentifier.ItemPath;
                    result.ClientHandle = itemIdentifier.ClientHandle;
                }
            }
        }

        private void UpdateActualTimes(IActualTime[] results, OPCHDA_TIME pStartTime, OPCHDA_TIME pEndTime)
        {
            DateTime fILETIME = OpcCom.Interop.GetFILETIME(Interop.Convert(pStartTime.ftTime));
            DateTime fILETIME2 = OpcCom.Interop.GetFILETIME(Interop.Convert(pEndTime.ftTime));
            foreach (IActualTime actualTime in results)
            {
                actualTime.StartTime = fILETIME;
                actualTime.EndTime = fILETIME2;
            }
        }

        private ItemAttributeCollection UpdateResults(ItemIdentifier item, AttributeValueCollection[] attributes, ref IntPtr pErrors)
        {
            int[] int32s = OpcCom.Interop.GetInt32s(ref pErrors, attributes.Length, deallocate: true);
            if (attributes == null || int32s == null)
            {
                throw new InvalidResponseException();
            }

            for (int i = 0; i < attributes.Length; i++)
            {
                attributes[i].ResultID = OpcCom.Interop.GetResultID(int32s[i]);
            }

            ItemAttributeCollection itemAttributeCollection = new ItemAttributeCollection();
            foreach (AttributeValueCollection value in attributes)
            {
                itemAttributeCollection.Add(value);
            }

            UpdateResult(item, itemAttributeCollection, 0);
            return itemAttributeCollection;
        }

        private void UpdateResults(ItemIdentifier[] items, ItemIdentifier[] results, ref IntPtr pErrors)
        {
            int[] int32s = OpcCom.Interop.GetInt32s(ref pErrors, items.Length, deallocate: true);
            if (results == null || int32s == null)
            {
                throw new InvalidResponseException();
            }

            for (int i = 0; i < results.Length; i++)
            {
                UpdateResult(items[i], results[i], int32s[i]);
                if (typeof(IResult).IsInstanceOfType(results[i]))
                {
                    ((IResult)results[i]).ResultID = OpcCom.Interop.GetResultID(int32s[i]);
                }
            }
        }

        private void UpdateResults(ICollection[] items, ResultCollection[] results, int count, ref IntPtr pErrors)
        {
            int[] int32s = OpcCom.Interop.GetInt32s(ref pErrors, count, deallocate: true);
            if (int32s == null)
            {
                throw new InvalidResponseException();
            }

            int num = 0;
            for (int i = 0; i < items.Length; i++)
            {
                for (int j = 0; j < items[i].Count; j++)
                {
                    if (num >= count)
                    {
                        break;
                    }

                    Result value = new Result(OpcCom.Interop.GetResultID(int32s[num++]));
                    results[i].Add(value);
                }
            }
        }

        private int MarshalValues(ItemValueCollection[] items, ref int[] handles, ref object[] values, ref int[] qualities, ref DateTime[] timestamps)
        {
            int count = GetCount(items);
            handles = new int[count];
            timestamps = new DateTime[count];
            values = new object[count];
            qualities = new int[count];
            int[] serverHandles = GetServerHandles(items);
            int num = 0;
            for (int i = 0; i < items.Length; i++)
            {
                foreach (ItemValue item in items[i])
                {
                    handles[num] = serverHandles[i];
                    ref DateTime reference = ref timestamps[num];
                    reference = item.Timestamp;
                    values[num] = OpcCom.Interop.GetVARIANT(item.Value);
                    qualities[num] = item.Quality.GetCode();
                    num++;
                }
            }

            return count;
        }

        private int MarshalTimestamps(ItemTimeCollection[] items, ref int[] handles, ref DateTime[] timestamps)
        {
            int count = GetCount(items);
            handles = new int[count];
            timestamps = new DateTime[count];
            int[] serverHandles = GetServerHandles(items);
            int num = 0;
            for (int i = 0; i < items.Length; i++)
            {
                foreach (DateTime item in items[i])
                {
                    handles[num] = serverHandles[i];
                    timestamps[num] = item;
                    num++;
                }
            }

            return count;
        }

        private int MarshalAnnotatations(AnnotationValueCollection[] items, ref int[] serverHandles, ref OPCHDA_FILETIME[] ftTimestamps, ref OPCHDA_ANNOTATION[] annotations)
        {
            int count = GetCount(items);
            int[] serverHandles2 = GetServerHandles(items);
            serverHandles = new int[count];
            annotations = new OPCHDA_ANNOTATION[count];
            DateTime[] array = new DateTime[count];
            int num = 0;
            for (int i = 0; i < items.Length; i++)
            {
                for (int j = 0; j < items[i].Count; j++)
                {
                    serverHandles[num] = serverHandles2[i];
                    ref DateTime reference = ref array[num];
                    reference = items[i][j].Timestamp;
                    annotations[num] = default(OPCHDA_ANNOTATION);
                    annotations[num].dwNumValues = 1;
                    annotations[num].ftTimeStamps = OpcCom.Interop.GetFILETIMEs(new DateTime[1]
                    {
                        array[j]
                    });
                    annotations[num].szAnnotation = OpcCom.Interop.GetUnicodeStrings(new string[1]
                    {
                        items[i][j].Annotation
                    });
                    annotations[num].ftAnnotationTime = OpcCom.Interop.GetFILETIMEs(new DateTime[1]
                    {
                        items[i][j].CreationTime
                    });
                    annotations[num].szUser = OpcCom.Interop.GetUnicodeStrings(new string[1]
                    {
                        items[i][j].User
                    });
                    num++;
                }
            }

            ftTimestamps = Interop.GetFILETIMEs(array);
            return count;
        }

        private IdentifiedResult[] GetIdentifiedResults(ResultCollection[] results)
        {
            if (results == null || results.Length == 0)
            {
                return new IdentifiedResult[0];
            }

            IdentifiedResult[] array = new IdentifiedResult[results.Length];
            for (int i = 0; i < results.Length; i++)
            {
                array[i] = new IdentifiedResult(results[i]);
                if (results[i] == null || results[i].Count == 0)
                {
                    array[i].ResultID = ResultID.Hda.S_NODATA;
                    continue;
                }

                ResultID resultID = results[i][0].ResultID;
                foreach (Result item in results[i])
                {
                    if (resultID.Code != item.ResultID.Code)
                    {
                        resultID = ResultID.E_FAIL;
                        break;
                    }
                }
            }

            return array;
        }
    }

}
