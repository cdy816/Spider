using System;
using System.Runtime.InteropServices;
using Opc;
using Opc.Ae;
using OpcRcw.Ae;

namespace OpcCom.Ae
{
    [Serializable]
    public class Subscription : ISubscription, IDisposable
    {
        private class Callback : IOPCEventSink
        {
            private object m_clientHandle;

            public event EventChangedEventHandler EventChanged
            {
                add
                {
                    lock (this)
                    {
                        this.m_EventChanged = (EventChangedEventHandler)Delegate.Combine(this.m_EventChanged, value);
                    }
                }
                remove
                {
                    lock (this)
                    {
                        this.m_EventChanged = (EventChangedEventHandler)Delegate.Remove(this.m_EventChanged, value);
                    }
                }
            }

            private event EventChangedEventHandler m_EventChanged;

            public Callback(object clientHandle)
            {
                m_clientHandle = clientHandle;
            }

            public void OnEvent(int hClientSubscription, int bRefresh, int bLastRefresh, int dwCount, ONEVENTSTRUCT[] pEvents)
            {
                try
                {
                    lock (this)
                    {
                        if (this.m_EventChanged != null)
                        {
                            EventNotification[] eventNotifications = Interop.GetEventNotifications(pEvents);
                            for (int i = 0; i < eventNotifications.Length; i++)
                            {
                                eventNotifications[i].ClientHandle = m_clientHandle;
                            }

                            this.m_EventChanged(eventNotifications, bRefresh != 0, bLastRefresh != 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _ = ex.StackTrace;
                }
            }
        }

        private bool m_disposed;

        private object m_subscription;

        private object m_clientHandle;

        private bool m_supportsAE11 = true;

        private ConnectionPoint m_connection;

        private Callback m_callback;

        public event EventChangedEventHandler EventChanged
        {
            add
            {
                lock (this)
                {
                    Advise();
                    m_callback.EventChanged += value;
                }
            }
            remove
            {
                lock (this)
                {
                    m_callback.EventChanged -= value;
                    Unadvise();
                }
            }
        }

        internal Subscription(SubscriptionState state, object subscription)
        {
            m_subscription = subscription;
            m_clientHandle = Opc.Convert.Clone(state.ClientHandle);
            m_supportsAE11 = true;
            m_callback = new Callback(state.ClientHandle);
            try
            {
                _ = (IOPCEventSubscriptionMgt2)m_subscription;
            }
            catch
            {
                m_supportsAE11 = false;
            }
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
                if (disposing && m_connection != null)
                {
                    m_connection.Dispose();
                    m_connection = null;
                }

                if (m_subscription != null)
                {
                    OpcCom.Interop.ReleaseServer(m_subscription);
                    m_subscription = null;
                }
            }

            m_disposed = true;
        }

        public SubscriptionState GetState()
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw new NotConnectedException();
                }

                int pbActive = 0;
                int pdwBufferTime = 0;
                int pdwMaxSize = 0;
                int phClientSubscription = 0;
                int pdwKeepAliveTime = 0;
                try
                {
                    ((IOPCEventSubscriptionMgt)m_subscription).GetState(out pbActive, out pdwBufferTime, out pdwMaxSize, out phClientSubscription);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.GetState", e);
                }

                if (m_supportsAE11)
                {
                    try
                    {
                        ((IOPCEventSubscriptionMgt2)m_subscription).GetKeepAlive(out pdwKeepAliveTime);
                    }
                    catch (Exception e2)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt2.GetKeepAlive", e2);
                    }
                }

