using System;
using System.Collections;
using System.Runtime.InteropServices;
using Opc;
using Opc.Da;
using OpcCom.Da;
using OpcRcw.Comn;
using OpcRcw.Da;

namespace OpcCom.Da20
{
    // Token: 0x02000034 RID: 52
    public class Server : OpcCom.Da.Server
    {
        private bool m_disposed;

        private object m_group;

        private int m_groupHandle;

        private char[] m_separators;

        private object m_separatorsLock = new object();

        internal Server()
        {
        }

        public Server(URL url, object server)
            : base(url, server)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                lock (this)
                {
                    if (disposing && m_group != null)
                    {
                        try
                        {
                            ((IOPCServer)m_server).RemoveGroup(m_groupHandle, 0);
                        }
                        catch
                        {
                        }
                    }

                    if (m_group != null)
                    {
                        Interop.ReleaseServer(m_group);
                        m_group = null;
                        m_groupHandle = 0;
                    }
                }

                m_disposed = true;
            }

            base.Dispose(disposing);
        }

        public override void Initialize(URL url, ConnectData connectData)
        {
            lock (this)
            {
                base.Initialize(url, connectData);
                m_separators = null;
                try
                {
                    int pdwLcid = 0;
                    ((IOPCCommon)m_server).GetLocaleID(out pdwLcid);
                    int pRevisedUpdateRate = 0;
                    Guid riid = typeof(IOPCItemMgt).GUID;
                    ((IOPCServer)m_server).AddGroup("", 1, 0, 0, IntPtr.Zero, IntPtr.Zero, pdwLcid, out m_groupHandle, out pRevisedUpdateRate, ref riid, out m_group);
                }
                catch (Exception e)
                {
                    Uninitialize();
                    throw Interop.CreateException("IOPCServer.AddGroup", e);
                }
            }
        }

        public override ItemValueResult[] Read(Item[] items)
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
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                IdentifiedResult[] array = AddItems(items);
                ItemValueResult[] array2 = new ItemValueResult[items.Length];
                try
                {
                    ArrayList arrayList = new ArrayList(items.Length);
                    ArrayList arrayList2 = new ArrayList(items.Length);
                    ArrayList arrayList3 = new ArrayList(items.Length);
                    ArrayList arrayList4 = new ArrayList(items.Length);
                    for (int i = 0; i < items.Length; i++)
                    {
                        array2[i] = new ItemValueResult(array[i]);
                        if (array[i].ResultID.Failed())
                        {
                            array2[i].ResultID = array[i].ResultID;
                            array2[i].DiagnosticInfo = array[i].DiagnosticInfo;
                        }
                        else if (items[i].MaxAgeSpecified && (items[i].MaxAge < 0 || items[i].MaxAge == int.MaxValue))
                        {
                            arrayList.Add(items[i]);
                            arrayList2.Add(array2[i]);
                        }
                        else
                        {
                            arrayList3.Add(items[i]);
                            arrayList4.Add(array2[i]);
                        }
                    }

                    if (arrayList2.Count > 0)
                    {
                        try
                        {
                            int[] array3 = new int[arrayList2.Count];
                            for (int j = 0; j < arrayList2.Count; j++)
                            {
                                array3[j] = (int)((ItemValueResult)arrayList2[j]).ServerHandle;
                            }

                            IntPtr ppErrors = IntPtr.Zero;
                            ((IOPCItemMgt)m_group).SetActiveState(arrayList2.Count, array3, 1, out ppErrors);
                            Marshal.FreeCoTaskMem(ppErrors);
                        }
                        catch (Exception e)
                        {
                            throw Interop.CreateException("IOPCItemMgt.SetActiveState", e);
                        }

                        ReadValues((Item[])arrayList.ToArray(typeof(Item)), (ItemValueResult[])arrayList2.ToArray(typeof(ItemValueResult)), cache: true);
                    }

                    if (arrayList4.Count > 0)
                    {
                        ReadValues((Item[])arrayList3.ToArray(typeof(Item)), (ItemValueResult[])arrayList4.ToArray(typeof(ItemValueResult)), cache: false);
                    }
                }
                finally
                {
                    RemoveItems(array);
                }

                return array2;
            }
        }

        public override IdentifiedResult[] Write(ItemValue[] items)
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
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                Item[] array = new Item[items.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    array[i] = new Item(items[i]);
                }

                IdentifiedResult[] array2 = AddItems(array);
                try
                {
                    ArrayList arrayList = new ArrayList(items.Length);
                    ArrayList arrayList2 = new ArrayList(items.Length);
                    for (int j = 0; j < items.Length; j++)
                    {
                        if (!array2[j].ResultID.Failed())
                        {
                            if (items[j].QualitySpecified || items[j].TimestampSpecified)
                            {
                                array2[j].ResultID = ResultID.Da.E_NO_WRITEQT;
                                array2[j].DiagnosticInfo = null;
                            }
                            else
                            {
                                arrayList.Add(array2[j]);
                                arrayList2.Add(items[j]);
                            }
                        }
                    }

                    if (arrayList.Count > 0)
                    {
                        int[] array3 = new int[arrayList.Count];
                        object[] array4 = new object[arrayList.Count];
                        for (int k = 0; k < array3.Length; k++)
                        {
                            array3[k] = (int)((IdentifiedResult)arrayList[k]).ServerHandle;
                            array4[k] = Interop.GetVARIANT(((ItemValue)arrayList2[k]).Value);
                        }

                        IntPtr ppErrors = IntPtr.Zero;
                        try
                        {
                            ((IOPCSyncIO)m_group).Write(arrayList.Count, array3, array4, out ppErrors);
                        }
                        catch (Exception e)
                        {
                            throw Interop.CreateException("IOPCSyncIO.Write", e);
                        }

                        int[] int32s = Interop.GetInt32s(ref ppErrors, arrayList.Count, deallocate: true);
                        for (int l = 0; l < arrayList.Count; l++)
                        {
                            IdentifiedResult identifiedResult = (IdentifiedResult)arrayList[l];
                            identifiedResult.ResultID = Interop.GetResultID(int32s[l]);
                            identifiedResult.DiagnosticInfo = null;
                            if (int32s[l] == -1073479674)
                            {
                                array2[l].ResultID = new ResultID(ResultID.Da.E_READONLY, -1073479674L);
                            }
                        }
                    }
                }
                finally
                {
                    RemoveItems(array2);
                }

                return array2;
            }
        }

        public override BrowseElement[] Browse(ItemIdentifier itemID, BrowseFilters filters, out Opc.Da.BrowsePosition position)
        {
            if (filters == null)
            {
                throw new ArgumentNullException("filters");
            }

            position = null;
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                BrowsePosition position2 = null;
                ArrayList arrayList = new ArrayList();
                if (filters.BrowseFilter != browseFilter.item)
                {
                    BrowseElement[] elements = GetElements(arrayList.Count, itemID, filters, branches: true, ref position2);
                    if (elements != null)
                    {
                        arrayList.AddRange(elements);
                    }

                    position = position2;
                    if (position != null)
                    {
                        return (BrowseElement[])arrayList.ToArray(typeof(BrowseElement));
                    }
                }

                if (filters.BrowseFilter != browseFilter.branch)
                {
                    BrowseElement[] elements2 = GetElements(arrayList.Count, itemID, filters, branches: false, ref position2);
                    if (elements2 != null)
                    {
                        arrayList.AddRange(elements2);
                    }

                    position = position2;
                }

                return (BrowseElement[])arrayList.ToArray(typeof(BrowseElement));
            }
        }

        public override BrowseElement[] BrowseNext(ref Opc.Da.BrowsePosition position)
        {
            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                if (position == null && (object)position.GetType() != typeof(BrowsePosition))
                {
                    throw new BrowseCannotContinueException();
                }

                BrowsePosition position2 = (BrowsePosition)position;
                ItemIdentifier itemID = position2.ItemID;
                BrowseFilters filters = position2.Filters;
                ArrayList arrayList = new ArrayList();
                if (position2.IsBranch)
                {
                    BrowseElement[] elements = GetElements(arrayList.Count, itemID, filters, branches: true, ref position2);
                    if (elements != null)
                    {
                        arrayList.AddRange(elements);
                    }

                    position = position2;
                    if (position != null)
                    {
                        return (BrowseElement[])arrayList.ToArray(typeof(BrowseElement));
                    }
                }

                if (filters.BrowseFilter != browseFilter.branch)
                {
                    BrowseElement[] elements2 = GetElements(arrayList.Count, itemID, filters, branches: false, ref position2);
                    if (elements2 != null)
                    {
                        arrayList.AddRange(elements2);
                    }

                    position = position2;
                }

                return (BrowseElement[])arrayList.ToArray(typeof(BrowseElement));
            }
        }

        public override ItemPropertyCollection[] GetProperties(ItemIdentifier[] itemIDs, PropertyID[] propertyIDs, bool returnValues)
        {
            if (itemIDs == null)
            {
                throw new ArgumentNullException("itemIDs");
            }

            if (itemIDs.Length == 0)
            {
                return new ItemPropertyCollection[0];
            }

            lock (this)
            {
                if (m_server == null)
                {
                    throw new NotConnectedException();
                }

                ItemPropertyCollection[] array = new ItemPropertyCollection[itemIDs.Length];
                for (int i = 0; i < itemIDs.Length; i++)
                {
                    array[i] = new ItemPropertyCollection();
                    array[i].ItemName = itemIDs[i].ItemName;
                    array[i].ItemPath = itemIDs[i].ItemPath;
                    try
                    {
                        ItemProperty[] properties = GetProperties(itemIDs[i].ItemName, propertyIDs, returnValues);
                        if (properties != null)
                        {
                            array[i].AddRange(properties);
                        }

                        array[i].ResultID = ResultID.S_OK;
                    }
                    catch (ResultIDException ex)
                    {
                        array[i].ResultID = ex.Result;
                    }
                    catch (Exception e)
                    {
                        array[i].ResultID = new ResultID(Marshal.GetHRForException(e));
                    }
                }

                return array;
            }
        }

        private IdentifiedResult[] AddItems(Item[] items)
        {
            int num = items.Length;
            OPCITEMDEF[] oPCITEMDEFs = OpcCom.Da.Interop.GetOPCITEMDEFs(items);
            for (int i = 0; i < oPCITEMDEFs.Length; i++)
            {
                oPCITEMDEFs[i].bActive = 0;
            }

            IntPtr ppAddResults = IntPtr.Zero;
            IntPtr ppErrors = IntPtr.Zero;
            int pdwLcid = 0;
            ((IOPCCommon)m_server).GetLocaleID(out pdwLcid);
            GCHandle gCHandle = GCHandle.Alloc(pdwLcid, GCHandleType.Pinned);
            try
            {
                int pRevisedUpdateRate = 0;
                ((IOPCGroupStateMgt)m_group).SetState(IntPtr.Zero, out pRevisedUpdateRate, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, gCHandle.AddrOfPinnedObject(), IntPtr.Zero);
            }
            catch (Exception e)
            {
                throw Interop.CreateException("IOPCGroupStateMgt.SetState", e);
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
                ((IOPCItemMgt)m_group).AddItems(num, oPCITEMDEFs, out ppAddResults, out ppErrors);
            }
            catch (Exception e2)
            {
                throw Interop.CreateException("IOPCItemMgt.AddItems", e2);
            }
            finally
            {
                if (gCHandle.IsAllocated)
                {
                    gCHandle.Free();
                }
            }

            int[] itemResults = OpcCom.Da.Interop.GetItemResults(ref ppAddResults, num, deallocate: true);
            int[] int32s = Interop.GetInt32s(ref ppErrors, num, deallocate: true);
            IdentifiedResult[] array = new IdentifiedResult[num];
            for (int j = 0; j < num; j++)
            {
                array[j] = new IdentifiedResult(items[j]);
                array[j].ServerHandle = null;
                array[j].ResultID = Interop.GetResultID(int32s[j]);
                array[j].DiagnosticInfo = null;
                if (array[j].ResultID.Succeeded())
                {
                    array[j].ServerHandle = itemResults[j];
                }
            }

            return array;
        }

        private void RemoveItems(IdentifiedResult[] items)
        {
            try
            {
                ArrayList arrayList = new ArrayList(items.Length);
                foreach (IdentifiedResult identifiedResult in items)
                {
                    if (identifiedResult.ResultID.Succeeded() && (object)identifiedResult.ServerHandle.GetType() == typeof(int))
                    {
                        arrayList.Add((int)identifiedResult.ServerHandle);
                    }
                }

                if (arrayList.Count != 0)
                {
                    IntPtr ppErrors = IntPtr.Zero;
                    ((IOPCItemMgt)m_group).RemoveItems(arrayList.Count, (int[])arrayList.ToArray(typeof(int)), out ppErrors);
                    Interop.GetInt32s(ref ppErrors, arrayList.Count, deallocate: true);
                }
            }
            catch
            {
            }
        }

        private void ReadValues(Item[] items, ItemValueResult[] results, bool cache)
        {
            if (items.Length == 0 || results.Length == 0)
            {
                return;
            }

            int[] array = new int[results.Length];
            for (int i = 0; i < results.Length; i++)
            {
                array[i] = System.Convert.ToInt32(results[i].ServerHandle);
            }

            IntPtr ppItemValues = IntPtr.Zero;
            IntPtr ppErrors = IntPtr.Zero;
            try
            {
                ((IOPCSyncIO)m_group).Read(cache ? OPCDATASOURCE.OPC_DS_CACHE : OPCDATASOURCE.OPC_DS_DEVICE, results.Length, array, out ppItemValues, out ppErrors);
            }
            catch (Exception e)
            {
                throw Interop.CreateException("IOPCSyncIO.Read", e);
            }

            ItemValue[] itemValues = OpcCom.Da.Interop.GetItemValues(ref ppItemValues, results.Length, deallocate: true);
            int[] int32s = Interop.GetInt32s(ref ppErrors, results.Length, deallocate: true);
            GetLocale();
            for (int j = 0; j < results.Length; j++)
            {
                results[j].ResultID = Interop.GetResultID(int32s[j]);
                results[j].DiagnosticInfo = null;
                if (results[j].ResultID.Succeeded())
                {
                    results[j].Value = itemValues[j].Value;
                    results[j].Quality = itemValues[j].Quality;
                    results[j].QualitySpecified = itemValues[j].QualitySpecified;
                    results[j].Timestamp = itemValues[j].Timestamp;
                    results[j].TimestampSpecified = itemValues[j].TimestampSpecified;
                }

                if (int32s[j] == -1073479674)
                {
                    results[j].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                }

                if (results[j].Value == null || (object)items[j].ReqType == null)
                {
                    continue;
                }

                try
                {
                    results[j].Value = ChangeType(results[j].Value, items[j].ReqType, "en-US");
                }
                catch (Exception ex)
                {
                    results[j].Value = null;
                    results[j].Quality = Quality.Bad;
                    results[j].QualitySpecified = true;
                    results[j].Timestamp = DateTime.MinValue;
                    results[j].TimestampSpecified = false;
                    if ((object)ex.GetType() == typeof(OverflowException))
                    {
                        results[j].ResultID = Interop.GetResultID(-1073479669);
                    }
                    else
                    {
                        results[j].ResultID = Interop.GetResultID(-1073479676);
                    }
                }
            }
        }

        private ItemProperty[] GetAvailableProperties(string itemID)
        {
            if (itemID == null || itemID.Length == 0)
            {
                throw new ResultIDException(ResultID.Da.E_INVALID_ITEM_NAME);
            }

            int pdwCount = 0;
            IntPtr ppPropertyIDs = IntPtr.Zero;
            IntPtr ppDescriptions = IntPtr.Zero;
            IntPtr ppvtDataTypes = IntPtr.Zero;
            try
            {
                ((IOPCItemProperties)m_server).QueryAvailableProperties(itemID, out pdwCount, out ppPropertyIDs, out ppDescriptions, out ppvtDataTypes);
            }
            catch (Exception)
            {
                throw new ResultIDException(ResultID.Da.E_UNKNOWN_ITEM_NAME);
            }

            int[] int32s = Interop.GetInt32s(ref ppPropertyIDs, pdwCount, deallocate: true);
            short[] int16s = Interop.GetInt16s(ref ppvtDataTypes, pdwCount, deallocate: true);
            string[] unicodeStrings = Interop.GetUnicodeStrings(ref ppDescriptions, pdwCount, deallocate: true);
            if (pdwCount == 0)
            {
                return null;
            }

            ItemProperty[] array = new ItemProperty[pdwCount];
            for (int i = 0; i < pdwCount; i++)
            {
                array[i] = new ItemProperty();
                array[i].ID = OpcCom.Da.Interop.GetPropertyID(int32s[i]);
                array[i].Description = unicodeStrings[i];
                array[i].DataType = Interop.GetType((VarEnum)int16s[i]);
                array[i].ItemName = null;
                array[i].ItemPath = null;
                array[i].ResultID = ResultID.S_OK;
                array[i].Value = null;
            }

            return array;
        }

        private void GetItemIDs(string itemID, ItemProperty[] properties)
        {
            try
            {
                int[] array = new int[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    array[i] = properties[i].ID.Code;
                }

                IntPtr ppszNewItemIDs = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCItemProperties)m_server).LookupItemIDs(itemID, properties.Length, array, out ppszNewItemIDs, out ppErrors);
                string[] unicodeStrings = Interop.GetUnicodeStrings(ref ppszNewItemIDs, properties.Length, deallocate: true);
                int[] int32s = Interop.GetInt32s(ref ppErrors, properties.Length, deallocate: true);
                for (int j = 0; j < properties.Length; j++)
                {
                    properties[j].ItemName = null;
                    properties[j].ItemPath = null;
                    if (int32s[j] >= 0)
                    {
                        properties[j].ItemName = unicodeStrings[j];
                    }
                }
            }
            catch
            {
                foreach (ItemProperty itemProperty in properties)
                {
                    itemProperty.ItemName = null;
                    itemProperty.ItemPath = null;
                }
            }
        }

        private void GetValues(string itemID, ItemProperty[] properties)
        {
            try
            {
                int[] array = new int[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    array[i] = properties[i].ID.Code;
                }

                IntPtr ppvData = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCItemProperties)m_server).GetItemProperties(itemID, properties.Length, array, out ppvData, out ppErrors);
                object[] vARIANTs = Interop.GetVARIANTs(ref ppvData, properties.Length, deallocate: true);
                int[] int32s = Interop.GetInt32s(ref ppErrors, properties.Length, deallocate: true);
                for (int j = 0; j < properties.Length; j++)
                {
                    properties[j].Value = null;
                    if (properties[j].ResultID.Succeeded())
                    {
                        properties[j].ResultID = Interop.GetResultID(int32s[j]);
                        if (int32s[j] == -1073479674)
                        {
                            properties[j].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                        }

                        if (properties[j].ResultID.Succeeded())
                        {
                            properties[j].Value = OpcCom.Da.Interop.UnmarshalPropertyValue(properties[j].ID, vARIANTs[j]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ResultID resultID = new ResultID(Marshal.GetHRForException(e));
                foreach (ItemProperty itemProperty in properties)
                {
                    itemProperty.Value = null;
                    itemProperty.ResultID = resultID;
                }
            }
        }

        private ItemProperty[] GetProperties(string itemID, PropertyID[] propertyIDs, bool returnValues)
        {
            ItemProperty[] array = null;
            if (propertyIDs == null)
            {
                array = GetAvailableProperties(itemID);
            }
            else
            {
                ItemProperty[] availableProperties = GetAvailableProperties(itemID);
                array = new ItemProperty[propertyIDs.Length];
                for (int i = 0; i < propertyIDs.Length; i++)
                {
                    ItemProperty[] array2 = availableProperties;
                    foreach (ItemProperty itemProperty in array2)
                    {
                        if (itemProperty.ID == propertyIDs[i])
                        {
                            array[i] = (ItemProperty)itemProperty.Clone();
                            array[i].ID = propertyIDs[i];
                            break;
                        }
                    }

                    if (array[i] == null)
                    {
                        array[i] = new ItemProperty();
                        array[i].ID = propertyIDs[i];
                        array[i].ResultID = ResultID.Da.E_INVALID_PID;
                    }
                }
            }

            if (array != null)
            {
                GetItemIDs(itemID, array);
                if (returnValues)
                {
                    GetValues(itemID, array);
                }
            }

            return array;
        }

        private EnumString GetEnumerator(string itemID, BrowseFilters filters, bool branches, bool flat)
        {
            IOPCBrowseServerAddressSpace iOPCBrowseServerAddressSpace = (IOPCBrowseServerAddressSpace)m_server;
            if (!flat)
            {
                string text = (itemID != null) ? itemID : "";
                try
                {
                    iOPCBrowseServerAddressSpace.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_TO, text);
                }
                catch
                {
                    try
                    {
                        iOPCBrowseServerAddressSpace.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_DOWN, text);
                    }
                    catch
                    {
                        try
                        {
                            while (true)
                            {
                                iOPCBrowseServerAddressSpace.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_UP, string.Empty);
                            }
                        }
                        catch
                        {
                        }

                        string[] array = null;
                        lock (m_separatorsLock)
                        {
                            array = ((m_separators == null) ? text.Split(m_separators) : text.Split(m_separators));
                        }

                        for (int i = 0; i < array.Length; i++)
                        {
                            if (array[i] != null && array[i].Length != 0)
                            {
                                try
                                {
                                    iOPCBrowseServerAddressSpace.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_DOWN, array[i]);
                                }
                                catch
                                {
                                    throw new ResultIDException(ResultID.Da.E_UNKNOWN_ITEM_NAME, "Cannot browse because the server is not compliant because it does not support the BROWSE_TO function.");
                                }
                            }
                        }
                    }
                }
            }

            try
            {
                IEnumString ppIEnumString = null;
                OPCBROWSETYPE dwBrowseFilterType = branches ? OPCBROWSETYPE.OPC_BRANCH : OPCBROWSETYPE.OPC_LEAF;
                if (flat)
                {
                    dwBrowseFilterType = OPCBROWSETYPE.OPC_FLAT;
                }

                iOPCBrowseServerAddressSpace.BrowseOPCItemIDs(dwBrowseFilterType, (filters.ElementNameFilter != null) ? filters.ElementNameFilter : "", 0, 0, out ppIEnumString);
                return new EnumString(ppIEnumString);
            }
            catch
            {
                throw new ResultIDException(ResultID.Da.E_UNKNOWN_ITEM_NAME);
            }
        }

        private void DetectAndSaveSeparators(string browseName, string itemID)
        {
            if (!itemID.EndsWith(browseName))
            {
                return;
            }

            char c = itemID[itemID.Length - browseName.Length - 1];
            lock (m_separatorsLock)
            {
                int num = -1;
                if (m_separators != null)
                {
                    for (int i = 0; i < m_separators.Length; i++)
                    {
                        if (m_separators[i] == c)
                        {
                            num = i;
                            break;
                        }
                    }

                    if (num == -1)
                    {
                        char[] array = new char[m_separators.Length + 1];
                        Array.Copy(m_separators, array, m_separators.Length);
                        m_separators = array;
                    }
                }

                if (num == -1)
                {
                    if (m_separators == null)
                    {
                        m_separators = new char[1];
                    }

                    m_separators[m_separators.Length - 1] = c;
                }
            }
        }

        private BrowseElement GetElement(ItemIdentifier itemID, string name, BrowseFilters filters, bool isBranch)
        {
            if (name == null)
            {
                return null;
            }

            BrowseElement browseElement = new BrowseElement();
            browseElement.Name = name;
            browseElement.HasChildren = isBranch;
            browseElement.ItemPath = null;
            try
            {
                string szItemID = null;
                ((IOPCBrowseServerAddressSpace)m_server).GetItemID(browseElement.Name, out szItemID);
                browseElement.ItemName = szItemID;
                if (browseElement.ItemName != null)
                {
                    DetectAndSaveSeparators(browseElement.Name, browseElement.ItemName);
                }
            }
            catch
            {
                browseElement.ItemName = name;
            }

            try
            {
                OPCITEMDEF oPCITEMDEF = default(OPCITEMDEF);
                oPCITEMDEF.szItemID = browseElement.ItemName;
                oPCITEMDEF.szAccessPath = null;
                oPCITEMDEF.hClient = 0;
                oPCITEMDEF.bActive = 0;
                oPCITEMDEF.vtRequestedDataType = 0;
                oPCITEMDEF.dwBlobSize = 0;
                oPCITEMDEF.pBlob = IntPtr.Zero;
                IntPtr ppValidationResults = IntPtr.Zero;
                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCItemMgt)m_group).ValidateItems(1, new OPCITEMDEF[1]
                {
                    oPCITEMDEF
                }, 0, out ppValidationResults, out ppErrors);
                OpcCom.Da.Interop.GetItemResults(ref ppValidationResults, 1, deallocate: true);
                int[] int32s = Interop.GetInt32s(ref ppErrors, 1, deallocate: true);
                browseElement.IsItem = (int32s[0] >= 0);
            }
            catch
            {
                browseElement.IsItem = false;
            }

            try
            {
                if (filters.ReturnAllProperties)
                {
                    browseElement.Properties = GetProperties(browseElement.ItemName, null, filters.ReturnPropertyValues);
                    return browseElement;
                }

                if (filters.PropertyIDs != null)
                {
                    browseElement.Properties = GetProperties(browseElement.ItemName, filters.PropertyIDs, filters.ReturnPropertyValues);
                    return browseElement;
                }

                return browseElement;
            }
            catch
            {
                browseElement.Properties = null;
                return browseElement;
            }
        }

        private BrowseElement[] GetElements(int elementsFound, ItemIdentifier itemID, BrowseFilters filters, bool branches, ref BrowsePosition position)
        {
            EnumString enumString = null;
            if (position == null)
            {
                IOPCBrowseServerAddressSpace iOPCBrowseServerAddressSpace = (IOPCBrowseServerAddressSpace)m_server;
                OPCNAMESPACETYPE pNameSpaceType = OPCNAMESPACETYPE.OPC_NS_HIERARCHIAL;
                try
                {
                    iOPCBrowseServerAddressSpace.QueryOrganization(out pNameSpaceType);
                }
                catch (Exception e)
                {
                    throw Interop.CreateException("IOPCBrowseServerAddressSpace.QueryOrganization", e);
                }

                if (pNameSpaceType == OPCNAMESPACETYPE.OPC_NS_FLAT)
                {
                    if (branches)
                    {
                        return new BrowseElement[0];
                    }

                    if (itemID != null && itemID.ItemName != null && itemID.ItemName.Length > 0)
                    {
                        throw new ResultIDException(ResultID.Da.E_UNKNOWN_ITEM_NAME);
                    }
                }

                enumString = GetEnumerator(itemID?.ItemName, filters, branches, pNameSpaceType == OPCNAMESPACETYPE.OPC_NS_FLAT);
            }
            else
            {
                enumString = position.Enumerator;
            }

            ArrayList arrayList = new ArrayList();
            BrowseElement browseElement = null;
            int num = 0;
            string[] array = null;
            if (position != null)
            {
                num = position.Index;
                array = position.Names;
                position = null;
            }

            do
            {
                if (array != null)
                {
                    for (int i = num; i < array.Length; i++)
                    {
                        if (filters.MaxElementsReturned != 0 && filters.MaxElementsReturned == arrayList.Count + elementsFound)
                        {
                            position = new BrowsePosition(itemID, filters, enumString, branches);
                            position.Names = array;
                            position.Index = i;
                            break;
                        }

                        browseElement = GetElement(itemID, array[i], filters, branches);
                        if (browseElement == null)
                        {
                            break;
                        }

                        arrayList.Add(browseElement);
                    }
                }

                if (position != null)
                {
                    break;
                }

                array = enumString.Next(10);
                num = 0;
            }
            while (array != null && array.Length > 0);
            if (position == null)
            {
                enumString.Dispose();
            }

            return (BrowseElement[])arrayList.ToArray(typeof(BrowseElement));
        }

        protected override OpcCom.Da.Subscription CreateSubscription(object group, SubscriptionState state, int filters)
        {
            return new Subscription(group, state, filters);
        }
    }
}
