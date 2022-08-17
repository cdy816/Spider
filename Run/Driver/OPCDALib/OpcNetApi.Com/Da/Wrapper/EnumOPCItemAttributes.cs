using System;
using System.Collections;
using System.Runtime.InteropServices;
using Opc.Da;
using OpcRcw.Da;

namespace OpcCom.Da.Wrapper
{
    [CLSCompliant(false)]
    public class EnumOPCItemAttributes : IEnumOPCItemAttributes
    {
        public class ItemAttributes
        {
            public string ItemID;

            public string AccessPath;

            public int ClientHandle = -1;

            public int ServerHandle = -1;

            public bool Active;

            public Type CanonicalDataType;

            public Type RequestedDataType;

            public accessRights AccessRights = accessRights.readWritable;

            public euType EuType = euType.noEnum;

            public double MaxValue;

            public double MinValue;

            public string[] EuInfo;
        }

        private ArrayList m_items = new ArrayList();

        private int m_index;

        internal EnumOPCItemAttributes(ICollection items)
        {
            if (items == null)
            {
                return;
            }

            foreach (ItemAttributes item in items)
            {
                m_items.Add(item);
            }
        }

        public void Skip(int celt)
        {
            lock (this)
            {
                try
                {
                    m_index += celt;
                    if (m_index > m_items.Count)
                    {
                        m_index = m_items.Count;
                    }
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void Clone(out IEnumOPCItemAttributes ppEnumItemAttributes)
        {
            lock (this)
            {
                try
                {
                    ppEnumItemAttributes = new EnumOPCItemAttributes(m_items);
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void Reset()
        {
            lock (this)
            {
                try
                {
                    m_index = 0;
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }

        public void Next(int celt, out IntPtr ppItemArray, out int pceltFetched)
        {
            lock (this)
            {
                try
                {
                    pceltFetched = 0;
                    ppItemArray = IntPtr.Zero;
                    if (m_index >= m_items.Count)
                    {
                        return;
                    }

                    pceltFetched = m_items.Count - m_index;
                    if (pceltFetched > celt)
                    {
                        pceltFetched = celt;
                    }

                    ppItemArray = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(OPCITEMATTRIBUTES)) * pceltFetched);
                    IntPtr ptr = ppItemArray;
                    for (int i = 0; i < pceltFetched; i++)
                    {
                        ItemAttributes itemAttributes = (ItemAttributes)m_items[m_index + i];
                        OPCITEMATTRIBUTES oPCITEMATTRIBUTES = default(OPCITEMATTRIBUTES);
                        oPCITEMATTRIBUTES.szItemID = itemAttributes.ItemID;
                        oPCITEMATTRIBUTES.szAccessPath = itemAttributes.AccessPath;
                        oPCITEMATTRIBUTES.hClient = itemAttributes.ClientHandle;
                        oPCITEMATTRIBUTES.hServer = itemAttributes.ServerHandle;
                        oPCITEMATTRIBUTES.bActive = (itemAttributes.Active ? 1 : 0);
                        oPCITEMATTRIBUTES.vtCanonicalDataType = (short)OpcCom.Interop.GetType(itemAttributes.CanonicalDataType);
                        oPCITEMATTRIBUTES.vtRequestedDataType = (short)OpcCom.Interop.GetType(itemAttributes.RequestedDataType);
                        oPCITEMATTRIBUTES.dwAccessRights = (int)Interop.MarshalPropertyValue(Property.ACCESSRIGHTS, itemAttributes.AccessRights);
                        oPCITEMATTRIBUTES.dwBlobSize = 0;
                        oPCITEMATTRIBUTES.pBlob = IntPtr.Zero;
                        oPCITEMATTRIBUTES.dwEUType = (OPCEUTYPE)Interop.MarshalPropertyValue(Property.EUTYPE, itemAttributes.EuType);
                        oPCITEMATTRIBUTES.vEUInfo = null;
                        switch (itemAttributes.EuType)
                        {
                            case euType.analog:
                                oPCITEMATTRIBUTES.vEUInfo = new double[2]
                                {
                                itemAttributes.MinValue,
                                itemAttributes.MaxValue
                                };
                                break;
                            case euType.enumerated:
                                oPCITEMATTRIBUTES.vEUInfo = itemAttributes.EuInfo;
                                break;
                        }

                        Marshal.StructureToPtr((object)oPCITEMATTRIBUTES, ptr, fDeleteOld: false);
                        ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(OPCITEMATTRIBUTES)));
                    }

                    m_index += pceltFetched;
                }
                catch (Exception e)
                {
                    throw Server.CreateException(e);
                }
            }
        }
    }
}
