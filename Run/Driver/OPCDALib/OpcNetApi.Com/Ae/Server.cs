using System;
using System.Collections;
using System.Runtime.InteropServices;
using Opc;
using Opc.Ae;
using OpcRcw.Ae;
using OpcRcw.Comn;

namespace OpcCom.Ae
{
    [Serializable]
    public class Server : OpcCom.Server, Opc.Ae.IServer, Opc.IServer, IDisposable
    {
        private bool m_disposed;

        private bool m_supportsAE11 = true;

        private object m_browser;

        private int m_handles = 1;

        private Hashtable m_subscriptions = new Hashtable();

        public Server(URL url, object server)
            : base(url, server)
        {
            m_supportsAE11 = true;
            try
            {
                _ = (IOPCEventServer2)server;
            }
            catch
            {
                m_supportsAE11 = false;
            }
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
                        }

                        m_subscriptions.Clear();
                    }

                    if (m_browser != null)
                    {
                        OpcCom.Interop.ReleaseServer(m_browser);
                        m_browser = null;
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

        public ServerStatus GetStatus()
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                IntPtr ppEventServerStatus = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer)m_server).GetStatus(out ppEventServerStatus);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.GetStatus", e);
                }

                return Interop.GetServerStatus(ref ppEventServerStatus, deallocate: true);
            }
        }

        public ISubscription CreateSubscription(SubscriptionState state)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (state == null)
                {
                    throw new ArgumentNullException("state");
                }

                object ppUnk = null;
                Guid riid = typeof(IOPCEventSubscriptionMgt).GUID;
                int pdwRevisedBufferTime = 0;
                int pdwRevisedMaxSize = 0;
                try
                {
                    ((IOPCEventServer)m_server).CreateEventSubscription(state.Active ? 1 : 0, state.BufferTime, state.MaxSize, ++m_handles, ref riid, out ppUnk, out pdwRevisedBufferTime, out pdwRevisedMaxSize);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.CreateEventSubscription", e);
                }

                state.BufferTime = pdwRevisedBufferTime;
                state.MaxSize = pdwRevisedMaxSize;
                Subscription subscription = new Subscription(state, ppUnk);
                subscription.ModifyState(32, state);
                m_subscriptions.Add(m_handles, subscription);
                return subscription;
            }
        }

        public int QueryAvailableFilters()
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                int pdwFilterMask = 0;
                try
                {
                    ((IOPCEventServer)m_server).QueryAvailableFilters(out pdwFilterMask);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.QueryAvailableFilters", e);
                }

                return pdwFilterMask;
            }
        }

        public Category[] QueryEventCategories(int eventType)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                int pdwCount = 0;
                IntPtr ppdwEventCategories = IntPtr.Zero;
                IntPtr ppszEventCategoryDescs = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer)m_server).QueryEventCategories(eventType, out pdwCount, out ppdwEventCategories, out ppszEventCategoryDescs);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.QueryEventCategories", e);
                }

                if (pdwCount == 0)
                {
                    return new Category[0];
                }

                int[] int32s = OpcCom.Interop.GetInt32s(ref ppdwEventCategories, pdwCount, deallocate: true);
                string[] unicodeStrings = OpcCom.Interop.GetUnicodeStrings(ref ppszEventCategoryDescs, pdwCount, deallocate: true);
                Category[] array = new Category[pdwCount];
                for (int i = 0; i < pdwCount; i++)
                {
                    array[i] = new Category();
                    array[i].ID = int32s[i];
                    array[i].Name = unicodeStrings[i];
                }

                return array;
            }
        }

        public string[] QueryConditionNames(int eventCategory)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                int pdwCount = 0;
                IntPtr ppszConditionNames = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer)m_server).QueryConditionNames(eventCategory, out pdwCount, out ppszConditionNames);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.QueryConditionNames", e);
                }

                if (pdwCount == 0)
                {
                    return new string[0];
                }

                return OpcCom.Interop.GetUnicodeStrings(ref ppszConditionNames, pdwCount, deallocate: true);
            }
        }

        public string[] QuerySubConditionNames(string conditionName)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                int pdwCount = 0;
                IntPtr ppszSubConditionNames = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer)m_server).QuerySubConditionNames(conditionName, out pdwCount, out ppszSubConditionNames);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.QuerySubConditionNames", e);
                }

                if (pdwCount == 0)
                {
                    return new string[0];
                }

                return OpcCom.Interop.GetUnicodeStrings(ref ppszSubConditionNames, pdwCount, deallocate: true);
            }
        }

        public string[] QueryConditionNames(string sourceName)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                int pdwCount = 0;
                IntPtr ppszConditionNames = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer)m_server).QuerySourceConditions(sourceName, out pdwCount, out ppszConditionNames);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.QuerySourceConditions", e);
                }

                if (pdwCount == 0)
                {
                    return new string[0];
                }

                return OpcCom.Interop.GetUnicodeStrings(ref ppszConditionNames, pdwCount, deallocate: true);
            }
        }

        public Opc.Ae.Attribute[] QueryEventAttributes(int eventCategory)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                int pdwCount = 0;
                IntPtr ppdwAttrIDs = IntPtr.Zero;
                IntPtr ppszAttrDescs = IntPtr.Zero;
                IntPtr ppvtAttrTypes = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer)m_server).QueryEventAttributes(eventCategory, out pdwCount, out ppdwAttrIDs, out ppszAttrDescs, out ppvtAttrTypes);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.QueryEventAttributes", e);
                }

                if (pdwCount == 0)
                {
                    return new Opc.Ae.Attribute[0];
                }

                int[] int32s = OpcCom.Interop.GetInt32s(ref ppdwAttrIDs, pdwCount, deallocate: true);
                string[] unicodeStrings = OpcCom.Interop.GetUnicodeStrings(ref ppszAttrDescs, pdwCount, deallocate: true);
                short[] int16s = OpcCom.Interop.GetInt16s(ref ppvtAttrTypes, pdwCount, deallocate: true);
                Opc.Ae.Attribute[] array = new Opc.Ae.Attribute[pdwCount];
                for (int i = 0; i < pdwCount; i++)
                {
                    array[i] = new Opc.Ae.Attribute();
                    array[i].ID = int32s[i];
                    array[i].Name = unicodeStrings[i];
                    array[i].DataType = OpcCom.Interop.GetType((VarEnum)int16s[i]);
                }

                return array;
            }
        }

        public ItemUrl[] TranslateToItemIDs(string sourceName, int eventCategory, string conditionName, string subConditionName, int[] attributeIDs)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                IntPtr ppszAttrItemIDs = IntPtr.Zero;
                IntPtr ppszNodeNames = IntPtr.Zero;
                IntPtr ppCLSIDs = IntPtr.Zero;
                int num = (attributeIDs != null) ? attributeIDs.Length : 0;
                try
                {
                    ((IOPCEventServer)m_server).TranslateToItemIDs((sourceName != null) ? sourceName : "", eventCategory, (conditionName != null) ? conditionName : "", (subConditionName != null) ? subConditionName : "", num, (num > 0) ? attributeIDs : new int[0], out ppszAttrItemIDs, out ppszNodeNames, out ppCLSIDs);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.TranslateToItemIDs", e);
                }

                string[] unicodeStrings = OpcCom.Interop.GetUnicodeStrings(ref ppszAttrItemIDs, num, deallocate: true);
                string[] unicodeStrings2 = OpcCom.Interop.GetUnicodeStrings(ref ppszNodeNames, num, deallocate: true);
                Guid[] gUIDs = OpcCom.Interop.GetGUIDs(ref ppCLSIDs, num, deallocate: true);
                ItemUrl[] array = new ItemUrl[num];
                for (int i = 0; i < num; i++)
                {
                    array[i] = new ItemUrl();
                    array[i].ItemName = unicodeStrings[i];
                    array[i].ItemPath = null;
                    array[i].Url.Scheme = "opcda";
                    array[i].Url.HostName = unicodeStrings2[i];
                    array[i].Url.Path = $"{{{gUIDs[i]}}}";
                }

                return array;
            }
        }

        public Condition GetConditionState(string sourceName, string conditionName, int[] attributeIDs)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                IntPtr ppConditionState = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer)m_server).GetConditionState((sourceName != null) ? sourceName : "", (conditionName != null) ? conditionName : "", (attributeIDs != null) ? attributeIDs.Length : 0, (attributeIDs != null) ? attributeIDs : new int[0], out ppConditionState);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.GetConditionState", e);
                }

                Condition[] conditions = Interop.GetConditions(ref ppConditionState, 1, deallocate: true);
                for (int i = 0; i < conditions[0].Attributes.Count; i++)
                {
                    conditions[0].Attributes[i].ID = attributeIDs[i];
                }

                return conditions[0];
            }
        }

        public ResultID[] EnableConditionByArea(string[] areas)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (areas == null || areas.Length == 0)
                {
                    return new ResultID[0];
                }

                IntPtr ppErrors = IntPtr.Zero;
                int[] array = null;
                if (m_supportsAE11)
                {
                    try
                    {
                        ((IOPCEventServer2)m_server).EnableConditionByArea2(areas.Length, areas, out ppErrors);
                    }
                    catch (Exception e)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer2.EnableConditionByArea2", e);
                    }

                    array = OpcCom.Interop.GetInt32s(ref ppErrors, areas.Length, deallocate: true);
                }
                else
                {
                    try
                    {
                        ((IOPCEventServer)m_server).EnableConditionByArea(areas.Length, areas);
                    }
                    catch (Exception e2)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer.EnableConditionByArea", e2);
                    }

                    array = new int[areas.Length];
                }

                ResultID[] array2 = new ResultID[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    ref ResultID reference = ref array2[i];
                    reference = Interop.GetResultID(array[i]);
                }

                return array2;
            }
        }

        public ResultID[] DisableConditionByArea(string[] areas)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (areas == null || areas.Length == 0)
                {
                    return new ResultID[0];
                }

                IntPtr ppErrors = IntPtr.Zero;
                int[] array = null;
                if (m_supportsAE11)
                {
                    try
                    {
                        ((IOPCEventServer2)m_server).DisableConditionByArea2(areas.Length, areas, out ppErrors);
                    }
                    catch (Exception e)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer2.DisableConditionByArea2", e);
                    }

                    array = OpcCom.Interop.GetInt32s(ref ppErrors, areas.Length, deallocate: true);
                }
                else
                {
                    try
                    {
                        ((IOPCEventServer)m_server).DisableConditionByArea(areas.Length, areas);
                    }
                    catch (Exception e2)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer.DisableConditionByArea", e2);
                    }

                    array = new int[areas.Length];
                }

                ResultID[] array2 = new ResultID[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    ref ResultID reference = ref array2[i];
                    reference = Interop.GetResultID(array[i]);
                }

                return array2;
            }
        }

        public ResultID[] EnableConditionBySource(string[] sources)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (sources == null || sources.Length == 0)
                {
                    return new ResultID[0];
                }

                IntPtr ppErrors = IntPtr.Zero;
                int[] array = null;
                if (m_supportsAE11)
                {
                    try
                    {
                        ((IOPCEventServer2)m_server).EnableConditionBySource2(sources.Length, sources, out ppErrors);
                    }
                    catch (Exception e)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer2.EnableConditionBySource2", e);
                    }

                    array = OpcCom.Interop.GetInt32s(ref ppErrors, sources.Length, deallocate: true);
                }
                else
                {
                    try
                    {
                        ((IOPCEventServer)m_server).EnableConditionBySource(sources.Length, sources);
                    }
                    catch (Exception e2)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer.EnableConditionBySource", e2);
                    }

                    array = new int[sources.Length];
                }

                ResultID[] array2 = new ResultID[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    ref ResultID reference = ref array2[i];
                    reference = Interop.GetResultID(array[i]);
                }

                return array2;
            }
        }

        public ResultID[] DisableConditionBySource(string[] sources)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (sources == null || sources.Length == 0)
                {
                    return new ResultID[0];
                }

                IntPtr ppErrors = IntPtr.Zero;
                int[] array = null;
                if (m_supportsAE11)
                {
                    try
                    {
                        ((IOPCEventServer2)m_server).DisableConditionBySource2(sources.Length, sources, out ppErrors);
                    }
                    catch (Exception e)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer2.DisableConditionBySource2", e);
                    }

                    array = OpcCom.Interop.GetInt32s(ref ppErrors, sources.Length, deallocate: true);
                }
                else
                {
                    try
                    {
                        ((IOPCEventServer)m_server).DisableConditionBySource(sources.Length, sources);
                    }
                    catch (Exception e2)
                    {
                        throw OpcCom.Interop.CreateException("IOPCEventServer.DisableConditionBySource", e2);
                    }

                    array = new int[sources.Length];
                }

                ResultID[] array2 = new ResultID[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    ref ResultID reference = ref array2[i];
                    reference = Interop.GetResultID(array[i]);
                }

                return array2;
            }
        }

        public EnabledStateResult[] GetEnableStateByArea(string[] areas)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (areas == null || areas.Length == 0)
                {
                    return new EnabledStateResult[0];
                }

                if (!m_supportsAE11)
                {
                    EnabledStateResult[] array = new EnabledStateResult[areas.Length];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new EnabledStateResult();
                        array[i].Enabled = false;
                        array[i].EffectivelyEnabled = false;
                        array[i].ResultID = ResultID.E_FAIL;
                    }

                    return array;
                }

                IntPtr pbEnabled = IntPtr.Zero;
                IntPtr pbEffectivelyEnabled = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer2)m_server).GetEnableStateByArea(areas.Length, areas, out pbEnabled, out pbEffectivelyEnabled, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer2.GetEnableStateByArea", e);
                }

                int[] int32s = OpcCom.Interop.GetInt32s(ref pbEnabled, areas.Length, deallocate: true);
                int[] int32s2 = OpcCom.Interop.GetInt32s(ref pbEffectivelyEnabled, areas.Length, deallocate: true);
                int[] int32s3 = OpcCom.Interop.GetInt32s(ref ppErrors, areas.Length, deallocate: true);
                EnabledStateResult[] array2 = new EnabledStateResult[int32s3.Length];
                for (int j = 0; j < int32s3.Length; j++)
                {
                    array2[j] = new EnabledStateResult();
                    array2[j].Enabled = (int32s[j] != 0);
                    array2[j].EffectivelyEnabled = (int32s2[j] != 0);
                    array2[j].ResultID = Interop.GetResultID(int32s3[j]);
                }

                return array2;
            }
        }

        public EnabledStateResult[] GetEnableStateBySource(string[] sources)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (sources == null || sources.Length == 0)
                {
                    return new EnabledStateResult[0];
                }

                if (!m_supportsAE11)
                {
                    EnabledStateResult[] array = new EnabledStateResult[sources.Length];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = new EnabledStateResult();
                        array[i].Enabled = false;
                        array[i].EffectivelyEnabled = false;
                        array[i].ResultID = ResultID.E_FAIL;
                    }

                    return array;
                }

                IntPtr pbEnabled = IntPtr.Zero;
                IntPtr pbEffectivelyEnabled = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer2)m_server).GetEnableStateBySource(sources.Length, sources, out pbEnabled, out pbEffectivelyEnabled, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer2.GetEnableStateBySource", e);
                }

                int[] int32s = OpcCom.Interop.GetInt32s(ref pbEnabled, sources.Length, deallocate: true);
                int[] int32s2 = OpcCom.Interop.GetInt32s(ref pbEffectivelyEnabled, sources.Length, deallocate: true);
                int[] int32s3 = OpcCom.Interop.GetInt32s(ref ppErrors, sources.Length, deallocate: true);
                EnabledStateResult[] array2 = new EnabledStateResult[int32s3.Length];
                for (int j = 0; j < int32s3.Length; j++)
                {
                    array2[j] = new EnabledStateResult();
                    array2[j].Enabled = (int32s[j] != 0);
                    array2[j].EffectivelyEnabled = (int32s2[j] != 0);
                    array2[j].ResultID = Interop.GetResultID(int32s3[j]);
                }

                return array2;
            }
        }

        public ResultID[] AcknowledgeCondition(string acknowledgerID, string comment, EventAcknowledgement[] conditions)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (conditions == null || conditions.Length == 0)
                {
                    return new ResultID[0];
                }

                int num = conditions.Length;
                string[] array = new string[num];
                string[] array2 = new string[num];
                OpcRcw.Ae.FILETIME[] array3 = new OpcRcw.Ae.FILETIME[num];
                int[] array4 = new int[num];
                for (int i = 0; i < num; i++)
                {
                    array[i] = conditions[i].SourceName;
                    array2[i] = conditions[i].ConditionName;
                    ref OpcRcw.Ae.FILETIME reference = ref array3[i];
                    reference = Interop.Convert(OpcCom.Interop.GetFILETIME(conditions[i].ActiveTime));
                    array4[i] = conditions[i].Cookie;
                }

                IntPtr ppErrors = IntPtr.Zero;
                try
                {
                    ((IOPCEventServer)m_server).AckCondition(conditions.Length, acknowledgerID, comment, array, array2, array3, array4, out ppErrors);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.AckCondition", e);
                }

                int[] int32s = OpcCom.Interop.GetInt32s(ref ppErrors, num, deallocate: true);
                ResultID[] array5 = new ResultID[num];
                for (int j = 0; j < num; j++)
                {
                    ref ResultID reference2 = ref array5[j];
                    reference2 = Interop.GetResultID(int32s[j]);
                }

                return array5;
            }
        }

        public BrowseElement[] Browse(string areaID, BrowseType browseType, string browseFilter)
        {
            lock (this)
            {
                IBrowsePosition position = null;
                BrowseElement[] result = Browse(areaID, browseType, browseFilter, 0, out position);
                position?.Dispose();
                return result;
            }
        }

        public BrowseElement[] Browse(string areaID, BrowseType browseType, string browseFilter, int maxElements, out IBrowsePosition position)
        {
            position = null;
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                InitializeBrowser();
                ChangeBrowsePosition(areaID);
                var uCOMIEnumString = (System.Runtime.InteropServices.ComTypes.IEnumString)CreateEnumerator(browseType, browseFilter);
                ArrayList arrayList = new ArrayList();
                if (FetchElements(browseType, maxElements, uCOMIEnumString, arrayList) != 0)
                {
                    OpcCom.Interop.ReleaseServer(uCOMIEnumString);
                }
                else
                {
                    position = new BrowsePosition(areaID, browseType, browseFilter, uCOMIEnumString);
                }

                return (BrowseElement[])arrayList.ToArray(typeof(BrowseElement));
            }
        }

        public BrowseElement[] BrowseNext(int maxElements, ref IBrowsePosition position)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (position == null)
                {
                    throw new ArgumentNullException("position");
                }

                InitializeBrowser();
                ChangeBrowsePosition(((BrowsePosition)position).AreaID);
                var enumerator = ((BrowsePosition)position).Enumerator;
                ArrayList arrayList = new ArrayList();
                if (FetchElements(((BrowsePosition)position).BrowseType, maxElements, enumerator, arrayList) != 0)
                {
                    position.Dispose();
                    position = null;
                }

                return (BrowseElement[])arrayList.ToArray(typeof(BrowseElement));
            }
        }

        private void InitializeBrowser()
        {
            if (m_browser == null)
            {
                object ppUnk = null;
                Guid riid = typeof(IOPCEventAreaBrowser).GUID;
                try
                {
                    ((IOPCEventServer)m_server).CreateAreaBrowser(ref riid, out ppUnk);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventServer.CreateAreaBrowser", e);
                }

                if (ppUnk == null)
                {
                    throw new InvalidResponseException("unknown == null");
                }

                m_browser = ppUnk;
            }
        }

        private void ChangeBrowsePosition(string areaID)
        {
            string szString = (areaID != null) ? areaID : "";
            try
            {
                ((IOPCEventAreaBrowser)m_browser).ChangeBrowsePosition(OPCAEBROWSEDIRECTION.OPCAE_BROWSE_TO, szString);
            }
            catch (Exception e)
            {
                throw OpcCom.Interop.CreateException("IOPCEventAreaBrowser.ChangeBrowsePosition", e);
            }
        }

        private object CreateEnumerator(BrowseType browseType, string browseFilter)
        {
            OPCAEBROWSETYPE browseType2 = Interop.GetBrowseType(browseType);
            IEnumString ppIEnumString = null;
            try
            {
                ((IOPCEventAreaBrowser)m_browser).BrowseOPCAreas(browseType2, (browseFilter != null) ? browseFilter : "", out ppIEnumString);
            }
            catch (Exception e)
            {
                throw OpcCom.Interop.CreateException("IOPCEventAreaBrowser.BrowseOPCAreas", e);
            }

            if (ppIEnumString == null)
            {
                throw new InvalidResponseException("enumerator == null");
            }

            return ppIEnumString;
        }

        private string GetQualifiedName(string name, BrowseType browseType)
        {
            string pszQualifiedAreaName = null;
            if (browseType == BrowseType.Area)
            {
                try
                {
                    ((IOPCEventAreaBrowser)m_browser).GetQualifiedAreaName(name, out pszQualifiedAreaName);
                    return pszQualifiedAreaName;
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCEventAreaBrowser.GetQualifiedAreaName", e);
                }
            }

            try
            {
                ((IOPCEventAreaBrowser)m_browser).GetQualifiedSourceName(name, out pszQualifiedAreaName);
                return pszQualifiedAreaName;
            }
            catch (Exception e2)
            {
                throw OpcCom.Interop.CreateException("IOPCEventAreaBrowser.GetQualifiedSourceName", e2);
            }
        }

        private int FetchElements(BrowseType browseType, int maxElements, System.Runtime.InteropServices.ComTypes.IEnumString enumerator, ArrayList elements)
        {
            string[] array = new string[1];
            int celt = (maxElements > 0 && maxElements - elements.Count < array.Length) ? (maxElements - elements.Count) : array.Length;

            int pceltFetched = 0;
            int num;
            for (num = enumerator.Next(celt, array, (IntPtr)pceltFetched); num == 0; num = enumerator.Next(celt, array, (IntPtr)pceltFetched))
            {
                for (int i = 0; i < pceltFetched; i++)
                {
                    BrowseElement browseElement = new BrowseElement
                    {
                        Name = array[i],
                        QualifiedName = GetQualifiedName(array[i], browseType),
                        NodeType = browseType
                    };
                    elements.Add(browseElement);
                }

                if (maxElements > 0 && elements.Count >= maxElements)
                {
                    break;
                }

                celt = ((maxElements > 0 && maxElements - elements.Count < array.Length) ? (maxElements - elements.Count) : array.Length);
            }

            return num;
        }
    }
}
