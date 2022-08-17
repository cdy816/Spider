using System;
using System.Collections;
using Opc;
using Opc.Hda;
using OpcRcw.Comn;
using OpcRcw.Hda;

namespace OpcCom.Hda
{
    public class Browser : IBrowser, IDisposable
    {
        private const int BLOCK_SIZE = 10;

        private bool m_disposed;

        private Server m_server;

        private IOPCHDA_Browser m_browser;

        private BrowseFilterCollection m_filters = new BrowseFilterCollection();

        public BrowseFilterCollection Filters
        {
            get
            {
                lock (this)
                {
                    return (BrowseFilterCollection)m_filters.Clone();
                }
            }
        }

        internal Browser(Server server, IOPCHDA_Browser browser, BrowseFilter[] filters, ResultID[] results)
        {
            if (browser == null)
            {
                throw new ArgumentNullException("browser");
            }

            m_server = server;
            m_browser = browser;
            if (filters == null)
            {
                return;
            }

            ArrayList arrayList = new ArrayList();
            for (int i = 0; i < filters.Length; i++)
            {
                if (results[i].Succeeded())
                {
                    arrayList.Add(filters[i]);
                }
            }

            m_filters = new BrowseFilterCollection(arrayList);
        }

        ~Browser()
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
            if (!m_disposed)
            {
                lock (this)
                {
                    m_server = null;
                    OpcCom.Interop.ReleaseServer(m_browser);
                    m_browser = null;
                }

                m_disposed = true;
            }
        }

        public BrowseElement[] Browse(ItemIdentifier itemID)
        {
            IBrowsePosition position = null;
            BrowseElement[] result = Browse(itemID, 0, out position);
            position?.Dispose();
            return result;
        }

        public BrowseElement[] Browse(ItemIdentifier itemID, int maxElements, out IBrowsePosition position)
        {
            position = null;
            if (maxElements <= 0)
            {
                maxElements = int.MaxValue;
            }

            lock (this)
            {
                string text = (itemID != null && itemID.ItemName != null) ? itemID.ItemName : "";
                try
                {
                    m_browser.ChangeBrowsePosition(OPCHDA_BROWSEDIRECTION.OPCHDA_BROWSE_DIRECT, text);
                }
                catch (Exception e)
                {
                    throw OpcCom.Interop.CreateException("IOPCHDA_Browser.ChangeBrowsePosition", e);
                }

                EnumString enumerator = GetEnumerator(isBranch: true);
                ArrayList arrayList = FetchElements(enumerator, maxElements, isBranch: true);
                if (arrayList.Count >= maxElements)
                {
                    position = new BrowsePosition(text, enumerator, fetchingItems: false);
                    return (BrowseElement[])arrayList.ToArray(typeof(BrowseElement));
                }

                enumerator.Dispose();
                enumerator = GetEnumerator(isBranch: false);
                ArrayList arrayList2 = FetchElements(enumerator, maxElements - arrayList.Count, isBranch: false);
                if (arrayList2 != null)
                {
                    arrayList.AddRange(arrayList2);
                }

                if (arrayList.Count >= maxElements)
                {
                    position = new BrowsePosition(text, enumerator, fetchingItems: true);
                    return (BrowseElement[])arrayList.ToArray(typeof(BrowseElement));
                }

                enumerator.Dispose();
                return (BrowseElement[])arrayList.ToArray(typeof(BrowseElement));
            }
        }

