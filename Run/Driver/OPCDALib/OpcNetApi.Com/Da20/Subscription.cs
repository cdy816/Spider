using System;
using System.Collections;
using Opc;
using Opc.Da;
using OpcCom.Da;
using OpcRcw.Da;

namespace OpcCom.Da20
{
    public class Subscription : OpcCom.Da.Subscription
    {
        internal Subscription(object group, SubscriptionState state, int filters)
            : base(group, state, filters)
        {
        }

        public override void Refresh()
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
                    ((IOPCAsyncIO2)m_group).Refresh2(OPCDATASOURCE.OPC_DS_CACHE, ++m_counter, out pdwCancelID);
                }
                catch (Exception e)
                {
                    throw Interop.CreateException("IOPCAsyncIO2.RefreshMaxAge", e);
                }
            }
        }

        public override void SetEnabled(bool enabled)
        {
            lock (this)
            {
                if (m_group == null)
                {
                    throw new NotConnectedException();
                }

                try
                {
                    ((IOPCAsyncIO2)m_group).SetEnable(enabled ? 1 : 0);
                }
                catch (Exception e)
                {
                    throw Interop.CreateException("IOPCAsyncIO2.SetEnable", e);
                }
            }
        }

        public override bool GetEnabled()
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
                    ((IOPCAsyncIO2)m_group).GetEnable(out pbEnable);
                    return pbEnable != 0;
                }
                catch (Exception e)
                {
                    throw Interop.CreateException("IOPCAsyncIO2.GetEnable", e);
                }
            }
        }

        protected override ItemValueResult[] Read(ItemIdentifier[] itemIDs, Item[] items)
        {
            ItemValueResult[] array = new ItemValueResult[itemIDs.Length];
            ArrayList arrayList = new ArrayList();
            ArrayList arrayList2 = new ArrayList();
            for (int i = 0; i < itemIDs.Length; i++)
            {
                array[i] = new ItemValueResult(itemIDs[i]);
                if (items[i].MaxAgeSpecified && (items[i].MaxAge < 0 || items[i].MaxAge == int.MaxValue))
                {
                    arrayList.Add(array[i]);
                }
                else
                {
                    arrayList2.Add(array[i]);
                }
            }

            if (arrayList.Count > 0)
            {
                Read((ItemValueResult[])arrayList.ToArray(typeof(ItemValueResult)), cache: true);
            }

            if (arrayList2.Count > 0)
            {
                Read((ItemValueResult[])arrayList2.ToArray(typeof(ItemValueResult)), cache: false);
            }

            return array;
        }

        private void Read(ItemValueResult[] items, bool cache)
        {
            if (items.Length == 0)
            {
                return;
            }

            int[] array = new int[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                array[i] = (int)items[i].ServerHandle;
            }

            IntPtr ppItemValues = IntPtr.Zero;
            IntPtr ppErrors = IntPtr.Zero;
            try
            {
                ((IOPCSyncIO)m_group).Read(cache ? OPCDATASOURCE.OPC_DS_CACHE : OPCDATASOURCE.OPC_DS_DEVICE, items.Length, array, out ppItemValues, out ppErrors);
            }
            catch (Exception e)
            {
                throw Interop.CreateException("IOPCSyncIO.Read", e);
            }

            ItemValue[] itemValues = OpcCom.Da.Interop.GetItemValues(ref ppItemValues, items.Length, deallocate: true);
            int[] int32s = Interop.GetInt32s(ref ppErrors, items.Length, deallocate: true);
            for (int j = 0; j < items.Length; j++)
            {
                items[j].ResultID = Interop.GetResultID(int32s[j]);
                items[j].DiagnosticInfo = null;
                if (int32s[j] == -1073479674)
                {
                    items[j].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                }

                if (items[j].ResultID.Succeeded())
                {
                    items[j].Value = itemValues[j].Value;
                    items[j].Quality = itemValues[j].Quality;
                    items[j].QualitySpecified = itemValues[j].QualitySpecified;
                    items[j].Timestamp = itemValues[j].Timestamp;
                    items[j].TimestampSpecified = itemValues[j].TimestampSpecified;
                }
            }
        }

        protected override IdentifiedResult[] Write(ItemIdentifier[] itemIDs, ItemValue[] items)
        {
            IdentifiedResult[] array = new IdentifiedResult[itemIDs.Length];
            ArrayList arrayList = new ArrayList(itemIDs.Length);
            ArrayList arrayList2 = new ArrayList(itemIDs.Length);
            for (int i = 0; i < items.Length; i++)
            {
                array[i] = new IdentifiedResult(itemIDs[i]);
                if (items[i].QualitySpecified || items[i].TimestampSpecified)
                {
                    array[i].ResultID = ResultID.Da.E_NO_WRITEQT;
                    array[i].DiagnosticInfo = null;
                }
                else
                {
                    arrayList.Add(array[i]);
                    arrayList2.Add(items[i]);
                }
            }

            if (arrayList.Count == 0)
            {
                return array;
            }

            int[] array2 = new int[arrayList.Count];
            object[] array3 = new object[arrayList.Count];
            for (int j = 0; j < array2.Length; j++)
            {
                array2[j] = (int)((IdentifiedResult)arrayList[j]).ServerHandle;
                array3[j] = Interop.GetVARIANT(((ItemValue)arrayList2[j]).Value);
            }

            IntPtr ppErrors = IntPtr.Zero;
            try
            {
                ((IOPCSyncIO)m_group).Write(arrayList.Count, array2, array3, out ppErrors);
            }
            catch (Exception e)
            {
                throw Interop.CreateException("IOPCSyncIO.Write", e);
            }

            int[] int32s = Interop.GetInt32s(ref ppErrors, arrayList.Count, deallocate: true);
            for (int k = 0; k < arrayList.Count; k++)
            {
                IdentifiedResult identifiedResult = (IdentifiedResult)arrayList[k];
                identifiedResult.ResultID = Interop.GetResultID(int32s[k]);
                identifiedResult.DiagnosticInfo = null;
                if (int32s[k] == -1073479674)
                {
                    array[k].ResultID = new ResultID(ResultID.Da.E_READONLY, -1073479674L);
                }
            }

            return array;
        }

        protected override IdentifiedResult[] BeginRead(ItemIdentifier[] itemIDs, Item[] items, int requestID, out int cancelID)
        {
            try
            {
                int[] array = new int[itemIDs.Length];
                for (int i = 0; i < itemIDs.Length; i++)
                {
                    array[i] = (int)itemIDs[i].ServerHandle;
                }

                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCAsyncIO2)m_group).Read(itemIDs.Length, array, requestID, out cancelID, out ppErrors);
                int[] int32s = Interop.GetInt32s(ref ppErrors, itemIDs.Length, deallocate: true);
                IdentifiedResult[] array2 = new IdentifiedResult[itemIDs.Length];
                for (int j = 0; j < itemIDs.Length; j++)
                {
                    array2[j] = new IdentifiedResult(itemIDs[j]);
                    array2[j].ResultID = Interop.GetResultID(int32s[j]);
                    array2[j].DiagnosticInfo = null;
                    if (int32s[j] == -1073479674)
                    {
                        array2[j].ResultID = new ResultID(ResultID.Da.E_WRITEONLY, -1073479674L);
                    }
                }

                return array2;
            }
            catch (Exception e)
            {
                throw Interop.CreateException("IOPCAsyncIO2.Read", e);
            }
        }

        protected override IdentifiedResult[] BeginWrite(ItemIdentifier[] itemIDs, ItemValue[] items, int requestID, out int cancelID)
        {
            cancelID = 0;
            ArrayList arrayList = new ArrayList();
            ArrayList arrayList2 = new ArrayList();
            IdentifiedResult[] array = new IdentifiedResult[itemIDs.Length];
            for (int i = 0; i < itemIDs.Length; i++)
            {
                array[i] = new IdentifiedResult(itemIDs[i]);
                array[i].ResultID = ResultID.S_OK;
                array[i].DiagnosticInfo = null;
                if (items[i].QualitySpecified || items[i].TimestampSpecified)
                {
                    array[i].ResultID = ResultID.Da.E_NO_WRITEQT;
                    array[i].DiagnosticInfo = null;
                }
                else
                {
                    arrayList.Add(array[i]);
                    arrayList2.Add(Interop.GetVARIANT(items[i].Value));
                }
            }

            if (arrayList.Count == 0)
            {
                return array;
            }

            try
            {
                int[] array2 = new int[arrayList.Count];
                for (int j = 0; j < arrayList.Count; j++)
                {
                    array2[j] = (int)((IdentifiedResult)arrayList[j]).ServerHandle;
                }

                IntPtr ppErrors = IntPtr.Zero;
                ((IOPCAsyncIO2)m_group).Write(arrayList.Count, array2, (object[])arrayList2.ToArray(typeof(object)), requestID, out cancelID, out ppErrors);
                int[] int32s = Interop.GetInt32s(ref ppErrors, arrayList.Count, deallocate: true);
                for (int k = 0; k < arrayList.Count; k++)
                {
                    IdentifiedResult identifiedResult = (IdentifiedResult)arrayList[k];
                    identifiedResult.ResultID = Interop.GetResultID(int32s[k]);
                    identifiedResult.DiagnosticInfo = null;
                    if (int32s[k] == -1073479674)
                    {
                        array[k].ResultID = new ResultID(ResultID.Da.E_READONLY, -1073479674L);
                    }
                }

                return array;
            }
            catch (Exception e)
            {
                throw Interop.CreateException("IOPCAsyncIO2.Write", e);
            }
        }
    }
}