                SubscriptionState subscriptionState = new SubscriptionState();
                subscriptionState.Active = (pbActive != 0);
                subscriptionState.ClientHandle = m_clientHandle;
                subscriptionState.BufferTime = pdwBufferTime;
                subscriptionState.MaxSize = pdwMaxSize;
                subscriptionState.KeepAlive = pdwKeepAliveTime;
                return subscriptionState;
            }
        }

        public SubscriptionState ModifyState(int masks, SubscriptionState state)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw new NotConnectedException();
                }

                int num = state.Active ? 1 : 0;
                GCHandle gCHandle = GCHandle.Alloc(num, GCHandleType.Pinned);
                GCHandle gCHandle2 = GCHandle.Alloc(state.BufferTime, GCHandleType.Pinned);
                GCHandle gCHandle3 = GCHandle.Alloc(state.MaxSize, GCHandleType.Pinned);
                IntPtr pbActive = ((masks & 4) != 0) ? gCHandle.AddrOfPinnedObject() : IntPtr.Zero;
                IntPtr pdwBufferTime = ((masks & 8) != 0) ? gCHandle2.AddrOfPinnedObject() : IntPtr.Zero;
                IntPtr pdwMaxSize = ((masks & 0x10) != 0) ? gCHandle3.AddrOfPinnedObject() : IntPtr.Zero;
                int hClientSubscription = 0;
                int pdwRevisedBufferTime = 0;
                int pdwRevisedMaxSize = 0;
                try
                {
                    ((IOPCEventSubscriptionMgt)m_subscription).SetState(pbActive, pdwBufferTime, pdwMaxSize, hClientSubscription, out pdwRevisedBufferTime, out pdwRevisedMaxSize);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.SetState", e);
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
                }

                if ((masks & 0x20) != 0 && m_supportsAE11)
                {
                    int pdwRevisedKeepAliveTime = 0;
                    try
                    {
                        ((IOPCEventSubscriptionMgt2)m_subscription).SetKeepAlive(state.KeepAlive, out pdwRevisedKeepAliveTime);
                    }
                    catch (Exception e2)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt2.SetKeepAlive", e2);
                    }
                }

                return GetState();
            }
        }

        public SubscriptionFilters GetFilters()
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw new NotConnectedException();
                }

                int pdwEventType = 0;
                int pdwNumCategories = 0;
                IntPtr ppdwEventCategories = IntPtr.Zero;
                int pdwLowSeverity = 0;
                int pdwHighSeverity = 0;
                int pdwNumAreas = 0;
                IntPtr ppszAreaList = IntPtr.Zero;
                int pdwNumSources = 0;
                IntPtr ppszSourceList = IntPtr.Zero;
                try
                {
                    ((IOPCEventSubscriptionMgt)m_subscription).GetFilter(out pdwEventType, out pdwNumCategories, out ppdwEventCategories, out pdwLowSeverity, out pdwHighSeverity, out pdwNumAreas, out ppszAreaList, out pdwNumSources, out ppszSourceList);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.GetFilter", e);
                }

                int[] int32s = OpcCom.Interop.GetInt32s(ref ppdwEventCategories, pdwNumCategories, deallocate: true);
                string[] unicodeStrings = OpcCom.Interop.GetUnicodeStrings(ref ppszAreaList, pdwNumAreas, deallocate: true);
                string[] unicodeStrings2 = OpcCom.Interop.GetUnicodeStrings(ref ppszSourceList, pdwNumSources, deallocate: true);
                SubscriptionFilters subscriptionFilters = new SubscriptionFilters();
                subscriptionFilters.EventTypes = pdwEventType;
                subscriptionFilters.LowSeverity = pdwLowSeverity;
                subscriptionFilters.HighSeverity = pdwHighSeverity;
                subscriptionFilters.Categories.AddRange(int32s);
                subscriptionFilters.Areas.AddRange(unicodeStrings);
                subscriptionFilters.Sources.AddRange(unicodeStrings2);
                return subscriptionFilters;
            }
        }

        public void SetFilters(SubscriptionFilters filters)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    ((IOPCEventSubscriptionMgt)m_subscription).SetFilter(filters.EventTypes, filters.Categories.Count, filters.Categories.ToArray(), filters.LowSeverity, filters.HighSeverity, filters.Areas.Count, filters.Areas.ToArray(), filters.Sources.Count, filters.Sources.ToArray());
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.SetFilter", e);
                }
            }
        }

        public int[] GetReturnedAttributes(int eventCategory)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw new NotConnectedException();
                }

                int pdwCount = 0;
                IntPtr ppdwAttributeIDs = IntPtr.Zero;
                try
                {
                    ((IOPCEventSubscriptionMgt)m_subscription).GetReturnedAttributes(eventCategory, out pdwCount, out ppdwAttributeIDs);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.GetReturnedAttributes", e);
                }

                return OpcCom.Interop.GetInt32s(ref ppdwAttributeIDs, pdwCount, deallocate: true);
            }
        }

        public void SelectReturnedAttributes(int eventCategory, int[] attributeIDs)
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    ((IOPCEventSubscriptionMgt)m_subscription).SelectReturnedAttributes(eventCategory, (attributeIDs != null) ? attributeIDs.Length : 0, (attributeIDs != null) ? attributeIDs : new int[0]);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.SelectReturnedAttributes", e);
                }
            }
        }

        public void Refresh()
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    ((IOPCEventSubscriptionMgt)m_subscription).Refresh(m_connection.Cookie);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.Refresh", e);
                }
            }
        }

        public void CancelRefresh()
        {
            lock (this)
            {
                if (m_subscription == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    ((IOPCEventSubscriptionMgt)m_subscription).CancelRefresh(m_connection.Cookie);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventSubscriptionMgt.Refresh", e);
                }
            }
        }

        private void Advise()
        {
            if (m_connection == null)
            {
                m_connection = new ConnectionPoint(m_subscription, typeof(IOPCEventSink).GUID);
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