        public BrowseElement[] BrowseNext(int maxElements, ref IBrowsePosition position)
        {
            if (position == null || (object)position.GetType() != typeof(BrowsePosition))
            {
                throw new ArgumentException("Not a valid browse position object.", "position");
            }

            if (maxElements <= 0)
            {
                maxElements = int.MaxValue;
            }

            lock (this)
            {
                BrowsePosition browsePosition = (BrowsePosition)position;
                ArrayList arrayList = new ArrayList();
                if (!browsePosition.FetchingItems)
                {
                    arrayList = FetchElements(browsePosition.Enumerator, maxElements, isBranch: true);
                    if (arrayList.Count >= maxElements)
                    {
                        return (BrowseElement[])arrayList.ToArray(typeof(BrowseElement));
                    }

                    browsePosition.Enumerator.Dispose();
                    browsePosition.Enumerator = null;
                    browsePosition.FetchingItems = true;
                    try
                    {
                        m_browser.ChangeBrowsePosition(OPCHDA_BROWSEDIRECTION.OPCHDA_BROWSE_DIRECT, browsePosition.BranchPath);
                    }
                    catch (Exception e)
                    {
                        throw OpcCom.Interop.CreateException("IOPCHDA_Browser.ChangeBrowsePosition", e);
                    }

                    browsePosition.Enumerator = GetEnumerator(isBranch: false);
                }

                ArrayList arrayList2 = FetchElements(browsePosition.Enumerator, maxElements - arrayList.Count, isBranch: false);
                if (arrayList2 != null)
                {
                    arrayList.AddRange(arrayList2);
                }

                if (arrayList.Count >= maxElements)
                {
                    return (BrowseElement[])arrayList.ToArray(typeof(BrowseElement));
                }

                position.Dispose();
                position = null;
                return (BrowseElement[])arrayList.ToArray(typeof(BrowseElement));
            }
        }

        private EnumString GetEnumerator(bool isBranch)
        {
            try
            {
                OPCHDA_BROWSETYPE dwBrowseType = isBranch ? OPCHDA_BROWSETYPE.OPCHDA_BRANCH : OPCHDA_BROWSETYPE.OPCHDA_LEAF;
                IEnumString ppIEnumString = null;
                m_browser.GetEnum(dwBrowseType, out ppIEnumString);
                return new EnumString(ppIEnumString);
            }
            catch (Exception e)
            {
                throw OpcCom.Interop.CreateException("IOPCHDA_Browser.GetEnum", e);
            }
        }

        private string GetFullBranchName(string name)
        {
            string pszBranchPos = null;
            try
            {
                m_browser.ChangeBrowsePosition(OPCHDA_BROWSEDIRECTION.OPCHDA_BROWSE_DOWN, name);
            }
            catch
            {
                return null;
            }

            try
            {
                m_browser.GetBranchPosition(out pszBranchPos);
            }
            catch
            {
            }

            m_browser.ChangeBrowsePosition(OPCHDA_BROWSEDIRECTION.OPCHDA_BROWSE_UP, "");
            return pszBranchPos;
        }

        private ArrayList FetchElements(EnumString enumerator, int maxElements, bool isBranch)
        {
            ArrayList arrayList = new ArrayList();
            while (arrayList.Count < maxElements)
            {
                int num = 10;
                if (arrayList.Count + num > maxElements)
                {
                    num = maxElements - arrayList.Count;
                }

                string[] array = enumerator.Next(num);
                if (array == null || array.Length == 0)
                {
                    break;
                }

                string[] array2 = array;
                foreach (string text in array2)
                {
                    BrowseElement browseElement = new BrowseElement();
                    browseElement.Name = text;
                    browseElement.ItemPath = null;
                    browseElement.HasChildren = isBranch;
                    string pszItemID = null;
                    try
                    {
                        if (isBranch)
                        {
                            pszItemID = GetFullBranchName(text);
                        }
                        else
                        {
                            m_browser.GetItemID(text, out pszItemID);
                        }
                    }
                    catch
                    {
                        pszItemID = null;
                    }

                    browseElement.ItemName = pszItemID;
                    arrayList.Add(browseElement);
                }
            }

            IdentifiedResult[] array3 = m_server.ValidateItems((ItemIdentifier[])arrayList.ToArray(typeof(ItemIdentifier)));
            if (array3 != null)
            {
                for (int j = 0; j < array3.Length; j++)
                {
                    if (array3[j].ResultID.Succeeded())
                    {
                        ((BrowseElement)arrayList[j]).IsItem = true;
                    }
                }
            }

            return arrayList;
        }
    }

}
