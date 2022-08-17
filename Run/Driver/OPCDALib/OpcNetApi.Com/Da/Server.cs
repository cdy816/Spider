using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using Opc;
using Opc.Da;
using OpcRcw.Da;

namespace OpcCom.Da
{
    public class Server : OpcCom.Server, Opc.Da.IServer, Opc.IServer, IDisposable
    {
        private bool m_disposed;

        protected int m_filters = 9;

        private Hashtable m_subscriptions = new Hashtable();

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
        }

        protected override void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                lock (this)
                {
                    if (disposing && m_server != null)
                    {
                        foreach (Subscription value in m_subscriptions.Values)
                        {
                            value.Dispose();
                            try
                            {
                                SubscriptionState state = value.GetState();
                                ((IOPCServer)m_server).RemoveGroup((int)state.ServerHandle, 0);
                            }
                            catch
                            {
                            }
                        }

                        m_subscriptions.Clear();
                    }

                    if (m_server != null)
                    {
                        OpcCom.Interop.ReleaseServer(m_server);
                        m_server = null;
                    }
                }

                m_disposed = true;
            }

            base.Dispose(disposing);
        }

        public override string GetErrorText(string locale, ResultID resultID)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    string ppString = null;
                    ((IOPCServer)m_server).GetErrorString(resultID.Code, OpcCom.Interop.GetLocale(locale), out ppString);
                    return ppString;
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCServer.GetErrorString", e);
                }
            }
        }

        public int GetResultFilters()
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                return m_filters;
            }
        }

        public void SetResultFilters(int filters)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                m_filters = filters;
            }
        }

        public ServerStatus GetStatus()
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                IntPtr ppServerStatus = IntPtr.Zero;
                try
                {
                    ((IOPCServer)m_server).GetStatus(out ppServerStatus);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCServer.GetStatus", e);
                }

                return Interop.GetServerStatus(ref ppServerStatus, deallocate: true);
            }
        }

        public virtual ItemValueResult[] Read(Item[] items)
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

                int num = items.Length;
                if (num == 0)
                {
                    throw new ArgumentOutOfRangeException("items.Length", "0");
                }

                string[] array = new string[num];
                int[] array2 = new int[num];
                for (int i = 0; i < num; i++)
                {
                    array[i] = items[i].ItemName;
                    array2[i] = (items[i].MaxAgeSpecified ? items[i].MaxAge : 0);
                }

                IntPtr ppvValues = IntPtr.Zero;
                IntPtr ppwQualities = IntPtr.Zero;
                IntPtr ppftTimeStamps = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCItemIO)m_server).Read(num, array, array2, out ppvValues, out ppwQualities, out ppftTimeStamps, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCItemIO.Read", e);
                }

                object[] vARIANTs = OpcCom.Interop.GetVARIANTs(ref ppvValues, num, deallocate: true);
                short[] int16s = OpcCom.Interop.GetInt16s(ref ppwQualities, num, deallocate: true);
                DateTime[] fILETIMEs = OpcCom.Interop.GetFILETIMEs(ref ppftTimeStamps, num, deallocate: true);
                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, num, deallocate: true);
                string locale = GetLocale();
                ItemValueResult[] array3 = new ItemValueResult[num];
                for (int j = 0; j < array3.Length; j++)
                {
                    array3[j] = new ItemValueResult(items[j]);
                    array3[j].ServerHandle = null;
                    array3[j].Value = vARIANTs[j];
                    array3[j].Quality = new Quality(int16s[j]);
                    array3[j].QualitySpecified = true;
                    array3[j].Timestamp = fILETIMEs[j];
                    array3[j].TimestampSpecified = (fILETIMEs[j] != DateTime.MinValue);
                    array3[j].ResultID = OpcCom.Interop.GetResultID(int32s[j]);
                    array3[j].DiagnosticInfo = null;
                    if (int32s[j] == -1073479674)
                    {
                        array3[j].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                    }

                    if (array3[j].Value != null && (object)items[j].ReqType != null)
                    {
                        try
                        {
                            array3[j].Value = ChangeType(vARIANTs[j], items[j].ReqType, locale);
                        }
                        catch (Exception ex)
                        {
                            array3[j].Value = null;
                            array3[j].Quality = Quality.Bad;
                            array3[j].QualitySpecified = true;
                            array3[j].Timestamp = DateTime.MinValue;
                            array3[j].TimestampSpecified = false;
                            if ((object)ex.GetType() == typeof(OverflowException))
                            {
                                array3[j].ResultID = OpcCom.Interop.GetResultID(-1073479669);
                            }
                            else
                            {
                                array3[j].ResultID = OpcCom.Interop.GetResultID(-1073479676);
                            }
                        }
                    }

                    if ((m_filters & 1) == 0)
                    {
                        array3[j].ItemName = null;
                    }

                    if ((m_filters & 2) == 0)
                    {
                        array3[j].ItemPath = null;
                    }

                    if ((m_filters & 4) == 0)
                    {
                        array3[j].ClientHandle = null;
                    }

                    if ((m_filters & 8) == 0)
                    {
                        array3[j].Timestamp = DateTime.MinValue;
                        array3[j].TimestampSpecified = false;
                    }
                }

                return array3;
            }
        }

        public virtual IdentifiedResult[] Write(ItemValue[] items)
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

                int num = items.Length;
                if (num == 0)
                {
                    throw new ArgumentOutOfRangeException("items.Length", "0");
                }

                string[] array = new string[num];
                for (int i = 0; i < num; i++)
                {
                    array[i] = items[i].ItemName;
                }

                OPCITEMVQT[] oPCITEMVQTs = Interop.GetOPCITEMVQTs(items);
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCItemIO)m_server).WriteVQT(num, array, oPCITEMVQTs, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCItemIO.Read", e);
                }

                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, num, deallocate: true);
                IdentifiedResult[] array2 = new IdentifiedResult[num];
                for (int j = 0; j < num; j++)
                {
                    array2[j] = new IdentifiedResult(items[j]);
                    array2[j].ServerHandle = null;
                    array2[j].ResultID = OpcCom.Interop.GetResultID(int32s[j]);
                    array2[j].DiagnosticInfo = null;
                    if (int32s[j] == -1073479674)
                    {
                        array2[j].ResultID = new ResultID(ResultID.Da.E_READONLY, -1073479674L);
                    }

                    if ((m_filters & 1) == 0)
                    {
                        array2[j].ItemName = null;
                    }

                    if ((m_filters & 2) == 0)
                    {
                        array2[j].ItemPath = null;
                    }

                    if ((m_filters & 4) == 0)
                    {
                        array2[j].ClientHandle = null;
                    }
                }

                return array2;
            }
        }

        public ISubscription CreateSubscription(SubscriptionState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                SubscriptionState subscriptionState = (SubscriptionState)state.Clone();
                Guid riid = typeof(IOPCItemMgt).GUID;
                object ppUnk = null;
                int phServerGroup = 0;
                int pRevisedUpdateRate = 0;
                GCHandle gCHandle = GCHandle.Alloc(subscriptionState.Deadband, GCHandleType.Pinned);
                try
                {
                    ((IOPCServer)m_server).AddGroup((subscriptionState.Name != null) ? subscriptionState.Name : "", subscriptionState.Active ? 1 : 0, subscriptionState.UpdateRate, 0, IntPtr.Zero, gCHandle.AddrOfPinnedObject(), OpcCom.Interop.GetLocale(subscriptionState.Locale), out phServerGroup, out pRevisedUpdateRate, ref riid, out ppUnk);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCServer.AddGroup", e);
                }
                finally
                {
                    if (gCHandle.IsAllocated)
                    {
                        gCHandle.Free();
                    }
                }

                try
                {
                    int pdwRevisedKeepAliveTime = 0;
                    ((IOPCGroupStateMgt2)ppUnk).SetKeepAlive(subscriptionState.KeepAlive, out pdwRevisedKeepAliveTime);
                    subscriptionState.KeepAlive = pdwRevisedKeepAliveTime;
                }
                catch
                {
                    subscriptionState.KeepAlive = 0;
                }

                subscriptionState.ServerHandle = phServerGroup;
                if (pRevisedUpdateRate > subscriptionState.UpdateRate)
                {
                    subscriptionState.UpdateRate = pRevisedUpdateRate;
                }

                Subscription subscription = CreateSubscription(ppUnk, subscriptionState, m_filters);
                m_subscriptions[phServerGroup] = subscription;
                return subscription;
            }
        }

        public void CancelSubscription(ISubscription subscription)
        {
            if (subscription == null)
            {
                throw new ArgumentNullException("subscription");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (!typeof(Subscription).IsInstanceOfType(subscription))
                {
                    throw new ArgumentException("Incorrect object type.", "subscription");
                }

                SubscriptionState state = subscription.GetState();
                if (!m_subscriptions.ContainsKey(state.ServerHandle))
                {
                    throw new ArgumentException("Handle not found.", "subscription");
                }

                m_subscriptions.Remove(state.ServerHandle);
                subscription.Dispose();
                try
                {
                    ((IOPCServer)m_server).RemoveGroup((int)state.ServerHandle, 0);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCServer.RemoveGroup", e);
                }
            }
        }

        public virtual BrowseElement[] Browse(ItemIdentifier itemID, BrowseFilters filters, out Opc.Da.BrowsePosition position)
        {
            if (filters == null)
            {
                throw new ArgumentNullException("filters");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                position = null;
                int pdwCount = 0;
                int pbMoreElements = 0;
                IntPtr pszContinuationPoint = IntPtr.Zero;
                IntPtr ppBrowseElements = IntPtr.Zero;
                try
                {
                    ((IOPCBrowse)m_server).Browse((itemID != null && itemID.ItemName != null) ? itemID.ItemName : "", ref pszContinuationPoint, filters.MaxElementsReturned, Interop.GetBrowseFilter(filters.BrowseFilter), (filters.ElementNameFilter != null) ? filters.ElementNameFilter : "", (filters.VendorFilter != null) ? filters.VendorFilter : "", filters.ReturnAllProperties ? 1 : 0, filters.ReturnPropertyValues ? 1 : 0, (filters.PropertyIDs != null) ? filters.PropertyIDs.Length : 0, Interop.GetPropertyIDs(filters.PropertyIDs), out pbMoreElements, out pdwCount, out ppBrowseElements);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCBrowse.Browse", e);
                }

                BrowseElement[] browseElements = Interop.GetBrowseElements(ref ppBrowseElements, pdwCount, deallocate: true);
                string text = Marshal.PtrToStringUni(pszContinuationPoint);
                Marshal.FreeCoTaskMem(pszContinuationPoint);
                if (pbMoreElements != 0 || (text != null && text != ""))
                {
                    position = new BrowsePosition(itemID, filters, text);
                }

                ProcessResults(browseElements, filters.PropertyIDs);
                return browseElements;
            }
        }

        public virtual BrowseElement[] BrowseNext(ref Opc.Da.BrowsePosition position)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (position == null || (object)position.GetType() != typeof(BrowsePosition))
                {
                    throw new BrowseCannotContinueException();
                }

                BrowsePosition browsePosition = (BrowsePosition)position;
                if (browsePosition == null || browsePosition.ContinuationPoint == null || browsePosition.ContinuationPoint == "")
                {
                    throw new BrowseCannotContinueException();
                }

                int pdwCount = 0;
                int pbMoreElements = 0;
                ItemIdentifier itemID = ((BrowsePosition)position).ItemID;
                BrowseFilters filters = ((BrowsePosition)position).Filters;
                IntPtr pszContinuationPoint = Marshal.StringToCoTaskMemUni(browsePosition.ContinuationPoint);
                IntPtr ppBrowseElements = IntPtr.Zero;
                try
                {
                    ((IOPCBrowse)m_server).Browse((itemID != null && itemID.ItemName != null) ? itemID.ItemName : "", ref pszContinuationPoint, filters.MaxElementsReturned, Interop.GetBrowseFilter(filters.BrowseFilter), (filters.ElementNameFilter != null) ? filters.ElementNameFilter : "", (filters.VendorFilter != null) ? filters.VendorFilter : "", filters.ReturnAllProperties ? 1 : 0, filters.ReturnPropertyValues ? 1 : 0, (filters.PropertyIDs != null) ? filters.PropertyIDs.Length : 0, Interop.GetPropertyIDs(filters.PropertyIDs), out pbMoreElements, out pdwCount, out ppBrowseElements);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCBrowse.BrowseNext", e);
                }

                BrowseElement[] browseElements = Interop.GetBrowseElements(ref ppBrowseElements, pdwCount, deallocate: true);
                browsePosition.ContinuationPoint = Marshal.PtrToStringUni(pszContinuationPoint);
                Marshal.FreeCoTaskMem(pszContinuationPoint);
                if (pbMoreElements == 0 && (browsePosition.ContinuationPoint == null || browsePosition.ContinuationPoint == ""))
                {
                    position = null;
                }

                ProcessResults(browseElements, filters.PropertyIDs);
                return browseElements;
            }
        }

        public virtual ItemPropertyCollection[] GetProperties(ItemIdentifier[] itemIDs, PropertyID[] propertyIDs, bool returnValues)
        {
            if (itemIDs == null)
            {
                throw new ArgumentNullException("itemIDs");
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                string[] array = new string[itemIDs.Length];
                for (int i = 0; i < itemIDs.Length; i++)
                {
                    array[i] = itemIDs[i].ItemName;
                }

                IntPtr ppItemProperties = IntPtr.Zero;
                try
                {
                    ((IOPCBrowse)m_server).GetProperties(itemIDs.Length, array, returnValues ? 1 : 0, (propertyIDs != null) ? propertyIDs.Length : 0, Interop.GetPropertyIDs(propertyIDs), out ppItemProperties);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCBrowse.GetProperties", e);
                }

                ItemPropertyCollection[] itemPropertyCollections = Interop.GetItemPropertyCollections(ref ppItemProperties, itemIDs.Length, deallocate: true);
                if (propertyIDs != null && propertyIDs.Length > 0)
                {
                    ItemPropertyCollection[] array2 = itemPropertyCollections;
                    foreach (ItemPropertyCollection itemPropertyCollection in array2)
                    {
                        for (int k = 0; k < itemPropertyCollection.Count; k++)
                        {
                            itemPropertyCollection[k].ID = propertyIDs[k];
                        }
                    }
                }

                return itemPropertyCollections;
            }
        }

        protected object ChangeType(object source, System.Type type, string locale)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);
            }
            catch
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            }

            try
            {
                object obj2 = Opc.Convert.ChangeType(source, type);
                if ((object)typeof(float) == type && float.IsInfinity(System.Convert.ToSingle(obj2)))
                {
                    throw new OverflowException();
                }

                return obj2;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
        }

        protected virtual Subscription CreateSubscription(object group, SubscriptionState state, int filters)
        {
            return new Subscription(group, state, filters);
        }

        private void ProcessResults(BrowseElement[] elements, PropertyID[] propertyIDs)
        {
            if (elements == null)
            {
                return;
            }

            foreach (BrowseElement browseElement in elements)
            {
                if (browseElement.Properties == null)
                {
                    continue;
                }

                ItemProperty[] properties = browseElement.Properties;
                foreach (ItemProperty itemProperty in properties)
                {
                    if (propertyIDs == null)
                    {
                        continue;
                    }

                    for (int k = 0; k < propertyIDs.Length; k++)
                    {
                        PropertyID iD = propertyIDs[k];
                        if (itemProperty.ID.Code == iD.Code)
                        {
                            itemProperty.ID = iD;
                            break;
                        }
                    }
                }
            }
        }
    }
}
